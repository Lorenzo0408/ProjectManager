using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ProjectManager.Model
{
    public class UserStory : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public ObservableCollection<Task> Tasks { get; set; } = new ObservableCollection<Task>();

        private bool isCompletedUS;
        public bool IsCompletedUS
        {
            get => isCompletedUS;
            set
            {
                if (isCompletedUS == value) return;
                isCompletedUS = value;
                OnPropertyChanged();
            }
        }

        public string USColor { get; set; } = "White";

        public enum StoryState
        {
            ProductLog,
            Backlog,
            Embarquee,
            AValider
        }

        public StoryState State { get; set; } = StoryState.ProductLog;


        public UserStory()
        {
            Tasks.CollectionChanged += Tasks_CollectionChanged;
        }

        private void Tasks_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var it in e.OldItems.OfType<Task>())
                    it.PropertyChanged -= Task_PropertyChanged;
            }
            if (e.NewItems != null)
            {
                foreach (var it in e.NewItems.OfType<Task>())
                    it.PropertyChanged += Task_PropertyChanged;
            }
            UpdateIsCompletedUS();
        }

        private void Task_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Task.IsCompletedTask))
                UpdateIsCompletedUS();
        }

        private void UpdateIsCompletedUS()
        {
            IsCompletedUS = Tasks.Count > 0 && Tasks.All(t => t.IsCompletedTask);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
