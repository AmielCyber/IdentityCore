using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Classifieds.Web.Pages.Auth;

[AllowAnonymous]
public class LoginModel : PageModel
{
    [BindProperty] public InputModel Input { get; set; }
    public string ReturnUrl { get; set; }
    [TempData] public string ErrorMessage { get; set; }

    public class InputModel
    {
        [Required] [EmailAddress] public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")] public bool RememberMe { get; set; }
    }

    public void OnGet(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        
        // Real db call to check if username and password are correct goes here!
        if (Input.Email == "admin@test.com" && Input.Password == "P@ssword1")
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, Input.Email),
                new Claim(ClaimTypes.Name, Input.Email),
                new Claim(ClaimTypes.Role, "ROLE"),
                new Claim("RandomDataPoint", "RandomValue"),
            };
            var identityUser = new ClaimsIdentity (claims, CookieAuthenticationDefaults .AuthenticationScheme);
            var principal = new ClaimsPrincipal(identityUser);
            
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principal,
                new AuthenticationProperties { IsPersistent = Input. RememberMe });

            // Guard again redirect attack
            return LocalRedirect(returnUrl);
        }
        else
        {
            return Unauthorized();
        }
    }
}