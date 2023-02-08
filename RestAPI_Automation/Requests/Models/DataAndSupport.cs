namespace RestAPI_Automation.Requests;

public class DataAndSupport
{
    public Data Data { get; set; }
    public Support Support { get; set; }
}

public class Support
{
    public Uri url { get; set; }
    public string text { get; set; }
}