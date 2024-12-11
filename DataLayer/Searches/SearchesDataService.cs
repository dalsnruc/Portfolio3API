
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class SearchesDataService : ISearchesDataService
    {
        public IList<Searches> GetSearches(int userid, int page, int pageSize)
        {
            var db = new imdbContext();

            if (db.Searches.FirstOrDefault(x => x.UserId == userid) == null)
            {
                throw new ArgumentException("User not found");
            }

            return db.Searches
                .Where(bn => bn.UserId == userid)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public Searches? GetSearch(int userid, int searchid)
        {
            var db = new imdbContext();

            if (db.Searches.FirstOrDefault(x => x.UserId == userid) == null)
            {
                throw new ArgumentException("User not found");
            }

            return db.Searches
                .FirstOrDefault(bn => bn.UserId == userid && bn.SearchId == searchid);
        }

        public Searches CreateSearch(int userid, string content)
        {
            var db = new imdbContext();

            if (db.Users.FirstOrDefault(x => x.Id == userid) == null)
            {
                throw new ArgumentException("User not found");
            }


            var search = new Searches
            {
                UserId = userid,
                Content = content,
                Date = DateTime.UtcNow,
            };

            db.Searches.Add(search);
            db.SaveChanges();
            return search;
        }

        public bool DeleteSearch(int userid, int searchid)
        {
            var db = new imdbContext();

            if (db.Searches.FirstOrDefault(x => x.UserId == userid) == null)
            {
                throw new ArgumentException("User not found");
            }

            var search = db.Searches
                .FirstOrDefault(x => x.UserId == userid && x.SearchId == searchid);

            if (search == null)
            {
                return false;
            }

            db.Searches.Remove(search);
            return db.SaveChanges() > 0;

        }

        public int NumberOfSearches(int userId)
        {
            var db = new imdbContext();
            return db.Searches.Count(bn => bn.UserId == userId);
        }
    }
}
