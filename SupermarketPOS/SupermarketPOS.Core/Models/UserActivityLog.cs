namespace SupermarketPOS.Core.Models;

/// <summary>
/// لۆگی چالاکی بەکارهێنەر - User Activity Log
/// </summary>
public class UserActivityLog : BaseEntity
{
    /// <summary>پێناسەی بەکارهێنەر - User ID</summary>
    public int UserId { get; set; }

    /// <summary>بەکارهێنەر - User</summary>
    public virtual User User { get; set; } = null!;

    /// <summary>چالاکی - Activity</summary>
    public string Activity { get; set; } = string.Empty;

    /// <summary>وردەکاری - Details</summary>
    public string? Details { get; set; }

    /// <summary>ئای پی - IP Address</summary>
    public string? IpAddress { get; set; }
}
