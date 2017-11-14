using Newtonsoft.Json;
using System.IO;

namespace imageanalyzer.dotnet.model.utilities
{
    public static class ProjectHelper
    {
        public static void SaveToFile(meta.Project project, string filename)
        {
            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter stream = new StreamWriter(filename))
            using (JsonWriter writer = new JsonTextWriter(stream))
            {
                serializer.Serialize(writer, project);
            }
        }

        public static meta.Project LoadFromFile(string filename)
        {
            var project = new meta.Project();
            var serializer = new JsonSerializer();
            using (var stream = new StreamReader(filename))
            using (var reader = new JsonTextReader(stream))
            {
                project = serializer.Deserialize<meta.Project>(reader);
            }
            return project;
        }
    }


}
