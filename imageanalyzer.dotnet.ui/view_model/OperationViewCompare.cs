using System;
using System.Collections.Generic;
using System.Threading;

namespace imageanalyzer.dotnet.ui.view_model
{
    public class OperationViewCompare
        : Notifier
    {
        public OperationViewCompare(CancellationTokenSource _cancel)
        {
            CancelCommand = new command.CancelCommand(_cancel);
            view_result = new ObservableCollectionDisp<Tuple<double, string>>();
            result = new Dictionary<double, string>();
        }
        
        public ObservableCollectionDisp<Tuple<double, string>> ViewResult
        {
            get { return view_result; }
            set
            {
                view_result = value;
                NotifyPropertyChanged("ViewResult");
            }
        }

        public Dictionary<double, string> Result
        {
            get { return result; }
            set
            {
                result = value;
                NotifyPropertyChanged("Result");
            }
        }

        public command.CancelCommand CancelCommand { get; set; }

        private ObservableCollectionDisp<Tuple<double, string>> view_result;
        private Dictionary<double, string> result;
    }
}
