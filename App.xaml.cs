using LEARN_MVVM.Data;
using LEARN_MVVM.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Windows;
using Wpf.Ui;

namespace LEARN_MVVM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public sealed partial class App : Application
    {
        // Initialize Snackbar Provider
        private static readonly IServiceProvider _serviceProvider;
        public static IServiceProvider ServiceProvider => _serviceProvider;

        // Path to Database
        public static string DbPath { get; }

        static App()
        {
#if RELEASE
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "WeatherAppDataBase.db");
#elif DEBUG
            string path = Environment.CurrentDirectory;
            DbPath = System.IO.Path.Join(path, "WeatherAppDebugDataBase.sqlite3");
#endif
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
            
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<WeatherAppContext>();

            Task.Factory.StartNew(async () => await context.Database.MigrateAsync());

            //Task.Run(async () => await context.Database.MigrateAsync());

            //Application.Current.Dispatcher.Invoke(async () => await context.Database.MigrateAsync());

            // TODO: Unit tests schreiben und prüfen async Methods in void methods aufrufen
            // TODO: mstest / nunit / xunit
            // Alle testfälle im happy path testen
            // Alle testfälle im error path testen
        }

        private Task Throws()
        {
            throw new NotImplementedException();
        }
    }
}