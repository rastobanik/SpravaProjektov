using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SpravaProjektovUI_Avalonia.ViewModels
{
    public partial class ProjectEditorViewModel : ObservableObject
    {
        private readonly MainViewModel _mainViewModel;

        [ObservableProperty]
        private string _id = string.Empty;

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private string _abbreviation = string.Empty;

        [ObservableProperty]
        private string _customer = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        public ProjectEditorViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        // Konštruktor pre ÚPRAVU existujúceho projektu (v budúcnosti nahradíte vašou triedou ProjectModel)
        public ProjectEditorViewModel(MainViewModel mainViewModel, string existujuciProjekt)
        {
            _mainViewModel = mainViewModel;
            // Rozoberieme testovací string (z predchádzajúceho príkladu) na ukážku
            Id = "1";
            Name = existujuciProjekt;
            Abbreviation = "SKR";
            Customer = "Zákazník s.r.o.";
        }

        public ProjectEditorViewModel()
        {
            _mainViewModel = null!;

            Id = "999";
            Name = "Ukážkový projekt";
            Abbreviation = "UKZ";
            Customer = "Dizajnér s.r.o.";
        }

        [RelayCommand]
        private async Task  UlozitAsync()
        {
            // Tu pridajte logiku na uloženie projektu
            _mainViewModel.PrejdiNaDashboard();
        }

        [RelayCommand]
        private async Task  ZrusitAsync()
        {
            // Tu pridajte logiku na zrušenie úprav
            _mainViewModel.PrejdiNaDashboard();
        }
    }
}
