using pizzalandClient.Interfaces;

namespace pizzalandClient.Services;
public class TokenProvider() : ITokenProvider
{
    private string? _token;
    private string? _userId;


    public string? GetToken(CancellationToken cancellationToken)
    {
        return _token ?? null;
    }

    public void ClearToken()
    {
        _token = null;
    }

    public void SetToken(string jwt)
    {
        _token = jwt;
    }

    public void SetUserId(string id)
    {
        _userId = id;
    }

    public string? GetUserId()
    {
        return _userId;
    }
}