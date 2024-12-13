using WebApi.Models.PlotAndPosterModels;

namespace WebApi.Models.TitleModels
{
    public class AllTitlesModel
    {
        public string? Url { get; set; }
        public string TitleType { get; set; }
        public string PrimaryTitle { get; set; } = string.Empty;
        public string Genre {  get; set; }
        public PlotAndPosterModel PlotAndPoster { get; set; }
    }
}
