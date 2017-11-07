using System;
using System.Windows.Controls;
using System.Threading;
using System.IO;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imageanalyzer.dotnet.ui
{
    class ObserverTaskProgress 
        : imageanalyzer.dotnet.core.interfaces.IObserverTask
    {
        public ObserverTaskProgress(ProgressBar progress)
        {
            m_progress = progress;
        }

        public void HandleComplete()
        {
            Task.Factory.StartNew(() =>
            {
                m_progress.Dispatcher.Invoke(new Action(delegate { m_progress.Value += 1; }));
            });
        }
        public void HandleStart()
        { }

        public void HandleError(string aMessage, int aErrorCode)
        { }

        private ProgressBar m_progress;
    }
}
