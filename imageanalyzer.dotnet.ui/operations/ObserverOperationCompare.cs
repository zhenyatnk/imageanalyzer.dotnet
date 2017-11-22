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
            observer = new operations.ObserverOperation(_task_name, _view);
            view_compare = _view_compare;
        }

        //IObserverOperationCompare
        public void HandleCompareComplete(double percent, string filename)
		{
            if (!view_compare.Result.ContainsKey(percent))
                view_compare.Result.Add(percent, filename);
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
            var i = 0;
            foreach(var find in view_compare.Result)
            {
                view_compare.ViewResult.dispatcher.Invoke(new Action(() => { view_compare.ViewResult.Add(new Tuple<double, string>(find.Key, find.Value)); }));
                if (i++ == 10)
                    break;
            }
        }

        private view_model.OperationViewCompare view_compare;
        private ObserverOperation observer;
    }
}
