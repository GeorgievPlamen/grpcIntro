using Microsoft.EntityFrameworkCore;
using PizzalandCore.Interfaces;

namespace PizzalandCore.Db.Repositories;

public class UserRepository(PizzalandContext dbContext) : IUserRepository
{
    private readonly PizzalandContext _dbContext = dbContext;

    public async Task<User> AddUserAsync(User user)
    {
        await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }

    public async Task<bool> DeleteUserAsync(User id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if (user == null)
        {
            return false;
        }

        _dbContext.Users.Remove(user);

        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<User?> GetUserAsync(Guid id)
    {
        return await _dbContext.Users.FindAsync(id);
    }

    public async Task<List<User>> GetUsersAsync()
    {
        return await _dbContext.Users.ToListAsync();

    }

    public async Task<User?> UpdateUserAsync(User user)
    {
        _dbContext.Users.Update(user);

        await _dbContext.SaveChangesAsync();
        return user;
    }
}
