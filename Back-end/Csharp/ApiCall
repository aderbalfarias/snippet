// Using HttpClient

public async Task<string> ApiCall
(
    string baseUrl, 
    string endpoint, 
    HttpMethod requestType, 
    string token = null, 
    string parameters = "",
    HttpContent content = null
)
{
    using (HttpClient client = new HttpClient())
    {
        if (!string.IsNullOrEmpty(token))
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var response = requestType == HttpMethod.Post && content != null
            ? await client.PostAsync($"{baseUrl}/{endpoint}", content)
            : await client.GetAsync($"{baseUrl}/{endpoint}/{parameters}");

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}


//Using RestSharp

//Get:
var client = new RestClient("[base url]");
var request = new RestRequest("api/item/", Method.GET);
var queryResult = client.Execute<List<Items>>(request).Data;

//Post:
var client = new RestClient("[base url]");
var request = new RestRequest("api/item/", Method.POST);
request.RequestFormat = DataFormat.Json;
request.AddBody(new Item
{
    ItemName = someName,
    Price = 19.99
});
client.Execute(request);

//Delete
var item = new Item(){//body};
var client = new RestClient("[base url]");
var request = new RestRequest("api/item/{id}", Method.DELETE);
request.AddParameter("id", idItem);
client.Execute(request)
