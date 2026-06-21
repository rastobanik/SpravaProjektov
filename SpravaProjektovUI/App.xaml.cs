using Microsoft.Extensions.Configuration;
using SpravaProjektovUI.Infrastructure;
using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Windows;

namespace SpravaProjektovUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ProjectApiClient ApiClient { get; private set; } = null!;

        public static LoginApiClient LoginApiClient { get; private set; } = null!;

        public static IConfiguration Configuration { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

            var baseUrl = Configuration["Api:BaseUrl"];

            if (string.IsNullOrEmpty(baseUrl))
            {
                MessageBox.Show("Base URL is not configured. Please check the appsettings.json file.", "Configuration Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
                return;
            }


            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl!)
            };

            ApiClient = new ProjectApiClient(httpClient);

            LoginApiClient = new LoginApiClient(httpClient);

            var loginWindow = new LoginWindow();
            loginWindow.Show();
        }
    }
}
