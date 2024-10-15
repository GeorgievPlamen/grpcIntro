namespace PizzalandCore.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserAsync(Guid id);
    Task<List<User>> GetUsersAsync();
    Task<User> AddUserAsync(User user);
    Task<bool> DeleteUserAsync(User id);
    Task<User?> UpdateUserAsync(User user);
}