namespace SupermarketPOS.Core.Models;

/// <summary>
/// کڕیارەکان - Customers
/// </summary>
public class Customer : BaseEntity
{
    /// <summary>ناو - Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>ژمارەی تەلەفۆن - Phone Number</summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>ئیمەیڵ - Email</summary>
    public string? Email { get; set; }

    /// <summary>ناونیشان - Address</summary>
    public string? Address { get; set; }

    /// <summary>خاڵی دڵسۆزی - Loyalty Points</summary>
    public int LoyaltyPoints { get; set; }

    /// <summary>کۆی کڕینەکان - Total Purchases</summary>
    public decimal TotalPurchases { get; set; }

    /// <summary>باڵانسی قەرز - Credit Balance</summary>
    public decimal CreditBalance { get; set; }

    /// <summary>تێبینی - Notes</summary>
    public string? Notes { get; set; }

    /// <summary>فرۆشتنەکان - Sales</summary>
    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
