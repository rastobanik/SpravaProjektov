using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace SpravaProjektovUI_Avalonia.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly MainViewModel _mainViewModel;

    [ObservableProperty]
    private string _username = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    // Cez konštruktor si vypýtame hlavný MainViewModel
    public LoginViewModel(MainViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
    }

    // Prázdny konštruktor pre potreby XAML Previewera (Dizajnéra)
    public LoginViewModel()
    {
        _mainViewModel = null!;
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        // Tu neskôr prebehne reálne volanie vášho REST API
        if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
        {
            // Simulácia úspešného prihlásenia -> Prepnutie na hlavnú tabuľku
            _mainViewModel.PrejdiNaDashboard();
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        Username = string.Empty;
        Password = string.Empty;
    }
}
