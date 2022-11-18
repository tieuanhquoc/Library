using System.Threading.Channels;

namespace TieuAnhQuoc.QueueService
{
    public interface IQueueService
    {
        ValueTask QueueBackgroundWorkItemAsync(Func<CancellationToken, ValueTask> workItem);
        ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken);
    }

    public class QueueService : IQueueService
    {
        private readonly Channel<Func<CancellationToken, ValueTask>> _queue;

        public QueueService(int capacity)
        {
            BoundedChannelOptions options = new(capacity)
            {
                FullMode = BoundedChannelFullMode.Wait
            };
            _queue = Channel.CreateBounded<Func<CancellationToken, ValueTask>>(options);
        }

        public async ValueTask QueueBackgroundWorkItemAsync(
            Func<CancellationToken, ValueTask> workItem)
        {
            if (workItem is null)
            {
                return;
            }

            await _queue.Writer.WriteAsync(workItem);
        }

        public async ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(
            CancellationToken cancellationToken)
        {
            Console.WriteLine($"Total queue: {_queue.Reader.Count}");
            var workItem = await _queue.Reader.ReadAsync(cancellationToken);
            return workItem;
        }
    }
}