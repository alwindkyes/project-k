public class StatusResponse
{
    public int StatusCode { get; set;}
    public string StatusMessage { get; set;}

    public StatusResponse(int statusCode, string statusMessage)
    {
        StatusCode = statusCode;
        StatusMessage = statusMessage;
    }
}