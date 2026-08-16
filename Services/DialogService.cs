using System.Windows;

namespace ProjectManager.Services
{
    public static class DialogService
    {
        public static bool Confirm(string message)
        {
            var result = MessageBox.Show(
                message,
                "Are you sure ?",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            return result == MessageBoxResult.Yes;
        }
    }
}
