using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace imageanalyzer.dotnet.ui.view_model.command
{
	public class CheckCommand
        : ICommand
	{
        public CheckCommand(ViewModel _model_view)
        {
            model_view = _model_view;
        }

        public void Execute(object parameter)
		{
            var cancel = new CancellationTokenSource();
            var operation = new operations.OperationCheck(model_view.Project, model_view.Operations, cancel);
            operation.AddObserver(new operations.ObserverOperation("Checking", new OperationView(model_view.Operations, cancel)));
            operation.Execute();
        }

		public bool CanExecute(object parameter)
		{
			return true;
		}

        public event EventHandler CanExecuteChanged;
        private ViewModel model_view;
    }
}
