using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Andoromeda.Framework.Logger
{
    internal class LogSemaphore
    {
        private ConcurrentQueue<TaskCompletionSource<LogItem>> _queue = new ConcurrentQueue<TaskCompletionSource<LogItem>>();

        public Task<LogItem> WaitForLogAttainAsync()
        {
            var task = new TaskCompletionSource<LogItem>();
            _queue.Enqueue(task);
            return task.Task;
        }

        public void LogAttain(LogItem item)
        {
            if (_queue.TryDequeue(out var task))
            {
                task.SetResult(item);
            }
        }
    }
}
