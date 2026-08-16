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
    /// Logique d'interaction pour TaskCreatorView.xaml
    /// </summary>
    public partial class TaskCreatorView : Window
    {
        public TaskCreatorView(TaskCreatorVM viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void NumberOnly(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LabelPriority.Content = $"Priority: {((int)SliderVal.Value)}";
        }
    }
}
