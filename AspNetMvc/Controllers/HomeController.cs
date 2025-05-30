using AspNetMvc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AspNetMvc.Controllers;

public class HomeController(
        ILogger<HomeController> logger,
        SiteContext context,
        UserManager<User> userManager) : Controller
{
    public IActionResult Index(Guid id)
    {
        var currentUserEmail = User.Identity.IsAuthenticated ? User.Identity.Name : null;
        ViewBag.CurrentUserEmail = currentUserEmail;

        var avatarSrc = User.Claims.FirstOrDefault(x => x.Type == "Avatar")?.Value ?? "";
        ViewBag.AvatarSrc = avatarSrc;

        var users = context.UserInfos
            .Include(x => x.Reviews)
            .Include(x => x.UserSkills)
            .ThenInclude(x => x.Skill)
            .ToList();

        ViewBag.Professions = context.UserInfos.Select(u => u.Profession).Distinct().ToList();
        ViewBag.Skills = context.Skills.Select(s => s.Title).ToList();

        return View(users);
    }

    [HttpGet]
    public IActionResult Search(string text, string[] professions, string[] skills)
    {
        var users = context.UserInfos
            .Include(x => x.Reviews)
            .Include(x => x.UserSkills)
            .ThenInclude(x => x.Skill)
            .AsQueryable();

        if (!string.IsNullOrEmpty(text))
        {
            users = users.Where(u => u.Name.Contains(text));
        }

        if (professions != null && professions.Length > 0)
        {
            users = users.Where(u => professions.Contains(u.Profession));
        }

        if (skills != null && skills.Length > 0)
        {
            users = users.Where(u => skills.All(skill => u.UserSkills.Any(us => us.Skill.Title == skill)));
        }

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return PartialView("_UserList", users.ToList());
        }

        return View("Index", users.ToList());
    }

    [HttpGet]
    public IActionResult GetFilteredUsers(string text, string[] professions, string[] skills)
    {
        var users = context.UserInfos
            .Include(x => x.Reviews)
            .Include(x => x.UserSkills)
            .ThenInclude(x => x.Skill)
            .AsQueryable();

        if (!string.IsNullOrEmpty(text))
        {
            users = users.Where(u => u.Name.Contains(text));
        }

        if (professions != null && professions.Length > 0)
        {
            users = users.Where(u => professions.Contains(u.Profession));
        }

        if (skills != null && skills.Length > 0)
        {
            users = users.Where(u => skills.All(skill => u.UserSkills.Any(us => us.Skill.Title == skill)));
        }

        var result = users.Select(u => new
        {
            lat = u.Lat,
            lng = u.Lng,
            name = u.Name,
            profession = u.Profession,
            averageRating = u.Reviews.Any() ? u.Reviews.Average(r => r.Rating) : 0.0
        }).ToList();

        return Json(result);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}