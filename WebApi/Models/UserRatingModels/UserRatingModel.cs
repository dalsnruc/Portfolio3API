using DataLayer;
using WebApi.Models.TitleModels;

namespace WebApi.Models.UserRatingModels
{
    public class UserRatingModel
    {
        public string Url { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; }
        public UserTitleModel Title { get; set; }

    }
}
