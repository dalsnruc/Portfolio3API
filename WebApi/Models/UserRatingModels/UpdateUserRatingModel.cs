namespace WebApi.Models.UserRatingModels
{
    public class UpdateUserRatingModel
    {

        public string? TitleId { get; set; }
        public int Rating { get; set; }
        public DateTime? Date { get; set; }

    }
}
