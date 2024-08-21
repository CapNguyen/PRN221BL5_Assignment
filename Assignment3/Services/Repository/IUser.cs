using Assignment3.Models;
using Assignment3.Pages.Authentication;
using static Assignment3.Pages.Authentication.Login;

namespace Assignment3.Services.Repository;

public interface IUser
{
    Task<User> Login(LoginRequest request);
    Task<bool> SignUp(User user);
    Task<User> GetUser(int id); 
    Task<bool> UpdateUser(User user);
}