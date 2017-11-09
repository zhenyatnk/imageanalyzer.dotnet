using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace imageanalyzer.dotnet.ui.operations
{
	public class ObserverOperation
		: Notifier, IObserverOperation
    {
		public ObserverOperation(string _task_name, view_model.OperationView _view)
		{
			task_name = _task_name;
            view = _view;
		}

		//IObserver
		public void HandleStart()
		{
            view.Title = task_name + " starting...";
        }

		public void HandleChangeProgress(double aPercent)
		{
            view.Title = task_name + "...";
            view.Progress = aPercent;
		}
		public void HandleComplete()
		{
            view.Title = task_name + " complete";
            view.Complete = true;
        }

		private string task_name;
        private view_model.OperationView view;
    }
}
