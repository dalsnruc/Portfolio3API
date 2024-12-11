namespace WebApi.Models.NameModels
{
    public class CreateNameModel
    {
        public string PrimaryName { get; set; } = string.Empty;
        public string BirthYear { get; set; } = string.Empty;
        public string? DeathYear { get; set; } = string.Empty;

    }
}
