using Microsoft.Extensions.Hosting;
using Serilog;

namespace TieuAnhQuoc.QueueService;

public class QueuedHostedService : BackgroundService
{
    private readonly int _threads;
    private readonly IQueueService _queueService;

    public QueuedHostedService(IQueueService queueService)
    {
        _queueService = queueService;
        var threads = Environment.GetEnvironmentVariable("QUEUE_THREAD_NUMBER");
        int.TryParse(threads, out var threadNumber);
        _threads = threadNumber <= 0 ? 1 : threadNumber;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Log.Information(
            "{QueuedHostedService} is running.{NewLine}Tap W to add a work item to the background queue",
            nameof(QueuedHostedService), Environment.NewLine);
        return ProcessTaskQueueAsync(stoppingToken);
    }

    private async Task ProcessTaskQueueAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Parallel.ForEachAsync(Enumerable.Range(0, _threads), stoppingToken, async (_, _) =>
            {
                try
                {
                    var workItem = await _queueService.DequeueAsync(stoppingToken);
                    await workItem(stoppingToken);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error occurred executing task work item");
                }
            });
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        Log.Information("{QueuedHostedService} is stopping", nameof(QueuedHostedService));
        await base.StopAsync(cancellationToken);
    }
}