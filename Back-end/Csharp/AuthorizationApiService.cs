public class AuthorizationApiService : IAuthorizationApiService
{
    private readonly IMemoryCache _cache;
    private readonly HttpClient _httpClient;
    private readonly AppSettings _appSettings;
    private readonly ILogger<AuthorizationApiService> _logger;

    public AuthorizationApiService
    (
  	    AppSettings appSettings,
        IMemoryCache cache,
        ILogger<AuthorizationApiService> logger
    )
    {
        _appSettings = appSettings;

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        _httpClient = new HttpClient();
        _cache = cache;
        _logger = logger;
    }

    /// <summary>
    /// 	Get token from the service
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>Token with the expiration time</returns>
    public async Task<string> GetTokenAsync(string clientId, string clientSecret,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        try
        {
            if (_cache.TryGetValue(clientId, out string token))
            {
                return token;
            }

            _httpClient.DefaultRequestHeaders
                .Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(Encoding.ASCII
.GetBytes($"{clientId}:{clientSecret}")
                ));

            var requestData = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            };

            var requestBody = new FormUrlEncodedContent(requestData);

            var httpReponse = await _httpClient
                .PostAsync(_appSettings.TokenEndpoint, 
requestBody, cancellationToken);

            httpReponse.EnsureSuccessStatusCode();

            var response = await httpReponse.Content.ReadAsStringAsync();

            //Check errors before try to get the token
            var tokenObject = JObject.Parse(response);

            var tokenExpires = tokenObject.Value<double>("expires_in");
            token = tokenObject.Value<string>("access_token");

            //Set the token with the expire time to the memory cache
            _cache.Set<string>(clientId, token, TimeSpan.FromSeconds(tokenExpires));

            return token;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Getting Authorisation Token", null);
        }

        return null;
    }
} 	
