# Database Schema Documentation | دۆکیومێنتی شێمای بنکەی دراوە

## Overview | گشتی

The SupermarketPOS system uses SQL Server with Entity Framework Core for data persistence. The database is designed to support comprehensive supermarket operations including sales, inventory, customer management, and reporting.

سیستەمی SupermarketPOS بەکارهێنانی SQL Server لەگەڵ Entity Framework Core دەکات بۆ مانەوەی داتا. بنکەی دراوەکە دیزاینکراوە بۆ پشتگیری کارە گشتییەکانی سوپەرمارکێت لەوانە فرۆشتن، مەخزەن، بەڕێوەبردنی کڕیار، و ڕاپۆرتکردن.

---

## Tables | خشتەکان

### 1. Users | بەکارهێنەران

Stores system users with authentication and authorization information.

**Columns:**
- `Id` (int, PK) - Unique identifier
- `Username` (nvarchar(50), Unique, Required) - Login username
- `PasswordHash` (nvarchar(MAX), Required) - BCrypt hashed password
- `FullName` (nvarchar(200), Required) - User's full name
- `Role` (int, Required) - User role (1=Admin, 2=Manager, 3=Cashier)
- `Phone` (nvarchar(20)) - Contact phone number
- `IsActive` (bit, Default: 1) - Account active status
- `LastLogin` (datetime2) - Last login timestamp
- `CreatedAt` (datetime2, Default: GETDATE()) - Record creation date
- `UpdatedAt` (datetime2) - Last update date
- `IsDeleted` (bit, Default: 0) - Soft delete flag

**Indexes:**
- Unique index on `Username`

**Kurdish Fields:**
- ناوی بەکارهێنەر (Username)
- وشەی نهێنی (PasswordHash)
- ناوی تەواو (FullName)
- ڕۆڵ (Role)

---

### 2. Categories | پۆلێنەکان

Product categories for organization and reporting.

**Columns:**
- `Id` (int, PK) - Unique identifier
- `Name` (nvarchar(100), Required) - Category name
- `Description` (nvarchar(MAX)) - Category description
- `CreatedAt` (datetime2) - Record creation date
- `UpdatedAt` (datetime2) - Last update date
- `IsDeleted` (bit, Default: 0) - Soft delete flag

**Relationships:**
- One-to-Many with Products

---

### 3. Suppliers | دابینکەران

Supplier information for inventory management.

**Columns:**
- `Id` (int, PK) - Unique identifier
- `Name` (nvarchar(200), Required) - Supplier name
- `Phone` (nvarchar(20), Required) - Contact phone
- `Email` (nvarchar(100)) - Email address
- `Address` (nvarchar(MAX)) - Physical address
- `TotalDebt` (decimal(18,2), Default: 0) - Outstanding debt
- `Notes` (nvarchar(MAX)) - Additional notes
- `CreatedAt` (datetime2) - Record creation date
- `UpdatedAt` (datetime2) - Last update date
- `IsDeleted` (bit, Default: 0) - Soft delete flag

**Relationships:**
- One-to-Many with Products

---

### 4. Products | بەرهەمەکان

Product catalog with pricing and inventory information.

**Columns:**
- `Id` (int, PK) - Unique identifier
- `Barcode` (nvarchar(50), Unique, Required) - Product barcode
- `Name` (nvarchar(200), Required) - Product name
- `PurchasePrice` (decimal(18,2), Required) - Cost price
- `SellingPrice` (decimal(18,2), Required) - Retail price
- `StockQuantity` (int, Default: 0) - Current stock level
- `MinimumStockLevel` (int, Default: 10) - Reorder point
- `ExpiryDate` (datetime2) - Product expiration date
- `ImagePath` (nvarchar(MAX)) - Product image location
- `Description` (nvarchar(MAX)) - Product description
- `CategoryId` (int, FK, Required) - Foreign key to Categories
- `SupplierId` (int, FK) - Foreign key to Suppliers
- `CreatedAt` (datetime2) - Record creation date
- `UpdatedAt` (datetime2) - Last update date
- `IsDeleted` (bit, Default: 0) - Soft delete flag

**Indexes:**
- Unique index on `Barcode`
- Index on `CategoryId`
- Index on `SupplierId`

**Relationships:**
- Many-to-One with Categories (Required)
- Many-to-One with Suppliers (Optional)
- One-to-Many with SaleItems
- One-to-Many with InventoryTransactions

**Kurdish Fields:**
- بارکۆد (Barcode)
- ناو (Name)
- نرخی کڕین (PurchasePrice)
- نرخی فرۆشتن (SellingPrice)
- بڕی مەخزەن (StockQuantity)

---

### 5. Customers | کڕیارەکان

Customer information and loyalty program data.

**Columns:**
- `Id` (int, PK) - Unique identifier
- `Name` (nvarchar(200), Required) - Customer name
- `Phone` (nvarchar(20), Required) - Contact phone
- `Email` (nvarchar(100)) - Email address
- `Address` (nvarchar(MAX)) - Physical address
- `LoyaltyPoints` (int, Default: 0) - Accumulated loyalty points
- `TotalPurchases` (decimal(18,2), Default: 0) - Lifetime purchase value
- `CreditBalance` (decimal(18,2), Default: 0) - Outstanding credit
- `Notes` (nvarchar(MAX)) - Additional notes
- `CreatedAt` (datetime2) - Record creation date
- `UpdatedAt` (datetime2) - Last update date
- `IsDeleted` (bit, Default: 0) - Soft delete flag

**Indexes:**
- Index on `Phone`

**Relationships:**
- One-to-Many with Sales

**Business Rules:**
- 1 loyalty point per 1000 IQD spent
- Credit balance can be paid down over time

---

### 6. Sales | فرۆشتنەکان

Sales transaction header information.

**Columns:**
- `Id` (int, PK) - Unique identifier
- `InvoiceNumber` (nvarchar(50), Unique, Required) - Invoice number
- `SaleDate` (datetime2, Required, Default: GETDATE()) - Sale timestamp
- `Subtotal` (decimal(18,2), Required) - Sum before discounts/tax
- `DiscountAmount` (decimal(18,2), Default: 0) - Total discount applied
- `DiscountPercentage` (decimal(5,2), Default: 0) - Percentage discount
- `TaxAmount` (decimal(18,2), Default: 0) - Tax amount
- `Total` (decimal(18,2), Required) - Final total
- `AmountPaid` (decimal(18,2), Required) - Amount tendered
- `Change` (decimal(18,2), Default: 0) - Change returned
- `PaymentMethod` (int, Required) - Payment type (1=Cash, 2=Card, 3=Credit, 4=Mixed)
- `Notes` (nvarchar(MAX)) - Additional notes
- `UserId` (int, FK, Required) - Foreign key to Users (cashier)
- `CustomerId` (int, FK) - Foreign key to Customers
- `CreatedAt` (datetime2) - Record creation date
- `UpdatedAt` (datetime2) - Last update date
- `IsDeleted` (bit, Default: 0) - Soft delete flag

**Indexes:**
- Unique index on `InvoiceNumber`
- Index on `SaleDate`
- Index on `UserId`
- Index on `CustomerId`

**Relationships:**
- Many-to-One with Users (Required)
- Many-to-One with Customers (Optional)
- One-to-Many with SaleItems

**Invoice Number Format:** `INV-YYYYMMDD-####`
Example: INV-20250115-0001

---

### 7. SaleItems | بڕگەکانی فرۆشتن

Individual line items for each sale.

**Columns:**
- `Id` (int, PK) - Unique identifier
- `SaleId` (int, FK, Required) - Foreign key to Sales
- `ProductId` (int, FK, Required) - Foreign key to Products
- `Quantity` (int, Required) - Quantity sold
- `UnitPrice` (decimal(18,2), Required) - Price per unit at time of sale
- `Subtotal` (decimal(18,2), Required) - Quantity * UnitPrice
- `Discount` (decimal(18,2), Default: 0) - Discount on this item
- `Total` (decimal(18,2), Required) - Final line total
- `CreatedAt` (datetime2) - Record creation date
- `UpdatedAt` (datetime2) - Last update date
- `IsDeleted` (bit, Default: 0) - Soft delete flag

**Indexes:**
- Index on `SaleId`
- Index on `ProductId`

**Relationships:**
- Many-to-One with Sales (Cascade delete)
- Many-to-One with Products

**Calculations:**
- Subtotal = Quantity × UnitPrice
- Total = Subtotal - Discount

---

### 8. InventoryTransactions | مامەڵەکانی مەخزەن

Audit trail for all inventory movements.

**Columns:**
- `Id` (int, PK) - Unique identifier
- `ProductId` (int, FK, Required) - Foreign key to Products
- `TransactionType` (int, Required) - Type (1=Purchase, 2=Sale, 3=Adjustment, 4=Return, 5=Damage)
- `Quantity` (int, Required) - Quantity moved
- `PreviousStock` (int, Required) - Stock before transaction
- `NewStock` (int, Required) - Stock after transaction
- `Reason` (nvarchar(200), Required) - Reason for transaction
- `Notes` (nvarchar(MAX)) - Additional details
- `UserId` (int, FK, Required) - User who made the transaction
- `CreatedAt` (datetime2) - Transaction timestamp
- `UpdatedAt` (datetime2) - Last update date
- `IsDeleted` (bit, Default: 0) - Soft delete flag

**Indexes:**
- Index on `ProductId`
- Index on `UserId`
- Index on `CreatedAt`

**Relationships:**
- Many-to-One with Products
- Many-to-One with Users

**Transaction Types:**
1. Purchase - Stock added from supplier
2. Sale - Stock reduced from sale
3. Adjustment - Manual stock correction
4. Return - Stock added from customer return
5. Damage - Stock removed due to damage/expiry

---

### 9. UserActivityLogs | لۆگی چالاکی بەکارهێنەر

Audit log for user actions in the system.

**Columns:**
- `Id` (int, PK) - Unique identifier
- `UserId` (int, FK, Required) - Foreign key to Users
- `Activity` (nvarchar(200), Required) - Activity description
- `Details` (nvarchar(MAX)) - Additional details
- `IpAddress` (nvarchar(50)) - IP address of client
- `CreatedAt` (datetime2) - Activity timestamp
- `UpdatedAt` (datetime2) - Last update date
- `IsDeleted` (bit, Default: 0) - Soft delete flag

**Indexes:**
- Index on `UserId`
- Index on `CreatedAt`

**Relationships:**
- Many-to-One with Users (Cascade delete)

**Common Activities:**
- چوونەژوورەوە (Login)
- چوونەدەرەوە (Logout)
- گۆڕینی وشەی نهێنی (Password change)
- دروستکردنی فرۆشتن (Sale created)
- دەستکاری بەرهەم (Product modified)

---

## Entity Relationships | پەیوەندییەکانی هەستی

```
Users (1) ──→ (N) Sales
Users (1) ──→ (N) InventoryTransactions
Users (1) ──→ (N) UserActivityLogs

Categories (1) ──→ (N) Products

Suppliers (1) ──→ (N) Products

Products (1) ──→ (N) SaleItems
Products (1) ──→ (N) InventoryTransactions

Customers (1) ──→ (N) Sales

Sales (1) ──→ (N) SaleItems
```

---

## Soft Delete Pattern | نەخشەی سڕینەوەی نەرم

All tables implement soft delete using the `IsDeleted` flag:
- Records are never physically deleted
- `IsDeleted = 1` marks a record as deleted
- Query filters automatically exclude deleted records
- Can be restored by setting `IsDeleted = 0`

هەموو خشتەکان سڕینەوەی نەرم جێبەجێ دەکەن بە بەکارهێنانی `IsDeleted`:
- تۆمارەکان هەرگیز بە فیزیکی ناسڕێنەوە
- `IsDeleted = 1` تۆمارەکە وەک سڕاوە نیشان دەکات
- فلتەرەکانی پرسیار بە ئۆتۆماتیکی تۆمارە سڕاوەکان دەردەهێنن
- دەتوانرێت بگەڕێندرێتەوە بە دانانی `IsDeleted = 0`

---

## Indexes and Performance | ئیندێکسەکان و ئەدا

**Primary Indexes (Automatically Created):**
- All tables have clustered index on `Id`

**Unique Indexes:**
- `Users.Username`
- `Products.Barcode`
- `Sales.InvoiceNumber`

**Foreign Key Indexes:**
- Automatically created for all FK relationships

**Performance Indexes:**
- `Sales.SaleDate` - For date-range reports
- `Customers.Phone` - For customer lookup
- `InventoryTransactions.CreatedAt` - For transaction history
- `UserActivityLogs.CreatedAt` - For audit reports

---

## Security Considerations | بیرکردنەوەکانی پارێزراوی

1. **Password Storage:**
   - Passwords hashed using BCrypt (cost factor 11)
   - Never store plain text passwords

2. **Connection String:**
   - Store in appsettings.json (excluded from source control)
   - Use Windows Authentication when possible
   - Encrypt connection strings in production

3. **Data Access:**
   - All queries use parameterized SQL (via EF Core)
   - Protection against SQL injection
   - Role-based access control enforced at application layer

4. **Audit Trail:**
   - All user actions logged in UserActivityLogs
   - Inventory changes tracked in InventoryTransactions
   - Soft delete preserves data for auditing

---

## Backup and Maintenance | پاڵپشت و چاککردنەوە

**Recommended Backup Schedule:**
- Full backup: Daily at midnight
- Differential backup: Every 6 hours
- Transaction log backup: Every hour (Full recovery model)

**Maintenance Tasks:**
- Rebuild indexes: Weekly
- Update statistics: Weekly
- Check database integrity: Daily
- Archive old logs: Monthly (keep last 12 months)

**تابلۆی پێشنیارکراوی پاڵپشت:**
- پاڵپشتی تەواو: ڕۆژانە لە نیوەشەودا
- پاڵپشتی جیاواز: هەر ٦ کاتژمێر جارێک
- پاڵپشتی لۆگی مامەڵە: هەر کاتژمێرێک (مۆدێلی گەڕانەوەی تەواو)

---

## Database Size Estimates | خەمڵاندنی قەبارەی بنکەی دراوە

**Small Store (< 50 transactions/day):**
- Initial: ~50 MB
- After 1 year: ~500 MB

**Medium Store (50-200 transactions/day):**
- Initial: ~50 MB
- After 1 year: ~2 GB

**Large Store (> 200 transactions/day):**
- Initial: ~50 MB
- After 1 year: ~5 GB

**Note:** Regular archiving and purging of old data recommended.

---

## Sample Queries | پرسیارە نموونەییەکان

### Get Top Selling Products (Last 30 Days)

```sql
SELECT TOP 10
    p.Name,
    SUM(si.Quantity) as TotalQuantity,
    SUM(si.Total) as TotalRevenue
FROM SaleItems si
JOIN Products p ON si.ProductId = p.Id
JOIN Sales s ON si.SaleId = s.Id
WHERE s.SaleDate >= DATEADD(day, -30, GETDATE())
    AND s.IsDeleted = 0
    AND si.IsDeleted = 0
GROUP BY p.Name
ORDER BY TotalQuantity DESC
```

### Get Low Stock Products

```sql
SELECT
    p.Name,
    p.Barcode,
    p.StockQuantity,
    p.MinimumStockLevel,
    c.Name as CategoryName
FROM Products p
JOIN Categories c ON p.CategoryId = c.Id
WHERE p.StockQuantity <= p.MinimumStockLevel
    AND p.IsDeleted = 0
ORDER BY p.StockQuantity ASC
```

### Daily Sales Report

```sql
SELECT
    COUNT(*) as TotalTransactions,
    SUM(Total) as TotalRevenue,
    SUM(DiscountAmount) as TotalDiscounts,
    AVG(Total) as AverageTransaction
FROM Sales
WHERE CAST(SaleDate AS DATE) = CAST(GETDATE() AS DATE)
    AND IsDeleted = 0
```

---

## Migration History | مێژووی مایگرەیشن

**v1.0.0 - InitialCreate**
- Created all core tables
- Established relationships
- Added indexes
- Configured soft delete

**Future Migrations:**
- Will be documented here as schema evolves

---

**Database Version:** 1.0.0
**Last Updated:** 2025
**Compatibility:** SQL Server 2016+, Azure SQL Database

