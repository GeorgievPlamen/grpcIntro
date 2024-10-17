namespace pizzalandClient.Interfaces;

public interface ITokenProvider
{
    string? GetToken(CancellationToken cancellationToken);
    public void ClearToken();
    public void SetToken(string jwt);
    public void SetUserId(string id);
    string? GetUserId();
}

