using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using SpravaProjektovUI_Avalonia.ViewModels;
using SpravaProjektovUI_Avalonia.Views;

namespace SpravaProjektovUI_Avalonia;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        // k backendu pristupovat cez Refit

#if DEBUG
        // Nový spôsob pripojenia vývojárskych nástrojov v Avalonia 12
        this.AttachDeveloperTools();
#endif
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}