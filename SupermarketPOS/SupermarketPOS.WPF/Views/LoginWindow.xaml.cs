using SupermarketPOS.Application.Services;
using System.Windows;
using System.Windows.Input;

namespace SupermarketPOS.WPF.Views;

/// <summary>
/// پەنجەرەی چوونەژوورەوە - Login Window
/// </summary>
public partial class LoginWindow : Window
{
    private readonly AuthenticationService _authService;

    public LoginWindow(AuthenticationService authService)
    {
        InitializeComponent();
        _authService = authService;

        // Enter بۆ چوونەژوورەوە - Enter to login
        UsernameTextBox.KeyDown += (s, e) => { if (e.Key == Key.Enter) PasswordBox.Focus(); };
        PasswordBox.KeyDown += async (s, e) => { if (e.Key == Key.Enter) await LoginAsync(); };
    }

    private async void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        await LoginAsync();
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        System.Windows.Application.Current.Shutdown();
    }

    private async Task LoginAsync()
    {
        var username = UsernameTextBox.Text.Trim();
        var password = PasswordBox.Password;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            MessageBox.Show("تکایە ناوی بەکارهێنەر و وشەی نهێنی داخڵ بکە", "هەڵە",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            var (success, message, user) = await _authService.LoginAsync(username, password);

            if (success && user != null)
            {
                // کردنەوەی پەنجەرەی سەرەکی - Open main window
                var mainWindow = App.GetService<MainWindow>();
                if (mainWindow != null)
                {
                    mainWindow.Show();
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show(message, "هەڵە لە چوونەژوورەوە",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"هەڵە: {ex.Message}", "هەڵە",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
