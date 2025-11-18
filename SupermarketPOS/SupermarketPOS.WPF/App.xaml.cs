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
public partial class App : Application
{
    private ServiceProvider? _serviceProvider;
    public IConfiguration? Configuration { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

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
                MessageBox.Show(
                    $"هەڵە لە گرێدان بە بنکەی دراوە:\n{ex.Message}\n\nتکایە دڵنیابە لە کارکردنی SQL Server",
                    "هەڵەی بنکەی دراوە",
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
