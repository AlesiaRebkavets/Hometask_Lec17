namespace RestAPI_Automation.Requests;

public class CreatedUsers
{
    public int page { get; set; }
    public int per_page { get; set; }
    public int total { get; set; }
    public int total_pages { get; set; }
    public List<Data> Data { get; set; }
}