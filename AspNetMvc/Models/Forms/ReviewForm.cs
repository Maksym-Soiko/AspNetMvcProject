using System.ComponentModel;

namespace AspNetMvc.Models.Forms;

public class ReviewForm
{
    public ReviewForm() { }

    public ReviewForm(ReviewModel model)
    {
        Rating = model.Rating;
        Text = model.Text;
        UserInfoId = model.UserInfo.Id;
    }

    public void Update(ReviewModel model)
    {
        model.Rating = Rating;
        model.Text = Text;
        model.UserInfo = new UserInfoModel { Id = UserInfoId };
    }

    [DisplayName("Оцінка")]
    public int Rating { get; set; }

    [DisplayName("Відгук")]
    public string Text { get; set; } = null!;

    public Guid UserInfoId { get; set; }
}
