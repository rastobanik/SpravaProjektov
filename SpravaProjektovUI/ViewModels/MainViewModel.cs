using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpravaProjektovUI.Infrastructure;
using SpravaProjektovUI.Models;
using System.Collections.ObjectModel;

namespace SpravaProjektovUI.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly ProjectApiClient _api;

        public ObservableCollection<ProjectDto> Projects { get; } = [];

        public MainViewModel(ProjectApiClient api)
        {
            _api = api;
        }

        public async Task InitAsync()
        {
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            var data = await _api.GetAllAsync();

            Projects.Clear();
            foreach (var p in data)
                Projects.Add(p);
        }

        [ObservableProperty]
        private ProjectDto? _selectedProject;
        

        [RelayCommand]
        private async Task Pridat()
        {
            var window = new Pridat();
            window.ShowDialog();

            await LoadAsync();
        }

        [RelayCommand]
        private async Task Aktualizovat()
        {
            if (SelectedProject == null) return;

            var window = new Pridat(SelectedProject);
            window.ShowDialog();

            await LoadAsync();
        }

        [RelayCommand]
        private async Task Zmazat()
        {
            if (SelectedProject == null) return;            

            if (string.IsNullOrEmpty(SelectedProject.Id)) return;

            await _api.DeleteAsync(SelectedProject.Id);

            Projects.Remove(SelectedProject);
            SelectedProject = null;

            await LoadAsync();
        }        
    }
}
