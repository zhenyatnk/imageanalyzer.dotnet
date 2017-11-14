using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Text;

namespace imageanalyzer.dotnet.ui.operations
{
    public class OperationAnalyze
        : CBaseObservableOperation, IOperation 
    {
        public OperationAnalyze(ICollection<model.FileMetaInfo> _files_metainfo, CancellationToken _cancel)
        {
            files_metainfo = _files_metainfo;
            cancel = _cancel;
        }

        public void Execute()
        {
            GetObserver().NotifyStart();
            Task.Factory.StartNew(() =>
            {
                using (imageanalyzer.dotnet.core.interfaces.IAnalyzer analyzer = imageanalyzer.dotnet.core.IAnalyzerCreate.Create())
                {
                    Wrapped<int> worked = new Wrapped<int>(0);
                    var count = files_metainfo.Count;
                    foreach (var file_metainfo in files_metainfo)
                    {
                        analyzer.add_task(file_metainfo.imagefile_full_name, new List<imageanalyzer.dotnet.core.interfaces.IObserverTask>
                                                                                    { new ObserverTaskMeta(file_metainfo),
                                                                                      new ObserverTaskProgress(GetObserver(), count, worked)});
                    }
                    while (!cancel.IsCancellationRequested && !analyzer.complete())
                        Thread.Sleep(500);
                }
                GetObserver().NotifyComplete();
            });
        }

        private ICollection<model.FileMetaInfo> files_metainfo;
        private CancellationToken cancel;
    }
}
