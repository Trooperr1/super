namespace SupermarketPOS.Core.Models;

/// <summary>
/// دابینکەران - Suppliers
/// </summary>
public class Supplier : BaseEntity
{
    /// <summary>ناوی دابینکەر - Supplier Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>ژمارەی تەلەفۆن - Phone Number</summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>ئیمەیڵ - Email</summary>
    public string? Email { get; set; }

    /// <summary>ناونیشان - Address</summary>
    public string? Address { get; set; }

    /// <summary>کۆی قەرز - Total Debt</summary>
    public decimal TotalDebt { get; set; }

    /// <summary>تێبینی - Notes</summary>
    public string? Notes { get; set; }

    /// <summary>بەرهەمەکان - Products from this supplier</summary>
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
