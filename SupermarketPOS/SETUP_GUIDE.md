# Setup Guide | ڕێنمایی دامەزراندن

## Quick Start for Developers | دەستپێکردنی خێرا بۆ گەشەپێدەران

### Step 1: Install Prerequisites | هەنگاوی یەکەم: دامەزراندنی پێداویستییەکان

#### 1.1 Install .NET 8 SDK

**Windows:**
1. Download from: https://dotnet.microsoft.com/download/dotnet/8.0
2. Run the installer
3. Verify installation:
```bash
dotnet --version
```

**تکایە:** دەبێت 8.0.0 یان بەرزتر ببینیت

#### 1.2 Install SQL Server

**Option 1: SQL Server Express (Recommended for development)**
1. Download from: https://www.microsoft.com/sql-server/sql-server-downloads
2. Choose "Express" edition
3. Install with default settings
4. Note: Server name will be `localhost\SQLEXPRESS`

**Option 2: SQL Server LocalDB (Lightweight)**
```bash
# Included with Visual Studio 2022
# Or download SQL Server Express LocalDB
```

**Option 3: SQL Server Developer Edition (Full features)**
1. Download from Microsoft
2. Install with default settings
3. Server name will be `localhost` or `.\`

#### 1.3 Install Visual Studio 2022 (Optional but Recommended)

**Community Edition (Free):**
1. Download from: https://visualstudio.microsoft.com/
2. During installation, select:
   - .NET desktop development
   - Data storage and processing

**Or use Visual Studio Code:**
1. Download from: https://code.visualstudio.com/
2. Install C# extension

---

### Step 2: Database Setup | هەنگاوی دووەم: دامەزراندنی بنکەی دراوە

#### 2.1 Verify SQL Server is Running

**Windows Services:**
1. Press `Win + R`
2. Type `services.msc`
3. Find "SQL Server (SQLEXPRESS)" or "SQL Server"
4. Ensure it's "Running"
5. If not, right-click → Start

**Command Line:**
```bash
# Check if SQL Server is accessible
sqlcmd -S localhost\SQLEXPRESS -E -Q "SELECT @@VERSION"
```

#### 2.2 Update Connection String

Open: `SupermarketPOS.WPF/appsettings.json`

**For SQL Server Express:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=SupermarketPOS;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

**For SQL Server (default instance):**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=SupermarketPOS;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

**For SQL Server with username/password:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=SupermarketPOS;User Id=sa;Password=YourPassword;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

**For LocalDB:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SupermarketPOS;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

---

### Step 3: Build and Run | هەنگاوی سێیەم: بیلدکردن و کارپێکردن

#### 3.1 Using Command Line

```bash
# Navigate to project directory
cd SupermarketPOS

# Restore NuGet packages
dotnet restore

# Build the solution
dotnet build

# Navigate to WPF project
cd SupermarketPOS.WPF

# Run the application
dotnet run
```

#### 3.2 Using Visual Studio

1. Open `SupermarketPOS.sln` in Visual Studio
2. Wait for NuGet packages to restore
3. Right-click on `SupermarketPOS.WPF` → Set as Startup Project
4. Press `F5` or click "Start"

#### 3.3 First Run

On first run, the application will:
1. Create the database automatically
2. Run migrations
3. Seed initial data (admin user, sample products, etc.)
4. Open the login window

If you see any errors, check the troubleshooting section below.

---

### Step 4: Login and Test | هەنگاوی چوارەم: چوونەژوورەوە و تاقیکردنەوە

#### 4.1 Login with Default Credentials

**Administrator Account:**
- Username: `admin`
- Password: `admin123`

**Cashier Account:**
- Username: `cashier`
- Password: `cashier123`

#### 4.2 Test Core Features

1. **Dashboard:** View system overview
2. **POS/Sales:** Try adding products to cart
3. **Inventory:** Browse sample products
4. **Customers:** View customer list
5. **Reports:** Check daily sales report

---

## Database Management | بەڕێوەبردنی بنکەی دراوە

### Create Migration

When you modify database models:

```bash
cd SupermarketPOS.Infrastructure

# Create new migration
dotnet ef migrations add YourMigrationName --startup-project ../SupermarketPOS.WPF

# Apply migration
dotnet ef database update --startup-project ../SupermarketPOS.WPF
```

### Reset Database

To start fresh:

```bash
# Drop database
dotnet ef database drop --startup-project ../SupermarketPOS.WPF

# Recreate and migrate
dotnet ef database update --startup-project ../SupermarketPOS.WPF
```

### Backup Database

**SQL Server Management Studio (SSMS):**
1. Connect to your SQL Server
2. Right-click "SupermarketPOS" database
3. Tasks → Back Up
4. Choose destination
5. Click OK

**Command Line:**
```sql
BACKUP DATABASE SupermarketPOS
TO DISK = 'C:\Backups\SupermarketPOS.bak'
WITH FORMAT;
```

---

## Customization Guide | ڕێنمایی دڵخوازکردن

### Change Company Information

Edit `appsettings.json`:

```json
{
  "AppSettings": {
    "CompanyName": "Your Supermarket Name in Kurdish",
    "CompanyNameEn": "Your Supermarket Name in English",
    "Address": "Your Address",
    "Phone": "Your Phone Number",
    "TaxRate": 5.0,
    "Currency": "IQD",
    "ReceiptFooter": "Thank you message"
  }
}
```

### Add More Sample Data

Edit `SupermarketPOS.Infrastructure/Data/DbSeeder.cs`:

```csharp
// Add more products
var products = new List<Product>
{
    new() {
        Barcode = "2001",
        Name = "Your Product Name",
        PurchasePrice = 1000,
        SellingPrice = 1500,
        StockQuantity = 100,
        CategoryId = category1.Id
    },
    // Add more...
};
```

### Change UI Theme

Edit `App.xaml`:

```xml
<!-- Change colors -->
<materialDesign:BundledTheme
    BaseTheme="Light"  <!-- or "Dark" -->
    PrimaryColor="Blue"  <!-- Your choice -->
    SecondaryColor="Amber" />  <!-- Your choice -->
```

Available colors: Red, Pink, Purple, DeepPurple, Indigo, Blue, LightBlue, Cyan, Teal, Green, LightGreen, Lime, Yellow, Amber, Orange, DeepOrange, Brown, Grey, BlueGrey

---

## Troubleshooting | چارەسەرکردنی کێشەکان

### Issue 1: Cannot connect to SQL Server

**Error:** "A network-related or instance-specific error..."

**Solutions:**

1. **Check SQL Server is running:**
   - Open Services (services.msc)
   - Find "SQL Server (SQLEXPRESS)"
   - Start if stopped

2. **Enable TCP/IP:**
   - Open "SQL Server Configuration Manager"
   - SQL Server Network Configuration → Protocols
   - Enable TCP/IP
   - Restart SQL Server

3. **Firewall:**
   - Allow SQL Server through Windows Firewall
   - Default port: 1433

4. **Check connection string:**
   - Verify server name
   - Try: `localhost`, `localhost\SQLEXPRESS`, `(localdb)\mssqllocaldb`

### Issue 2: Migration errors

**Error:** "Build failed" or migration issues

**Solution:**
```bash
# Clean solution
dotnet clean

# Delete bin and obj folders
rm -rf **/bin **/obj

# Delete Migrations folder
rm -rf SupermarketPOS.Infrastructure/Migrations

# Rebuild
dotnet build

# Create fresh migration
cd SupermarketPOS.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../SupermarketPOS.WPF
dotnet ef database update --startup-project ../SupermarketPOS.WPF
```

### Issue 3: Kurdish text not displaying correctly

**Problem:** Kurdish characters appear broken or as boxes

**Solutions:**

1. **Install Kurdish language pack:**
   - Settings → Time & Language → Language
   - Add Kurdish (Sorani) - Arabic script

2. **Check fonts:**
   - System should use: Segoe UI, Calibri, or Arial
   - These fonts support Arabic script

3. **Verify RTL setting:**
   - Windows should have RTL languages enabled

### Issue 4: NuGet package restore failed

**Solution:**
```bash
# Clear NuGet cache
dotnet nuget locals all --clear

# Restore packages
dotnet restore --force
```

### Issue 5: Application won't start

**Check:**
1. .NET 8 SDK is installed: `dotnet --version`
2. SQL Server is running
3. Connection string is correct
4. No port conflicts (if running multiple apps)

**View logs:**
```bash
# Run with detailed logging
dotnet run --verbosity detailed
```

---

## Development Tips | ئامۆژگارییەکانی گەشەپێدان

### Debugging

**Visual Studio:**
- Set breakpoints (F9)
- Start debugging (F5)
- Step over (F10), Step into (F11)

**Check database:**
- Use SQL Server Management Studio (SSMS)
- Or Visual Studio's SQL Server Object Explorer
- View → SQL Server Object Explorer

### Testing Sales Flow

1. Login as cashier
2. Go to POS module
3. Search product by barcode or name
4. Add to cart
5. Apply discount (optional)
6. Complete sale
7. Print receipt

### Adding New Features

**Example: Add new field to Product**

1. Update model: `SupermarketPOS.Core/Models/Product.cs`
```csharp
public string? Brand { get; set; }
```

2. Update DbContext configuration: `SupermarketPOS.Infrastructure/Data/SupermarketDbContext.cs`
```csharp
entity.Property(e => e.Brand).HasMaxLength(100);
```

3. Create migration:
```bash
dotnet ef migrations add AddBrandToProduct --startup-project ../SupermarketPOS.WPF
dotnet ef database update --startup-project ../SupermarketPOS.WPF
```

4. Update UI and ViewModels accordingly

---

## Production Deployment | بڵاوکردنەوەی پرۆداکشن

### Publishing the Application

```bash
# Publish for Windows x64
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true

# Output will be in: SupermarketPOS.WPF/bin/Release/net8.0-windows/win-x64/publish/
```

### Setup for End Users

1. Copy the published folder to target machine
2. Ensure SQL Server is installed
3. Update appsettings.json with production connection string
4. Run the .exe file
5. On first run, database will be created automatically

### Security Checklist

- [ ] Change default passwords
- [ ] Use strong SQL Server password
- [ ] Enable Windows Firewall
- [ ] Regular database backups
- [ ] Restrict user permissions based on roles
- [ ] Keep Windows and .NET updated

---

## Support Resources | سەرچاوەکانی پشتگیری

### Documentation
- .NET 8 Docs: https://docs.microsoft.com/dotnet/
- EF Core: https://docs.microsoft.com/ef/core/
- WPF: https://docs.microsoft.com/dotnet/desktop/wpf/
- Material Design: https://materialdesigninxaml.net/

### Community
- Stack Overflow: Tag your questions with [wpf] [entity-framework-core] [.net-8.0]
- GitHub Issues: For project-specific issues

---

**Happy Coding! | کۆدنەوێسینێکی خۆش!** 🚀
