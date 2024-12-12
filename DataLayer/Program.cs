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
//Console.WriteLine(titledataservice.GetTitle("tt26693752").TitleType);
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

//Console.WriteLine();

/*
int testUserId = 1; // Replace with a valid user id
UserRating testRating = new UserRating
{
    UserId = testUserId,
    TitleId = "tt0104988",
    Rating = 5,
    Date = DateTime.UtcNow,
};

bool result = userratingdataservice.UpdateUserRating(testUserId, testRating);
Console.WriteLine(result);
*/

string testUsername = "TEST4";
string testPassword = "newPassword";
string testEmail = "testuser@example.com";
DateTime testBirthday = new DateTime(1995, 5, 15); // Replace with desired birthday
string testPhonenumber = "1234567890";

// Create an instance of your service or class containing the UpdateUser method


// Call the UpdateUser method
bool result = userdataservice.UpdateUser(
    testUsername,
    testPassword,
    testEmail,
    testBirthday,
    testPhonenumber
);

// Output the result
Console.WriteLine($"Update result: {result}");





static void PrintUsers(IUserDataService userdataService)
{
    foreach (var e in userdataService.GetUsers())
    {
        Console.WriteLine($"{e.Id}, {e.Username}, {e.Email}");
    }

}

static void PrintAllProperties(object obj)
{
    if (obj == null)
    {
        Console.WriteLine("Object is null.");
        return;
    }

    Type type = obj.GetType();
    Console.WriteLine($"Properties of {type.Name}:");

    foreach (PropertyInfo property in type.GetProperties())
    {
        object value = property.GetValue(obj);

        if (value is IEnumerable enumerable && !(value is string))
        {
            Console.WriteLine($"{property.Name}: Collection");

            foreach (var item in enumerable)
            {
                Console.WriteLine($" - Item in {property.Name}:");
                PrintAllProperties(item); // Recursively print each item
            }
        }
        else
        {
            Console.WriteLine($"{property.Name}: {value}");
        }
    }
}
