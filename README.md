, # RevenueCat.NET

[![NuGet](https://img.shields.io/nuget/v/RevenueCat.NET.svg)](https://www.nuget.org/packages/RevenueCat.NET/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A professional, production-ready .NET 8 client library for the RevenueCat REST API v2. Built with Refit for type-safe, reliable HTTP communication.

## Features

- **Complete API Coverage**: Full implementation of RevenueCat REST API v2 with 100+ endpoints
- **Built with Refit**: Type-safe HTTP client with automatic request/response handling
- **Modern .NET 8**: Built with latest C# features (primary constructors, records, file-scoped namespaces)
- **SOLID Principles**: Clean architecture with dependency injection support
- **Type-Safe**: Strong typing with nullable reference types and comprehensive models
- **Async/Await**: Fully asynchronous API with cancellation token support
- **Resilient**: Built-in retry logic, rate limiting handling, and timeout management
- **Performance**: Connection pooling, HTTP compression, and efficient JSON serialization
- **Extensible**: Interface-based design for easy testing and mocking
- **Well-Documented**: Comprehensive XML documentation and usage examples

## Installation

```bash
dotnet add package RevenueCat.NET
```

## Quick Start

```csharp
using RevenueCat.NET;

var client = new RevenueCatClient("your_v2_api_key");

var projects = await client.Projects.ListAsync();

var customers = await client.Customers.ListAsync("proj_abc123");

var customer = await client.Customers.GetAsync(
    "proj_abc123", 
    "customer_id",
    expand: new[] { "attributes" }
);
```

## Configuration

```csharp
var client = new RevenueCatClient("your_api_key", options =>
{
    options.Timeout = TimeSpan.FromSeconds(60);
    options.MaxRetryAttempts = 5;
    options.RetryDelay = TimeSpan.FromSeconds(1);
    options.EnableRetryOnRateLimit = true;
});
```

## Usage Examples

### Projects

```csharp
var projects = await client.Projects.ListAsync(limit: 20);
```

### Apps

```csharp
var apps = await client.Apps.ListAsync("proj_abc123");

var app = await client.Apps.CreateAsync("proj_abc123", new CreateAppRequest(
    Name: "My iOS App",
    Type: AppType.AppStore,
    AppStore: new AppStoreConfig(
        BundleId: "com.example.app",
        SharedSecret: "your_shared_secret"
    )
));
```

### Customers

```csharp
// Create a customer with attributes
var customer = await client.Customers.CreateAsync("proj_abc123", new CreateCustomerRequest(
    Id: "user_12345",
    Attributes: new[]
    {
        new CustomerAttributeInput("$email", "user@example.com"),
        new CustomerAttributeInput("$displayName", "John Doe")
    }
));

// Grant an entitlement to a customer
await client.Customers.GrantEntitlementAsync("proj_abc123", "customer_id",
    new GrantEntitlementRequest(
        EntitlementId: "ent_premium",
        ExpiresAt: DateTimeOffset.UtcNow.AddMonths(1).ToUnixTimeMilliseconds()
    ));

// Revoke a granted entitlement
await client.Customers.RevokeGrantedEntitlementAsync("proj_abc123", "customer_id",
    new RevokeGrantedEntitlementRequest(EntitlementId: "ent_premium"));

// Assign an offering override
await client.Customers.AssignOfferingAsync("proj_abc123", "customer_id",
    new AssignOfferingRequest(OfferingId: "offering_special"));

// Transfer customer data
await client.Customers.TransferAsync("proj_abc123", "old_customer_id", 
    new TransferCustomerRequest("new_customer_id"));
```

### Products

```csharp
var products = await client.Products.ListAsync(
    "proj_abc123",
    appId: "app_xyz789",
    expand: new[] { "items.app" }
);

var product = await client.Products.CreateAsync("proj_abc123", new CreateProductRequest(
    StoreIdentifier: "com.example.premium.monthly",
    AppId: "app_xyz789",
    Type: ProductType.Subscription,
    DisplayName: "Premium Monthly"
));
```

### Entitlements

```csharp
var entitlement = await client.Entitlements.CreateAsync("proj_abc123", 
    new CreateEntitlementRequest(
        LookupKey: "premium",
        DisplayName: "Premium Access"
    ));

await client.Entitlements.AttachProductsAsync("proj_abc123", "ent_abc123",
    new AttachProductsRequest(new[] { "prod_123", "prod_456" }));
```

### Offerings & Packages

```csharp
var offering = await client.Offerings.CreateAsync("proj_abc123",
    new CreateOfferingRequest(
        LookupKey: "default",
        DisplayName: "Default Offering",
        IsDefault: true
    ));

var package = await client.Packages.CreateAsync("proj_abc123", "offering_id",
    new CreatePackageRequest(
        LookupKey: "monthly",
        DisplayName: "Monthly Package",
        ProductId: "prod_123",
        Position: 1
    ));
```

### Subscriptions

```csharp
var subscriptions = await client.Subscriptions.ListAsync("proj_abc123", "customer_id");

await client.Subscriptions.CancelAsync("proj_abc123", "customer_id", "sub_id");

await client.Subscriptions.RefundAsync("proj_abc123", "customer_id", "sub_id");
```

### Purchases

```csharp
var purchases = await client.Purchases.ListAsync("proj_abc123", "customer_id");

await client.Purchases.RefundAsync("proj_abc123", "customer_id", "purchase_id");
```

### Charts & Metrics

```csharp
var metrics = await client.Charts.GetMetricsAsync(
    "proj_abc123",
    ChartMetricType.Revenue,
    startDate: 1704067200000, // Unix timestamp
    endDate: 1735689600000,
    appId: "app_123"
);
```

## Error Handling

```csharp
try
{
    var customer = await client.Customers.GetAsync("proj_abc123", "customer_id");
}
catch (NotFoundException ex)
{
    Console.WriteLine($"Customer not found: {ex.Message}");
}
catch (RateLimitException ex)
{
    Console.WriteLine($"Rate limited. Retry after: {ex.ErrorResponse?.BackoffMs}ms");
}
catch (RevenueCatException ex)
{
    Console.WriteLine($"API error: {ex.Message}");
    Console.WriteLine($"Error type: {ex.ErrorResponse?.Type}");
}
```

## Dependency Injection

```csharp
services.AddSingleton<IRevenueCatClient>(sp => 
    new RevenueCatClient("your_api_key"));
```

## API Coverage

### Core Resources
- ✅ **Projects** - List and manage projects
- ✅ **Apps** - Full CRUD operations for all store types
- ✅ **Customers** - Complete customer lifecycle management
- ✅ **Products** - Product catalog management
- ✅ **Entitlements** - Entitlement configuration and product attachment
- ✅ **Offerings** - Offering management with metadata support
- ✅ **Packages** - Package configuration with eligibility criteria
- ✅ **Paywalls** - Paywall creation and management

### Transactions & Billing
- ✅ **Subscriptions** - List, search, cancel, refund operations
- ✅ **Purchases** - One-time purchase management and refunds
- ✅ **Invoices** - Invoice retrieval and file access
- ✅ **Virtual Currency** - Balance management and transactions

### Analytics & Search
- ✅ **Charts & Metrics** - Revenue, MRR, ARR, churn, and more
- ✅ **Search** - Search subscriptions and purchases by store identifiers

### Advanced Features
- ✅ **Customer Transfer** - Transfer data between customers
- ✅ **Customer Attributes** - Manage custom attributes
- ✅ **Customer Aliases** - List customer aliases
- ✅ **Active Entitlements** - Query customer entitlements
- ✅ **Grant/Revoke Entitlements** - Grant and revoke promotional entitlements
- ✅ **Assign Offering** - Override customer offerings
- ✅ **Authenticated Management URLs** - Generate customer portal links
- ✅ **Store Product Creation** - Push products to App Store Connect
- ✅ **Expandable Fields** - Reduce API calls with field expansion
- ✅ **Pagination** - Efficient handling of large datasets

### Supported Stores
- ✅ Apple App Store
- ✅ Apple Mac App Store
- ✅ Google Play Store
- ✅ Amazon Appstore
- ✅ Stripe
- ✅ RevenueCat Billing (Web)
- ✅ Roku
- ✅ Paddle
- ✅ Promotional

## Examples

Comprehensive examples are available in the [`examples/`](examples/) directory:

- **[BasicUsage](examples/BasicUsage/)** - Quick start guide
- **[CustomerManagement](examples/CustomerManagement/)** - Customer CRUD, attributes, transfer
- **[SubscriptionManagement](examples/SubscriptionManagement/)** - Subscription lifecycle, cancel, refund
- **[ProductCatalog](examples/ProductCatalog/)** - Products, entitlements, offerings, packages
- **[ErrorHandling](examples/ErrorHandling/)** - Error handling patterns and retry logic
- **[VirtualCurrency](examples/VirtualCurrency/)** - Virtual currency management
- **[Pagination](examples/Pagination/)** - Efficient pagination techniques

See the [examples README](examples/README.md) for detailed information.

## Advanced Usage

### Expandable Fields

Reduce API calls by expanding related resources:

```csharp
var customer = await client.Customers.GetAsync(
    projectId,
    customerId,
    expand: new[] { "attributes", "active_entitlements" }
);

// Access expanded data directly
foreach (var attr in customer.Attributes.Items)
{
    Console.WriteLine($"{attr.Key}: {attr.Value}");
}
```

### Pagination

Efficiently handle large datasets:

```csharp
string? startingAfter = null;
do
{
    var page = await client.Customers.ListAsync(
        projectId,
        limit: 100,
        startingAfter: startingAfter
    );
    
    // Process page.Items
    
    // Get cursor for next page
    if (page.NextPage != null)
    {
        var uri = new Uri(page.NextPage);
        var query = HttpUtility.ParseQueryString(uri.Query);
        startingAfter = query["starting_after"];
    }
    else
    {
        startingAfter = null;
    }
} while (startingAfter != null);
```

### Search Operations

Search by store identifiers:

```csharp
// Search subscriptions
var subscriptions = await client.Subscriptions.SearchSubscriptionsAsync(
    projectId,
    storeSubscriptionIdentifier: "GPA.1234-5678-9012-34567"
);

// Search purchases
var purchases = await client.Purchases.SearchPurchasesAsync(
    projectId,
    storePurchaseIdentifier: "1000000123456789"
);
```

### Virtual Currency

Manage in-app currencies:

```csharp
// Add currency (multiple currencies at once)
var balances = await client.Customers.CreateVirtualCurrencyTransactionAsync(
    projectId,
    customerId,
    new CreateVirtualCurrencyTransactionRequest(
        Adjustments: new Dictionary<string, int>
        {
            { "GEMS", 100 },
            { "COINS", 500 }
        },
        Reference: "purchase_reward"
    ),
    idempotencyKey: "unique-key"
);

// Update balance directly (without transaction record)
await client.Customers.UpdateVirtualCurrencyBalanceAsync(
    projectId,
    customerId,
    new UpdateVirtualCurrencyBalanceRequest(
        Adjustments: new Dictionary<string, int>
        {
            { "GEMS", 50 }  // Set absolute balance
        }
    ),
    idempotencyKey: "another-unique-key"
);

// List all balances
var allBalances = await client.Customers.ListVirtualCurrencyBalancesAsync(
    projectId,
    customerId,
    includeEmptyBalances: true
);
```

### Customer Transfer

Transfer data between customers:

```csharp
var transfer = await client.Customers.TransferAsync(
    projectId,
    sourceCustomerId,
    new TransferCustomerRequest(
        TargetCustomerId: targetCustomerId,
        AppIds: new[] { "app_123" } // Optional: filter by apps
    )
);
```

## API Coverage Matrix

| Resource | List | Get | Create | Update | Delete | Actions |
|----------|------|-----|--------|--------|--------|---------|
| Projects | ✅ | - | - | - | - | - |
| Apps | ✅ | ✅ | ✅ | ✅ | ✅ | Get StoreKit Config, List API Keys |
| Customers | ✅ | ✅ | ✅ | - | ✅ | Transfer, Grant/Revoke Entitlement, Assign Offering, Manage Attributes |
| Products | ✅ | ✅ | ✅ | - | ✅ | Create in Store |
| Entitlements | ✅ | ✅ | ✅ | ✅ | ✅ | Attach/Detach Products |
| Offerings | ✅ | ✅ | ✅ | ✅ | ✅ | Set Default |
| Packages | ✅ | ✅ | ✅ | ✅ | ✅ | Attach/Detach Products |
| Paywalls | - | - | ✅ | - | - | - |
| Subscriptions | ✅ | ✅ | - | - | - | Cancel, Refund, Get Management URL, List Transactions |
| Purchases | ✅ | ✅ | - | - | - | Refund |
| Invoices | ✅ | - | - | - | - | Get File URL |
| Charts | - | ✅ | - | - | - | Get Overview Metrics |
| Virtual Currency | ✅ | - | ✅ | ✅ | - | - |

## Requirements

- .NET 8.0 or higher
- RevenueCat API v2 key

## Migration from v1.x

See [MIGRATION.md](MIGRATION.md) for detailed migration instructions from version 1.x to 2.0.

## License

MIT License - see [LICENSE](LICENSE) file for details

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## Author

**frknlkn** - [GitHub](https://github.com/frknlkn)

## Support

For issues and questions:
- GitHub Issues: [Create an issue](https://github.com/frknlkn/revenuecat-dotnet/issues)
- RevenueCat Documentation: [https://www.revenuecat.com/docs/api-v2](https://www.revenuecat.com/docs/api-v2)
- Examples: [examples/](examples/)
