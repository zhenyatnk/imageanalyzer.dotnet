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
	public class OpenProjectCommand
		: ICommand
	{
        public OpenProjectCommand(ViewModel _model_view)
        {
            model_view = _model_view;
        }

        public void Execute(object parameter)
		{
            if (model_view.Project.files_meta_info.Count != 0)
                model_view.SaveProjectCommand.Execute(null);

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Project file|*.prj";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                model_view.ProjectName = dialog.FileName;
                model_view.Project = Utilities.LoadProjectFromFile(model_view.ProjectName);

                var cancel = new CancellationTokenSource();
                var operation = new operations.OperationAnalyze(GetNotAnalyzed(model_view.Project), cancel.Token);
                operation.AddObserver(new operations.ObserverOperation("Analyzing", new OperationView(model_view.Operations, cancel)));
                operation.Execute();
            }
        }
		public bool CanExecute(object parameter)
		{
			return true;
		}

        private List<model.FileMetaInfo> GetNotAnalyzed (model.Project project)
        {
            List<model.FileMetaInfo> files_analyzing = new List<model.FileMetaInfo>();
            foreach (var file in project.files_meta_info)
            {
                if (string.IsNullOrEmpty(file.datafile_full_name))
                    files_analyzing.Add(file);
            }
            return files_analyzing;
        }
        
        public event EventHandler CanExecuteChanged;
        private ViewModel model_view;
    }
}
