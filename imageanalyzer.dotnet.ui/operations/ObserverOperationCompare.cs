using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace imageanalyzer.dotnet.ui.operations
{
	public class ObserverOperationCompare
		: Notifier, model.operations.IObserverOperationCompare
    {
		public ObserverOperationCompare(string _task_name, view_model.OperationView _view, view_model.OperationViewCompare _view_compare)
		{
            InitTimer();
            observer = new operations.ObserverOperation(_task_name, _view);
            result_lock = new object();
            view_compare = _view_compare;
        }

        //IObserverOperationCompare
        public void HandleCompareComplete(double percent, string filename)
		{
            lock (result_lock)
            {
                view_compare.Result.Add(percent, filename);
            }
        }
        public void HandleStart()
        {
            observer.HandleStart();
        }

        public void HandleChangeProgress(double aPercent)
        {
            observer.HandleChangeProgress(aPercent);
        }
        public void HandleComplete()
        {
            observer.HandleComplete();
            timer.Stop();
            UpdateViewResult();
        }

        private void UpdateViewResult()
        {
            view_compare.ViewResult.dispatcher.Invoke(new Action(() => { view_compare.ViewResult.Clear(); }));
            lock (result_lock)
            {
                var i = 0;
                foreach (var find in view_compare.Result)
                {
                    view_compare.ViewResult.dispatcher.Invoke(new Action(() => { view_compare.ViewResult.Add(new Tuple<double, string>(find.Key, find.Value)); }));
                    if (i++ == 5)
                        break;
                }
            }
        }

        private void InitTimer()
        {
            if (timer != null)
                timer.Stop();

            timer = new System.Timers.Timer(500);
            timer.Elapsed += (source, e) => { UpdateViewResult(); };
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private view_model.OperationViewCompare view_compare;
        private ObserverOperation observer;
        private System.Timers.Timer timer;
        private Object result_lock;
    }
}
