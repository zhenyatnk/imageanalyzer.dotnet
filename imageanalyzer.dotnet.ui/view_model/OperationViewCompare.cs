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
            result = new MultiSortedList<double, string>(new CompareReverse<double>());
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

        public MultiSortedList<double, string> Result
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
        private MultiSortedList<double, string> result;
    }
}
