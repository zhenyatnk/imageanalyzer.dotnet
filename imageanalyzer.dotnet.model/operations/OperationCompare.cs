using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace imageanalyzer.dotnet.model.operations
{
    public class OperationCompare
        : CBaseObservableOperationCompare, IOperationCompare
    {
        public OperationCompare(ICollection<meta.FileMetaInfo> _files_metainfo, string _image_filename_compare, CancellationToken _cancel)
        {
            files_metainfo = _files_metainfo;
            image_filename_compare = _image_filename_compare;
            cancel = _cancel;
        }

        public void Execute()
        {
            GetObserver().NotifyStart();
            Task.Factory.StartNew(() =>
            {
                var data_filename_compare = image_filename_compare + ".data";
                using (imageanalyzer.dotnet.core.interfaces.IAnalyzer analyzer = imageanalyzer.dotnet.core.IAnalyzerCreate.Create())
                {
                    analyzer.analyze_sync(image_filename_compare, data_filename_compare);
                }
                using (imageanalyzer.dotnet.core.interfaces.IComparator comparator = imageanalyzer.dotnet.core.IComparatorCreate.Create(data_filename_compare))
                {
                    var count_complete = 0;
                    var count = files_metainfo.Count;
                    foreach (var file_metainfo in files_metainfo)
                    {
                        GetObserver().NotifyCompareComplete(comparator.compare(file_metainfo.datafile_full_name), file_metainfo.imagefile_full_name);
                        GetObserver().NotifyChangeProgress((100.0 * (++count_complete)) / count);

                        if (cancel.IsCancellationRequested)
                            break;
                    }
                }
                GetObserver().NotifyComplete();
            });
        }

        private ICollection<meta.FileMetaInfo> files_metainfo;
        private CancellationToken cancel;
        private string image_filename_compare;
    }
}
