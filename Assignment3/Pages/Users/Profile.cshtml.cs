using Assignment3.Models;
using Assignment3.Services.Repository;
using Assignment3.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Assignment3.Pages.Users;

public class Profile : PageModel
{
    private IUser service;
    [BindProperty]
    public User User { get; set; }
    public Profile(IUser service)
    {
        this.service = service;
    }

    public async Task<IActionResult> OnGet()
    {
        var userId = HttpContext.Session.GetSession<int>("User");
        if (userId == 0)
        {
            return RedirectToPage("/Authentication/Login");
        }

        User = await service.GetUser(userId);
        return Page();
    }
    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        var check = await service.UpdateUser(User);
        TempData["Checkupdate"] = "Update Profile " + (check ? "Successful" : "Fail");
        return RedirectToPage("Profile");
    }



}