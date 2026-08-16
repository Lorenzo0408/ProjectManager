using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManager.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Project> Projects { get; set; } = new List<Project>();
    }
}
