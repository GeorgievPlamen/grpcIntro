using PizzalandCore.Models;

namespace PizzalandCore.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserAsync(Guid id);
    Task<User?> GetUserByEmailAsync(string email);
    Task<List<User>> GetUsersAsync();
    Task<User> AddUserAsync(User user);
    Task<bool> DeleteUserAsync(Guid id);
    Task<User?> UpdateUserAsync(User user);
}