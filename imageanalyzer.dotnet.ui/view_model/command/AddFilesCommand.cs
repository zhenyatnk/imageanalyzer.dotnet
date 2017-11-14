using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace imageanalyzer.dotnet.ui.view_model.command
{
	public class AddFilesCommand
		: ICommand
	{
        public AddFilesCommand(ViewModel _model_view)
        {
            model_view = _model_view;
        }

        public void Execute(object parameter)
		{
            var meta_info = GetMetaInfo();
            if(meta_info.Count != 0)
            {
                model_view.Project.files_meta_info.AddRange(meta_info);
                var cancel = new CancellationTokenSource();
                var operation = new model.operations.OperationAnalyze(meta_info, cancel.Token);
                operation.AddObserver(new operations.ObserverOperation("Analyzing", new OperationView(model_view.Operations, cancel)));
                operation.Execute();
            }
        }
		public bool CanExecute(object parameter)
		{
			return true;
		}

        private List<model.meta.FileMetaInfo> ToMetaInfo (string [] files_names)
        {
            var files_metainfo = new List<model.meta.FileMetaInfo>();

            foreach (string file in files_names)
            {
                var file_metadata = new model.meta.FileMetaInfo();
                file_metadata.imagefile_full_name = file;
                files_metainfo.Add(file_metadata);
            }
            return files_metainfo;
        }

        private List<model.meta.FileMetaInfo> GetMetaInfo()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                return ToMetaInfo(dialog.FileNames);
            return new List<model.meta.FileMetaInfo>();
        }

        public event EventHandler CanExecuteChanged;
        private ViewModel model_view;
    }
}
