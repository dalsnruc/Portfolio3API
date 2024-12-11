using DataLayer;
using WebApi.Models.KnownForTitlesModels;
using WebApi.Models.NameProfessionModels;

namespace WebApi.Models.NameModels
{
    public class NameModel
    {
        public string NameId { get; set; }
        public string Url { get; set; }
        public string PrimaryName { get; set; }
        public string BirthYear { get; set; }
        public string DeathYear { get; set; }
        public float? AverageRating { get; set; }
        public IList<KnownForTitlesModelTitle> KnownForTitles { get; set; }

        public IList<NameProfessionModel> NameProfession { get; set; }

    }
}
