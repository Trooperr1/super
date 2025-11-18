using SupermarketPOS.Application.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SupermarketPOS.WPF.Views;

/// <summary>
/// پەنجەرەی سەرەکی - Main Window
/// </summary>
public partial class MainWindow : Window
{
    private readonly AuthenticationService _authService;
    private readonly DispatcherTimer _timer;

    public MainWindow(AuthenticationService authService)
    {
        InitializeComponent();
        _authService = authService;

        // نیشاندانی ناوی بەکارهێنەر - Display user name
        if (_authService.CurrentUser != null)
        {
            UserNameTextBlock.Text = _authService.CurrentUser.FullName;
        }

        // کاتژمێر - Timer for date/time
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _timer.Tick += Timer_Tick;
        _timer.Start();

        // نیشاندانی لاپەڕەی سەرەتایی - Show default page
        ShowDashboard();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        DateTimeTextBlock.Text = DateTime.Now.ToString("yyyy/MM/dd - HH:mm:ss");
    }

    private void MenuButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is string tag)
        {
            switch (tag)
            {
                case "Dashboard":
                    ShowDashboard();
                    break;
                case "POS":
                    ShowPOS();
                    break;
                case "Inventory":
                    ShowInventory();
                    break;
                case "Customers":
                    ShowCustomers();
                    break;
                case "Reports":
                    ShowReports();
                    break;
                case "Settings":
                    ShowSettings();
                    break;
            }
        }
    }

    private void ShowDashboard()
    {
        StatusTextBlock.Text = "داشبۆرد";
        // TODO: Navigate to Dashboard page
        MainFrame.Content = new TextBlock
        {
            Text = "داشبۆرد\n\nلێرە ئامارەکان و زانیاری کورت دەبینیت",
            FontSize = 24,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            TextAlignment = TextAlignment.Center
        };
    }

    private void ShowPOS()
    {
        StatusTextBlock.Text = "فرۆشتن - کاشێر";
        // TODO: Navigate to POS page
        MainFrame.Content = new TextBlock
        {
            Text = "پەنجەرەی فرۆشتن\n\nلێرە دەتوانیت بەرهەم زیاد بکەیت و فرۆشتن تەواو بکەیت",
            FontSize = 24,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            TextAlignment = TextAlignment.Center
        };
    }

    private void ShowInventory()
    {
        StatusTextBlock.Text = "بەڕێوەبردنی مەخزەن";
        MainFrame.Content = new TextBlock
        {
            Text = "بەڕێوەبردنی مەخزەن\n\nلێرە دەتوانیت بەرهەمەکان بەڕێوە ببەیت",
            FontSize = 24,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            TextAlignment = TextAlignment.Center
        };
    }

    private void ShowCustomers()
    {
        StatusTextBlock.Text = "کڕیارەکان";
        MainFrame.Content = new TextBlock
        {
            Text = "بەڕێوەبردنی کڕیارەکان\n\nلێرە دەتوانیت کڕیارەکان بەڕێوە ببەیت",
            FontSize = 24,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            TextAlignment = TextAlignment.Center
        };
    }

    private void ShowReports()
    {
        StatusTextBlock.Text = "ڕاپۆرتەکان";
        MainFrame.Content = new TextBlock
        {
            Text = "ڕاپۆرتەکان\n\nلێرە دەتوانیت ڕاپۆرتی جۆراوجۆر ببینیت",
            FontSize = 24,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            TextAlignment = TextAlignment.Center
        };
    }

    private void ShowSettings()
    {
        StatusTextBlock.Text = "ڕێکخستنەکان";
        MainFrame.Content = new TextBlock
        {
            Text = "ڕێکخستنەکان\n\nلێرە دەتوانیت ڕێکخستنەکانی سیستەم گۆڕانکاری بکەیت",
            FontSize = 24,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            TextAlignment = TextAlignment.Center
        };
    }

    private async void LogoutButton_Click(object sender, RoutedEventArgs e)
    {
        var result = MessageBox.Show(
            "ئایا دڵنیایت لە چوونەدەرەوە?",
            "چوونەدەرەوە",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            await _authService.LogoutAsync();

            var loginWindow = App.GetService<LoginWindow>();
            if (loginWindow != null)
            {
                loginWindow.Show();
                this.Close();
            }
        }
    }

    protected override void OnClosed(EventArgs e)
    {
        _timer.Stop();
        base.OnClosed(e);
    }
}
