using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Text;

namespace imageanalyzer.dotnet.ui
{
    public static class Utilities
    {
        public static List<model.FileMetaInfo> GetNonAnalyzed(model.Project project)
        {
            List<model.FileMetaInfo> files_analyzing = new List<model.FileMetaInfo>();
            foreach (var file in project.files_meta_info)
            {
                if (string.IsNullOrEmpty(file.datafile_full_name))
                    files_analyzing.Add(file);
            }
            return files_analyzing;
        }

        public static void SaveProjectToFile(model.Project project, string filename)
        {
            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter stream = new StreamWriter(filename))
            using (JsonWriter writer = new JsonTextWriter(stream))
            {
                serializer.Serialize(writer, project);
            }
        }

        public static model.Project LoadProjectFromFile(string filename)
        {
            var project = new model.Project();
            var serializer = new JsonSerializer();
            using (var stream = new StreamReader(filename))
            using (var reader = new JsonTextReader(stream))
            {
                project = serializer.Deserialize<model.Project>(reader);
            }
            return project;
        }
    }
}
