using WebApi.Models.NameModels;

namespace WebApi.Models.BookmarkNameModels
{
    public class BookmarkNameModel
    {
        public string Url { get; set; }
        public string UserId { get; set; }
        public string NameId { get; set; }
        public DateTime Date { get; set; }
        public OnlyNameModel Name { get; set; }
    }
}
