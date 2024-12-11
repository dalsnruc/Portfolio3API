
namespace DataLayer;
public class BookmarkTitle
{
    public int UserId { get; set; }
    public string TitleId { get; set; }
    public DateTime Date { get; set; }
    public Title Title { get; set; }

}
