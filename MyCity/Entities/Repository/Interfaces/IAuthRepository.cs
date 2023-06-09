using Entities.DbSet;
using Entities.Models;

namespace Entities.Repository.Interfaces;

public interface IAuthRepository
{
    Task<bool> ValidatePassword(User user, string password);
    Task<(string, string)>GenerateTokens(User user);
    Task<(string, string)> GenerateTokens(string refreshToken);
    Task<User?> FindByEmailAsync(string email);
    Task<List<User>> GetAll();
    Task<bool> RegisterUser(User user, string password);
    Task<User?> FindByIdAsync(string userId);
    Task<bool> ValidateTokens(string jwtToken, string refreshToken);
    Task<bool> CreateRole(string roleName);
    Task<IEnumerable<BasicRoleModel>> GetAllRoles();
    Task<IEnumerable<BasicRoleModel>> GetAllRequredRoles(string userId);
    Task<(bool, string)> SubmitRoleRequest(string roleId, string userId);
}
