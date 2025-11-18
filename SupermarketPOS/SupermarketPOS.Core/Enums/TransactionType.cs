namespace SupermarketPOS.Core.Enums;

/// <summary>
/// جۆرەکانی مامەڵە - Transaction Types
/// </summary>
public enum TransactionType
{
    /// <summary>کڕین - Purchase</summary>
    Purchase = 1,

    /// <summary>فرۆشتن - Sale</summary>
    Sale = 2,

    /// <summary>ڕێکخستن - Adjustment</summary>
    Adjustment = 3,

    /// <summary>گەڕاندنەوە - Return</summary>
    Return = 4,

    /// <summary>زیان - Damage</summary>
    Damage = 5
}
