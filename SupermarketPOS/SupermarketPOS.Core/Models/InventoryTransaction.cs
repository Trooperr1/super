using SupermarketPOS.Core.Enums;

namespace SupermarketPOS.Core.Models;

/// <summary>
/// مامەڵەکانی مەخزەن - Inventory Transactions
/// </summary>
public class InventoryTransaction : BaseEntity
{
    /// <summary>پێناسەی بەرهەم - Product ID</summary>
    public int ProductId { get; set; }

    /// <summary>بەرهەم - Product</summary>
    public virtual Product Product { get; set; } = null!;

    /// <summary>جۆری مامەڵە - Transaction Type</summary>
    public TransactionType TransactionType { get; set; }

    /// <summary>بڕ - Quantity</summary>
    public int Quantity { get; set; }

    /// <summary>بڕی پێشوو - Previous Stock</summary>
    public int PreviousStock { get; set; }

    /// <summary>بڕی نوێ - New Stock</summary>
    public int NewStock { get; set; }

    /// <summary>هۆکار - Reason</summary>
    public string Reason { get; set; } = string.Empty;

    /// <summary>تێبینی - Notes</summary>
    public string? Notes { get; set; }

    /// <summary>پێناسەی بەکارهێنەر - User ID</summary>
    public int UserId { get; set; }

    /// <summary>بەکارهێنەر - User</summary>
    public virtual User User { get; set; } = null!;
}
