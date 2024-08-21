using Assignment3.Models;
using Assignment3.Services.Repository;
using Microsoft.EntityFrameworkCore;
using static Assignment3.Pages.Authentication.Login;

namespace Assignment3.Services.Implement;

public class UserService : IUser
{
    private readonly PRN221_BL5_Assignment3Context db;

    public UserService(PRN221_BL5_Assignment3Context db)
    {
        this.db = db;
    }

    public async Task<User> GetUser(int id)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.UserId == id);
        return user;

    }

    public async Task<User> Login(LoginRequest request)
    {
        var user = await db.Users.FirstOrDefaultAsync(
            u => u.Username.Equals(request.Username) && u.Password.Equals(request.Password));
        return user;
    }

    public async Task<bool> SignUp(User user)
    {
        var checkUser = db.Users.Any(u => u.Username.Equals(user.Username));
        if (!checkUser)
        {
            await db.Users.AddAsync(user);
            db.SaveChanges();
        }
        return checkUser;
    }

    public async Task<bool> UpdateUser(User user)
    {
        var u = await db.Users.FirstOrDefaultAsync(u => u.Username.Equals(user.Username));
        var check = u != null;
        if (check)
        {
             db.Users.Update(user);
            await db.SaveChangesAsync();
        }
        return check;
    }
}