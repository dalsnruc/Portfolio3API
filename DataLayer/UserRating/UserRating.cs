using System.ComponentModel.DataAnnotations;

namespace DataLayer;
public class UserRating
{
    public int UserId { get; set; }
    public string TitleId { get; set; }
    public int Rating { get; set; }
    public Title Title { get; set; }
    public DateTime Date { get; set; }
}
