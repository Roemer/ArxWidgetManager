using System.Threading;
using System.Threading.Tasks;

namespace Arx.Shared
{
    /// <summary>
    /// Widget which provides a worker method which is regularly called
    /// </summary>
    public abstract class WorkerWidgetBase : WidgetBase
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(false);
        private readonly Task _workerTask;

        protected WorkerWidgetBase(int sleep = 500)
        {
            BeforeStopEvent += () =>
            {
                _cancellationTokenSource.Cancel();
                _workerTask.Wait(1000);
            };

            var ct = _cancellationTokenSource.Token;
            _workerTask = Task.Run(() =>
            {
                while (!ct.IsCancellationRequested)
                {
                    if (!HasFocus)
                    {
                        _autoResetEvent.WaitOne();
                    }
                    DoWork();
                    Thread.Sleep(sleep);
                }
            }, ct);
        }

        protected override void WidgetFocus(LogiArxOrientation orientation)
        {
            base.WidgetFocus(orientation);
            _autoResetEvent.Set();
        }

        protected virtual void DoWork() { }
    }
}
