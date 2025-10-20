using LEARN_MVVM.Data;
using LEARN_MVVM.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Wpf.Ui;

namespace LEARN_MVVM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Initialize Snackbar Provider
        private static readonly IServiceProvider _serviceProvider;
        public static IServiceProvider ServiceProvider => _serviceProvider;

        // Path to Database
        public static string DbPath { get; }

        static App()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "WeatherAppDataBase.db");

            var services = new ServiceCollection();

            services.AddSingleton<ISnackbarService>(new SnackbarService());
            services.AddDbContext<WeatherAppContext>(options =>
            {
                options.UseSqlite($"Data Source={DbPath}");
            });
            services.AddScoped<WeatherRepository>();

            // TODO: EF Core einlesen
            // TODO: SQLite verwenden
            // TODO: Repository dafür erstellen (CRUD Repository --> Repository pattern)
            // TODO: WeaterApi anfragen in EF Core speichern
            // TODO: Alles mittels DI zu verwenden

            _serviceProvider = services.BuildServiceProvider();

            //DataBaseHelper.InitializeDatabase();

        }
    }
}
