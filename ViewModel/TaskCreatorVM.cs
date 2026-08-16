using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ProjectManager.Model;
using ProjectManager.View;

namespace ProjectManager.ViewModel
{
    public class TaskCreatorVM
    {
        public ProjectViewModel ParentProject;
        public KanbanScrumVM ParentKanban;
        public UserStory TargetUS { get; }
        public ICommand CreateTaskCommand { get; }
        public string TaskTitle { get; set; }
        public string TaskDescription { get; set; }
        public int TaskImportance { get; set; }
        public int TaskTempsEstime { get; set; }
        public DateTime TaskEcheance { get; set; } = DateTime.Now;

        public string TaskOwner { get; set; } = "Unassigned";

        public List<string> AvailableColors { get; } = new()
        {
            "Red",
            "Blue",
            "Orange",
            "Purple",
            "Yellow",
            "Cyan",
            "Magenta",
            "Gray",
            "White"
        };

        public string SelectedColor { get; set; } = "White";

        public TaskCreatorVM(ProjectViewModel parent)
        {
            ParentProject = parent;
            CreateTaskCommand = new RelayCommand(CreateTask);
        }

        public TaskCreatorVM(KanbanScrumVM parent, UserStory targetUS)
        {
            ParentKanban = parent;
            TargetUS = targetUS;    
            CreateTaskCommand = new RelayCommand(CreateTask);
        }

        public void CreateTask()
        {
            Model.Task newTask = new Model.Task
            {
                Id = 0,
                Title = TaskTitle,
                Description = TaskDescription,
                Importance = TaskImportance,
                TempsEstime = TaskTempsEstime,
                Echeance = TaskEcheance,
                TaskColor = SelectedColor,
                IsCompletedTask = false,
                Owner = TaskOwner,
                State = Model.Task.TaskState.Afaire
            };

            if (ParentProject != null)
            {
                ParentProject.AddTask(newTask);
                ParentProject.SelectedTask = newTask;
                ParentProject.OnPropertyChanged(nameof(ParentProject.SelectedTask));

                foreach (var window in System.Windows.Application.Current.Windows)
                {
                    if (window is TaskCreatorView taskCreator)
                        taskCreator.Close();
                }
            }
            else if (ParentKanban !=  null)
            {
                ParentKanban.AddTask(newTask, TargetUS);
                ParentKanban.SelectedTask = newTask;

                foreach (var window in System.Windows.Application.Current.Windows)
                {
                    if (window is TaskCreatorView taskCreator)
                        taskCreator.Close();
                }
            }
        }

        public void AddTask(Model.Task task)
        {
            ParentProject.SelectedUS.Tasks.Add(task);
            ParentProject.OnPropertyChanged(nameof(ParentProject.SelectedUS));
        }
    }
}
