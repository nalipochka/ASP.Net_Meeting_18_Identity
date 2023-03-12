using ASP.Net_Meeting_18_Identity.Data;
using ASP.Net_Meeting_18_Identity.Models.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ASP.Net_Meeting_18_Identity.Transliterator;

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

        [AllowAnonymous]
        public IActionResult GoogleAuth()
        {
            string? redirectUrl = Url.Action("AuthRedirect", "Account");
            var properties = signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google",properties);
        }
        [AllowAnonymous]
        public IActionResult FacebookAuth()
        {
            string? redirectUrl = Url.Action("AuthRedirect", "Account");
            var properties = signInManager.ConfigureExternalAuthenticationProperties("Facebook", redirectUrl);
            return new ChallengeResult("Facebook",properties);
        }

        public async Task<IActionResult> AuthRedirect()
        {
            ExternalLoginInfo loginInfo = await signInManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }
            var loginResult = await signInManager.ExternalLoginSignInAsync(loginInfo.LoginProvider, loginInfo.ProviderKey, false);
            string?[] userInfo =
            {
                loginInfo.Principal.FindFirst(ClaimTypes.Name)?.Value,
                loginInfo.Principal.FindFirst(ClaimTypes.Email)?.Value,
            };
            if(loginResult.Succeeded)
            {
                return View(userInfo);
            }
            User user = new User
            {
                UserName = Transliterator.Transliterator.ConvertToTranslit(userInfo[0]!),
                Email = userInfo[1]
            };
            var result =await userManager.CreateAsync(user);
            if(result.Succeeded)
            {
                result = await userManager.AddLoginAsync(user, loginInfo);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return View(userInfo);
                }
            }
            else
            {
                User? findetUser = await userManager.FindByEmailAsync(userInfo[1]);
                    //Users.FirstOrDefaultAsync(t => t.NormalizedEmail == user.Email!.ToUpper());
                if(findetUser != null)
                {
                    await userManager.AddLoginAsync(findetUser, loginInfo);
                    await signInManager.SignInAsync(findetUser, isPersistent: false);
                    return View(userInfo);
                }
                
            }
            return RedirectToAction(nameof(AccessDenied));
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
