using SupermarketPOS.Core.Interfaces;
using SupermarketPOS.Core.Models;
using BCrypt.Net;

namespace SupermarketPOS.Application.Services;

/// <summary>
/// خزمەتگوزاری دڵنیاکردنەوە - Authentication Service
/// </summary>
public class AuthenticationService
{
    private readonly IUnitOfWork _unitOfWork;
    private User? _currentUser;

    public AuthenticationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public User? CurrentUser => _currentUser;

    /// <summary>
    /// چوونەژوورەوە - Login
    /// </summary>
    public async Task<(bool Success, string Message, User? User)> LoginAsync(string username, string password)
    {
        var users = await _unitOfWork.Users.FindAsync(u => u.Username == username && u.IsActive);
        var user = users.FirstOrDefault();

        if (user == null)
        {
            return (false, "ناوی بەکارهێنەر یان وشەی نهێنی هەڵەیە", null);
        }

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return (false, "ناوی بەکارهێنەر یان وشەی نهێنی هەڵەیە", null);
        }

        user.LastLogin = DateTime.Now;
        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        _currentUser = user;

        // تۆمارکردنی لۆگی چالاکی - Log activity
        await LogActivityAsync(user.Id, "چوونەژوورەوە", $"بەکارهێنەر چووە ژوورەوە: {user.FullName}");

        return (true, "بەخێربێیت!", user);
    }

    /// <summary>
    /// چوونەدەرەوە - Logout
    /// </summary>
    public async Task LogoutAsync()
    {
        if (_currentUser != null)
        {
            await LogActivityAsync(_currentUser.Id, "چوونەدەرەوە", $"بەکارهێنەر چووەدەرەوە: {_currentUser.FullName}");
            _currentUser = null;
        }
    }

    /// <summary>
    /// گۆڕینی وشەی نهێنی - Change Password
    /// </summary>
    public async Task<(bool Success, string Message)> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
        {
            return (false, "بەکارهێنەر نەدۆزرایەوە");
        }

        if (!BCrypt.Net.BCrypt.Verify(oldPassword, user.PasswordHash))
        {
            return (false, "وشەی نهێنی کۆن هەڵەیە");
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        await LogActivityAsync(userId, "گۆڕینی وشەی نهێنی", "وشەی نهێنی گۆڕدرا");

        return (true, "وشەی نهێنی بەسەرکەوتوویی گۆڕدرا");
    }

    /// <summary>
    /// تۆمارکردنی چالاکی - Log Activity
    /// </summary>
    public async Task LogActivityAsync(int userId, string activity, string? details = null)
    {
        var log = new UserActivityLog
        {
            UserId = userId,
            Activity = activity,
            Details = details,
            CreatedAt = DateTime.Now
        };

        await _unitOfWork.UserActivityLogs.AddAsync(log);
        await _unitOfWork.SaveChangesAsync();
    }
}
