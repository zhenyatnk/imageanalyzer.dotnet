using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.IO;

namespace imageanalyzer.dotnet.ui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        public MainWindow()
        {
            project = new model.Project();
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            if(string.IsNullOrEmpty(project_filename))
            {
                SaveFileDialog dialog = new SaveFileDialog();
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    project_filename = dialog.FileName;
            }

            if (!string.IsNullOrEmpty(project_filename))
                Utilities.SaveProjectToFile(project, project_filename);
        }

        private void AddFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var files_metainfo = new List<model.FileMetaInfo>();
                var fileNames = Directory.GetFiles(dialog.SelectedPath);
                foreach (string file in fileNames)
                {
                    var file_metadata = new model.FileMetaInfo();
                    file_metadata.imagefile_full_name = file;
                    project.files_meta_info.Add(file_metadata);
                    files_metainfo.Add(file_metadata);
                }
                AnalyzeTask(files_metainfo);
            }
        }

        private void AddFiles_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var files_metainfo = new List<model.FileMetaInfo>();
                foreach (string file in dialog.FileNames)
                {
                    var file_metadata = new model.FileMetaInfo();
                    file_metadata.imagefile_full_name = file;
                    project.files_meta_info.Add(file_metadata);
                    files_metainfo.Add(file_metadata);
                }
                AnalyzeTask(files_metainfo);
            }
        }


        private void OpenProject_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Project file|*.prj";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                project_filename = dialog.FileName;
                project = Utilities.LoadProjectFromFile(project_filename);
                AnalyzeTask(Utilities.GetNonAnalyzed(project));
            }
        }

        private void AnalyzeTask(ICollection<model.FileMetaInfo> files_metainfo)
        {
            this.ProgressAnalyze.Value = 0;
            this.ProgressAnalyze.Maximum = files_metainfo.Count;

            Task.Factory.StartNew(() =>
                {
                    imageanalyzer.dotnet.core.interfaces.IAnalyzer analyzer = imageanalyzer.dotnet.core.IAnalyzerCreate.Create();
                    foreach (var file_metainfo in files_metainfo)
                    {
                        analyzer.add_task(file_metainfo.imagefile_full_name, new List<imageanalyzer.dotnet.core.interfaces.IObserverTask>
                                                                                    { new ObserverTaskMeta(file_metainfo),
                                                                                      new ObserverTaskProgress(this.ProgressAnalyze)});
                    }
                    analyzer.wait();
                });
        }
        
        private model.Project project;
        private string project_filename;

    }
}
