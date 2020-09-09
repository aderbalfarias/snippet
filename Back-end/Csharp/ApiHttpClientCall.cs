// HttpClient calls

var token = await _authorizationApiService
	.GetTokenAsync(_appSettings.ClientId, _appSettings.ClientSecret,
		$"{_appSettings.BaseApiUrl}{_appSettings.TokenEndpoint}");

var json = JsonConvert.SerializeObject(data,
	new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

using (HttpClient client = new HttpClient())
{
	client.DefaultRequestHeaders.Add("Content-Type", "application/json");
	client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

	var response = await client.PostAsync(
			$"{_appSettings.BaseApiUrl}{_appSettings.ApiEndpoint}",
			new StringContent(json, Encoding.UTF8, "application/json"));

	response.EnsureSuccessStatusCode();

	return (int)response.StatusCode;
}
