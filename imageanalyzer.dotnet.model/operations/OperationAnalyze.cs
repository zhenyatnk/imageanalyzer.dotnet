using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace imageanalyzer.dotnet.model.operations
{
    public class OperationAnalyze
        : CBaseObservableOperation, IOperation 
    {
        public OperationAnalyze(ICollection<meta.FileMetaInfo> _files_metainfo, CancellationToken _cancel)
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
                    utilities.Wrapped<int> worked = new utilities.Wrapped<int>(0);
                    var count = files_metainfo.Count;
                    foreach (var file_metainfo in files_metainfo)
                    {
                        analyzer.add_task(file_metainfo.imagefile_full_name, new List<imageanalyzer.dotnet.core.interfaces.IObserverTask>
                                                                                    { new tasks.ObserverTaskMeta(file_metainfo),
                                                                                      new tasks.ObserverTaskProgress(GetObserver(), count, worked)});
                    }
                    while (!cancel.IsCancellationRequested && !analyzer.complete())
                        Thread.Sleep(500);
                }
                GetObserver().NotifyComplete();
            });
        }

        private ICollection<meta.FileMetaInfo> files_metainfo;
        private CancellationToken cancel;
    }
}
