namespace WebApi.Models.UserRatingModels
{
    public class CreateUserRatingModel
    {
        public int UserId { get; set; }
        public string TitleId { get; set; }

        public int Rating { get; set; }
        public DateTime Date { get; set; }
    }
}
