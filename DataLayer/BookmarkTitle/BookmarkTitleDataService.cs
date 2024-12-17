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
//using Microsoft.EntityFrameworkCore;

//namespace DataLayer
//{
//    public class BookmarkTitleDataService : IBookmarkTitleDataService
//    {
//        private readonly imdbContext _db;

//        // Constructor to inject the context (Edited Section)
//        public BookmarkTitleDataService(imdbContext db)
//        {
//            _db = db;
//        }

//        // Get paginated bookmark titles (No changes in this method but reviewed)
//        public IList<BookmarkTitle> GetBookmarkTitles(int userid, int page, int pageSize)
//        {
//            // Checking if user exists using 'Any' for performance (Edited Section)
//            if (!_db.Users.Any(x => x.Id == userid))
//            {
//                throw new ArgumentException("User not found");
//            }

//            return _db.BookmarkTitles
//                .Where(bt => bt.UserId == userid)
//                .Skip(page * pageSize)
//                .Take(pageSize)
//                .Include(bt => bt.Title)
//                .ToList();
//        }

//        // Get a specific bookmark title (No changes in this method but reviewed)
//        public BookmarkTitle? GetBookmarkTitle(int userid, string titleid)
//        {
//            // Checking if user exists using 'Any' for performance (Edited Section)
//            if (!_db.Users.Any(x => x.Id == userid))
//            {
//                throw new ArgumentException("User not found");
//            }

//            return _db.BookmarkTitles
//                .Include(bt => bt.Title)
//                .FirstOrDefault(bt => bt.UserId == userid && bt.TitleId == titleid);
//        }

//        // Create a new bookmark title (No changes in logic, but added DI for db context)
//        public BookmarkTitle CreateBookmarkTitle(int userid, string titleid)
//        {
//            // Checking if user exists using 'Any' for performance (Edited Section)
//            if (!_db.Users.Any(x => x.Id == userid))
//            {
//                throw new ArgumentException("User not found");
//            }

//            var bookmarkTitle = new BookmarkTitle
//            {
//                UserId = userid,
//                TitleId = titleid,
//                Date = DateTime.UtcNow
//            };

//            _db.BookmarkTitles.Add(bookmarkTitle);
//            _db.SaveChanges();
//            return bookmarkTitle;
//        }

//        // Delete a bookmark title (No changes in this method but reviewed)
//        public bool DeleteBookmarkTitle(int userid, string titleid)
//        {
//            // Checking if user exists using 'Any' for performance (Edited Section)
//            if (!_db.Users.Any(x => x.Id == userid))
//            {
//                throw new ArgumentException("User not found");
//            }

//            var bookmarkTitle = _db.BookmarkTitles
//                .FirstOrDefault(bt => bt.UserId == userid && bt.TitleId == titleid);

//            if (bookmarkTitle == null)
//            {
//                return false;
//            }

//            _db.BookmarkTitles.Remove(bookmarkTitle);
//            return _db.SaveChanges() > 0;
//        }

//        // Get the total number of bookmark titles for a user (Fixed the query here)
//        public int NumberOfBookmarkTitles(int userId)
//        {
//            // Corrected the counting query to use BookmarkTitles (Edited Section)
//            return _db.BookmarkTitles.Count(bt => bt.UserId == userId);
//        }
//    }
//}
