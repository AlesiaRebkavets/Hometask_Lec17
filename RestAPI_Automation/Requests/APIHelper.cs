using Newtonsoft.Json;
using RestSharp;

namespace RestAPI_Automation.Requests;

public class APIHelper<T>
{
    public string baseURL = "https://reqres.in/ ";

    // concatenating base url and endpoint
    public string SetUrl(string endpoint)
    {
        string url = Path.Combine(baseURL, endpoint);
        return url;
    }

    // method returns RestClient value
    public RestClient RestClient()
    {
        var client = new RestClient(baseURL);
        return client;
    }

    // POST request ulr, header and parameters
    public RestRequest CreatePostRequest(string payload, string endpoint)
    {
        var restRequest = new RestRequest(endpoint, Method.Post);
        restRequest.AddHeader("Accept", "application/json");
        restRequest.AddParameter("application/json", payload, ParameterType.RequestBody);
        return restRequest;
    }

    // GET request url and header
    public RestRequest CreateGetRequest(string endpoint)
    {
        var restRequest = new RestRequest(SetUrl(endpoint), Method.Get);
        restRequest.AddHeader("Accept", "application/json");
        return restRequest;
    }

    // method returns response
    public RestResponse GetReponse(RestRequest request)
    {
        return RestClient().Execute(request);
    }

    // method returns deserialize response data
    public UserData GetContent<UserData>(RestResponse response)
    {
        var content = response.Content;
        UserData userDataObject = JsonConvert.DeserializeObject<UserData>(content);
        return userDataObject;
    }
}