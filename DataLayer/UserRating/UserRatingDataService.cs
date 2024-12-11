

using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class UserRatingDataService : IUserRatingDataService
    {
        public IList<UserRating> GetUserRatings(int userid, int page, int pageSize)
        {
            var db = new imdbContext();

            if (db.UserRatings.FirstOrDefault(x => x.UserId == userid) == null)
            {
                throw new ArgumentException("User not found");
            }

            return db.UserRatings
                .Where(ur => ur.UserId == userid)
                .Skip(page * pageSize)
                .Take(pageSize)
                .Include(ur => ur.Title)
                .ToList();
        }

        public UserRating? GetUserRating(int userid, string titleid)
        {
            var db = new imdbContext();

            if (db.Users.FirstOrDefault(x => x.Id == userid) == null)
            {
                throw new ArgumentException("User not found");
            }

            return db.UserRatings
                .Include(ur => ur.Title)
                .FirstOrDefault(ur => ur.UserId == userid && ur.TitleId == titleid);
        }

        public UserRating CreateUserRating(int userid, string titleid, int rating)
        {
            var db = new imdbContext();

            if (db.Users.FirstOrDefault(x => x.Id == userid) == null)
            {
                throw new ArgumentException("User not found");
            }

            var userrating = new UserRating
            {
                UserId = userid,
                TitleId = titleid,
                Rating = rating,
                Date = DateTime.UtcNow,
            };

            db.UserRatings.Add(userrating);
            db.SaveChanges();
            return userrating;
        }

        public bool UpdateUserRating(int userid, UserRating userrating)
        {
            var db = new imdbContext();

            if (db.Users.FirstOrDefault(x => x.Id == userid) == null)
            {
                throw new ArgumentException("User not found");
            }

            db.Update(userrating);

            return db.SaveChanges() > 0;

        }

        public bool DeleteUserRating(int userid, string titleid)
        {
            var db = new imdbContext();

            if (db.Users.FirstOrDefault(x => x.Id == userid) == null)
            {
                throw new ArgumentException("User not found");
            }

            var userrating = db.UserRatings
                .FirstOrDefault(x => x.UserId == userid && x.TitleId == titleid);

            if (userrating == null)
            {
                return false;
            }

            db.UserRatings.Remove(userrating);
            return db.SaveChanges() > 0;
        }

        public int NumberOfUserRatings(int userId)
        {
            var db = new imdbContext();
            return db.UserRatings.Count(ur => ur.UserId == userId);
        }


    }
}
