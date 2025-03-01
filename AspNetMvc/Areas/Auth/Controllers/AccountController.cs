using AspNetMvc.Areas.Auth.Models.Forms;
using AspNetMvc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AspNetMvc.Areas.Auth.Controllers;

[Area("Auth")]
public class AccountController(
    UserManager<User> userManager,
    SignInManager<User> signInManager) : Controller
{

    [HttpGet]
    public IActionResult Register(string returnUrl)
    {
        TempData["ReturnUrl"] = returnUrl;
        return View(new RegisterForm());
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromForm] RegisterForm form)
    {
        var returnUrl = TempData["ReturnUrl"] as string ?? "/";

        if (!ModelState.IsValid)
        {
            return View(form);
        }

        if (await userManager.FindByEmailAsync(form.Login) != null)
        {
            ModelState.AddModelError(nameof(form.Login), "Користувач з таким логіном вже існує.");
            return View(form);
        }

        var user = new User
        {
            Email = form.Login,
            UserName = form.Login
        };

        var result = await userManager.CreateAsync(user, form.Password);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(nameof(form.Login), "Помилка при реєстрації.");
            return View(form);
        }

        await signInManager.SignInWithClaimsAsync(user, true,
        [
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("Avatar", user.ProfileImage ?? "")
        ]);

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Login(string returnUrl)
    {
        TempData["ReturnUrl"] = returnUrl;
        return View(new LoginForm());
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromForm] LoginForm form)
    {
        var returnUrl = TempData["ReturnUrl"] as string ?? "/";

        if (!ModelState.IsValid)
        {
            return View(form);
        }

        var user = await userManager.FindByEmailAsync(form.Login);
        if (user == null)
        {
            ModelState.AddModelError(nameof(form.Login), "Користувача з таким логіном не існує.");
            return View(form);
        }

        if (!await userManager.CheckPasswordAsync(user, form.Password))
        {
            ModelState.AddModelError(nameof(form.Password), "Невірний пароль.");
            return View(form);
        }

        await signInManager.SignInWithClaimsAsync(user, true,
        [
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("Avatar", user.ProfileImage ?? "")
        ]);

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account", new { area = "Auth" });
    }
}