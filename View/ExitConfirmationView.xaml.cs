using System.Windows;

namespace ProjectManager.View
{
    public partial class ExitConfirmationView : Window
    {
        public bool Confirmed { get; private set; } = false;

        public ExitConfirmationView()
        {
            InitializeComponent();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Confirmed = false;
            Close();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            Confirmed = true;
            Close();
        }
    }
}
