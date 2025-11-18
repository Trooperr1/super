using SupermarketPOS.Core.Enums;

namespace SupermarketPOS.Core.Models;

/// <summary>
/// فرۆشتنەکان - Sales
/// </summary>
public class Sale : BaseEntity
{
    /// <summary>ژمارەی پسوڵە - Invoice Number</summary>
    public string InvoiceNumber { get; set; } = string.Empty;

    /// <summary>بەروار - Sale Date</summary>
    public DateTime SaleDate { get; set; } = DateTime.Now;

    /// <summary>کۆی گشتی - Subtotal</summary>
    public decimal Subtotal { get; set; }

    /// <summary>داشکان - Discount Amount</summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>ڕێژەی داشکان - Discount Percentage</summary>
    public decimal DiscountPercentage { get; set; }

    /// <summary>باج - Tax Amount</summary>
    public decimal TaxAmount { get; set; }

    /// <summary>کۆی گشتی - Total</summary>
    public decimal Total { get; set; }

    /// <summary>بڕی پارە - Amount Paid</summary>
    public decimal AmountPaid { get; set; }

    /// <summary>پارەی گەڕاوە - Change</summary>
    public decimal Change { get; set; }

    /// <summary>شێوازی پارەدان - Payment Method</summary>
    public PaymentMethod PaymentMethod { get; set; }

    /// <summary>تێبینی - Notes</summary>
    public string? Notes { get; set; }

    /// <summary>پێناسەی کاشێر - Cashier User ID</summary>
    public int UserId { get; set; }

    /// <summary>کاشێر - Cashier</summary>
    public virtual User User { get; set; } = null!;

    /// <summary>پێناسەی کڕیار - Customer ID</summary>
    public int? CustomerId { get; set; }

    /// <summary>کڕیار - Customer</summary>
    public virtual Customer? Customer { get; set; }

    /// <summary>بڕگەکانی فرۆشتن - Sale Items</summary>
    public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}
