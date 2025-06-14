namespace Ambev.DeveloperEvaluation.Domain.Constants
{
  public static class ValidationMessages
  {
    // Sale related messages
    public const string SaleIdRequired = "Sale ID is required";
    public const string ItemIdRequired = "Item ID is required";
    public const string SaleNumberRequired = "Sale number is required";
    public const string SaleDateRequired = "Sale date is required";
    public const string CustomerRequired = "Customer is required";
    public const string BranchRequired = "Branch is required";
    public const string SaleItemsRequired = "Sale items are required";
    public const string ProductIdRequired = "Product ID is required";
    public const string ProductNameRequired = "Product name is required";
    public const string QuantityGreaterThanZero = "Quantity must be greater than zero";
    public const string QuantityLessThanOrEqualTwenty = "Quantity cannot be greater than 20";
    public const string QuantityMaxLimit = "Quantity cannot be greater than 20";
    public const string UnitPriceGreaterThanZero = "Unit price must be greater than zero";
    public const string DiscountCannotBeNegative = "Discount cannot be negative";
    public const string DiscountMaxLimit = "Discount cannot be greater than 100%";
    public const string PageGreaterThanZero = "Page must be greater than zero";
    public const string PageSizeBetweenOneAndHundred = "Page size must be between 1 and 100";
    public const string StartDateLessThanEndDate = "Start date must be less than or equal to end date";
    public const string PageSizeGreaterThanZero = "Page size must be greater than zero";
    public const string PageSizeLessThanOrEqualHundred = "Page size cannot be greater than 100";

    // User related messages
    public const string UserIdRequired = "User ID is required";
    public const string EmailRequired = "Email is required";
    public const string EmailInvalidFormat = "The provided email address is not valid.";
    public const string EmailMaxLength = "The email address cannot be longer than 100 characters.";
    public const string UsernameRequired = "Username is required";
    public const string UsernameMinLength = "Username must be at least 3 characters long";
    public const string UsernameMaxLength = "Username cannot be longer than 50 characters";
    public const string PasswordRequired = "Password is required";
    public const string PasswordMinLength = "Password must be at least 8 characters long";
    public const string PasswordUppercaseRequired = "Password must contain at least one uppercase letter";
    public const string PasswordLowercaseRequired = "Password must contain at least one lowercase letter";
    public const string PasswordNumberRequired = "Password must contain at least one number";
    public const string PasswordSpecialCharRequired = "Password must contain at least one special character";
    public const string PhoneRequired = "Phone is required";
    public const string PhoneInvalidFormat = "Phone number must start with '+' followed by 11-15 digits";
    public const string UserStatusInvalid = "User status cannot be Unknown";
    public const string UserRoleInvalid = "User role cannot be None";

    // Cancellation validation messages
    public const string CancellationReasonRequired = "Cancellation reason is required";
    public const string CancellationReasonMaxLength = "Cancellation reason must have a maximum of 500 characters";
  }
}
