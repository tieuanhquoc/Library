# How do I get started?

#### Install

    Install-Package TieuAnhQuoc.QueueService

#### Register service

    builder.Services.AddSingleton<IQueueService>(_ => new QueueService([capacity]));
    builder.Services.AddHostedService<QueuedHostedService>();

#### Add ENV
    QUEUE_THREAD_NUMBER = [THREAD_NUMBER] // Default = 1

#### Use service

    private readonly IQueueService _queueService;
    public ServiceName(IQueueService queueService)
    {
        _queueService = queueService;
    }
    //
    await _queueService.QueueBackgroundWorkItemAsync(async _ =>
    {
        // TASK
    });