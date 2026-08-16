using ProjectManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManager.Model
{
    public class DashboardModel
    {
        public List<Project> Projects { get; set; } = new();
        public List<KanbanScrum> KanbanScrums { get; set; } = new();
    }
}
