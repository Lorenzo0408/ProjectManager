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
    /// Logique d'interaction pour EpicsCreatorView.xaml
    /// </summary>
    public partial class EpicsCreatorView : Window
    {
        public EpicsCreatorView(EpicCreatorVM parent)
        {
            InitializeComponent();
            DataContext = parent;
        }
    }
}
