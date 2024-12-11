namespace DataLayer
{
    public interface IBookmarkNameDataService
    {
        IList<BookmarkName> GetBookmarkNames(int userid, int page, int pageSize);

        BookmarkName? GetBookmarkName(int userid, string nameId);

        BookmarkName CreateBookmarkName(int userid, string nameId);

        bool DeleteBookmarkName(int userid, string nameid);

        int NumberOfBookmarkNames(int userid);

    }
}
