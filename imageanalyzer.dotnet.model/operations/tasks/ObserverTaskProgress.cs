using System.Threading.Tasks;

namespace imageanalyzer.dotnet.model.operations.tasks
{
    class ObserverTaskProgress 
        : imageanalyzer.dotnet.core.interfaces.IObserverTask
    {
        public ObserverTaskProgress(operations.INotifierProgress _notifier_progress, int _count, utilities.Wrapped<int> _worked )
        {
            notifier_progress = _notifier_progress;
            count = _count;
            worked = _worked;
        }

        public void HandleComplete()
        {
            Task.Factory.StartNew(() =>
            {
                ++worked.value;
                notifier_progress.NotifyChangeProgress((100.0 * worked.value) /count);
            });
        }
        public void HandleStart()
        { }

        public void HandleError(string aMessage, int aErrorCode)
        { }

        private operations.INotifierProgress notifier_progress;
        private int count;
        private utilities.Wrapped<int> worked;
    }
}
