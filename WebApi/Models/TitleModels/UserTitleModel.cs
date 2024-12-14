using WebApi.Models.TitleGenreModels;

namespace WebApi.Models.TitleModels
{
    public class UserTitleModel
    {
        public string Url {  get; set; }
        public string PrimaryTitle { get; set; }
        public IList<TitleGenreModel> TitleGenre { get; set; }
    }
}
