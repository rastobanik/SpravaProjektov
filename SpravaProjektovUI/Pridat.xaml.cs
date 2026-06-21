using SpravaProjektovUI.Models;
using SpravaProjektovUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SpravaProjektovUI
{
    /// <summary>
    /// Interaction logic for Pridat.xaml
    /// </summary>
    public partial class Pridat : Window
    {
        public Pridat()
        {
            InitializeComponent();
            var vm = new PridatViewModel(App.ApiClient);
            DataContext = vm;
            vm.RequestClose += () => this.Close();
        }

        public Pridat(ProjectDto project)
        {
            InitializeComponent();
            var vm = new PridatViewModel(App.ApiClient, project);
            DataContext = vm;

            vm.RequestClose += () => this.Close();
        }
    }
}
