using AspNetMvc.Models;

namespace AspNetMvc.Services;

public class UserInfoService : BaseService<UserInfoModel>
{
    public UserInfoService() : base("user-info.json") { }
}