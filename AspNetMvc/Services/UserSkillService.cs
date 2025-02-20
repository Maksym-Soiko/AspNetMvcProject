using AspNetMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetMvc.Services;

public class UserSkillService
{
    private readonly SiteContext _context;

    public UserSkillService(SiteContext context)
    {
        _context = context;
    }

    public List<UserSkillModel> GetSkillsForUser(Guid userId)
    {
        return _context.UserSkills
            .Where(skill => skill.UserInfo.Id == userId)
            .Include(skill => skill.Skill)
            .ToList();
    }

    public void SaveUserSkills(Guid userId, Dictionary<Guid, int> selectedSkills)
    {
        var existingSkills = GetSkillsForUser(userId);

        foreach (var skill in selectedSkills)
        {
            var existingSkill = existingSkills.FirstOrDefault(x => x.Skill.Id == skill.Key);
            if (existingSkill != null)
            {
                existingSkill.Level = skill.Value;
            }
            else
            {
                _context.UserSkills.Add(new UserSkillModel
                {
                    Id = Guid.NewGuid(),
                    UserInfo = _context.UserInfos.Find(userId),
                    Skill = _context.Skills.Find(skill.Key),
                    Level = skill.Value
                });
            }
        }

        _context.SaveChanges();
    }

    public void UpdateUserSkills(Guid userId, Dictionary<Guid, int> selectedSkills)
    {
        var existingSkills = GetSkillsForUser(userId);

        foreach (var skill in selectedSkills)
        {
            var existingSkill = existingSkills.FirstOrDefault(x => x.Skill.Id == skill.Key);

            if (existingSkill != null)
            {
                existingSkill.Level = skill.Value;
            }
            else
            {
                _context.UserSkills.Add(new UserSkillModel
                {
                    Id = Guid.NewGuid(),
                    UserInfo = _context.UserInfos.Find(userId),
                    Skill = _context.Skills.Find(skill.Key),
                    Level = skill.Value
                });
            }
        }

        _context.SaveChanges();
    }

    public void DeleteSkillsForUser(Guid userId)
    {
        var skills = GetSkillsForUser(userId);
        _context.UserSkills.RemoveRange(skills);
        _context.SaveChanges();
    }
}