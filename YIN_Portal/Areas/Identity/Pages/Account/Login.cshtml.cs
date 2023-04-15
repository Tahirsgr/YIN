using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using YIN_Portal.Data;
using YIN_Portal.Models;

namespace YIN_Portal.Areas.Identity.Pages
{
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
    
        private readonly ApplicationDbContext context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<ApplicationUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<ApplicationUser> userManager,ApplicationDbContext context)
        {
            _userManager = userManager;
         
            this.context = context;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {

            returnUrl = returnUrl == null || returnUrl == "/" ? Url.Content("~/") : returnUrl;

            if (ModelState.IsValid)
            {
                //await _signInManager.SignInAsync(await _userManager.FindByNameAsync(Input.Email), false);
                //return LocalRedirect(returnUrl);

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true


#if DEBUG
                var user1 = await _userManager.FindByNameAsync(Input.Email.ToLower());
                await _signInManager.SignInAsync(user1, true);
              
                return Redirect(returnUrl);
#endif

                var userTemp = await _userManager.FindByNameAsync(Input.Email.ToLower());
                if (userTemp == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid username/password");
                    return Page();
                }

                var result = await _signInManager.PasswordSignInAsync(Input.Email.ToLower(), Input.Password, Input.RememberMe, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(userTemp);
                        _logger.LogInformation("User logged in.");
                        
                        return Redirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                if (result.IsNotAllowed)
                {
                    ModelState.AddModelError(string.Empty, "User Approval is Pending");
                    return Page();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
