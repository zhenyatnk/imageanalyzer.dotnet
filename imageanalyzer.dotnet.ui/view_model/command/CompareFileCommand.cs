using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace imageanalyzer.dotnet.ui.view_model.command
{
    public class CompareFileCommand
        : ICommand
    {
        public CompareFileCommand(ViewModel _model_view)
        {
            model_view = _model_view;
        }

        public void Execute(object parameter)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                List<model.meta.FileMetaInfo> metas = new List<model.meta.FileMetaInfo>();
                foreach (var meta in model_view.Project.files_meta_info)
                    if (!string.IsNullOrEmpty(meta.datafile_full_name))
                        metas.Add(meta);

                var cancel = new CancellationTokenSource();
                model_view.Compare.CancelCommand.Execute(null);
                model_view.Compare = new OperationViewCompare(cancel);

                var operation = new model.operations.OperationCompare(metas, dialog.FileName, cancel.Token);
                operation.AddObserver(new operations.ObserverOperationCompare("Compare", new OperationView(model_view.Operations, cancel), model_view.Compare));
                operation.Execute();
            }
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }
        
        public event EventHandler CanExecuteChanged;
        private ViewModel model_view;
    }
}
