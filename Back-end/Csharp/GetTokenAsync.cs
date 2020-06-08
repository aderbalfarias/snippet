private async Task<string> GetTokenAsync(string baseUrl,
    string tokenEndpoint, string clientId, string clientSecret,
    CancellationToken cancellationToken = default(CancellationToken))
{
    string token = null;

    try
    {
        using (HttpClient httpClient = new HttpClient())
        {

            httpClient.DefaultRequestHeaders
                .Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(Encoding.ASCII
                        .GetBytes($"{clientId}:{clientSecret}")));

            var requestData = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            };

            var requestBody = new FormUrlEncodedContent(requestData);

            var httpReponse = await httpClient
                .PostAsync($"{baseUrl}/{tokenEndpoint}", 
requestBody, cancellationToken);

            httpReponse.EnsureSuccessStatusCode();

            var response = await httpReponse.Content.ReadAsStringAsync();

            var tokenObject = JObject.Parse(response);

            var tokenExpires = tokenObject.Value<double>("expires_in");
            token = tokenObject.Value<string>("access_token");
        }
    }
    catch (Exception e)
    {
        throw new Exception($"Error Getting Authorisation Token: {e}");
    }

    return token;
}
