namespace RestAPI_Automation.Requests;

public class UserToCreate
{
    public string name { get; set; }
    public string job { get; set; }
    public long id { get; set; }
    public DateTimeOffset createdAt { get; set; }
}