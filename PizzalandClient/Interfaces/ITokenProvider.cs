namespace pizzalandClient.Interfaces;

public interface ITokenProvider
{
    Task<string> GetTokenAsync(CancellationToken cancellationToken);
}

