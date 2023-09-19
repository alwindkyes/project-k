namespace project_k;

public class Status
{
    public int statusCode { get; set; }
    public string statusMessage { get; set; }
}

public class Token : Status
{
    public string token { get; set; }
}

public class LoginRequest
{
    public string username { get; set; }
    public string password { get; set; }
}