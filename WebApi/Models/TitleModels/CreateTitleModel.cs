namespace WebApi.Models.TitleModels
{
    public class CreateTitleModel
    {
        public string PrimaryTitle { get; set; }
        public string OriginalTitle { get; set; }
        public bool IsAdult { get; set; }
        public string? StartYear { get; set; }
    }
}
