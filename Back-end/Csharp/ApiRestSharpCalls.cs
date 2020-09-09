//RestSharp Calls

var token = await _authorizationApiService
	.GetTokenAsync(_appSettings.ClientId, _appSettings.ClientSecret,
		$"{_appSettings.BaseApiUrl}{_appSettings.TokenEndpoint}");

var request = new RestRequest();
request.AddJsonBody(data);
request.AddHeader("Content-Type", "application/json");
request.AddHeader("Authorization", $"Bearer {token}");

var client = new RestClient($"{_appSettings.BaseApiUrl}{_appSettings.Endpoint}");

var response = client.Post<Entity>(request);

if (response.IsSuccessful)
	return await Task.FromResult(response.Data);


///////////////////////////////////////////////////////////////////////////////////
	
var token = await _authorizationApiService
	.GetTokenAsync(_appSettings.ClientId, _appSettings.ClientSecret,
		$"{_appSettings.BaseApiUrl}{_appSettings.TokenEndpoint}");
		
RestClient restClient = new RestClient($"{_appSettings.BaseApiUrl}{_appSettings.Endpoint}", Method.POST);

restRequest.AddHeader("Accept", "application/json");
restRequest.AddHeader("Authorization", $"Bearer {token}");

restRequest.AddJsonBody(data);

var response = restClient.Execute(restRequest);

return (int)response.StatusCode;
