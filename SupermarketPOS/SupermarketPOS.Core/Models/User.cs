using SupermarketPOS.Core.Enums;

namespace SupermarketPOS.Core.Models;

/// <summary>
/// بەکارهێنەران - Users
/// </summary>
public class User : BaseEntity
{
    /// <summary>ناوی بەکارهێنەر - Username</summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>وشەی نهێنی - Password Hash</summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>ناوی تەواو - Full Name</summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>ڕۆڵ - Role</summary>
    public UserRole Role { get; set; }

    /// <summary>ژمارەی تەلەفۆن - Phone Number</summary>
    public string? Phone { get; set; }

    /// <summary>چالاکە - Is Active</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>دوایین چوونەژوورەوە - Last Login</summary>
    public DateTime? LastLogin { get; set; }

    /// <summary>فرۆشتنەکان - Sales made by this user</summary>
    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    /// <summary>لۆگی چالاکی - Activity Logs</summary>
    public virtual ICollection<UserActivityLog> ActivityLogs { get; set; } = new List<UserActivityLog>();
}
