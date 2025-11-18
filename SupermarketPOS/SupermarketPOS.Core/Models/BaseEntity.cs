namespace SupermarketPOS.Core.Models;

/// <summary>
/// کلاسی بنەڕەتی بۆ هەموو مۆدێلەکان - Base class for all entities
/// </summary>
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}
