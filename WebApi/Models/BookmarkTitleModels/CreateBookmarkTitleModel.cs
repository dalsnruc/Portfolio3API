namespace WebApi.Models.BookmarkTitleModels
{
    public class CreateBookmarkTitleModel
    {
        public int UserId { get; set; }
        public string TitleId { get; set; }
        public DateTime Date { get; set; }

    }
}
