/// <summary>
/// Require the X-Client-ID HTTP header to be specified in a request optionally forward it in the response.
/// </summary>
/// <seealso cref="HttpHeaderAttribute" />
public class ClientIdHttpHeaderAttribute : HttpHeaderAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CorrelationIdHttpHeaderAttribute"/> class.
    /// </summary>
    public ClientIdHttpHeaderAttribute()
        : base(HttpHeaders.XClientId)
    {
    }

    /// <summary>
    /// Returns <c>true</c> if the X-Client-ID HTTP header contains a non empty value, otherwise <c>false</c>.
    /// </summary>
    /// <param name="headerValues">The header values.</param>
    /// <returns><c>true</c> if the X-Client-ID HTTP header values are valid; otherwise, <c>false</c>.</returns>
    public override bool IsValid(StringValues headerValues) =>
        !StringValues.IsNullOrEmpty(headerValues) && headerValues.All(x => !string.IsNullOrWhiteSpace(x));
}

public static class HttpHeaders
{
    public const string XClientId = "X-Client-ID";
}

public static class RateLimiting
{
    public const string ClientRateLimiting = nameof(ClientRateLimiting);

    public const string ClientRateLimitPolicies = nameof(ClientRateLimitPolicies);
}
