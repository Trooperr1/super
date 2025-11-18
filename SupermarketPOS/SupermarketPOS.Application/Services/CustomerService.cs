using SupermarketPOS.Core.Interfaces;
using SupermarketPOS.Core.Models;

namespace SupermarketPOS.Application.Services;

/// <summary>
/// خزمەتگوزاری کڕیار - Customer Service
/// </summary>
public class CustomerService
{
    private readonly IUnitOfWork _unitOfWork;

    public CustomerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// زیادکردنی کڕیار - Add Customer
    /// </summary>
    public async Task<(bool Success, string Message, Customer? Customer)> AddCustomerAsync(Customer customer)
    {
        // پشکنینی ژمارەی تەلەفۆن - Check phone number
        var existing = await _unitOfWork.Customers.FindAsync(c => c.Phone == customer.Phone);
        if (existing.Any())
        {
            return (false, "ژمارەی تەلەفۆن پێشتر تۆمارکراوە", null);
        }

        await _unitOfWork.Customers.AddAsync(customer);
        await _unitOfWork.SaveChangesAsync();

        return (true, "کڕیار بەسەرکەوتوویی زیادکرا", customer);
    }

    /// <summary>
    /// دۆزینەوەی کڕیار بە ژمارەی تەلەفۆن - Find Customer by Phone
    /// </summary>
    public async Task<Customer?> GetCustomerByPhoneAsync(string phone)
    {
        var customers = await _unitOfWork.Customers.FindAsync(c => c.Phone == phone);
        return customers.FirstOrDefault();
    }

    /// <summary>
    /// نوێکردنەوەی کڕیار - Update Customer
    /// </summary>
    public async Task<(bool Success, string Message)> UpdateCustomerAsync(Customer customer)
    {
        await _unitOfWork.Customers.UpdateAsync(customer);
        await _unitOfWork.SaveChangesAsync();
        return (true, "کڕیار نوێکرایەوە");
    }

    /// <summary>
    /// گەڕان لە کڕیارەکان - Search Customers
    /// </summary>
    public async Task<IEnumerable<Customer>> SearchCustomersAsync(string searchTerm)
    {
        var customers = await _unitOfWork.Customers.FindAsync(c =>
            c.Name.Contains(searchTerm) ||
            c.Phone.Contains(searchTerm));
        return customers;
    }

    /// <summary>
    /// مێژووی کڕینەکانی کڕیار - Customer Purchase History
    /// </summary>
    public async Task<IEnumerable<Sale>> GetCustomerPurchaseHistoryAsync(int customerId)
    {
        var sales = await _unitOfWork.Sales.FindAsync(s => s.CustomerId == customerId);
        return sales.OrderByDescending(s => s.SaleDate);
    }

    /// <summary>
    /// پارەدانی قەرز - Pay Credit
    /// </summary>
    public async Task<(bool Success, string Message)> PayCreditAsync(int customerId, decimal amount)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(customerId);
        if (customer == null)
        {
            return (false, "کڕیار نەدۆزرایەوە");
        }

        if (amount > customer.CreditBalance)
        {
            return (false, "بڕەکە لە قەرزەکە زیاترە");
        }

        customer.CreditBalance -= amount;
        await _unitOfWork.Customers.UpdateAsync(customer);
        await _unitOfWork.SaveChangesAsync();

        return (true, "پارە پێدرا");
    }
}
