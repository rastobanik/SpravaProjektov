using CommunityToolkit.Mvvm.ComponentModel;

namespace SpravaProjektovUI_Avalonia.ViewModels;

public partial class MainViewModel : ObservableObject
{
    // Táto vlastnosť drží aktuálne zobrazený ViewModel (stránku)
    [ObservableProperty]
    private ObservableObject _currentPage;

    public MainViewModel()
    {
        // Po štarte aplikácie nastavíme ako prvú stránku Login
        // Odovzdáme jej referenciu na tento MainViewModel (this), aby mohla spustiť prepnutie
        _currentPage = new LoginViewModel(this);
    }

    // Metóda, ktorú zavolá LoginViewModel po úspešnom prihlásení
    public void PrejdiNaDashboard()
    {
        CurrentPage = new MainDashboardViewModel(this);
    }
}
