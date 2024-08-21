using Assignment3.Models;
using Assignment3.Services.Repository;
using Assignment3.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Assignment3.Pages.Authentication;

public class SignIn : PageModel
{
    private IUser service;

    [BindProperty]
    public User User { get; set; }
    public SignIn(IUser service)
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
        var check = await service.SignUp(User);
        TempData["CheckSignUp"] = "SignUp " + (check ? "Successful" : "Fail");
        return RedirectToPage("./Login");

    }
}