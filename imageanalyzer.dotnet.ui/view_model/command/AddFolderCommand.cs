using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Forms;
using System.IO;
using System.Windows.Input;

namespace imageanalyzer.dotnet.ui.view_model.command
{
	public class AddFolderCommand
		: ICommand
	{
        public AddFolderCommand(ViewModel _model_view)
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
                var operation = new operations.OperationAnalyze(meta_info, cancel.Token);
                operation.AddObserver(new operations.ObserverOperation("Analyzing", new OperationView(model_view.Operations, cancel)));
                operation.Execute();
            }
        }
		public bool CanExecute(object parameter)
		{
			return true;
		}

        private List<model.FileMetaInfo> ToMetaInfo (string [] files_names)
        {
            var files_metainfo = new List<model.FileMetaInfo>();

            foreach (string file in files_names)
            {
                var file_metadata = new model.FileMetaInfo();
                file_metadata.imagefile_full_name = file;
                files_metainfo.Add(file_metadata);
            }
            return files_metainfo;
        }

        private List<model.FileMetaInfo> GetMetaInfo()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                return ToMetaInfo(Directory.GetFiles(dialog.SelectedPath, "*.jpg"));
            return new List<model.FileMetaInfo>();
        }

        public event EventHandler CanExecuteChanged;
        private ViewModel model_view;
    }
}
