using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ProjectManager.Model;
using ProjectManager.View;

namespace ProjectManager.ViewModel
{
    public class ProjectCreatorViewModel
    {
        public string ProjectTitle { get; set; } = string.Empty;
        public string ProjectDesc {  get; set; } = string.Empty;
        public string ProjectLeader { get; set; } = string.Empty;

        public ICommand CreateProjectCommand { get; }
        public List<Project> Projects { get; set; }

        private DashBoardVM dashBoard;

        public ProjectCreatorViewModel(DashBoardVM parent)
        {
            this.dashBoard = parent;
            CreateProjectCommand = new RelayCommand(CreateProject);
        }

        public void CreateProject()
        {
            Model.Project project = new Model.Project
            {
                Id = 0,
                Title = ProjectTitle,
                Description = ProjectDesc,
                CreationDate = DateTime.Now,
                Epics = new List<Epic>(),
                Leader = ProjectLeader
            };

            this.dashBoard.Projects.Add(project);

            foreach (var window in System.Windows.Application.Current.Windows)
            {
                if (window is ProjectCreatorView projectCreator)
                {
                    projectCreator.Close();
                }
            }
        }
    }
}
