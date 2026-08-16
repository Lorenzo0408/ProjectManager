using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ProjectManager.Model
{
    public class Project : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public List<Epic> Epics { get; set; } = new List<Epic>();

        public string Leader { get; set; }

        public double ProgressPercentage
        {
            get
            {
                var allTasks = Epics
                    .SelectMany(e => e.Stories)
                    .SelectMany(us => us.Tasks)
                    .ToList();

                if (allTasks.Count == 0)
                    return 0;

                double completed = allTasks.Count(t => t.IsCompletedTask);
                return (completed / allTasks.Count) * 100.0;
            }
        }

        public void NotifyProgressChanged()
        {
            OnPropertyChanged(nameof(ProgressPercentage));
        }
    }
}
