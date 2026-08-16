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
    /// Logique d'interaction pour KanbanView.xaml
    /// </summary>
    public partial class KanbanView : Window
    {
        public KanbanView()
        {
            InitializeComponent();
        }

        public void BackButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Task_Drag(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            if (border?.DataContext is Model.Task task)
            {
                DragDrop.DoDragDrop(border, task, DragDropEffects.Move);
                System.Diagnostics.Debug.WriteLine("DRAG OK");
                e.Handled = true;

            }
        }

        private void ToDo_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(Model.Task)) is Model.Task task)
            {
                (DataContext as KanbanViewModel)?.MoveTaskTo(task, "todo");
                System.Diagnostics.Debug.WriteLine("DROP OK");

            }
        }

        private void InProgress_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(Model.Task)) is Model.Task task)
            {
                (DataContext as KanbanViewModel)?.MoveTaskTo(task, "progress");
                System.Diagnostics.Debug.WriteLine("DROP OK");

            }
        }

        private void Done_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(Model.Task)) is Model.Task task)
            {
                (DataContext as KanbanViewModel)?.MoveTaskTo(task, "done");
                System.Diagnostics.Debug.WriteLine("DROP OK");

            }
        }

        private void Kanban_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
            e.Handled = true;
        }


    }
}
