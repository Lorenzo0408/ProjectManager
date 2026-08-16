using ProjectManager.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace ProjectManager.ViewModel
{
    public class KanbanScrumCreatorVM
    {
        public string KanbanTitle { get; set; }
        public string KanbanLeader { get; set; }
        public string KanbanDescription { get; set; }

        public ICommand CreateKanbanCommand { get; set; }

        public List<Model.KanbanScrum> KanbanScrums { get; set; }
        private DashBoardVM dashBoard;

        public KanbanScrumCreatorVM(DashBoardVM dashboardVM)
        {
            this.dashBoard = dashboardVM;
            CreateKanbanCommand = new RelayCommand(CreateKanban);
        }

        public void CreateKanban()
        {
            Model.KanbanScrum kanbanScrum = new Model.KanbanScrum
            {
                Id = Guid.NewGuid().ToString(),
                Name = KanbanTitle,
                Description = KanbanDescription,
                Leader = KanbanLeader,
                CreationDate = DateTime.Now,
                Epics = new ObservableCollection<Model.Epic>()
            };

            this.dashBoard.KanbanScrums.Add(kanbanScrum);

            foreach (var window in System.Windows.Application.Current.Windows)
            {
                if (window is KanbanScrumCreatorView kanbanScrumCreator)
                {
                    kanbanScrumCreator.Close();
                }
            }
        }
    }
}
