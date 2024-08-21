using Assignment3.Services.Repository;
using Assignment3.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Assignment3.Pages.Authentication;

public class Login : PageModel
{
    private readonly IUser service;
    [BindProperty]
    public LoginRequest Request { get; set; }

    public Login(IUser service)
    {
        this.service = service;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        var u = await service.Login(Request);
        var check = u != null;
        TempData["CheckLogin"] = "Login " + (check ? "Successful" : "Fail");

        if (check)
        {
            HttpContext.Session.SetSession("User", u.UserId);
        }
        return RedirectToPage("/Index");
    }

    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        public LoginRequest()
        {
        }

        public LoginRequest(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }
    }
}