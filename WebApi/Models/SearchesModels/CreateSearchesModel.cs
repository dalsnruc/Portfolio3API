namespace WebApi.Models.SearchesModels
{
    public class CreateSearchesModel
    {
        public int SearchId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }

    }
}
