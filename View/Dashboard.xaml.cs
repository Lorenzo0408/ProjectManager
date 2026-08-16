using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProjectManager.View;
using ProjectManager.ViewModel;


namespace ProjectManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        private DashBoardVM parentVM;

        public Dashboard()
        {
            InitializeComponent();
            DataContext = new DashBoardVM();
            WindowState = WindowState.Maximized;
        }

        public void DeleteProject(Model.Project project)
        {
            parentVM.DeleteProject(project);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            var dialog = new ExitConfirmationView();
            dialog.ShowDialog();

            if (!dialog.Confirmed)
            {
                var vm = DataContext as DashBoardVM;
                vm?.SaveDashboardCommand.Execute(null);
            }

            

            base.OnClosing(e);
        }


    }
}