using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SupermarketPOS.Application.Services;
using SupermarketPOS.Core.Interfaces;
using SupermarketPOS.Infrastructure.Data;
using SupermarketPOS.Infrastructure.Repositories;
using SupermarketPOS.WPF.Views;
using System.IO;
using System.Windows;

namespace SupermarketPOS.WPF;

/// <summary>
/// سەرەکی بەرنامە - Application Main
/// </summary>
public partial class App : System.Windows.Application
{
    private ServiceProvider? _serviceProvider;
    public IConfiguration? Configuration { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        try
        {
            // خوێندنەوەی ڕێکخستنەکان - Load configuration
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            // ڕێکخستنی خزمەتگوزارییەکان - Configure services
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();

            // دروستکردنی بنکەی دراوە - Initialize database
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SupermarketDbContext>();
                try
                {
                    context.Database.Migrate();
                    DbSeeder.SeedAsync(context).Wait();
                }
                catch (Exception ex)
                {
                    // Log error to file
                    File.WriteAllText("error.log", $"Database Error:\n{ex}\n\nInner: {ex.InnerException}");

                    MessageBox.Show(
                        $"Database connection failed:\n\n{ex.Message}\n\nInner: {ex.InnerException?.Message}\n\nCheck error.log for details",
                        "Database Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    Shutdown();
                    return;
                }
            }

            // نیشاندانی پەنجەرەی چوونەژوورەوە - Show login window
            var loginWindow = _serviceProvider.GetRequiredService<LoginWindow>();
            loginWindow.Show();
        }
        catch (Exception ex)
        {
            // Log all startup errors
            File.WriteAllText("error.log", $"Startup Error:\n{ex}\n\nInner: {ex.InnerException}\n\nStack:\n{ex.StackTrace}");

            MessageBox.Show(
                $"Application startup failed:\n\n{ex.Message}\n\nInner: {ex.InnerException?.Message}\n\nCheck error.log for full details",
                "Startup Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            Shutdown();
        }
    }

    private void ConfigureServices(IServiceCollection services)
    {
        // بنکەی دراوە - Database
        services.AddDbContext<SupermarketDbContext>(options =>
            options.UseSqlServer(Configuration!.GetConnectionString("DefaultConnection")));

        // مەخزەنەکان - Repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // خزمەتگوزارییەکان - Services
        services.AddScoped<AuthenticationService>();
        services.AddScoped<ProductService>();
        services.AddScoped<SalesService>();
        services.AddScoped<CustomerService>();

        // پەنجەرەکان - Windows
        services.AddTransient<LoginWindow>();
        services.AddTransient<MainWindow>();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _serviceProvider?.Dispose();
        base.OnExit(e);
    }

    public static T? GetService<T>() where T : class
    {
        return (Current as App)?._serviceProvider?.GetService<T>();
    }
}
