namespace SupermarketPOS.Core.Models;

/// <summary>
/// بەرهەمەکان - Products
/// </summary>
public class Product : BaseEntity
{
    /// <summary>بارکۆد - Barcode</summary>
    public string Barcode { get; set; } = string.Empty;

    /// <summary>ناوی بەرهەم - Product Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>نرخی کڕین - Purchase Price</summary>
    public decimal PurchasePrice { get; set; }

    /// <summary>نرخی فرۆشتن - Selling Price</summary>
    public decimal SellingPrice { get; set; }

    /// <summary>بڕی مەخزەن - Stock Quantity</summary>
    public int StockQuantity { get; set; }

    /// <summary>کەمترین ئاستی مەخزەن - Minimum Stock Level</summary>
    public int MinimumStockLevel { get; set; } = 10;

    /// <summary>بەرواری بەسەرچوون - Expiry Date</summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>وێنە - Image Path</summary>
    public string? ImagePath { get; set; }

    /// <summary>وەسف - Description</summary>
    public string? Description { get; set; }

    /// <summary>پێناسەی پۆلێن - Category ID</summary>
    public int CategoryId { get; set; }

    /// <summary>پۆلێن - Category</summary>
    public virtual Category Category { get; set; } = null!;

    /// <summary>پێناسەی دابینکەر - Supplier ID</summary>
    public int? SupplierId { get; set; }

    /// <summary>دابینکەر - Supplier</summary>
    public virtual Supplier? Supplier { get; set; }

    /// <summary>مامەڵەکانی مەخزەن - Inventory Transactions</summary>
    public virtual ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();

    /// <summary>بڕگەکانی فرۆشتن - Sale Items</summary>
    public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}
