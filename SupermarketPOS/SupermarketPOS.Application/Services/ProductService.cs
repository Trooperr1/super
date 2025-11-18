using SupermarketPOS.Core.Interfaces;
using SupermarketPOS.Core.Models;
using SupermarketPOS.Core.Enums;

namespace SupermarketPOS.Application.Services;

/// <summary>
/// خزمەتگوزاری بەرهەم - Product Service
/// </summary>
public class ProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// دۆزینەوەی بەرهەم بە بارکۆد - Find Product by Barcode
    /// </summary>
    public async Task<Product?> GetProductByBarcodeAsync(string barcode)
    {
        var products = await _unitOfWork.Products.FindAsync(p => p.Barcode == barcode);
        return products.FirstOrDefault();
    }

    /// <summary>
    /// گەڕان لە بەرهەمەکان - Search Products
    /// </summary>
    public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
    {
        var products = await _unitOfWork.Products.FindAsync(p =>
            p.Name.Contains(searchTerm) ||
            p.Barcode.Contains(searchTerm));
        return products;
    }

    /// <summary>
    /// زیادکردنی بەرهەم - Add Product
    /// </summary>
    public async Task<(bool Success, string Message)> AddProductAsync(Product product, int userId)
    {
        // پشکنینی بوونی بارکۆد - Check if barcode exists
        var existingProduct = await GetProductByBarcodeAsync(product.Barcode);
        if (existingProduct != null)
        {
            return (false, "بارکۆد پێشتر تۆمارکراوە");
        }

        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();

        // تۆمارکردنی مامەڵەی مەخزەن - Log inventory transaction
        if (product.StockQuantity > 0)
        {
            var transaction = new InventoryTransaction
            {
                ProductId = product.Id,
                TransactionType = TransactionType.Purchase,
                Quantity = product.StockQuantity,
                PreviousStock = 0,
                NewStock = product.StockQuantity,
                Reason = "بەرهەمی نوێ",
                UserId = userId
            };

            await _unitOfWork.InventoryTransactions.AddAsync(transaction);
            await _unitOfWork.SaveChangesAsync();
        }

        return (true, "بەرهەم بەسەرکەوتوویی زیادکرا");
    }

    /// <summary>
    /// نوێکردنەوەی بەرهەم - Update Product
    /// </summary>
    public async Task<(bool Success, string Message)> UpdateProductAsync(Product product)
    {
        await _unitOfWork.Products.UpdateAsync(product);
        await _unitOfWork.SaveChangesAsync();
        return (true, "بەرهەم بەسەرکەوتوویی نوێکرایەوە");
    }

    /// <summary>
    /// سڕینەوەی بەرهەم - Delete Product
    /// </summary>
    public async Task<(bool Success, string Message)> DeleteProductAsync(int productId)
    {
        await _unitOfWork.Products.DeleteAsync(productId);
        await _unitOfWork.SaveChangesAsync();
        return (true, "بەرهەم سڕایەوە");
    }

    /// <summary>
    /// ڕێکخستنی مەخزەن - Adjust Stock
    /// </summary>
    public async Task<(bool Success, string Message)> AdjustStockAsync(int productId, int quantity, string reason, TransactionType type, int userId)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(productId);
        if (product == null)
        {
            return (false, "بەرهەم نەدۆزرایەوە");
        }

        var previousStock = product.StockQuantity;
        var newStock = type == TransactionType.Purchase || type == TransactionType.Adjustment
            ? previousStock + quantity
            : previousStock - quantity;

        if (newStock < 0)
        {
            return (false, "مەخزەن بەسە");
        }

        product.StockQuantity = newStock;
        await _unitOfWork.Products.UpdateAsync(product);

        // تۆمارکردنی مامەڵە - Log transaction
        var transaction = new InventoryTransaction
        {
            ProductId = productId,
            TransactionType = type,
            Quantity = quantity,
            PreviousStock = previousStock,
            NewStock = newStock,
            Reason = reason,
            UserId = userId
        };

        await _unitOfWork.InventoryTransactions.AddAsync(transaction);
        await _unitOfWork.SaveChangesAsync();

        return (true, "مەخزەن نوێکرایەوە");
    }

    /// <summary>
    /// بەرهەمەکانی کەم - Low Stock Products
    /// </summary>
    public async Task<IEnumerable<Product>> GetLowStockProductsAsync()
    {
        var products = await _unitOfWork.Products.FindAsync(p =>
            p.StockQuantity <= p.MinimumStockLevel);
        return products;
    }

    /// <summary>
    /// بەرهەمەکانی بەسەرچوو - Expired Products
    /// </summary>
    public async Task<IEnumerable<Product>> GetExpiredProductsAsync()
    {
        var today = DateTime.Today;
        var products = await _unitOfWork.Products.FindAsync(p =>
            p.ExpiryDate.HasValue && p.ExpiryDate.Value < today);
        return products;
    }
}
