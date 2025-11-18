using SupermarketPOS.Core.Models;

namespace SupermarketPOS.Core.Interfaces;

/// <summary>
/// ڕووکاری یەکەی کار - Unit of Work Interface
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IRepository<Product> Products { get; }
    IRepository<Category> Categories { get; }
    IRepository<Supplier> Suppliers { get; }
    IRepository<Customer> Customers { get; }
    IRepository<User> Users { get; }
    IRepository<Sale> Sales { get; }
    IRepository<SaleItem> SaleItems { get; }
    IRepository<InventoryTransaction> InventoryTransactions { get; }
    IRepository<UserActivityLog> UserActivityLogs { get; }

    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
