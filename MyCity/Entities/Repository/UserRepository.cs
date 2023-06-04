using Entities.DbSet;
using Entities.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Repository;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserRepository(
        ApplicationDbContext context,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<bool> AddToRoles(string id)
    {
        try
        {
            var user = await FindById(id);

            if (user is null) return false;


            var res = await _userManager.AddToRolesAsync(user, await _roleManager.Roles.Select(x=> x.Name!).ToListAsync());

            return res.Succeeded;
        }
        catch (Exception)
        {
            //log error
            return false;
        }
    }

    public async Task<bool> BlockUser(string id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) 
            {
                return false;
            }

            user.IsBlocked = true;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    public async Task<bool> ChangePassword(User user,string oldPassword, string newPassword)
    {
        try
        {

            var changed = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

            return changed.Succeeded;
        }
        catch (Exception ex)
        {
            //log error
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public async Task<User?> FindByEmail(string email)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user;
        } 
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<User?> FindById(string id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id);
            return user;

        }
        catch (Exception)
        {
            //log errors
            return null;
        }
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        try
        {
            IEnumerable<User> users = await _userManager.Users.ToListAsync();
            return users;
        }
        catch(Exception) 
        {
            return Enumerable.Empty<User>();
        }
    }
}
