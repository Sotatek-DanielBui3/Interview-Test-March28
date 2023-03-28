namespace WebApp.Models;

public class SearchUserRequest
{
    public bool SearchByEmail { get; set; }
    public string? Keyword { get; set; }
}
