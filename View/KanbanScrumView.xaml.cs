using ProjectManager.Model;
using ProjectManager.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ProjectManager.View
{
    public partial class KanbanScrumView : Window
    {
        // Formats utilisés pour identifier le type d'objet dans le DragEventArgs
        private const string StoryFormat = "ProjectManager.UserStory";
        private const string TaskFormat = "ProjectManager.Task";

        public KanbanScrumView(KanbanScrumVM vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private KanbanScrumVM VM => DataContext as KanbanScrumVM;

        // ---------- DEMARRAGE DU DRAG ----------

        private void Story_Drag(object sender, MouseButtonEventArgs e)
        {
            // Empêche le drag si le clic vient d’un bouton
            if (e.OriginalSource is DependencyObject dep &&
                FindParent<Button>(dep) != null)
            {
                return;
            }

            var item = FindAncestorData<UserStory>(e.OriginalSource as DependencyObject);
            if (item == null) return;

            var data = new DataObject(StoryFormat, item);
            DragDrop.DoDragDrop((DependencyObject)sender, data, DragDropEffects.Move);
        }


        private void Task_Drag(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var item = FindAncestorData<Model.Task>(e.OriginalSource as DependencyObject);
            if (item == null) return;

            var data = new DataObject(TaskFormat, item);
            DragDrop.DoDragDrop((DependencyObject)sender, data, DragDropEffects.Move);
        }

        // Remonte l'arbre visuel depuis l'élément cliqué pour retrouver
        // le ListBoxItem et son DataContext (US ou Task)
        private static T FindAncestorData<T>(DependencyObject source) where T : class
        {
            var current = source;
            while (current != null && !(current is ListBoxItem))
            {
                current = VisualTreeHelper.GetParent(current);
            }

            return (current as ListBoxItem)?.DataContext as T;
        }

        // ---------- SURVOL PENDANT LE DRAG ----------

        private void Kanban_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = (e.Data.GetDataPresent(StoryFormat) || e.Data.GetDataPresent(TaskFormat))
                ? DragDropEffects.Move
                : DragDropEffects.None;
            e.Handled = true;
        }

        // ---------- DROP : COLONNES USER STORY ----------

        private void ProductLog_Drop(object sender, DragEventArgs e) =>
            HandleStoryDrop(e, UserStory.StoryState.ProductLog, VM?.ProductLog);

        private void Backlog_Drop(object sender, DragEventArgs e) =>
            HandleStoryDrop(e, UserStory.StoryState.Backlog, VM?.Backlog);

        private void Embarquee_Drop(object sender, DragEventArgs e) =>
            HandleStoryDrop(e, UserStory.StoryState.Embarquee, VM?.Embarquee);

        private void AValider_Drop(object sender, DragEventArgs e) =>
            HandleStoryDrop(e, UserStory.StoryState.AValider, VM?.AValider);

        private void HandleStoryDrop(DragEventArgs e, UserStory.StoryState targetState,
            System.Collections.ObjectModel.ObservableCollection<UserStory> targetCollection)
        {
            if (VM == null || targetCollection == null) return;
            if (!e.Data.GetDataPresent(StoryFormat)) return;

            var story = (UserStory)e.Data.GetData(StoryFormat);
            if (story == null) return;

            // Retire l'item de sa colonne actuelle (quelle qu'elle soit)
            VM.ProductLog.Remove(story);
            VM.Backlog.Remove(story);
            VM.Embarquee.Remove(story);
            VM.AValider.Remove(story);

            story.State = targetState;
            targetCollection.Add(story);

            e.Handled = true;
        }

        // ---------- DROP : COLONNES TASK ----------

        private void Afaire_Drop(object sender, DragEventArgs e) =>
            HandleTaskDrop(e, Model.Task.TaskState.Afaire, VM?.Afaire);

        private void EnCours_Drop(object sender, DragEventArgs e) =>
            HandleTaskDrop(e, Model.Task.TaskState.EnCours, VM?.EnCours);

        private void Termine_Drop(object sender, DragEventArgs e) =>
            HandleTaskDrop(e, Model.Task.TaskState.Termine, VM?.Termine);

        private void HandleTaskDrop(DragEventArgs e, Model.Task.TaskState targetState,
            System.Collections.ObjectModel.ObservableCollection<Model.Task> targetCollection)
        {
            if (VM == null || targetCollection == null) return;
            if (!e.Data.GetDataPresent(TaskFormat)) return;

            var task = (Model.Task)e.Data.GetData(TaskFormat);
            if (task == null) return;

            VM.Afaire.Remove(task);
            VM.EnCours.Remove(task);
            VM.Termine.Remove(task);

            task.State = targetState;
            task.IsCompletedTask = targetState == Model.Task.TaskState.Termine;
            targetCollection.Add(task);

            e.Handled = true;
        }

        private void Button_Click_StopDrag(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);

            while (parent != null)
            {
                if (parent is T typedParent)
                    return typedParent;

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }

    }
}
