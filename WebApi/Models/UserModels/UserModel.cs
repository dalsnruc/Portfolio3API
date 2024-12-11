using DataLayer;
using WebApi.Models.BookmarkNameModels;
using WebApi.Models.BookmarkTitleModels;
using WebApi.Models.SearchesModels;
using WebApi.Models.UserRatingModels;

namespace WebApi.Models.UserModels
{
    public class UserModel
    {
        public string? Url { get; set; }
        public string Username { get; set; } = string.Empty;
        public string? Email { get; set; }

        public DateTime Birthday { get; set; }
        public string Phonenumber { get; set; }
        public IList<UserRatingModel> UserRating { get; set; }
        public IList<BookmarkNameModel> BookmarkName { get; set; }
        public IList<BookmarkTitleModel> BookmarkTitle { get; set; }
        public IList<SearchesModel> Searches { get; set; }
    }
}
