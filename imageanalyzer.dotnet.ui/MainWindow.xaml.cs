using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
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
            cancel_token = new CancellationTokenSource();
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            if (project.files_meta_info.Count != 0)
                project_filename = Save(project_filename);
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
            if(project.files_meta_info.Count != 0)
                project_filename = Save(project_filename);

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Project file|*.prj";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                cancel_token.Cancel();
                cancel_token = new CancellationTokenSource();
                project_filename = dialog.FileName;
                project = Utilities.LoadProjectFromFile(project_filename);
                var Task = AnalyzeTask(Utilities.GetNonAnalyzed(project));
            }
        }

        private Task AnalyzeTask(ICollection<model.FileMetaInfo> files_metainfo)
        {
            this.ProgressAnalyze.Value = 0;
            this.ProgressAnalyze.Maximum = files_metainfo.Count;
            var token = cancel_token.Token;

            return Task.Factory.StartNew(() =>
                {
                    using (imageanalyzer.dotnet.core.interfaces.IAnalyzer analyzer = imageanalyzer.dotnet.core.IAnalyzerCreate.Create())
                    {
                        foreach (var file_metainfo in files_metainfo)
                        {
                            analyzer.add_task(file_metainfo.imagefile_full_name, new List<imageanalyzer.dotnet.core.interfaces.IObserverTask>
                                                                                    { new ObserverTaskMeta(file_metainfo),
                                                                                      new ObserverTaskProgress(this.ProgressAnalyze)});
                        }
                        while (!token.IsCancellationRequested && !analyzer.complete())
                            Thread.Sleep(500);
                    }
                }, token);
        }
        private string Save(string project_name)
        {
            cancel_token.Cancel();
            cancel_token = new CancellationTokenSource();
            if (string.IsNullOrEmpty(project_name))
            {
                SaveFileDialog dialog = new SaveFileDialog();
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    project_name = dialog.FileName;
            }

            if (!string.IsNullOrEmpty(project_name))
                Utilities.SaveProjectToFile(project, project_name);
            return project_name;
        }
        
        private model.Project project;
        private string project_filename;
        private CancellationTokenSource cancel_token;
    }
}
