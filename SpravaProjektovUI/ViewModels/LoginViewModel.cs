using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpravaProjektovUI.Infrastructure;
using System.Windows;

namespace SpravaProjektovUI.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string password;

        private readonly LoginApiClient _api;

        public LoginViewModel(LoginApiClient api)
        {
            _api = api;
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            var result = await _api.LoginAsync(Username, Password);

            if (result == null)
            {
                MessageBox.Show("Invalid login");
                return;
            }

            // success → open MainWindow
            var main = new MainWindow();
            main.Show();

            Application.Current.Windows[0]?.Close();           
        }

        [RelayCommand]
        private async Task CancelAsync()
        {
            Application.Current.Shutdown();
        }
    }
}
