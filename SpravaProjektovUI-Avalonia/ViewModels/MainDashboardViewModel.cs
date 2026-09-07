using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SpravaProjektovUI_Avalonia.ViewModels;

public partial class MainDashboardViewModel : ObservableObject
{
    // Predpokladaný model projektu (ak ho máte iný, stačí zmeniť typ string na váš Model)
    [ObservableProperty]
    private ObservableCollection<string> _projects = new();

    [ObservableProperty]
    private string? _selectedProject;

    private readonly MainViewModel _mainViewModel;

    public MainDashboardViewModel(MainViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
        // Dočasné testovacie dáta pre grafický náhľad (Preview)
        _projects.Add("Projekt Alfa");
        _projects.Add("Projekt Beta");
        _projects.Add("Projekt Gama");
    }

    public MainDashboardViewModel()
    {
        _mainViewModel = null!;

        // Dočasné testovacie dáta pre grafický náhľad (Preview)
        _projects.Add("Projekt Alfa");
        _projects.Add("Projekt Beta");
        _projects.Add("Projekt Gama");
    }

    [RelayCommand]
    private void Pridat()
    {
        // Logika pre otvorenie editačného formulára (nový záznam)
        _mainViewModel.CurrentPage = new ProjectEditorViewModel(_mainViewModel);
    }

    [RelayCommand]
    private void Aktualizovat()
    {
        if (SelectedProject != null)
        {
            _mainViewModel.CurrentPage = new ProjectEditorViewModel(_mainViewModel, SelectedProject);
            // Logika pre úpravu vybraného projektu
        }
    }

    [RelayCommand]
    private void Zmazat()
    {
        if (SelectedProject != null)
        {
            Projects.Remove(SelectedProject);
        }
    }
}
