using ProjectManager.Model;
using ProjectManager.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace ProjectManager.ViewModel
{
    public class UserStoryCreatorVM
    {
        private ProjectViewModel ParentProjectVM;
        public Epic TargetEpic { get; }
        public KanbanScrumVM ParentKanbanVM { get; }

        public string UserStoryTitle { get; set; }
        public string UserStoryDescription { get; set; }

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

        public ICommand CreateUserStoryCommand { get; }

        public UserStoryCreatorVM(ProjectViewModel parentProject)
        {
            ParentProjectVM = parentProject;
            CreateUserStoryCommand = new RelayCommand(CreateUserStory);
        }

        public UserStoryCreatorVM(KanbanScrumVM parentKanban, Epic targetEpic)
        {
            ParentKanbanVM = parentKanban;
            TargetEpic = targetEpic;
            CreateUserStoryCommand = new RelayCommand(CreateUserStory);
        }

        public void CreateUserStory()
        {
            UserStory newUserStory = new UserStory
            {
                Id = 0,
                Title = UserStoryTitle,
                Description = UserStoryDescription,
                USColor = SelectedColor,
                IsCompletedUS = false,
                State = UserStory.StoryState.ProductLog,
                Tasks = new ObservableCollection<Model.Task>()
            };

            if(ParentProjectVM != null)
            {
                ParentProjectVM.AddUS(newUserStory);

                foreach (var window in System.Windows.Application.Current.Windows)
                {
                    if (window is UserStoriesCreatorView userStoriesCreatorView)
                    {
                        userStoriesCreatorView.Close();
                    }
                }
            }
            else if (ParentKanbanVM != null)
            {
                ParentKanbanVM.AddUS(newUserStory, TargetEpic);
                
                foreach (var window in System.Windows.Application.Current.Windows)
                {
                    if (window is UserStoriesCreatorView userStoriesCreatorView)
                    {
                        userStoriesCreatorView.Close();
                    }
                }
            }
        }
    }

}
