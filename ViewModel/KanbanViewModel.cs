using ProjectManager.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ProjectManager.Model;

namespace ProjectManager.ViewModel
{
    public class KanbanViewModel
    {

        public Project Project { get; }

        public ObservableCollection<Model.Task> ToDoTasks { get; }
        public ObservableCollection<Model.Task> InProgressTasks { get; }
        public ObservableCollection<Model.Task> DoneTasks { get; }

        public KanbanViewModel(Project project)
        {
            Project = project;

            var allTasks = project.Epics
                .SelectMany(e => e.Stories)
                .SelectMany(us => us.Tasks);

            ToDoTasks = new ObservableCollection<Model.Task>(allTasks.Where(t => !t.IsCompletedTask && t.Importance <= 1));
            InProgressTasks = new ObservableCollection<Model.Task>(allTasks.Where(t => !t.IsCompletedTask && t.Importance == 2));
            DoneTasks = new ObservableCollection<Model.Task>(allTasks.Where(t => t.IsCompletedTask));
        }

        public void MoveTaskTo(Model.Task task, string column)
        {
            // Retirer des anciennes colonnes
            ToDoTasks.Remove(task);
            InProgressTasks.Remove(task);
            DoneTasks.Remove(task);

            // Ajouter dans la nouvelle colonne
            switch (column)
            {
                case "todo":
                    task.IsCompletedTask = false;
                    task.Importance = 1;
                    ToDoTasks.Add(task);
                    break;

                case "progress":
                    task.IsCompletedTask = false;
                    task.Importance = 2;
                    InProgressTasks.Add(task);
                    break;

                case "done":
                    task.IsCompletedTask = true;
                    DoneTasks.Add(task);
                    break;
            }

            // Mise à jour du modèle Project
            Project.NotifyProgressChanged();
        }

    }




}
