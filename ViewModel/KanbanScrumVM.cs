using ProjectManager.Model;
using ProjectManager.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using static ProjectManager.Model.UserStory;

namespace ProjectManager.ViewModel
{
    public class KanbanScrumVM : INotifyPropertyChanged
    {
        public KanbanScrum CurrentKanban { get; }

        private Epic _selectedEpic;
        public Epic SelectedEpic
        {
            get => _selectedEpic;
            set
            {
                _selectedEpic = value;
                OnPropertyChanged(nameof(SelectedEpic));
                FilterUserStories();
            }
        }

        private UserStory _selectedUS;
        public UserStory SelectedUS
        {
            get => _selectedUS;
            set
            {
                _selectedUS = value;
                OnPropertyChanged(nameof(SelectedUS));
            }
        }

        private Model.Task _selectedTask;
        public Model.Task SelectedTask
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                OnPropertyChanged(nameof(SelectedTask));
            }
        }


        // Colonnes US
        private ObservableCollection<UserStory> _productLog;
        public ObservableCollection<UserStory> ProductLog
        {
            get => _productLog;
            set
            {
                _productLog = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<UserStory> _backlog;
        public ObservableCollection<UserStory> Backlog
        {
            get => _backlog;
            set
            {
                _backlog = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<UserStory> _embarquee;
        public ObservableCollection<UserStory> Embarquee
        {
            get => _embarquee;
            set
            {
                _embarquee = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<UserStory> _aValider;
        public ObservableCollection<UserStory> AValider
        {
            get => _aValider;
            set
            {
                _aValider = value;
                OnPropertyChanged();
            }
        }

        // Colonnes Tasks
        private ObservableCollection<Model.Task> _afaire;
        public ObservableCollection<Model.Task> Afaire
        {
            get => _afaire;
            set
            {
                _afaire = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Model.Task> _enCours;
        public ObservableCollection<Model.Task> EnCours
        {
            get => _enCours;
            set
            {
                _enCours = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Model.Task> _termine;
        public ObservableCollection<Model.Task> Termine
        {
            get => _termine;
            set
            {
                _termine = value;
                OnPropertyChanged();
            }
        }

        // Commandes de création
        public ICommand CreateEpicCommand { get; }
        public ICommand CreateUserStoryCommand { get; }
        public ICommand CreateTaskCommand { get; }

        public ICommand SelectEpicCommand { get; }
        public ICommand SelectUSCommand { get; }
        public ICommand SelectTaskCommand { get; }

        public KanbanScrumVM(KanbanScrum kanban)
        {
            CurrentKanban = kanban;

            ProductLog = new ObservableCollection<UserStory>();
            Backlog = new ObservableCollection<UserStory>();
            Embarquee = new ObservableCollection<UserStory>();
            AValider = new ObservableCollection<UserStory>();

            Afaire = new ObservableCollection<Model.Task>();
            EnCours = new ObservableCollection<Model.Task>();
            Termine = new ObservableCollection<Model.Task >();

            LoadColumns();

            CreateEpicCommand = new RelayCommand(CreateEpic);
            CreateUserStoryCommand = new RelayCommand<Epic>(CreateUserStory);
            CreateTaskCommand = new RelayCommand<UserStory>(CreateTask);

            SelectEpicCommand = new RelayCommand<Epic>(SelectEpic);
            SelectUSCommand = new RelayCommand<UserStory>(SelectUS);
            SelectTaskCommand = new RelayCommand<Model.Task>(SelectTask);
        }

        private void LoadColumns()
        {
            foreach (var epic in CurrentKanban.Epics)
            {
                foreach (var story in epic.Stories)
                {
                    switch (story.State)
                    {
                        case UserStory.StoryState.ProductLog: ProductLog.Add(story); break;
                        case UserStory.StoryState.Backlog: Backlog.Add(story); break;
                        case UserStory.StoryState.Embarquee: Embarquee.Add(story); break;
                        case UserStory.StoryState.AValider: AValider.Add(story); break;
                    }

                    foreach (var task in story.Tasks)
                    {
                        switch (task.State)
                        {
                            case Model.Task.TaskState.Afaire: Afaire.Add(task); break;
                            case Model.Task.TaskState.EnCours: EnCours.Add(task); break;
                            case Model.Task.TaskState.Termine: Termine.Add(task); break;
                        }
                    }
                }
            }
        }

        private void CreateEpic()
        {
            var vm = new EpicCreatorVM(this);
            var view = new EpicsCreatorView(vm);

            bool? result = view.ShowDialog();

            if (result == true)
            {
                RefreshColumns();
            }
        }

        public void AddEpic(Epic newEpic)
        {
            CurrentKanban.Epics.Add(newEpic);
            RefreshColumns();
        }

        public void AddUS(UserStory userStory, Epic targetEpic)
        {
            targetEpic.Stories.Add(userStory);       // <- source utilisée par LoadColumns()
            CurrentKanban.UserStories.Add(userStory); // <- garde les deux collections synchronisées
            RefreshColumns();
        }

        public void AddTask(Model.Task task, UserStory targetStory)
        {
            targetStory.Tasks.Add(task);      // <- source utilisée par LoadColumns()
            CurrentKanban.Task.Add(task);     // <- garde les deux collections synchronisées
            RefreshColumns();
        }

        private void CreateUserStory(Epic epic)
        {
            var vm = new UserStoryCreatorVM(this, epic);
            var view = new UserStoriesCreatorView(vm);
            bool? result = view.ShowDialog();
            if (result == true)
            {
                RefreshColumns();
            }   
        }

        private void CreateTask(UserStory story)
        {
            var vm = new TaskCreatorVM(this, story);
            var view = new TaskCreatorView(vm);
            bool? result = view.ShowDialog();
            if (result == true)
            {
                RefreshColumns();
            }
        }

        public void RefreshColumns()
        {
            ProductLog.Clear();
            Backlog.Clear();
            Embarquee.Clear();
            AValider.Clear();

            Afaire.Clear();
            EnCours.Clear();
            Termine.Clear();

            LoadColumns();
        }

        private void SelectEpic(Epic epic)
        {
            SelectedEpic = epic;
        }

        private void SelectUS(UserStory us)
        {
            SelectedUS = us;
        }

        private void SelectTask(Model.Task task)
        {
            SelectedTask = task;
        }

        private void FilterUserStories()
        {
            if (SelectedEpic == null)
            {
                // US
                ProductLog = new ObservableCollection<UserStory>(
                    CurrentKanban.UserStories.Where(us => us.State == StoryState.ProductLog));

                Backlog = new ObservableCollection<UserStory>(
                    CurrentKanban.UserStories.Where(us => us.State == StoryState.Backlog));

                Embarquee = new ObservableCollection<UserStory>(
                    CurrentKanban.UserStories.Where(us => us.State == StoryState.Embarquee));

                AValider = new ObservableCollection<UserStory>(
                    CurrentKanban.UserStories.Where(us => us.State == StoryState.AValider));

                // TASKS
                Afaire = new ObservableCollection<Model.Task>(
                    CurrentKanban.Task.Where(t => t.State == Model.Task.TaskState.Afaire));

                EnCours = new ObservableCollection<Model.Task>(
                    CurrentKanban.Task.Where(t => t.State == Model.Task.TaskState.EnCours));

                Termine = new ObservableCollection<Model.Task>(
                    CurrentKanban.Task.Where(t => t.State == Model.Task.TaskState.Termine));

                return;
            }

            // US filtrées
            ProductLog = new ObservableCollection<UserStory>(
                SelectedEpic.Stories.Where(us => us.State == StoryState.ProductLog));

            Backlog = new ObservableCollection<UserStory>(
                SelectedEpic.Stories.Where(us => us.State == StoryState.Backlog));

            Embarquee = new ObservableCollection<UserStory>(
                SelectedEpic.Stories.Where(us => us.State == StoryState.Embarquee));

            AValider = new ObservableCollection<UserStory>(
                SelectedEpic.Stories.Where(us => us.State == StoryState.AValider));

            // TASKS filtrées
            var filteredTasks = SelectedEpic.Stories.SelectMany(us => us.Tasks);

            Afaire = new ObservableCollection<Model.Task>(
                filteredTasks.Where(t => t.State == Model.Task.TaskState.Afaire));

            EnCours = new ObservableCollection<Model.Task>(
                filteredTasks.Where(t => t.State == Model.Task.TaskState.EnCours));

            Termine = new ObservableCollection<Model.Task>(
                filteredTasks.Where(t => t.State == Model.Task.TaskState.Termine));
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



    }

}
