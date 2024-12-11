
namespace DataLayer
{
    public interface ISearchesDataService
    {
        IList<Searches> GetSearches(int userid, int page, int pageSize);
        Searches? GetSearch(int userid, int searchid);
        Searches CreateSearch(int userid, string content);
        bool DeleteSearch(int userid, int searchid);
        int NumberOfSearches(int userid);

    }
}
