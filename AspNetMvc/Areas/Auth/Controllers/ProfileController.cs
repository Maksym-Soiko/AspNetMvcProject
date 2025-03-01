using AspNetMvc.Areas.Auth.Models.Forms;
using AspNetMvc.Models;
using AspNetMvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AspNetMvc.Areas.Auth.Controllers;

[Area("Auth")]
[Authorize]
public class ProfileController(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    SiteContext context,
    FileStorageService fileStorageService) : Controller
{
    private async Task<User> GetCurrentUserAsync()
    {
        var id = Guid.Parse(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
        return await context.Users
            .FirstAsync(x => x.Id == id);
    }

    public async Task<IActionResult> Index()
    {
        var user = await GetCurrentUserAsync();
        return View(user);
    }

    [HttpGet]
    public async Task<IActionResult> Edit()
    {
        var user = await GetCurrentUserAsync();

        return View(new ProfileForm
        {
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber,
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromForm] ProfileForm form, IFormFile? profileImage)
    {
        if (!ModelState.IsValid)
        {
            return View(form);
        }

        var model = await GetCurrentUserAsync();
        model.FullName = form.FullName;
        model.PhoneNumber = form.PhoneNumber;

        if (profileImage != null)
        {
            fileStorageService.DeleteFile(model.ProfileImage);
            model.ProfileImage = await fileStorageService.SaveFileAsync(profileImage);
        }

        await context.SaveChangesAsync();

        await signInManager.SignOutAsync();
        await signInManager.SignInWithClaimsAsync(model, true,
        [
            new Claim(ClaimTypes.Email, model.Email),
            new Claim(ClaimTypes.NameIdentifier, model.Id.ToString()),
            new Claim("Avatar", model.ProfileImage ?? "")
        ]);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> ChangePassword()
    {
        var user = await GetCurrentUserAsync();
        return View(new ChangePasswordForm());
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordForm form)
    {
        if (!ModelState.IsValid)
        {
            return View(form);
        }

        if (form.NewPassword != form.ConfirmPassword)
        {
            ModelState.AddModelError(nameof(form.ConfirmPassword),
                "Новий пароль і підтвердження паролю не збігаються.");
            return View(form);
        }

        var user = await GetCurrentUserAsync();

        var passwordCheck = await userManager.CheckPasswordAsync(user, form.OldPassword);
        if (!passwordCheck)
        {
            ModelState.AddModelError(nameof(form.OldPassword), "Неправильний старий пароль.");
            return View(form);
        }

        var result = await userManager.ChangePasswordAsync(user, form.OldPassword, form.NewPassword);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(form);
        }

        await signInManager.SignOutAsync();
        await signInManager.SignInWithClaimsAsync(user, true,
        [
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("Avatar", user.ProfileImage ?? "")
        ]);

        return RedirectToAction("Index");
    }

    [HttpDelete]
    public async Task<IActionResult> Delete()
    {
        await userManager.DeleteAsync(await GetCurrentUserAsync());
        await signInManager.SignOutAsync();
        return Json(new { success = true });
    }
}