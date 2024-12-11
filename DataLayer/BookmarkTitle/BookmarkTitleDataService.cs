using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class BookmarkTitleDataService : IBookmarkTitleDataService
    {
        public IList<BookmarkTitle> GetBookmarkTitles(int userid, int page, int pageSize)
        {
            var db = new imdbContext();

            if (db.Users.FirstOrDefault(x => x.Id == userid) == null)
            {
                throw new ArgumentException("User not found");
            }

            return db.BookmarkTitles
                .Where(bt => bt.UserId == userid)
                .Skip(page * pageSize)
                .Take(pageSize)
                .Include(bt => bt.Title)
                .ToList();
        }

        public BookmarkTitle? GetBookmarkTitle(int userid, string titleid)
        {
            var db = new imdbContext();

            if (db.Users.FirstOrDefault(x => x.Id == userid) == null)
            {
                throw new ArgumentException("User not found");
            }

            return db.BookmarkTitles
                .Include(bt => bt.Title)
                .FirstOrDefault(bt => bt.UserId == userid && bt.TitleId == titleid);
        }

        public BookmarkTitle CreateBookmarkTitle(int userid, string titleid)
        {
            var db = new imdbContext();

            if (db.Users.FirstOrDefault(x => x.Id == userid) == null)
            {
                throw new ArgumentException("User not found");
            }

            var bookmarktitle = new BookmarkTitle
            {
                UserId = userid,
                TitleId = titleid,
                Date = DateTime.UtcNow,
            };

            db.BookmarkTitles.Add(bookmarktitle);
            db.SaveChanges();
            return bookmarktitle;
        }

        public bool DeleteBookmarkTitle(int userid, string titleid)
        {
            var db = new imdbContext();

            if (db.Users.FirstOrDefault(x => x.Id == userid) == null)
            {
                throw new ArgumentException("User not found");
            }

            var bookmarktitle = db.BookmarkTitles
                .FirstOrDefault(x => x.UserId == userid && x.TitleId == titleid);

            if (bookmarktitle == null)
            {
                return false;
            }

            db.BookmarkTitles.Remove(bookmarktitle);
            return db.SaveChanges() > 0;
        }

        public int NumberOfBookmarkTitles(int userId)
        {
            var db = new imdbContext();
            return db.BookmarkNames.Count(bn => bn.UserId == userId);
        }

    }
}
