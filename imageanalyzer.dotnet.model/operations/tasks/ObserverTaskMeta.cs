﻿using System;
using System.IO;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace imageanalyzer.dotnet.model.operations.tasks
{
    class ObserverTaskMeta 
        : imageanalyzer.dotnet.core.interfaces.IObserverTask
    {
        public ObserverTaskMeta(meta.FileMetaInfo metainfo)
        {
            m_metainfo = metainfo;
        }

        public void HandleComplete()
        {
            Task.Factory.StartNew(() =>
            {
                m_metainfo.datafile_full_name = m_metainfo.imagefile_full_name + ".data";
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(m_metainfo.imagefile_full_name))
                    {
                        m_metainfo.md5_image_full_name = BitConverter.ToString(md5.ComputeHash(stream));
                    }
                }
            });     
        }
        public void HandleStart()
        {}

        public void HandleError(string aMessage, int aErrorCode)
        {}

        private meta.FileMetaInfo m_metainfo;
    }
}
