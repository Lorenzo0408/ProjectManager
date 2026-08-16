using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ProjectManager.Model
{
    public class Task : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Importance { get; set; }
        public int TempsEstime { get; set; }
        private DateTime echeance;
        public DateTime Echeance
        {
            get => echeance;
            set
            {
                if (echeance == value) return;
                echeance = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(isLate));
            }
        }


        private bool isCompletedTask;
        public bool IsCompletedTask
        {
            get => isCompletedTask;
            set
            {
                if (isCompletedTask == value) return;
                isCompletedTask = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(isLate));
            }

        }

        public string TaskColor { get; set; } = "White";

        public string Owner { get; set; } = "Unassigned";

        public bool isLate => DateTime.Now > Echeance && !IsCompletedTask;

        public event PropertyChangedEventHandler? PropertyChanged;

        public enum TaskState
        {
            Afaire,
            EnCours,
            Termine
        }

        public TaskState State { get; set; } = TaskState.Afaire;


        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
