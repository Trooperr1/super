namespace SupermarketPOS.Core.Models;

/// <summary>
/// پۆلێنەکانی بەرهەم - Product Categories
/// </summary>
public class Category : BaseEntity
{
    /// <summary>ناوی پۆلێن - Category Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>وەسف - Description</summary>
    public string? Description { get; set; }

    /// <summary>بەرهەمەکان - Products in this category</summary>
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
