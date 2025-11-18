using Microsoft.EntityFrameworkCore.Storage;
using SupermarketPOS.Core.Interfaces;
using SupermarketPOS.Core.Models;
using SupermarketPOS.Infrastructure.Data;

namespace SupermarketPOS.Infrastructure.Repositories;

/// <summary>
/// جێبەجێکردنی یەکەی کار - Unit of Work Implementation
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly SupermarketDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(SupermarketDbContext context)
    {
        _context = context;

        Products = new Repository<Product>(_context);
        Categories = new Repository<Category>(_context);
        Suppliers = new Repository<Supplier>(_context);
        Customers = new Repository<Customer>(_context);
        Users = new Repository<User>(_context);
        Sales = new Repository<Sale>(_context);
        SaleItems = new Repository<SaleItem>(_context);
        InventoryTransactions = new Repository<InventoryTransaction>(_context);
        UserActivityLogs = new Repository<UserActivityLog>(_context);
    }

    public IRepository<Product> Products { get; }
    public IRepository<Category> Categories { get; }
    public IRepository<Supplier> Suppliers { get; }
    public IRepository<Customer> Customers { get; }
    public IRepository<User> Users { get; }
    public IRepository<Sale> Sales { get; }
    public IRepository<SaleItem> SaleItems { get; }
    public IRepository<InventoryTransaction> InventoryTransactions { get; }
    public IRepository<UserActivityLog> UserActivityLogs { get; }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
