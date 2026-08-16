using System;
using System.Collections.Generic;
using System.Text;
using ProjectManager.View;
using ProjectManager.Model;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace ProjectManager.ViewModel
{
    public class EpicCreatorVM
    {
        private ProjectViewModel ParentProjectVM;
        public KanbanScrumVM ParentKanbanVM { get; }


        public string EpicTitle { get; set; }
        public string EpicDescription { get; set; }

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

        public ICommand CreateEpicCommand { get; }

        public EpicCreatorVM(ProjectViewModel parent)
        {
            this.ParentProjectVM = parent;
            CreateEpicCommand = new RelayCommand(CreateEpic);
        }

        public EpicCreatorVM(KanbanScrumVM parent)
        {
            ParentKanbanVM = parent;
            CreateEpicCommand = new RelayCommand(CreateEpic);
        }


        public void CreateEpic()
        {
            Epic newEpic = new Epic
            {
                Id = 0,
                Title = EpicTitle,
                Description = EpicDescription,
                EpicColor = SelectedColor,
                IsCompletedEpic = false
            };

            if (ParentProjectVM != null)
            {
                ParentProjectVM.AddEpic(newEpic);

                foreach (var window in System.Windows.Application.Current.Windows)
                {
                    if (window is EpicsCreatorView epicCreator)
                    {
                        epicCreator.Close();
                    }
                }
            }
            else if (ParentKanbanVM != null)
            {
                ParentKanbanVM.AddEpic(newEpic);

                foreach (var window in System.Windows.Application.Current.Windows)
                {
                    if (window is EpicsCreatorView epicCreator)
                    {
                        epicCreator.Close();
                    }
                }
            }
        }
    }
}
