using AspNetMvc.Models.Forms;
using AspNetMvc.Models;
using Microsoft.AspNetCore.Mvc;
using AspNetMvc.Services;

public class SkillController(
    ILogger<SkillModel> logger, 
    SiteContext context,
    FileStorageService fileStorageService) : Controller
{
    public IActionResult Index()
    {
        return View(context.Skills.ToList());
    }

    public IActionResult Details(Guid id)
    {
        return View(context.Skills.First(x => x.Id == id));
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new SkillForm());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] SkillForm form, IFormFile? Logo)
    {
        if (!ModelState.IsValid)
        {
            return View(form);
        }

        var model = new SkillModel();
        form.Update(model);

        if (Logo != null)
        {
            model.Logo = await fileStorageService.SaveFileAsync(Logo);
        }

        context.Skills.Add(model);
        await context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(Guid id)
    {
        var model = context.Skills.First(x => x.Id == id);
        if (model == null)
        {
            return NotFound();
        }

        var form = new SkillForm(model);
        ViewData["id"] = id;

        return View(form);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, [FromForm] SkillForm form, IFormFile? Logo)
    {
        var model = context.Skills.First(x => x.Id == id);

        if (model == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            form.Update(model);

            if (Logo != null)
            {
                fileStorageService.DeleteFile(model.Logo);
                model.Logo = await fileStorageService.SaveFileAsync(Logo);
            }

            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        ViewData["id"] = id;

        return View(form);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var model = context.Skills.FirstOrDefault(x => x.Id == id);

        if (model == null)
        {
            return NotFound();
        }

        if (!string.IsNullOrEmpty(model.Logo))
        {
           fileStorageService.DeleteFile(model.Logo);
        }

        context.Skills.Remove(model);
        await context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}