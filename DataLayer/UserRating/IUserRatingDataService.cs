
namespace DataLayer
{
    public interface IUserRatingDataService
    {
        IList<UserRating> GetUserRatings(int userid, int page, int pageSize);
        UserRating? GetUserRating(int userid, string titleid);
        UserRating CreateUserRating(int userid, string titleid, int rating);
        bool UpdateUserRating(int userid, UserRating userrating);
        bool DeleteUserRating(int userid, string titleid);
        int NumberOfUserRatings(int userid);
    }
}
