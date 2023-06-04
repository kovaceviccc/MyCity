using Entities.DbSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Repository.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAll();
    Task<User?> FindById(string id);
    Task<User?> FindByEmail(string email);
    Task<bool> BlockUser(string id);
    Task<bool> AddToRoles(string id);
    Task<bool> ChangePassword(User user,string oldPassword, string newPassword);
}
