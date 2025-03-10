using AspNetMvc.Models;
using AspNetMvc.Models.Forms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AspNetMvc.Controllers;

[Authorize]
public class ReviewController(SiteContext context) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Add(Guid userInfoId)
    {
        var userInfo = await context.UserInfos.FindAsync(userInfoId);
        if (userInfo == null)
        {
            return NotFound();
        }

        var form = new ReviewForm { UserInfoId = userInfoId };
        ViewData["UserInfo"] = userInfo;
        return View(form);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromForm] ReviewForm form)
    {
        if (!ModelState.IsValid)
        {
            ViewData["UserInfo"] = await context.UserInfos.FindAsync(form.UserInfoId);
            return View(form);
        }

        var model = new ReviewModel();
        form.Update(model);

        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var user = await context.Users.FindAsync(userId);

        if (user == null)
        {
            user = new User { Id = userId };
            context.Users.Add(user);
        }

        model.User = user;

        var userInfo = await context.UserInfos.FirstOrDefaultAsync(ui => ui.Id == form.UserInfoId);

        if (userInfo == null)
        {
            ModelState.AddModelError(string.Empty, "Інформація про користувача не знайдена.");
            ViewData["UserInfo"] = await context.UserInfos.FindAsync(form.UserInfoId);
            return View(form);
        }

        var existingReview = await context
            .Reviews
            .FirstOrDefaultAsync(r => r.UserInfo.Id == form.UserInfoId && r.User.Id == userId);

        if (existingReview != null)
        {
            ModelState.AddModelError(string.Empty, "Ви вже надіслали відгук про цього користувача.");
            ViewData["UserInfo"] = await context.UserInfos.FindAsync(form.UserInfoId);
            return View(form);
        }

        model.UserInfo = userInfo;

        context.Reviews.Add(model);
        await context.SaveChangesAsync();

        return RedirectToAction("Index", "Home");
    }
}