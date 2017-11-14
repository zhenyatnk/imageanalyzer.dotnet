
namespace imageanalyzer.dotnet.ui.view_model
{
    public class ViewModel
        :Notifier
    {
        public ViewModel()
        {
            project = new model.meta.Project();
            operations = new ObservableCollectionDisp<OperationView>();

            AddFilesCommand = new command.AddFilesCommand(this);
            AddFolderCommand = new command.AddFolderCommand(this);
            OpenProjectCommand = new command.OpenProjectCommand(this);
            SaveProjectCommand = new command.SaveProjectCommand(this);
            CheckCommand = new command.CheckCommand(this);
        }

        public model.meta.Project Project
        {
            get
            {
                return project;
            }
            set
            {
                project = value;
                NotifyPropertyChanged("Project");
            }
        }

        public string ProjectName
        {
            get
            {
                return project_name;
            }
            set
            {
                project_name = value;
                NotifyPropertyChanged("ProjectName");
            }
        }

        public ObservableCollectionDisp<OperationView> Operations
        {
            get
            {
                return operations;
            }
            set
            {
                operations = value;
                NotifyPropertyChanged("Operations");
            }
        }

        public command.AddFilesCommand AddFilesCommand { get;set;}
        public command.AddFolderCommand AddFolderCommand { get; set; }
        public command.OpenProjectCommand OpenProjectCommand { get; set; }
        public command.SaveProjectCommand SaveProjectCommand { get; set; }
        public command.CheckCommand CheckCommand { get; set; }

        private ObservableCollectionDisp<OperationView> operations;
        private model.meta.Project project;
        private string project_name;
    }
}
