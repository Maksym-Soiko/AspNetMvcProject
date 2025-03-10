using AspNetMvc.Models;
using AspNetMvc.Models.Forms;
using AspNetMvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AspNetMvc.Controllers
{
    public class UserInfoController(
        ILogger<UserInfoController> logger,
        SiteContext context,
        UserManager<User> userManager,
        FileStorageService fileStorage,
        UserSkillService userSkillService) : Controller

    {
        [Authorize]
        public IActionResult Index()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUserEmail = User.Identity.IsAuthenticated ? User.Identity.Name : null;
            var avatarSrc = User.Claims.FirstOrDefault(x => x.Type == "Avatar")?.Value ?? "";

            ViewBag.CurrentUserEmail = currentUserEmail;
            ViewBag.AvatarSrc = avatarSrc;

            IQueryable<UserInfoModel> userInfosQuery = context.UserInfos
                .Include(x => x.UserSkills)
                .ThenInclude(x => x.Skill);

            bool isAdminOrManager = User.IsInRole("Admin") || User.IsInRole("Manager");

            var userInfos = isAdminOrManager
                ? userInfosQuery.ToList()
                : userInfosQuery.Where(x => x.CreatedByUserId.ToString() == currentUserId).ToList();

            foreach (var userInfo in userInfos)
            {
                userInfo.CreatedByUserName = context.Users
                    .Where(u => u.Id == userInfo.CreatedByUserId)
                    .Select(u => u.UserName)
                    .FirstOrDefault();
            }

            return View(userInfos);
        }

        [Authorize]
        public IActionResult Details(Guid id)
        {
            var model = context.UserInfos
                .Include(u => u.UserSkills)
                .ThenInclude(us => us.Skill)
                .FirstOrDefault(x => x.Id == id);

            if (model == null)
            {
                return NotFound();
            }

            var reviews = context.Reviews
                .Include(r => r.User)
                .Where(r => r.UserInfo.Id == id)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();

            var userSkills = model.UserSkills.ToList();

            var currentUser = userManager.GetUserAsync(User).Result;
            var hasReviewed = reviews.Any(r => r.User.Id == currentUser.Id);

            ViewData["HasReviewed"] = hasReviewed;
            ViewData["UserSkills"] = userSkills ?? new List<UserSkillModel>();
            ViewData["Skills"] = context.Skills.ToList() ?? new List<SkillModel>();
            ViewData["Reviews"] = reviews ?? new List<ReviewModel>();

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new UserInfoForm();
            ViewData["Skills"] = context.Skills.ToList();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] UserInfoForm form, IFormFile[]? Photos, Dictionary<Guid, int> selectedSkills)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUserEmail = User.Identity.IsAuthenticated ? User.Identity.Name : null;

            var model = new UserInfoModel
            {
                Id = Guid.NewGuid(),
                MainImage = "default.jpg",
                CreatedByUserId = Guid.Parse(currentUserId),
                CreatedByUserName = currentUserEmail
            };
            form.Update(model, "wwwroot/user_info_uploads");

            if (Photos != null && Photos.Length > 0)
            {
                model.Images = await fileStorage.UploadUserPhotosAsync(Photos);
            }

            context.UserInfos.Add(model);
            await context.SaveChangesAsync();

            if (selectedSkills != null && selectedSkills.Count > 0)
            {
                userSkillService.SaveUserSkills(model.Id, selectedSkills);
            }
            else
            {
                logger.LogWarning("No skills were selected for the user.");
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            ViewData["id"] = id;
            var form = new UserInfoForm(context.UserInfos.First(x => x.Id == id));

            var userSkills = context.UserSkills.Where(x => x.UserInfo.Id == id).ToList();
            var skills = context.Skills.ToList();

            var userSkillForms = skills.Select(s =>
            {
                var userSkill = userSkills.FirstOrDefault(x => x.Skill.Id == s.Id);
                return new UserSkillForm
                {
                    SkillId = s.Id,
                    Selected = userSkill != null,
                    Level = userSkill != null ? userSkill.Level : 0,
                };
            }).ToList();

            ViewData["userSkillForms"] = userSkillForms;
            ViewData["skills"] = skills;

            return View(form);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, [FromForm] UserInfoForm form, [FromForm] List<UserSkillForm>? userSkillForms, IFormFile[]? Photos)
        {
            if (!ModelState.IsValid)
            {
                ViewData["id"] = id;
                var skills = context.Skills.ToList();
                ViewData["userSkillForms"] = userSkillForms ?? [];
                ViewData["skills"] = skills;
                return View(form);
            }

            var model = context.UserInfos
                .Include(u => u.UserSkills)
                .FirstOrDefault(x => x.Id == id);

            if (model == null)
            {
                return NotFound();
            }

            if (model.Images != null && model.Images.Count > 0)
            {
                foreach (var image in model.Images)
                {
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "user_info_uploads", image);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
            }

            if (Photos != null && Photos.Length > 0)
            {
                var newImages = await fileStorage.UploadUserPhotosAsync(Photos);
                model.Images = newImages;
            }

            form.Update(model);
            await context.SaveChangesAsync();

            context.UserSkills.RemoveRange(model.UserSkills);
            await context.SaveChangesAsync();

            foreach (var item in userSkillForms ?? [])
            {
                if (item.Selected)
                {
                    var newUserSkill = new UserSkillModel
                    {
                        Id = Guid.NewGuid(),
                        UserInfo = model,
                        Skill = context.Skills.First(s => s.Id == item.SkillId),
                        Level = item.Level,
                    };
                    context.UserSkills.Add(newUserSkill);
                }
            }

            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var model = await context.UserInfos.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            if (model.Images != null && model.Images.Count > 0)
            {
                foreach (var image in model.Images)
                {
                    fileStorage.DeleteFile(image);
                }
            }

            var userSkills = context.UserSkills.Where(x => x.UserInfo.Id == id).ToList();
            context.UserSkills.RemoveRange(userSkills);
            context.UserInfos.Remove(model);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SetMainImage(Guid id, string imageName)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = await context.UserInfos.FindAsync(id);

            if (model == null || model.Images == null || !model.Images.Contains(imageName))
            {
                return NotFound();
            }

            if (model.CreatedByUserId.ToString() != currentUserId && !User.IsInRole("Admin") && !User.IsInRole("Manager"))
            {
                return Forbid();
            }

            model.MainImage = imageName;
            await context.SaveChangesAsync();

            return RedirectToAction("Details", new { id });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateSkills(Guid userId, Dictionary<Guid, int> selectedSkills)
        {
            if (userId == Guid.Empty || selectedSkills == null || selectedSkills.Count == 0)
            {
                return BadRequest("Невірні дані для оновлення скілів.");
            }

            var userSkills = context.UserSkills.Where(x => x.UserInfo.Id == userId).ToList();
            foreach (var skill in selectedSkills)
            {
                var userSkill = userSkills.FirstOrDefault(x => x.Skill.Id == skill.Key);
                if (userSkill == null)
                {
                    var newUserSkill = new UserSkillModel
                    {
                        Id = Guid.NewGuid(),
                        UserInfo = context.UserInfos.First(u => u.Id == userId),
                        Skill = context.Skills.First(s => s.Id == skill.Key),
                        Level = skill.Value,
                    };
                    context.UserSkills.Add(newUserSkill);
                }
                else
                {
                    userSkill.Level = skill.Value;
                }
            }
            await context.SaveChangesAsync();

            return Ok("Скіли успішно оновлені.");
        }
    }
}