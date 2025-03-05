using AspNetMvc.Models;
using AspNetMvc.Models.Forms;
using AspNetMvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AspNetMvc.Controllers;

[Authorize(Roles = "Admin")]
public class UserViewController(
    ILogger<UserInfoController> logger,
    UserManager<User> userManager,
    SiteContext context,
    FileStorageService fileStorageService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        var currentUserEmail = User.Identity.IsAuthenticated ? User.Identity.Name : null;
        ViewBag.CurrentUserEmail = currentUserEmail;

        var avatarSrc = User.Claims.FirstOrDefault(x => x.Type == "Avatar")?.Value ?? "";
        ViewBag.AvatarSrc = avatarSrc;

        if (Guid.TryParse(userIdClaim, out Guid currentUserId))
        {
            var users = context.Users
                .Where(u => u.Id != currentUserId)
                .ToList();

            var userRoles = new Dictionary<Guid, IList<string>>();
            foreach (var user in users)
            {
                userRoles[user.Id] = await userManager.GetRolesAsync(user);
            }

            ViewBag.UserRoles = userRoles;

            return View(users);
        }

        return View(new List<User>());
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var model = context.Users.FirstOrDefault(x => x.Id == id);
        if (model == null)
        {
            return NotFound();
        }

        var form = new UserViewForm(model);
        ViewData["id"] = id;

        var userRoles = await userManager.GetRolesAsync(model);
        var allRoles = context.Roles.Select(r => r.Name).ToList();
        ViewBag.UserRoles = userRoles;
        ViewBag.AllRoles = allRoles;

        return View(form);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, [FromForm] UserViewForm form, IFormFile? ProfileImage, string[] SelectedRoles)
    {
        var model = context.Users.FirstOrDefault(x => x.Id == id);
        if (model == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            form.Update(model);

            if (ProfileImage != null && ProfileImage.Length > 0)
            {
                fileStorageService.DeleteFile(model.ProfileImage);
                model.ProfileImage = await fileStorageService.SaveFileAsync(ProfileImage);
            }

            var userRoles = await userManager.GetRolesAsync(model);
            var rolesToAdd = SelectedRoles.Except(userRoles).ToList();
            var rolesToRemove = userRoles.Except(SelectedRoles).ToList();

            await userManager.AddToRolesAsync(model, rolesToAdd);
            await userManager.RemoveFromRolesAsync(model, rolesToRemove);

            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        ViewData["id"] = id;
        ViewBag.UserRoles = await userManager.GetRolesAsync(model);
        ViewBag.AllRoles = context.Roles.Select(r => r.Name).ToList();

        return View(form);
    }
}