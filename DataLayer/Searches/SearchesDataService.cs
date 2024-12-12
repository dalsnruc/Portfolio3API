
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

        public void SaveSearch(int user_id, string content)
        {
            try
            {
                using var db = new imdbContext();

                //Saves to the Searches table
                var search = new Searches
                {
                    UserId = user_id,
                    Content = content,
                    Date = DateTime.UtcNow // Converts to UTC
                };

                db.Searches.Add(search);

                //Check if search content (string) exists in Global_searches
                var globalSearch = db.GlobalSearches.FirstOrDefault(gs => gs.Content == content);

                if (globalSearch != null)
                {
                    //If exist add +1 to the count
                    globalSearch.Count++;
                    db.GlobalSearches.Update(globalSearch);

                }
                else
                {
                    //If not exists, add content to Global_Searches with count 1
                    var newGlobalSearch = new GlobalSearches
                    {
                        Content = content,
                        Count = 1
                    };
                    db.GlobalSearches.Add(newGlobalSearch);


                }

                //Save all changes
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}\n{ex.StackTrace}");
                throw; // Re-throw the exception for debugging
            }


        }
    }
}
