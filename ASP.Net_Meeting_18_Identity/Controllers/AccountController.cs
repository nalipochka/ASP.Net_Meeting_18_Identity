using ASP.Net_Meeting_18_Identity.Data;
using ASP.Net_Meeting_18_Identity.Models.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Net_Meeting_18_Identity.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager) 
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel vM)
        {
            if (ModelState.IsValid)
            {
                User user = new User() { 
                    Email = vM.Email, 
                    UserName = vM.Login, 
                    YearOfBirth = vM.YearOfBirth 
                };
                var result = await userManager.CreateAsync(user, vM.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(vM);
        }

        public IActionResult Login(string? returnUrl = null)
        {
            LoginViewModel vM = new LoginViewModel() { ReturnUrl = returnUrl};
            return View(vM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vM)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(vM.Login, vM.Password, vM.IsPersistent, false);
                if(result.Succeeded)
                {
                    if(!string.IsNullOrEmpty(vM.ReturnUrl) && Url.IsLocalUrl(vM.ReturnUrl))
                    {
                        return RedirectToAction(vM.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Login/Password has wrong!");
            }
            return View(vM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
