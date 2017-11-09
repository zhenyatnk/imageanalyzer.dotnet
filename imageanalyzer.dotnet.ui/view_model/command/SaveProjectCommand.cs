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
	public class SaveProjectCommand
        : ICommand
	{
        public SaveProjectCommand(ViewModel _model_view)
        {
            model_view = _model_view;
        }

        public void Execute(object parameter)
		{
            if (string.IsNullOrEmpty(model_view.ProjectName))
            {
                SaveFileDialog dialog = new SaveFileDialog();
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    model_view.ProjectName = dialog.FileName;
            }

            if (!string.IsNullOrEmpty(model_view.ProjectName))
                Utilities.SaveProjectToFile(model_view.Project, model_view.ProjectName);
        }

		public bool CanExecute(object parameter)
		{
			return true;
		}

        public event EventHandler CanExecuteChanged;
        private ViewModel model_view;
    }
}
