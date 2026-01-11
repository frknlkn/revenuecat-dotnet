using Refit;
using RevenueCat.NET.Models;
using RevenueCat.NET.Models.Common;
using RevenueCat.NET.Models.Customers;
using RevenueCat.NET.Models.Subscriptions;
using RevenueCat.NET.Models.Purchases;
using RevenueCat.NET.Models.Invoices;

namespace RevenueCat.NET.Services;

/// <summary>
/// Service for managing customers in RevenueCat.
/// </summary>
/// <remarks>
/// API Documentation: <see href="https://www.revenuecat.com/docs/api-v2#tag/Customers"/>
/// </remarks>
public interface ICustomerService
{
    #region Customer CRUD Operations

    /// <summary>
    /// Lists all customers for a project.
    /// </summary>
    [Get("/v2/projects/{projectId}/customers")]
    Task<ListResponse<Customer>> ListAsync(
        string projectId,
        [Query] int? limit = null,
        [AliasAs("starting_after")] [Query] string? startingAfter = null,
        [Query] string? search = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a specific customer by ID.
    /// </summary>
    [Get("/v2/projects/{projectId}/customers/{customerId}")]
    Task<Customer> GetAsync(
        string projectId,
        string customerId,
        [Query(CollectionFormat.Multi)] string[]? expand = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new customer.
    /// </summary>
    [Post("/v2/projects/{projectId}/customers")]
    Task<Customer> CreateAsync(
        string projectId,
        [Body] CreateCustomerRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a customer permanently.
    /// </summary>
    [Delete("/v2/projects/{projectId}/customers/{customerId}")]
    Task<DeletedObject> DeleteAsync(
        string projectId,
        string customerId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Transfers customer data from one customer to another.
    /// </summary>
    [Post("/v2/projects/{projectId}/customers/{customerId}/actions/transfer")]
    Task<TransferResponse> TransferAsync(
        string projectId,
        string customerId,
        [Body] TransferCustomerRequest request,
        CancellationToken cancellationToken = default);

    #endregion

    #region Customer Aliases

    /// <summary>
    /// Lists all aliases for a customer.
    /// </summary>
    [Get("/v2/projects/{projectId}/customers/{customerId}/aliases")]
    Task<ListResponse<CustomerAlias>> ListAliasesAsync(
        string projectId,
        string customerId,
        [Query] int? limit = null,
        [AliasAs("starting_after")] [Query] string? startingAfter = null,
        CancellationToken cancellationToken = default);

    #endregion

    #region Customer Attributes

    /// <summary>
    /// Lists all attributes for a customer.
    /// </summary>
    [Get("/v2/projects/{projectId}/customers/{customerId}/attributes")]
    Task<ListResponse<CustomerAttribute>> ListAttributesAsync(
        string projectId,
        string customerId,
        [Query] int? limit = null,
        [AliasAs("starting_after")] [Query] string? startingAfter = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets or updates customer attributes in bulk.
    /// </summary>
    [Post("/v2/projects/{projectId}/customers/{customerId}/attributes")]
    Task<ListResponse<CustomerAttribute>> SetAttributesAsync(
        string projectId,
        string customerId,
        [Body] SetCustomerAttributesRequest request,
        CancellationToken cancellationToken = default);

    #endregion

    #region Customer Entitlements

    /// <summary>
    /// Lists all active entitlements for a customer.
    /// </summary>
    [Get("/v2/projects/{projectId}/customers/{customerId}/active_entitlements")]
    Task<ListResponse<CustomerEntitlement>> ListActiveEntitlementsAsync(
        string projectId,
        string customerId,
        [Query] int? limit = null,
        [AliasAs("starting_after")] [Query] string? startingAfter = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Grants an entitlement to a customer. As a side effect, a promotional subscription is created.
    /// </summary>
    [Post("/v2/projects/{projectId}/customers/{customerId}/actions/grant_entitlement")]
    Task<Customer> GrantEntitlementAsync(
        string projectId,
        string customerId,
        [Body] GrantEntitlementRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Revokes a granted entitlement from a customer. As a side effect, the promotional subscription 
    /// associated with the granted entitlement is expired.
    /// </summary>
    [Post("/v2/projects/{projectId}/customers/{customerId}/actions/revoke_granted_entitlement")]
    Task<Customer> RevokeGrantedEntitlementAsync(
        string projectId,
        string customerId,
        [Body] RevokeGrantedEntitlementRequest request,
        CancellationToken cancellationToken = default);

    #endregion

    #region Customer Offering Override

    /// <summary>
    /// Assigns or clears an offering override for a customer.
    /// </summary>
    [Post("/v2/projects/{projectId}/customers/{customerId}/actions/assign_offering")]
    Task AssignOfferingAsync(
        string projectId,
        string customerId,
        [Body] AssignOfferingRequest request,
        CancellationToken cancellationToken = default);

    #endregion

    #region Customer Subscriptions

    /// <summary>
    /// Lists all subscriptions for a customer.
    /// </summary>
    [Get("/v2/projects/{projectId}/customers/{customerId}/subscriptions")]
    Task<ListResponse<Subscription>> ListSubscriptionsAsync(
        string projectId,
        string customerId,
        [Query] string? environment = null,
        [Query] int? limit = null,
        [AliasAs("starting_after")] [Query] string? startingAfter = null,
        CancellationToken cancellationToken = default);

    #endregion

    #region Customer Purchases

    /// <summary>
    /// Lists all purchases for a customer.
    /// </summary>
    [Get("/v2/projects/{projectId}/customers/{customerId}/purchases")]
    Task<ListResponse<Purchase>> ListPurchasesAsync(
        string projectId,
        string customerId,
        [Query] string? environment = null,
        [Query] int? limit = null,
        [AliasAs("starting_after")] [Query] string? startingAfter = null,
        CancellationToken cancellationToken = default);

    #endregion

    #region Customer Invoices

    /// <summary>
    /// Lists all invoices for a customer.
    /// </summary>
    [Get("/v2/projects/{projectId}/customers/{customerId}/invoices")]
    Task<ListResponse<Invoice>> ListInvoicesAsync(
        string projectId,
        string customerId,
        [Query] int? limit = null,
        [AliasAs("starting_after")] [Query] string? startingAfter = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the invoice file URL (redirects to download URL).
    /// </summary>
    [Get("/v2/projects/{projectId}/customers/{customerId}/invoices/{invoiceId}/file")]
    Task<HttpResponseMessage> GetInvoiceFileAsync(
        string projectId,
        string customerId,
        string invoiceId,
        CancellationToken cancellationToken = default);

    #endregion

    #region Virtual Currencies

    /// <summary>
    /// Lists virtual currency balances for a customer.
    /// </summary>
    [Get("/v2/projects/{projectId}/customers/{customerId}/virtual_currencies")]
    Task<ListResponse<VirtualCurrencyBalance>> ListVirtualCurrencyBalancesAsync(
        string projectId,
        string customerId,
        [AliasAs("include_empty_balances")] [Query] bool? includeEmptyBalances = null,
        [Query] int? limit = null,
        [AliasAs("starting_after")] [Query] string? startingAfter = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a virtual currency transaction for a customer (adjusts balances with transaction record).
    /// </summary>
    [Post("/v2/projects/{projectId}/customers/{customerId}/virtual_currencies/transactions")]
    Task<ListResponse<VirtualCurrencyBalance>> CreateVirtualCurrencyTransactionAsync(
        string projectId,
        string customerId,
        [Body] CreateVirtualCurrencyTransactionRequest request,
        [Header("Idempotency-Key")] string? idempotencyKey = null,
        [AliasAs("include_empty_balances")] [Query] bool? includeEmptyBalances = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates virtual currency balances for a customer without creating a transaction record.
    /// </summary>
    [Post("/v2/projects/{projectId}/customers/{customerId}/virtual_currencies/update_balance")]
    Task<ListResponse<VirtualCurrencyBalance>> UpdateVirtualCurrencyBalanceAsync(
        string projectId,
        string customerId,
        [Body] UpdateVirtualCurrencyBalanceRequest request,
        [Header("Idempotency-Key")] string? idempotencyKey = null,
        [AliasAs("include_empty_balances")] [Query] bool? includeEmptyBalances = null,
        CancellationToken cancellationToken = default);

    #endregion
}

#region Request Models

/// <summary>
/// Represents an attribute input for creating or updating customer attributes.
/// </summary>
/// <param name="Name">The name of the attribute (e.g., "$email", "$displayName", or custom attribute names).</param>
/// <param name="Value">The value of the attribute.</param>
public sealed record CustomerAttributeInput(
    [property: System.Text.Json.Serialization.JsonPropertyName("name")]
    string Name,
    [property: System.Text.Json.Serialization.JsonPropertyName("value")]
    string Value
);

/// <summary>
/// Request to create a new customer.
/// </summary>
/// <param name="Id">The customer ID.</param>
/// <param name="Attributes">Optional customer attributes to set during creation.</param>
public sealed record CreateCustomerRequest(
    [property: System.Text.Json.Serialization.JsonPropertyName("id")]
    string Id,
    [property: System.Text.Json.Serialization.JsonPropertyName("attributes")]
    IReadOnlyList<CustomerAttributeInput>? Attributes = null
);

/// <summary>
/// Request to transfer customer data from one customer to another.
/// </summary>
/// <param name="TargetCustomerId">The target customer ID to transfer data to.</param>
/// <param name="AppIds">Optional list of app IDs to filter the transfer.</param>
public sealed record TransferCustomerRequest(
    [property: System.Text.Json.Serialization.JsonPropertyName("target_customer_id")]
    string TargetCustomerId,
    [property: System.Text.Json.Serialization.JsonPropertyName("app_ids")]
    IReadOnlyList<string>? AppIds = null
);

/// <summary>
/// Response from a customer transfer operation.
/// </summary>
/// <param name="Object">The object type (always "transfer").</param>
/// <param name="SourceCustomerId">The source customer ID.</param>
/// <param name="TargetCustomerId">The target customer ID.</param>
/// <param name="TransferredAt">The timestamp when the transfer occurred (milliseconds since epoch).</param>
public sealed record TransferResponse(
    [property: System.Text.Json.Serialization.JsonPropertyName("object")]
    string Object,
    [property: System.Text.Json.Serialization.JsonPropertyName("source_customer_id")]
    string SourceCustomerId,
    [property: System.Text.Json.Serialization.JsonPropertyName("target_customer_id")]
    string TargetCustomerId,
    [property: System.Text.Json.Serialization.JsonPropertyName("transferred_at")]
    long TransferredAt
);

/// <summary>
/// Request to set or update customer attributes.
/// </summary>
/// <param name="Attributes">The list of attributes to set or update.</param>
public sealed record SetCustomerAttributesRequest(
    [property: System.Text.Json.Serialization.JsonPropertyName("attributes")]
    IReadOnlyList<CustomerAttributeInput> Attributes
);

/// <summary>
/// Request to grant an entitlement to a customer.
/// </summary>
/// <param name="EntitlementId">The ID of the entitlement to grant to the customer.</param>
/// <param name="ExpiresAt">The date after which the access to the entitlement expires in ms since epoch.</param>
public sealed record GrantEntitlementRequest(
    [property: System.Text.Json.Serialization.JsonPropertyName("entitlement_id")]
    string EntitlementId,
    [property: System.Text.Json.Serialization.JsonPropertyName("expires_at")]
    long ExpiresAt
);

/// <summary>
/// Request to revoke a granted entitlement from a customer.
/// </summary>
/// <param name="EntitlementId">The ID of the granted entitlement to revoke from the customer.</param>
public sealed record RevokeGrantedEntitlementRequest(
    [property: System.Text.Json.Serialization.JsonPropertyName("entitlement_id")]
    string EntitlementId
);

/// <summary>
/// Request to assign or clear an offering override for a customer.
/// </summary>
/// <param name="OfferingId">The ID of the offering to assign. Set to null to clear any existing override.</param>
public sealed record AssignOfferingRequest(
    [property: System.Text.Json.Serialization.JsonPropertyName("offering_id")]
    string? OfferingId
);

/// <summary>
/// Request to create a virtual currency transaction.
/// </summary>
/// <param name="Adjustments">Dictionary of currency_code to adjustment amount (can be positive or negative).</param>
/// <param name="Reference">Optional reference for the transaction.</param>
public sealed record CreateVirtualCurrencyTransactionRequest(
    [property: System.Text.Json.Serialization.JsonPropertyName("adjustments")]
    IReadOnlyDictionary<string, int> Adjustments,
    [property: System.Text.Json.Serialization.JsonPropertyName("reference")]
    string? Reference = null
);

/// <summary>
/// Request to update virtual currency balances without creating a transaction.
/// </summary>
/// <param name="Adjustments">Dictionary of currency_code to new balance value.</param>
/// <param name="Reference">Optional reference for the update.</param>
public sealed record UpdateVirtualCurrencyBalanceRequest(
    [property: System.Text.Json.Serialization.JsonPropertyName("adjustments")]
    IReadOnlyDictionary<string, int> Adjustments,
    [property: System.Text.Json.Serialization.JsonPropertyName("reference")]
    string? Reference = null
);

#endregion
