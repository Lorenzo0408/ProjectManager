using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ProjectManager.Model
{
    public class Epic : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        private bool isCompletedEpic;

        public string EpicColor { get; set; } = "White";

        public bool IsCompletedEpic
        {
            get => isCompletedEpic;

            set
            {
                if (isCompletedEpic == value) return;
                isCompletedEpic = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<UserStory> Stories { get; set; } = new ObservableCollection<UserStory>();

        public Epic()
        {
            Stories.CollectionChanged += Stories_CollectionChanged;
        }

        private void Stories_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var it in e.OldItems.OfType<UserStory>())
                    it.PropertyChanged -= UserStory_PropertyChanged;
            }
            if (e.NewItems != null)
            {
                foreach (var it in e.NewItems.OfType<UserStory>())
                    it.PropertyChanged += UserStory_PropertyChanged;
            }
            UpdateIsCompletedEpic();
        }

        private void UserStory_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(UserStory.IsCompletedUS))
                UpdateIsCompletedEpic();
        }

        private void UpdateIsCompletedEpic()
        {
            IsCompletedEpic = Stories.Count > 0 && Stories.All(us => us.IsCompletedUS);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
