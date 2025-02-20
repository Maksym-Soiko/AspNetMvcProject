using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AspNetMvc.Models.Forms
{
    public class UserInfoForm
    {
        public UserInfoForm() { }

        public UserInfoForm(UserInfoModel model)
        {
            Name = model.Name;
            Age = model.Age;
            BirthDate = model.BirthDate;
            Email = model.Email;
            Description = model.Description;
            University = model.University;
            Specialty = model.Specialty;
            YearsOfExperience = model.YearsOfExperience;
            if (model.Profession != null)
            {
                ProfessionId = Professions.FindIndex(x => x == model.Profession);
            }
        }

        public void Update(UserInfoModel model)
        {
            model.Name = Name;
            model.Age = Age;
            model.BirthDate = BirthDate;
            model.Email = Email;
            model.Description = Description;
            model.University = University;
            model.Specialty = Specialty;
            model.YearsOfExperience = YearsOfExperience;
            model.Profession = Professions[ProfessionId ?? 0];
        }

        public void Update(UserInfoModel model, string uploadsFolder)
        {
            model.Name = Name;
            model.Age = Age;
            model.BirthDate = BirthDate;
            model.Email = Email;
            model.Description = Description;
            model.University = University;
            model.Specialty = Specialty;
            model.YearsOfExperience = YearsOfExperience;
            model.Profession = Professions[ProfessionId ?? 0];

            if (Images != null && Images.Count > 0)
            {
                model.Images = new List<string>();

                foreach (var file in Images)
                {
                    if (file.Length > 0)
                    {
                        var filePath = Path.Combine(uploadsFolder, Path.GetFileName(file.FileName));

                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        model.Images.Add(Path.GetFileName(file.FileName));
                    }
                }
            }
        }

        [DisplayName("Повне ім'я")]
        [Required(ErrorMessage = "Це поле є обов'язковим")]
        [MinLength(3, ErrorMessage = "Мінімум 3 символи")]
        [MaxLength(20, ErrorMessage = "Максимум 20 символів")]
        public string Name { get; set; } = null!;

        [DisplayName("Вік")]
        [Range(16, 100, ErrorMessage = "Вік повинен бути від 16 до 100 років")]
        public int Age { get; set; }

        [DisplayName("Дата народження")]
        [Required(ErrorMessage = "Дата народження є обов'язковою")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(UserInfoForm), nameof(ValidateBirthDate))]
        public DateTime BirthDate { get; set; }

        [DisplayName("Електронна пошта")]
        [Required(ErrorMessage = "Поле електронної пошти є обов'язковим")]
        [EmailAddress(ErrorMessage = "Некоректна адреса електронної пошти")]
        public string Email { get; set; }

        [DisplayName("Про себе")]
        [Required(ErrorMessage = "Поле опису про себе є обов'язковим")]
        [MaxLength(600, ErrorMessage = "Максимальна довжина опису - 600 символів")]
        public string Description { get; set; }

        [DisplayName("Місце навчання")]
        [Required(ErrorMessage = "Поле місця навчання є обов'язковим")]
        [MaxLength(120, ErrorMessage = "Максимальна довжина - 120 символів")]
        public string University { get; set; }

        [DisplayName("Спеціальність")]
        [Required(ErrorMessage = "Поле спеціальності є обов'язковим")]
        [MaxLength(60, ErrorMessage = "Максимальна довжина - 60 символів")]
        public string Specialty { get; set; }

        [DisplayName("Років досвіду")]
        [Range(0, 50, ErrorMessage = "Досвід роботи повинен бути від 0 до 50 років")]
        public int YearsOfExperience { get; set; }

        [DisplayName("Професія")]
        [Required(ErrorMessage = "Оберіть з варіантів")]
        public int? ProfessionId { get; set; }

        [DisplayName("Зображення")]
        public List<IFormFile>? Images { get; set; }

        public List<string> Professions =>
        [
            "Frontend Developer",
            "Backend Developer",
            "Fullstack Developer",
            "QA Engineer",
            "DevOps Engineer",
            "Project Manager",
            "Product Manager",
            "UI/UX Designer",
            "Data Scientist",
            "Data Engineer",
            "Business Analyst",
            "System Analyst",
            "Security Engineer",
            "Technical Writer",
            "Technical Support",
            "Other"
        ];

        public static ValidationResult? ValidateBirthDate(DateTime birthDate, ValidationContext context)
        {
            var today = DateTime.Today;
            var minDate = today.AddYears(-100);
            var maxDate = today.AddYears(-16);

            if (birthDate < minDate || birthDate > maxDate)
            {
                return new ValidationResult("Дата народження повинна бути у межах від 16 до 100 років.");
            }

            return ValidationResult.Success;
        }
    }
}