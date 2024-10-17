namespace PizzalandCore.Interfaces;

public interface IJwtGenerator
{
    public string GetJwt(string email, string name);
}