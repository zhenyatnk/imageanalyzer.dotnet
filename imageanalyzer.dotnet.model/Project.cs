﻿using System.Collections.Generic;

namespace imageanalyzer.dotnet.model
{
    public class Project
    {
        public Project()
        {
            files_meta_info = new List<FileMetaInfo>();
        }

        public List<FileMetaInfo> files_meta_info;
    }
}
