using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpravaProjektovUI.Infrastructure;
using SpravaProjektovUI.Models;

namespace SpravaProjektovUI.ViewModels
{
    public partial class PridatViewModel : ObservableObject
    {
        public event Action? RequestClose;

        private readonly ProjectApiClient _api;

        private readonly bool pridat = false;

        public PridatViewModel(ProjectApiClient api)
        {
            _api = api;
            pridat = true;
        }

        public PridatViewModel(ProjectApiClient api, ProjectDto project)
        {
            _api = api;
            Id = project.Id;
            Name = project.Name;
            Abbreviation = project.Abbreviation;
            Customer = project.Customer;
        }

        [ObservableProperty]
        private string? id;

        [ObservableProperty]
        private string? name;

        [ObservableProperty]
        private string? abbreviation;

        [ObservableProperty]
        private string? customer;


        [RelayCommand]
        private async Task Ulozit()
        {
            if (_api == null)
            {
                return;
            }

            var project = new ProjectDto
            {
                Id = Id,
                Name = Name,
                Abbreviation = Abbreviation,
                Customer = Customer
            };

            if (pridat)
            { 
                await _api.CreateAsync(project);
            }
            else
            {

                await _api.UpadateAsync(project);
            }

            RequestClose?.Invoke();
        }

        [RelayCommand]
        private async Task Zrusit()
        {
            RequestClose?.Invoke();
        }
    }
}
