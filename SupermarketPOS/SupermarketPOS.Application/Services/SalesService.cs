using SupermarketPOS.Core.Interfaces;
using SupermarketPOS.Core.Models;
using SupermarketPOS.Core.Enums;

namespace SupermarketPOS.Application.Services;

/// <summary>
/// خزمەتگوزاری فرۆشتن - Sales Service
/// </summary>
public class SalesService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ProductService _productService;

    public SalesService(IUnitOfWork unitOfWork, ProductService productService)
    {
        _unitOfWork = unitOfWork;
        _productService = productService;
    }

    /// <summary>
    /// دروستکردنی فرۆشتن - Create Sale
    /// </summary>
    public async Task<(bool Success, string Message, Sale? Sale)> CreateSaleAsync(
        List<SaleItem> items,
        decimal discountAmount,
        decimal discountPercentage,
        decimal taxPercentage,
        PaymentMethod paymentMethod,
        decimal amountPaid,
        int userId,
        int? customerId = null,
        string? notes = null)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            // حیسابکردنی کۆ - Calculate totals
            decimal subtotal = items.Sum(i => i.Total);
            decimal discountTotal = discountAmount + (subtotal * discountPercentage / 100);
            decimal afterDiscount = subtotal - discountTotal;
            decimal taxAmount = afterDiscount * taxPercentage / 100;
            decimal total = afterDiscount + taxAmount;
            decimal change = amountPaid - total;

            if (amountPaid < total && paymentMethod != PaymentMethod.Credit)
            {
                return (false, "بڕی پارە بەس نییە", null);
            }

            // دروستکردنی ژمارەی پسوڵە - Generate invoice number
            string invoiceNumber = await GenerateInvoiceNumberAsync();

            // دروستکردنی فرۆشتن - Create sale
            var sale = new Sale
            {
                InvoiceNumber = invoiceNumber,
                SaleDate = DateTime.Now,
                Subtotal = subtotal,
                DiscountAmount = discountTotal,
                DiscountPercentage = discountPercentage,
                TaxAmount = taxAmount,
                Total = total,
                AmountPaid = amountPaid,
                Change = change,
                PaymentMethod = paymentMethod,
                UserId = userId,
                CustomerId = customerId,
                Notes = notes
            };

            await _unitOfWork.Sales.AddAsync(sale);
            await _unitOfWork.SaveChangesAsync();

            // زیادکردنی بڕگەکان - Add sale items
            foreach (var item in items)
            {
                item.SaleId = sale.Id;
                await _unitOfWork.SaleItems.AddAsync(item);

                // کەمکردنەوەی مەخزەن - Reduce stock
                await _productService.AdjustStockAsync(
                    item.ProductId,
                    item.Quantity,
                    $"فرۆشتن - پسوڵە {invoiceNumber}",
                    TransactionType.Sale,
                    userId);
            }

            // نوێکردنەوەی زانیاری کڕیار - Update customer info
            if (customerId.HasValue)
            {
                var customer = await _unitOfWork.Customers.GetByIdAsync(customerId.Value);
                if (customer != null)
                {
                    customer.TotalPurchases += total;
                    customer.LoyaltyPoints += (int)(total / 1000); // 1 خاڵ بۆ هەر 1000 دینار

                    if (paymentMethod == PaymentMethod.Credit)
                    {
                        customer.CreditBalance += total;
                    }

                    await _unitOfWork.Customers.UpdateAsync(customer);
                }
            }

            await _unitOfWork.CommitTransactionAsync();

            return (true, "فرۆشتن تەواوبوو", sale);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return (false, $"هەڵە: {ex.Message}", null);
        }
    }

    /// <summary>
    /// گەڕاندنەوەی فرۆشتن - Return Sale
    /// </summary>
    public async Task<(bool Success, string Message)> ReturnSaleAsync(int saleId, int userId, string reason)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var sale = await _unitOfWork.Sales.GetByIdAsync(saleId);
            if (sale == null)
            {
                return (false, "فرۆشتن نەدۆزرایەوە");
            }

            // گەڕاندنەوەی مەخزەن - Return stock
            var saleItems = await _unitOfWork.SaleItems.FindAsync(si => si.SaleId == saleId);
            foreach (var item in saleItems)
            {
                await _productService.AdjustStockAsync(
                    item.ProductId,
                    item.Quantity,
                    $"گەڕاندنەوە - پسوڵە {sale.InvoiceNumber}: {reason}",
                    TransactionType.Return,
                    userId);
            }

            // نوێکردنەوەی کڕیار - Update customer
            if (sale.CustomerId.HasValue)
            {
                var customer = await _unitOfWork.Customers.GetByIdAsync(sale.CustomerId.Value);
                if (customer != null)
                {
                    customer.TotalPurchases -= sale.Total;
                    customer.LoyaltyPoints -= (int)(sale.Total / 1000);
                    if (customer.LoyaltyPoints < 0) customer.LoyaltyPoints = 0;

                    await _unitOfWork.Customers.UpdateAsync(customer);
                }
            }

            // سڕینەوەی فرۆشتن - Delete sale
            await _unitOfWork.Sales.DeleteAsync(saleId);

            await _unitOfWork.CommitTransactionAsync();

            return (true, "فرۆشتن گەڕایەوە");
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return (false, $"هەڵە: {ex.Message}");
        }
    }

    /// <summary>
    /// دروستکردنی ژمارەی پسوڵە - Generate Invoice Number
    /// </summary>
    private async Task<string> GenerateInvoiceNumberAsync()
    {
        var today = DateTime.Today;
        var salesCount = await _unitOfWork.Sales.CountAsync(s =>
            s.SaleDate.Date == today);

        return $"INV-{today:yyyyMMdd}-{salesCount + 1:D4}";
    }

    /// <summary>
    /// ڕاپۆرتی فرۆشتنی ڕۆژانە - Daily Sales Report
    /// </summary>
    public async Task<object> GetDailySalesReportAsync(DateTime date)
    {
        var sales = await _unitOfWork.Sales.FindAsync(s => s.SaleDate.Date == date.Date);
        var salesList = sales.ToList();

        return new
        {
            Date = date.ToString("yyyy-MM-dd"),
            TotalSales = salesList.Count,
            TotalAmount = salesList.Sum(s => s.Total),
            TotalDiscount = salesList.Sum(s => s.DiscountAmount),
            TotalTax = salesList.Sum(s => s.TaxAmount),
            CashSales = salesList.Count(s => s.PaymentMethod == PaymentMethod.Cash),
            CardSales = salesList.Count(s => s.PaymentMethod == PaymentMethod.Card),
            CreditSales = salesList.Count(s => s.PaymentMethod == PaymentMethod.Credit)
        };
    }

    /// <summary>
    /// بەرهەمە فرۆشراوەکان - Best Selling Products
    /// </summary>
    public async Task<IEnumerable<object>> GetBestSellingProductsAsync(DateTime startDate, DateTime endDate, int topCount = 10)
    {
        var sales = await _unitOfWork.Sales.FindAsync(s =>
            s.SaleDate.Date >= startDate.Date && s.SaleDate.Date <= endDate.Date);

        var saleIds = sales.Select(s => s.Id).ToList();
        var saleItems = await _unitOfWork.SaleItems.FindAsync(si => saleIds.Contains(si.SaleId));

        var bestSelling = saleItems
            .GroupBy(si => si.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                TotalQuantity = g.Sum(si => si.Quantity),
                TotalRevenue = g.Sum(si => si.Total)
            })
            .OrderByDescending(x => x.TotalQuantity)
            .Take(topCount);

        return bestSelling;
    }
}
