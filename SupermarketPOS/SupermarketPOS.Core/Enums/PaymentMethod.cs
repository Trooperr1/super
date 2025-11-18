namespace SupermarketPOS.Core.Enums;

/// <summary>
/// شێوازەکانی پارەدان - Payment Methods
/// </summary>
public enum PaymentMethod
{
    /// <summary>کاش - Cash</summary>
    Cash = 1,

    /// <summary>کارت - Card</summary>
    Card = 2,

    /// <summary>قەرز - Credit</summary>
    Credit = 3,

    /// <summary>تێکەڵ - Mixed/Split</summary>
    Mixed = 4
}
