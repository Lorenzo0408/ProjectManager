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
using ProjectManager.ViewModel;

namespace ProjectManager.View
{
    /// <summary>
    /// Logique d'interaction pour UserStoriesCreatorView.xaml
    /// </summary>
    public partial class UserStoriesCreatorView : Window
    {
        public UserStoriesCreatorView(UserStoryCreatorVM viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
