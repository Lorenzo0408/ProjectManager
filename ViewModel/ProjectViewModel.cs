using ProjectManager.Model;
using ProjectManager.Services;
using ProjectManager.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Task = ProjectManager.Model.Task;

namespace ProjectManager.ViewModel
{
    public class ProjectViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Project CurrentProject { get; }
        public ObservableCollection<Epic> Epics { get; }
        private Epic selectedEpic;
        public Epic SelectedEpic
        {
            get => selectedEpic;
            set
            {
                selectedEpic = value;
                OnPropertyChanged();
            }
        }

        private UserStory selectedUS;
        public UserStory SelectedUS
        {
            get => selectedUS;
            set
            {
                selectedUS = value;
                OnPropertyChanged();
            }
        }

        private Task selectedTask;
        public Task SelectedTask
        {
            get => selectedTask;
            set
            {
                selectedTask = value;
                OnPropertyChanged();
            }
        }

        public int TotalTasks
        {
            get
            {
                return CurrentProject.Epics.SelectMany(e => e.Stories).SelectMany(us => us.Tasks).Count();
            }
        }

        public int CompletedTasks
        {
            get
            {
                return CurrentProject.Epics.SelectMany(e => e.Stories).SelectMany(us => us.Tasks).Count(t => t.IsCompletedTask);
            }
        }

        public double ProgressPercentage
        {
            get
            {
                if (TotalTasks == 0)
                    return 0;

                return (double)CompletedTasks / TotalTasks * 100.0;
            }
        }


        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ICommand CreateEpicCommand { get; }
        public ICommand CreateUSCommand { get; }
        public ICommand CreateTaskCommand { get; }

        public ICommand SelectEpicCommand { get; }
        public ICommand SelectUSCommand { get; }
        public ICommand SelectTaskCommand { get; }

        public ICommand CompleteTaskCommand { get; }
        public ICommand CompleteUSCommand { get; }
        public ICommand CompleteEpicCommand { get; }

        public ICommand DeleteEpicCommand { get; }
        public ICommand DeleteUSCommand { get; }
        public ICommand DeleteTaskCommand { get; }

        public ProjectViewModel(Project project)
        {
            CurrentProject = project;
            Epics = new ObservableCollection<Epic>(project.Epics);

            CreateEpicCommand = new RelayCommand(CreateEpic);
            CreateUSCommand = new RelayCommand(CreateUS);
            CreateTaskCommand = new RelayCommand(CreateTask);

            SelectEpicCommand = new RelayCommand<Epic>(epic => { SelectedEpic = epic; SelectedUS = null; SelectedTask = null; });
            SelectUSCommand = new RelayCommand<UserStory>(US => { SelectedUS = US; SelectedEpic = Epics.FirstOrDefault(e => e.Stories.Contains(US)); SelectedTask = null; });
            SelectTaskCommand = new RelayCommand<Task>(task => { SelectedTask = task; SelectedUS = Epics.SelectMany(e => e.Stories).FirstOrDefault(us => us.Tasks.Contains(task)); SelectedEpic = Epics.FirstOrDefault(e => e.Stories.Contains(SelectedUS)); });

            CompleteTaskCommand = new RelayCommand<Task>(CompleteTask);
            CompleteUSCommand = new RelayCommand<UserStory>(CompleteUS);
            CompleteEpicCommand = new RelayCommand<Epic>(CompleteEpic);

            DeleteEpicCommand = new RelayCommand<Epic>(DeleteEpic);
            DeleteUSCommand = new RelayCommand<UserStory>(DeleteUS);
            DeleteTaskCommand = new RelayCommand<Task>(DeleteTask);
        }

        public void CreateEpic()
        {
            var vm = new EpicCreatorVM(this);
            var view = new EpicsCreatorView(vm);
            view.Show();
        }

        public void AddEpic(Epic e)
        {
            Epics.Add(e);
            CurrentProject.Epics.Add(e);
        }

        public void OpenEpicCreator()
        {
            var vm = new EpicCreatorVM(this);
            var view = new EpicsCreatorView(vm);
            view.ShowDialog();
        }

        public void RemoveEpic(Epic e)
        {
            Epics.Remove(e);
            CurrentProject.Epics.Remove(e);
        }

        public void CreateUS()
        {
            var vm = new UserStoryCreatorVM(this);
            var view = new UserStoriesCreatorView(vm);
            view.ShowDialog();
        }

        public void AddUS(UserStory us)
        {
            SelectedEpic.Stories.Add(us);
            OnPropertyChanged(nameof(SelectedEpic));
        }

        public void RemoveUS(UserStory us)
        {
            SelectedEpic.Stories.Remove(us);
        }

        public void CreateTask()
        {
            var vm = new TaskCreatorVM(this);
            var view = new TaskCreatorView(vm);
            view.ShowDialog();
        }

        public void AddTask(Task task)
        {
            SelectedUS.Tasks.Add(task);
            OnPropertyChanged(nameof(TotalTasks));
            OnPropertyChanged(nameof(CompletedTasks));
            OnPropertyChanged(nameof(ProgressPercentage));
            DashBoardVM.Instance.RefreshDashboardItems();
            CurrentProject.NotifyProgressChanged();
            DashBoardVM.Instance.OnPropertyChanged(nameof(DashBoardVM.Projects));

        }

        public void RemoveTask(Task task)
        {
            SelectedUS.Tasks.Remove(task);
            OnPropertyChanged(nameof(TotalTasks));
            OnPropertyChanged(nameof(CompletedTasks));
            OnPropertyChanged(nameof(ProgressPercentage));
            DashBoardVM.Instance.RefreshDashboardItems();
            CurrentProject.NotifyProgressChanged();
            DashBoardVM.Instance.OnPropertyChanged(nameof(DashBoardVM.Projects));


        }

        public void CompleteTask(Task task)
        {
            task.IsCompletedTask = true;

            OnPropertyChanged(nameof(TotalTasks));
            OnPropertyChanged(nameof(CompletedTasks));
            OnPropertyChanged(nameof(ProgressPercentage));
            DashBoardVM.Instance.RefreshDashboardItems();
            CurrentProject.NotifyProgressChanged();
            DashBoardVM.Instance.OnPropertyChanged(nameof(DashBoardVM.Projects));



            var parentUS = Epics.SelectMany(e => e.Stories)
                    .FirstOrDefault(us => us.Tasks.Contains(task));

            if (parentUS == null)
                return;

            if (parentUS.Tasks.All(t => t.IsCompletedTask))
            {
                parentUS.IsCompletedUS = true;

                // Notifier WPF que l'US a changé
                parentUS.OnPropertyChanged(nameof(UserStory.IsCompletedUS));

                // Notifier WPF que l'Epic a changé (pour rafraîchir la navigation)
                OnPropertyChanged(nameof(SelectedEpic));

            }
        }


        public void UncompleteTask(Task task)
        {
            task.IsCompletedTask = false;
            SelectedUS.IsCompletedUS = false;
            OnPropertyChanged(nameof(SelectedUS));
            OnPropertyChanged(nameof(SelectedEpic));

            OnPropertyChanged(nameof(TotalTasks));
            OnPropertyChanged(nameof(CompletedTasks));
            OnPropertyChanged(nameof(ProgressPercentage));
            DashBoardVM.Instance.RefreshDashboardItems();
            CurrentProject.NotifyProgressChanged();
            DashBoardVM.Instance.OnPropertyChanged(nameof(DashBoardVM.Projects));

        }

        public void CompleteUS(UserStory us)
        {
            if (us.Tasks.All(t => t.IsCompletedTask == true))
            {
                us.IsCompletedUS = true;
                OnPropertyChanged(nameof(SelectedEpic));
            }

        }

        public void CompleteEpic(Epic e)
        {
            if (e.Stories.All(e => e.IsCompletedUS == true))
            {
                e.IsCompletedEpic = true;
                OnPropertyChanged(nameof(CurrentProject));
            }
        }

        public void DeleteEpic(Epic e)
        {
            if (e == null)
                return;

            var dialog = new DeleteConfirmationView(
                $"Do you want to delete the epic \"{e.Title}\" ?");

            dialog.ShowDialog();

            if (!dialog.Confirmed)
                return;

            Epics.Remove(e);
            CurrentProject.Epics.Remove(e);

            if (SelectedEpic == e)
            {
                SelectedEpic = null;
                SelectedUS = null;
                SelectedTask = null;

                OnPropertyChanged(nameof(TotalTasks));
                OnPropertyChanged(nameof(CompletedTasks));
                OnPropertyChanged(nameof(ProgressPercentage));

                DashBoardVM.Instance.RefreshDashboardItems();
                CurrentProject.NotifyProgressChanged();
                DashBoardVM.Instance.OnPropertyChanged(nameof(DashBoardVM.Projects));
            }
        }


        public void DeleteUS(UserStory us)
        {
            if (us == null || SelectedEpic == null)
                return;

            var dialog = new DeleteConfirmationView(
                $"Do you want to delete the user story \"{us.Title}\" ?");

            dialog.ShowDialog();

            if (!dialog.Confirmed)
                return;

            SelectedEpic.Stories.Remove(us);

            if (SelectedUS == us)
            {
                SelectedUS = null;
                SelectedTask = null;

                OnPropertyChanged(nameof(TotalTasks));
                OnPropertyChanged(nameof(CompletedTasks));
                OnPropertyChanged(nameof(ProgressPercentage));

                DashBoardVM.Instance.RefreshDashboardItems();
                CurrentProject.NotifyProgressChanged();
                DashBoardVM.Instance.OnPropertyChanged(nameof(DashBoardVM.Projects));
            }
        }


        public void DeleteTask(Task task)
        {
            if (task == null || SelectedUS == null)
                return;

            var dialog = new DeleteConfirmationView(
                $"Do you want to delete the task \"{task.Title}\" ?");

            dialog.ShowDialog();

            if (!dialog.Confirmed)
                return;

            SelectedUS.Tasks.Remove(task);

            if (SelectedTask == task)
                SelectedTask = null;

            OnPropertyChanged(nameof(TotalTasks));
            OnPropertyChanged(nameof(CompletedTasks));
            OnPropertyChanged(nameof(ProgressPercentage));

            DashBoardVM.Instance.RefreshDashboardItems();
            CurrentProject.NotifyProgressChanged();
            DashBoardVM.Instance.OnPropertyChanged(nameof(DashBoardVM.Projects));
        }

        public ICommand OpenKanbanCommand => new RelayCommand(OpenKanban);

        private void OpenKanban()
        {
            var kanban = new KanbanView();
            kanban.DataContext = new KanbanViewModel(CurrentProject);
            kanban.Show();
        }


    }
}
