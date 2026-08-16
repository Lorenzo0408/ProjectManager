using ProjectManager.Model;
using ProjectManager.Services;
using ProjectManager.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Collections.Specialized;

namespace ProjectManager.ViewModel
{
    public class DashBoardVM : INotifyPropertyChanged
    {
        private readonly Dashboard _dashboard;

        public ObservableCollection<Project> Projects { get; set; }
        public ObservableCollection<KanbanScrum> KanbanScrums { get; set; }

        // Liste fusionnée pour le Dashboard
        public ObservableCollection<object> DashboardItems { get; set; }

        public DashboardModel DashboardModel { get; set; }
        public static DashBoardVM Instance { get; private set; }

        // Commands
        public ICommand OpenProjectCommand { get; }
        public ICommand OpenProjectPageCommand { get; }
        public ICommand DeleteProjectCommand { get; }

        public ICommand SaveDashboardCommand { get; }
        public ICommand LoadDashboardCommand { get; }

        public ICommand OpenScrumKanbanCommand { get; }
        public ICommand OpenScrumKanbanPageCommand { get; }
        public ICommand DeleteScrumKanbanCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public DashBoardVM()
        {
            Instance = this;

            Projects = new ObservableCollection<Project>();
            KanbanScrums = new ObservableCollection<KanbanScrum>();
            DashboardItems = new ObservableCollection<object>();

            Projects.CollectionChanged += OnSourceCollectionChanged;
            KanbanScrums.CollectionChanged += OnSourceCollectionChanged;

            DashboardModel = new DashboardModel();

            // Commands
            OpenProjectCommand = new RelayCommand(OpenProjectCreator);
            OpenProjectPageCommand = new RelayCommand<Project>(OpenProjectPage);
            DeleteProjectCommand = new RelayCommand<Project>(DeleteProject);

            OpenScrumKanbanCommand = new RelayCommand(OpenKanbanCreator);
            OpenScrumKanbanPageCommand = new RelayCommand<KanbanScrum>(OpenKanbanScrumPage);
            DeleteScrumKanbanCommand = new RelayCommand<KanbanScrum>(DeleteScrumKanban);

            SaveDashboardCommand = new RelayCommand(SaveDashboard);
            LoadDashboardCommand = new RelayCommand(LoadDashboard);

            RefreshDashboardItems();

        }

        // -----------------------------
        // PROJECTS
        // -----------------------------

        private void OpenProjectCreator()
        {
            var view = new ProjectCreatorView(this);
            view.ShowDialog();
        }

        private void OpenProjectPage(Project project)
        {
            var view = new ProjectView(project);
            view.ShowDialog();
        }

        public void DeleteProject(Project project)
        {
            if (project == null)
                return;

            var dialog = new DeleteConfirmationView(
                $"Do you want to delete the project \"{project.Title}\" ?");

            dialog.ShowDialog();

            if (!dialog.Confirmed)
                return;

            Projects.Remove(project);
            RefreshDashboardItems();
        }

        // -----------------------------
        // SCRUM KANBAN
        // -----------------------------

        private void OpenKanbanCreator()
        {
            var view = new KanbanScrumCreatorView(this);
            view.ShowDialog();
        }


        private void OpenKanbanScrumPage(object kanban)
        {
            KanbanScrumVM vm;

            if (kanban is KanbanScrum model)
            {
                vm = new KanbanScrumVM(model);
            }
            else if (kanban is KanbanScrumVM existingVM)
            {
                vm = existingVM;
            }
            else
            {
                return; // type inconnu → on ne fait rien
            }

            var view = new KanbanScrumView(vm);
            view.ShowDialog();
        }



        private void DeleteScrumKanban(KanbanScrum kanban)
        {
            if (kanban == null)
                return;

            var dialog = new DeleteConfirmationView(
                $"Do you want to delete the Scrum Kanban \"{kanban.Name}\" ?");

            dialog.ShowDialog();

            if (!dialog.Confirmed)
                return;

            KanbanScrums.Remove(kanban);
            RefreshDashboardItems();
        }

        // -----------------------------
        // SAVE / LOAD
        // -----------------------------

        private void SaveDashboard()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Dashboard File (*.pmdb)|*.pmdb",
                DefaultExt = ".pmdb"
            };

            if (dialog.ShowDialog() == true)
            {
                DashboardModel.Projects = new List<Project>(Projects);
                DashboardModel.KanbanScrums = new List<KanbanScrum>(KanbanScrums);

                SaveLoadService.SaveDashboard(DashboardModel, dialog.FileName);
            }
        }

        private void LoadDashboard()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Dashboard File (*.pmdb)|*.pmdb",
                DefaultExt = ".pmdb"
            };

            if (dialog.ShowDialog() == true)
            {
                var loaded = SaveLoadService.LoadDashboard(dialog.FileName);

                DashboardModel = loaded;

                Projects.Clear();
                foreach (var project in loaded.Projects)
                    Projects.Add(project);

                KanbanScrums.Clear();
                foreach (var kanban in loaded.KanbanScrums)
                    KanbanScrums.Add(kanban);

                RefreshDashboardItems();
            }
        }

        // -----------------------------
        // MERGE LISTS FOR DASHBOARD
        // -----------------------------

        public void RefreshDashboardItems()
        {
            DashboardItems.Clear();

            foreach (var p in Projects)
                DashboardItems.Add(p);

            foreach (var k in KanbanScrums)
                DashboardItems.Add(k);

            OnPropertyChanged(nameof(DashboardItems));
        }

        private void OnSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            RefreshDashboardItems();
        }
    }
}