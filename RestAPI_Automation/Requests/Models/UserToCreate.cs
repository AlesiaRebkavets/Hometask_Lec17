using Newtonsoft.Json;

namespace RestAPI_Automation.Requests;

public class UserToCreate
{
    public string Name { get; set; }
    public string Job { get; set; }
    [JsonProperty("Id")] public int Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public UserToCreate(string name, string job)
    {
        Name = name;
        Job = job;
    }
}