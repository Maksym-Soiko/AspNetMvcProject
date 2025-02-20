using Microsoft.AspNetCore.Mvc;
using AspNetMvc.Models;
using System.Diagnostics;
namespace AspNetMvc.Controllers;

    public class HomeController(ILogger<HomeController> logger) : Controller
    {
        public IActionResult AboutMe()
        {
            var aboutMeModel = new UserInfoModel
            {
                Name = "Максим",
                Age = 21,
                University = "Хмельницький національний університет",
                Specialty = "Комп'ютерна інженерія",
                //Skills =
                //[
                //    new SkillModel { Title = "HTML", Color = "orange" },
                //    new SkillModel { Title = "CSS", Color = "blue" },
                //    new SkillModel { Title = "JavaScript", Color = "yellow" },
                //    new SkillModel { Title = "Vue.js", Color = "emerald" },
                //    new SkillModel { Title = "Python", Color = "indigo" },
                //    new SkillModel { Title = "C++", Color = "blue" },
                //    new SkillModel { Title = "C#", Color = "purple" },
                //    new SkillModel { Title = "ASP.NET Core", Color = "sky" },
                //    new SkillModel { Title = "SQL", Color = "teal" }
                //]
            };

            return View(aboutMeModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }