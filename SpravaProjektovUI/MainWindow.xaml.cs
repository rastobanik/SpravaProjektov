using SpravaProjektovUI.ViewModels;
using System.Windows;

namespace SpravaProjektovUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var vm = new MainViewModel(App.ApiClient);
            DataContext = vm;

            Loaded += async (_, _) => await vm.InitAsync();
        }
    }
}