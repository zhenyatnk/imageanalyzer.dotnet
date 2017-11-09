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
            InitializeComponent();
            view_model = new ui.view_model.ViewModel();
            DataContext = view_model;
            Subscride(()=>{ view_model.CheckCommand.Execute(null); }, 1000*60);
        }

        protected override void OnClosed(EventArgs e)
        {
            if (view_model.Project.files_meta_info.Count != 0)
                view_model.SaveProjectCommand.Execute(null);
        }

        private void Subscride(OperationFunction function, double interval)
        {
            var timer = new System.Timers.Timer(interval);

            timer.Elapsed += (source, e) => { function(); };
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        delegate void OperationFunction();

        private view_model.ViewModel view_model;
    }
}
