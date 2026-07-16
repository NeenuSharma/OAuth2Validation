using Identity.API.Entities;

namespace Identity.API.Repository;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
}