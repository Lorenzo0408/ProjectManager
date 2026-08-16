using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ProjectManager.Model
{
    public class KanbanScrum
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Leader { get; set; }
        public DateTime CreationDate { get; set; }
        public ObservableCollection<Epic> Epics { get; set; } = new ObservableCollection<Epic>();

        public ObservableCollection<UserStory> UserStories
        {
            get
            {
                return new ObservableCollection<UserStory>(
                    Epics.SelectMany(epic => epic.Stories)
                );
            }
        }

        public ObservableCollection<Model.Task> Task
        {
            get
            {
                return new ObservableCollection<Model.Task>(
                    UserStories.SelectMany(us => us.Tasks)
                );
            }
        }
            

    }
}
