using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace imageanalyzer.dotnet.ui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Add_Folder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                AnalyzeTask(Directory.GetFiles(dialog.SelectedPath));
        }

        private void Add_Files_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                AnalyzeTask(dialog.FileNames);
        }

        private void AnalyzeTask(string[] fileNames)
        {
            this.ProgressAnalyze.Value = 0;
            this.ProgressAnalyze.Maximum = fileNames.Length;
            Task.Factory.StartNew(() =>
                {
                    imageanalyzer.dotnet.core.interfaces.IAnalyzer analyzer = imageanalyzer.dotnet.core.IAnalyzerCreate.Create();
                    foreach (string file in fileNames)
                    {
                        analyzer.add_task(file, new ObserverTask(this.ProgressAnalyze));
                    }
                    analyzer.wait();
                });
        }

    }
}
