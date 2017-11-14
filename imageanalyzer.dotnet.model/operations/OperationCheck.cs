using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace imageanalyzer.dotnet.model.operations
{
    public class OperationCheck
        : CBaseObservableOperation, IOperation 
    {
        public OperationCheck(meta.Project _project, IObserverOperation _observer_analyze, CancellationTokenSource _cancel)
        {
            project = _project;
            observer_analyze = _observer_analyze;
            cancel = _cancel;
        }

        public void Execute()
        {
            GetObserver().NotifyStart();
            Task.Factory.StartNew(() =>
            {
                List<meta.FileMetaInfo> list_need_analyze = new List<meta.FileMetaInfo>();
                using (imageanalyzer.dotnet.core.interfaces.IAnalyzer analyzer = imageanalyzer.dotnet.core.IAnalyzerCreate.Create())
                {
                    var count = project.files_meta_info.Count;
                    var worked = 0;
                    foreach (var file_metainfo in project.files_meta_info)
                    {
                        if(!string.IsNullOrEmpty(file_metainfo.md5_image_full_name))
                        {
                            using (var md5 = MD5.Create())
                            using (var stream = File.OpenRead(file_metainfo.imagefile_full_name))
                                if(BitConverter.ToString(md5.ComputeHash(stream)) != file_metainfo.md5_image_full_name)
                                {
                                    file_metainfo.datafile_full_name = "";
                                    file_metainfo.md5_image_full_name = "";
                                    list_need_analyze.Add(file_metainfo);
                                }
                        }
                        if (!cancel.IsCancellationRequested)
                            break;
                        ++worked;
                        GetObserver().NotifyChangeProgress((100.0 * worked)/ count);

                    }
                }
                GetObserver().NotifyComplete();
                return list_need_analyze;
            }).ContinueWith( list => {
                if(list.Result.Count != 0)
                {
                    var operation = new OperationAnalyze(list.Result, cancel.Token);
                    operation.AddObserver(observer_analyze);
                    operation.Execute();
                }
            });
        }

        private meta.Project project;
        private IObserverOperation observer_analyze;
        private CancellationTokenSource cancel;
    }
}
