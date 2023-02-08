using System.Net.Mime;
using Newtonsoft.Json;
using RestSharp;

namespace RestAPI_Automation.Requests;

public static class APIHelper
{
    public static RestClient restClient = new RestClient(Constants.baseURL);

    // concatenating base url and endpoint
    public static string SetUrl(string endpoint) => Path.Combine(Constants.baseURL, endpoint);

    // POST request ulr, header and parameters
    public static T CreatePostRequest<T>(string payload)
    {
        RestRequest restRequest = new RestRequest(SetUrl(""), Method.Post);
        restRequest.AddHeader("HttpRequestHeader.Accept", MediaTypeNames.Application.Json);
        restRequest.AddParameter(MediaTypeNames.Application.Json, payload, ParameterType.RequestBody);

        return GetContent<T>(restClient.Execute(restRequest));
        // return restRequest;
    }

    // GET request url and header
    public static T CreateGetRequest<T>(string endpoint)
    {
        var restRequest = new RestRequest(SetUrl(endpoint));
        restRequest.AddHeader("HttpRequestHeader.Accept", MediaTypeNames.Application.Json);

        return GetContent<T>(restClient.Execute(restRequest));
    }

    // the same as CreateGetRequest<T> class, but returns serialized data
    // is needed for task1 to return status code description
    public static RestResponse GetRequest(string endpoint)
    {
        var restRequest = new RestRequest(SetUrl(endpoint));
        restRequest.AddHeader("HttpRequestHeader.Accept", MediaTypeNames.Application.Json);

        return GetReponse(restRequest);
    }

    // method returns response
    public static RestResponse GetReponse(RestRequest request)
    {
        return restClient.Execute(request);
    }

    // method returns deserialize response data
    public static UserData GetContent<UserData>(RestResponse response)
    {
        var content = response.Content;
        UserData userDataObject = JsonConvert.DeserializeObject<UserData>(content);
        return userDataObject;
    }
}