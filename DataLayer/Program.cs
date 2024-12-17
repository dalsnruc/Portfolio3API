using DataLayer;
using System.Reflection;
using System.Collections;

var titledataservice = new TitleDataService();
var userdataservice = new UserDataService();
var namedataservice = new NameDataService();
var genredataservice = new GenreDataService();
var bookmarknamedataservice = new BookmarkNameDataService();
var bookmarktitledataservice = new BookmarkTitleDataService();
var searchesdataservice = new SearchesDataService();
var userratingdataservice = new UserRatingDataService();

//Console.WriteLine(namedataservice.GetName("nm0000035").KnownForTitles.FirstOrDefault().Title.PrimaryTitle);
//Console.WriteLine(namedataservice.GetName("nm0000035").PrimaryName);
//Console.WriteLine(titledataservice.GetTitles(1, 10).FirstOrDefault().TitleRating.AverageRating);
//Console.WriteLine(titledataservice.GetTitle("tt0272626").TitleRating.AverageRating);
//Console.WriteLine(genredataservice.GetAllGenres().FirstOrDefault().Name);

//Console.WriteLine(bookmarknamedataservice.GetBookmarkName(1, "nm0000005").Name.PrimaryName);
//Console.WriteLine(bookmarknamedataservice.GetBookmarkNames(1,0,1).FirstOrDefault().NameId);
//Console.WriteLine(bookmarknamedataservice.CreateBookmarkName(1, "nm0000001"));
//Console.WriteLine(bookmarknamedataservice.DeleteBookmarkName(4, "nm0000001"));

//Console.WriteLine(bookmarktitledataservice.GetBookmarkTitle(1, "tt20854604").Title.PrimaryTitle);
//Console.WriteLine(bookmarktitledataservice.GetBookmarkTitles(1, 0,1).FirstOrDefault().TitleId);
//Console.WriteLine(bookmarktitledataservice.CreateBookmarkTitle(1, "tt27908694"));
//Console.WriteLine(bookmarktitledataservice.DeleteBookmarkTitle(1, "tt27908694"));

//Console.WriteLine(searchesdataservice.GetSearches(1, 0,1).FirstOrDefault().Content);
//Console.WriteLine(searchesdataservice.GetSearch(1, 2).Content);
//searchesdataservice.SaveSearch(1, "Test");
//Console.WriteLine(searchesdataservice.DeleteSearch(1, 3));

//Console.WriteLine(userratingdataservice.GetUserRatings(1, 0, 1).FirstOrDefault().Title.OriginalTitle);
//Console.WriteLine(userratingdataservice.GetUserRating(1, "tt0104988").Title.OriginalTitle);
//Console.WriteLine(userratingdataservice.CreateUserRating(4, "tt24657706", 7));
//Console.WriteLine(userratingdataservice.DeleteUserRating(4, "tt24657706"));

//Console.WriteLine(userdataservice.CreateUser("FirstWithSalt", "pwd", "salt@gmail.com", new DateTime(1995, 5, 15), "12345678", "123"));
//Console.WriteLine(userdataservice.UpdateUser("TESTER", "nymail@gmail.com", new DateTime(1995, 5, 15), "12345678"));




static void PrintUsers(IUserDataService userdataService)
{
    foreach (var e in userdataService.GetUsers())
    {
        Console.WriteLine($"{e.Id}, {e.Username}, {e.Email}");
    }

}

