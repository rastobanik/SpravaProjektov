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

        [ObservableProperty]
        private string? errorMessage;

        private readonly LoginApiClient _api;

        public LoginViewModel(LoginApiClient api)
        {
            _api = api;
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            try
            {
                var result = await _api.LoginAsync(Username, Password);

                if (result == null)
                {
                    MessageBox.Show("Chybne meno alebo heslo");
                    return;
                }

                // success → open MainWindow
                var main = new MainWindow();
                main.Show();

                Application.Current.Windows[0]?.Close();
            }
            catch
            {                
                MessageBox.Show("Nepodarilo sa prihlasit");
            }
        }

        [RelayCommand]
        private async Task CancelAsync()
        {
            Application.Current.Shutdown();
        }
    }
}
