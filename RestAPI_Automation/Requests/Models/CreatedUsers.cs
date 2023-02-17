using Newtonsoft.Json;

namespace RestAPI_Automation.Requests;

public class CreatedUsers
{
    public int Page { get; set; }

    [JsonProperty("per_page")] public int PerPage { get; set; }

    public int Total { get; set; }

    [JsonProperty("total_pages")] public int TotalPages { get; set; }
    public List<Data> Data { get; set; }
}