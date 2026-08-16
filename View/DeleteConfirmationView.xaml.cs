using System.Windows;

namespace ProjectManager.View
{
    public partial class DeleteConfirmationView : Window
    {
        public bool Confirmed { get; private set; } = false;

        public DeleteConfirmationView(string message)
        {
            InitializeComponent();
            MessageText.Text = message;
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
