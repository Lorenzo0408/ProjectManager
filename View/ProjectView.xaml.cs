using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ProjectManager.Model;
using ProjectManager.ViewModel;

namespace ProjectManager.View
{
    /// <summary>
    /// Logique d'interaction pour Project.xaml
    /// </summary>
    public partial class ProjectView : Window
    {
        public Button TaskButton;

        public Project project;

        public ProjectView(Project project)
        {
            InitializeComponent();
            DataContext = new ProjectViewModel(project);
        }

        public void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TaskCompleted_Checked(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ProjectViewModel;
            if (vm?.SelectedTask != null)
                vm.CompleteTask(vm.SelectedTask);
        }

        private void TaskCompleted_Unchecked(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ProjectViewModel;
            if (vm?.SelectedTask != null)
                vm.UncompleteTask(vm.SelectedTask);
        }

    }
}
