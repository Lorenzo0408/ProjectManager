using ProjectManager.ViewModel;
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

namespace ProjectManager.View
{
    /// <summary>
    /// Logique d'interaction pour KanbanScrumCreatorView.xaml
    /// </summary>
    public partial class KanbanScrumCreatorView : Window
    {
        public KanbanScrumCreatorView(DashBoardVM parentVM)
        {
            InitializeComponent();
            DataContext = new KanbanScrumCreatorVM(parentVM);
        }
    }
}
