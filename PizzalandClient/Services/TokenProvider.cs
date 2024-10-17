using pizzalandClient.Interfaces;

namespace pizzalandClient.Services;
public class TokenProvider(IHttpContextAccessor httpContextAccessor) : ITokenProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private string? _token;


    public async Task<string?> GetTokenAsync(CancellationToken cancellationToken)
    {
        if (_token == null)
        {
            _token = _httpContextAccessor?.HttpContext?.Items["JWT"]?.ToString();
        }

        return _token ?? null;
    }
}