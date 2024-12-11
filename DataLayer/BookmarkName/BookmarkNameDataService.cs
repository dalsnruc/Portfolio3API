using Microsoft.EntityFrameworkCore;


namespace DataLayer
{
    public class BookmarkNameDataService : IBookmarkNameDataService
    {
        public IList<BookmarkName> GetBookmarkNames(int userid, int page, int pageSize)
        {
            var db = new imdbContext();

            if (db.Users.FirstOrDefault(x => x.Id == userid) == null)
            {
                throw new ArgumentException("User not found");
            }

            return db.BookmarkNames
                .Where(bn => bn.UserId == userid)
                .Skip(page * pageSize)
                .Take(pageSize)
                .Include(bn => bn.Name)
                .ToList();
        }

        public BookmarkName? GetBookmarkName(int userid, string nameid)
        {
            var db = new imdbContext();

            if (db.Users.FirstOrDefault(x => x.Id == userid) == null)
            {
                throw new ArgumentException("User not found");
            }

            return db.BookmarkNames
                .Include(bn => bn.Name) 
                .FirstOrDefault(bn => bn.UserId == userid && bn.NameId == nameid);
        }

        public BookmarkName CreateBookmarkName(int userid, string nameid)
        {
            var db = new imdbContext();

            if (db.Users.FirstOrDefault(x => x.Id == userid) == null)
            {
                throw new ArgumentException("User not found");
            }


            var bookmarkname = new BookmarkName
            {
                UserId = userid,
                NameId = nameid,
                Date = DateTime.UtcNow,
            };

            db.BookmarkNames.Add(bookmarkname);
            db.SaveChanges();
            return bookmarkname;
        }

        public bool DeleteBookmarkName(int userid, string nameid)
        {
            var db = new imdbContext();

            if (db.Users.FirstOrDefault(x => x.Id == userid) == null)
            {
                throw new ArgumentException("User not found");
            }

            var bookmarkname = db.BookmarkNames
                .FirstOrDefault(x => x.UserId == userid && x.NameId == nameid);

            if (bookmarkname == null)
            {
                return false;
            }

            db.BookmarkNames.Remove(bookmarkname);
            return db.SaveChanges() > 0;

        }

        public int NumberOfBookmarkNames(int userId)
        {
            var db = new imdbContext();
            return db.BookmarkNames.Count(bn => bn.UserId == userId);
        }

    }
}
