namespace SupermarketPOS.Core.Models;

/// <summary>
/// بڕگەکانی فرۆشتن - Sale Items
/// </summary>
public class SaleItem : BaseEntity
{
    /// <summary>پێناسەی فرۆشتن - Sale ID</summary>
    public int SaleId { get; set; }

    /// <summary>فرۆشتن - Sale</summary>
    public virtual Sale Sale { get; set; } = null!;

    /// <summary>پێناسەی بەرهەم - Product ID</summary>
    public int ProductId { get; set; }

    /// <summary>بەرهەم - Product</summary>
    public virtual Product Product { get; set; } = null!;

    /// <summary>بڕ - Quantity</summary>
    public int Quantity { get; set; }

    /// <summary>نرخی یەکە - Unit Price</summary>
    public decimal UnitPrice { get; set; }

    /// <summary>کۆی گشتی - Subtotal</summary>
    public decimal Subtotal { get; set; }

    /// <summary>داشکان - Discount</summary>
    public decimal Discount { get; set; }

    /// <summary>کۆ - Total</summary>
    public decimal Total { get; set; }
}
