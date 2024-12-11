namespace DataLayer
{
    public interface IBookmarkTitleDataService
    {

        IList<BookmarkTitle> GetBookmarkTitles(int userid, int page, int pageSize);

        BookmarkTitle? GetBookmarkTitle(int userid, string titleid);

        BookmarkTitle CreateBookmarkTitle(int userid, string titleid);

        bool DeleteBookmarkTitle(int userid, string titleid);

        int NumberOfBookmarkTitles(int userid);

    }
}
