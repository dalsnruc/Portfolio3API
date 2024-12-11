using WebApi.Models.TitleModels;

namespace WebApi.Models.BookmarkTitleModels
{
    public class BookmarkTitleModel
    {
        public string Url { get; set; }
        public string UserId { get; set; }
        public string TitleId { get; set; }
        public DateTime Date { get; set; }
        public UserTitleModel Title { get; set; }

    }
}
