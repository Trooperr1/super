# Supermarket POS System | سیستەمی POS بۆ سوپەرمارکێت

A comprehensive Point of Sale (POS) system built with .NET 8 WPF for supermarkets, featuring full Kurdish language support with RTL layout.

سیستەمێکی تەواوی فرۆشتن (POS) بە .NET 8 WPF بۆ سوپەرمارکێتەکان، لەگەڵ پشتگیری تەواوی زمانی کوردی و ڕێکخستنی RTL.

---

## 📋 Features | تایبەتمەندییەکان

### Core Modules | مۆدیولە سەرەکییەکان

#### 🛒 Sales/Cashier Module | مۆدیولی فرۆشتن/کاشێر
- Barcode scanner integration | یەکخستنی سکانەری بارکۆد
- Manual product search in Kurdish | گەڕانی دەستی بەرهەم بە کوردی
- Shopping cart with live calculations | سەبەتەی کڕین لەگەڵ حیسابکردنی زیندوو
- Multiple payment methods (Cash, Card, Credit) | چەند شێوازێکی پارەدان
- Discount application | جێبەجێکردنی داشکان
- Receipt printing | چاپکردنی پسوڵە
- Return/refund functionality | کرداری گەڕاندنەوە
- Hold/retrieve sales | ڕاگرتن و وەرگرتنەوەی فرۆشتنەکان

#### 📦 Inventory Management | بەڕێوەبردنی مەخزەن
- Product CRUD operations | کردارەکانی CRUD بۆ بەرهەمەکان
- Stock alerts for low inventory | ئاگاداریی مەخزەنی کەم
- Stock adjustment tracking | شوێنکەوتنی ڕێکخستنی مەخزەن
- Category management | بەڕێوەبردنی پۆلێنەکان
- Supplier management | بەڕێوەبردنی دابینکەران
- Barcode generation | دروستکردنی بارکۆد
- Expiry date tracking | شوێنکەوتنی بەرواری بەسەرچوون

#### 👥 Customer Management | بەڕێوەبردنی کڕیارەکان
- Customer registration | تۆمارکردنی کڕیار
- Loyalty points system | سیستەمی خاڵی دڵسۆزی
- Purchase history | مێژووی کڕینەکان
- Credit account management | بەڕێوەبردنی هەژماری قەرز

#### 📊 Reporting Module | مۆدیولی ڕاپۆرتکردن
- Daily/weekly/monthly sales reports | ڕاپۆرتی فرۆشتنی ڕۆژانە/هەفتانە/مانگانە
- Best-selling products | بەرهەمە باشترین فرۆشراوەکان
- Inventory valuation | نرخدانانی مەخزەن
- Cashier performance | ئەدای کاشێرەکان
- Export to PDF/Excel | هەناردەکردن بۆ PDF/Excel

#### 🔐 User Management | بەڕێوەبردنی بەکارهێنەر
- Role-based access (Admin, Manager, Cashier) | دەستگەیشتنی پێگەیی
- Activity logging | لۆگکردنی چالاکی
- Password management | بەڕێوەبردنی وشەی نهێنی
- Session tracking | شوێنکەوتنی دانیشتن

---

## 🛠️ Tech Stack | تەکنەلۆجیاکان

- **.NET 8** - Latest .NET framework
- **WPF** - Windows Presentation Foundation for desktop UI
- **Entity Framework Core 8** - ORM for database operations
- **SQL Server** - Database management system
- **MaterialDesignThemes** - Modern UI components
- **BCrypt.Net** - Password hashing
- **ClosedXML** - Excel file generation

---

## 🚀 Getting Started | دەستپێکردن

### Prerequisites | پێداویستییەکان

Before running the application, ensure you have:
پێش کارپێکردنی بەرنامەکە، دڵنیابە لە هەبوونی:

1. **.NET 8 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
2. **SQL Server** (LocalDB, Express, or Full) - [Download](https://www.microsoft.com/sql-server/sql-server-downloads)
3. **Visual Studio 2022** or **Visual Studio Code** (optional but recommended)
4. **Windows 10/11** - Required for WPF applications

### Installation Steps | هەنگاوەکانی دامەزراندن

#### 1. Clone or Download the Project | دابەزاندن یان کۆپیکردنی پرۆژەکە

```bash
git clone <repository-url>
cd SupermarketPOS
```

#### 2. Configure Database Connection | ڕێکخستنی گرێدان بە بنکەی دراوە

Open `SupermarketPOS.WPF/appsettings.json` and update the connection string:

کردنەوەی `SupermarketPOS.WPF/appsettings.json` و نوێکردنەوەی ستڕینگی گرێدان:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=SupermarketPOS;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

**Connection String Options | هەڵبژاردەکانی ستڕینگی گرێدان:**

For SQL Server Express:
```
Server=localhost\\SQLEXPRESS;Database=SupermarketPOS;Trusted_Connection=True;TrustServerCertificate=True
```

For SQL Server with authentication:
```
Server=localhost;Database=SupermarketPOS;User Id=your_username;Password=your_password;TrustServerCertificate=True
```

#### 3. Restore NuGet Packages | گەڕاندنەوەی پاکێجەکانی NuGet

```bash
dotnet restore
```

#### 4. Create Database Migration | دروستکردنی مایگرەیشنی بنکەی دراوە

```bash
cd SupermarketPOS.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../SupermarketPOS.WPF
```

#### 5. Update Database | نوێکردنەوەی بنکەی دراوە

```bash
dotnet ef database update --startup-project ../SupermarketPOS.WPF
```

**Note:** The application will automatically create the database and seed initial data on first run.

**تێبینی:** بەرنامەکە بە شێوەیەکی ئۆتۆماتیکی بنکەی دراوە دەخوێنێتەوە و داتای سەرەتایی دەچێنێت لە یەکەم جارت.

#### 6. Build the Solution | بیلدکردنی سۆڵیوشن

```bash
cd ..
dotnet build
```

#### 7. Run the Application | کارپێکردنی بەرنامەکە

```bash
cd SupermarketPOS.WPF
dotnet run
```

Or open the solution in Visual Studio and press F5.

یان کردنەوەی سۆڵیوشنەکە لە Visual Studio و دەستی نان لە F5.

---

## 🔑 Default Login Credentials | زانیاری چوونەژوورەوەی سەرەتایی

The system comes with pre-configured users:

سیستەمەکە بە بەکارهێنەرانی پێش-ڕێکخراو دێت:

### Administrator | بەڕێوەبەر
- **Username:** `admin`
- **Password:** `admin123`
- **Role:** Full system access

### Cashier | کاشێر
- **Username:** `cashier`
- **Password:** `cashier123`
- **Role:** POS and basic operations

**⚠️ Important | گرنگ:** Change these passwords after first login for security!

**گرنگ:** ئەم وشە نهێنیانە بگۆڕە دوای یەکەم چوونەژوورەوە بۆ پارێزراوی!

---

## 📂 Project Structure | پێکهاتەی پرۆژە

```
SupermarketPOS/
│
├── SupermarketPOS.Core/              # Domain layer | توێژی دۆمەین
│   ├── Models/                       # Entity models | مۆدێلەکانی هەستی
│   ├── Enums/                        # Enumerations | ژمێرەکان
│   └── Interfaces/                   # Repository interfaces | ڕووکارە مەخزەنەکان
│
├── SupermarketPOS.Infrastructure/    # Data access layer | توێژی دەستگەیشتن بە داتا
│   ├── Data/                         # DbContext & seeder | کۆنتێکست و چاندن
│   └── Repositories/                 # Repository implementations | جێبەجێکردنی مەخزەنەکان
│
├── SupermarketPOS.Application/       # Business logic layer | توێژی لۆژیکی بزنس
│   ├── Services/                     # Business services | خزمەتگوزارییەکان
│   ├── ViewModels/                   # MVVM ViewModels | ڤیومۆدێلەکان
│   └── Helpers/                      # Utility classes | کلاسە یارمەتیدەرەکان
│
└── SupermarketPOS.WPF/               # Presentation layer | توێژی پێشکەش
    ├── Views/                        # XAML views | پەنجەرەکانی XAML
    ├── Resources/                    # Images, icons | وێنە، ئایکۆنەکان
    ├── Localization/                 # Language resources | سەرچاوەکانی زمان
    └── appsettings.json              # Configuration | ڕێکخستنەکان
```

---

## 🗄️ Database Schema | شێمای بنکەی دراوە

### Main Tables | خشتە سەرەکییەکان

- **Products** - Product information with barcode, pricing, stock
- **Categories** - Product categories
- **Suppliers** - Supplier information
- **Customers** - Customer data with loyalty points
- **Users** - System users with roles
- **Sales** - Sales transactions
- **SaleItems** - Individual items in each sale
- **InventoryTransactions** - Stock movement history
- **UserActivityLogs** - Audit trail

---

## ⌨️ Keyboard Shortcuts | کورتبڕەکانی تەختەکلیل

| Shortcut | Action |
|----------|--------|
| `F1` | Open help |
| `F2` | New sale |
| `F3` | Find product |
| `F4` | Customer lookup |
| `F5` | Refresh |
| `F9` | Open settings |
| `Ctrl+S` | Save |
| `Ctrl+P` | Print receipt |
| `Esc` | Cancel/Close |
| `Enter` | Confirm action |

---

## 🎨 Customization | دڵخوازکردن

### Change Company Information | گۆڕینی زانیاری کۆمپانیا

Edit `appsettings.json`:

```json
{
  "AppSettings": {
    "CompanyName": "ناوی کۆمپانیاکەت",
    "Address": "ناونیشانی کۆمپانیاکەت",
    "Phone": "07501234567",
    "TaxRate": 5.0
  }
}
```

### Theme Customization | دڵخوازکردنی ڕووکار

The app uses MaterialDesign themes. To change colors, edit `App.xaml`:

```xml
<materialDesign:BundledTheme BaseTheme="Light"
                           PrimaryColor="DeepPurple"
                           SecondaryColor="Lime" />
```

Available colors: Red, Pink, Purple, DeepPurple, Indigo, Blue, LightBlue, Cyan, Teal, Green, LightGreen, Lime, Yellow, Amber, Orange, DeepOrange, Brown, Grey, BlueGrey

---

## 🔧 Troubleshooting | چارەسەرکردنی کێشەکان

### Database Connection Issues | کێشەکانی گرێدان بە بنکەی دراوە

**Problem:** Cannot connect to SQL Server

**Solutions:**
1. Ensure SQL Server is running
2. Check connection string in appsettings.json
3. For SQL Express, use: `Server=localhost\\SQLEXPRESS`
4. Enable TCP/IP in SQL Server Configuration Manager

**کێشە:** ناتوانێت گرێبدات بە SQL Server

**چارەسەرەکان:**
1. دڵنیابە SQL Server کاردەکات
2. پشکنینی ستڕینگی گرێدان لە appsettings.json
3. بۆ SQL Express، بەکاربێنە: `Server=localhost\\SQLEXPRESS`
4. چالاککردنی TCP/IP لە SQL Server Configuration Manager

### Migration Errors | هەڵەکانی مایگرەیشن

If you get migration errors, try:

```bash
# Delete migrations folder
cd SupermarketPOS.Infrastructure
rm -rf Migrations

# Create new migration
dotnet ef migrations add InitialCreate --startup-project ../SupermarketPOS.WPF

# Update database
dotnet ef database update --startup-project ../SupermarketPOS.WPF
```

### Kurdish Text Display Issues | کێشەکانی نیشاندانی دەقی کوردی

If Kurdish text appears broken:
1. Ensure Windows has Kurdish language pack installed
2. Use fonts: Segoe UI, Calibri, or Arial
3. Check FlowDirection is set to RightToLeft in XAML

---

## 📝 Features Roadmap | نەخشەی تایبەتمەندییەکان

### Implemented ✅
- [x] User authentication and authorization
- [x] Product management
- [x] Sales processing
- [x] Customer management
- [x] Inventory tracking
- [x] Basic reporting
- [x] Kurdish language support with RTL

### Planned 🔄
- [ ] Advanced reporting with charts
- [ ] Barcode label printing
- [ ] Multi-store support
- [ ] Cloud backup
- [ ] Mobile app integration
- [ ] SMS notifications
- [ ] Email receipts
- [ ] Advanced analytics dashboard

---

## 🤝 Contributing | بەشداربوون

Contributions are welcome! Please feel free to submit pull requests or open issues.

بەشداربوون بەخێرهاتووە! تکایە پووڵ ڕیکوێست یان کێشە بنێرە.

---

## 📄 License | مۆڵەت

This project is provided as-is for educational and commercial use.

ئەم پرۆژەیە پێشکەش دەکرێت بۆ بەکارهێنانی پەروەردەیی و بازرگانی.

---

## 📞 Support | پشتگیری

For support and questions:
- Open an issue on GitHub
- Email: support@example.com

بۆ پشتگیری و پرسیارەکان:
- کێشەیەک بکەرەوە لە GitHub
- ئیمەیڵ: support@example.com

---

## 🙏 Acknowledgments | سوپاسگوزاری

- MaterialDesignInXAML for beautiful UI components
- Entity Framework Core team
- Kurdish language community

---

**Built with ❤️ for Kurdish businesses | دروستکراوە بە خۆشەویستی بۆ بزنسە کوردییەکان**

