using System;
using System.Threading;
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
            var operation = new model.operations.OperationCheck(model_view.Project, new operations.ObserverOperation("Analyze", new OperationView(model_view.Operations, cancel)), cancel);
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
