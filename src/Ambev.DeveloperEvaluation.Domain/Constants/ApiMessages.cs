namespace Ambev.DeveloperEvaluation.Domain.Constants
{
  public static class ApiMessages
  {
    // General messages
    public const string ResourceNotFound = "Resource not found";
    public const string ValidationFailed = "Validation Failed";
    public const string OperationSuccessful = "Operation completed successfully";

    // Authentication messages
    public const string UserAuthenticated = "User authenticated successfully";
    public const string InvalidCredentials = "Invalid email or password";
    public const string UserNotFound = "User not found";

    // Sale messages
    public const string SaleNotFound = "Sale with ID {0} not found";
    public const string SaleCreated = "Sale created successfully";
    public const string SaleUpdated = "Sale updated successfully";
    public const string SaleDeleted = "Sale deleted successfully";
    public const string SaleAlreadyCancelled = "Sale is already cancelled";
    public const string SaleCancelled = "Sale cancelled successfully";
    public const string ItemAlreadyCancelled = "Item is already cancelled";
    public const string ItemCancelled = "Item cancelled successfully";
    public const string ItemNotFoundInSale = "Item not found in sale";
    public const string CannotCancelItemFromCancelledSale = "Cannot cancel an item from a cancelled sale";

    // User messages
    public const string UserCreated = "User created successfully";
    public const string UserUpdated = "User updated successfully";
    public const string UserDeleted = "User deleted successfully";

    // Validation messages
    public const string CannotUpdateCancelledSale = "Cannot update a cancelled sale";
  }
}
