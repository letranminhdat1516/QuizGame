using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using QuizGame.Service.Interface;
using QuizGame.Service.BusinessModel;

namespace Asm_PRN222_QuizGame.Admin.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _accountMemberService;

        public LoginModel(IUserService accountMemberService)
        {
            _accountMemberService = accountMemberService;
        }

        [BindProperty]
        public UserModel AccountMember { get; set; }

        public string? ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToPage("/DashBoard/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {


            var account = await _accountMemberService.Login(AccountMember.Email, AccountMember.Password);

            if (account == null)
            {
                ErrorMessage = "Invalid email or password!";
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.UserId.ToString()),
                new Claim(ClaimTypes.Name, account.UserName),
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.Role, account.Role.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {

                ExpiresUtc =  DateTime.UtcNow.AddMinutes(30),
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );

            return RedirectToPage("/DashBoard/Index");
        }
    }
}