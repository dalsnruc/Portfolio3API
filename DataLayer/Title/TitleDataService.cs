using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace DataLayer;

public class TitleDataService : ITitleDataService
{

    /*
    public IList<Title> GetTitles(int userid)
    {

        var db = new imdbContext();

        if (db.Users.FirstOrDefault(x => x.Id == userid) == null)
        {
            throw new ArgumentException("User not found");
        }

        return db.Titles
            .ToList();
    }
    */

    public IList<Title> GetTitles(int page, int pageSize)
    {
        var db = new imdbContext();

        return db.Titles
            .Where(t=>t.TitleType== "movie")
            .Skip(page * pageSize)
            .Take(pageSize)
            .Include(t => t.PlotAndPoster)
            .ToList();
    }
    public IList<Title> GetTvSeries(int page, int pageSize)
    {
        var db = new imdbContext();

        return db.Titles
            .Where(t => t.TitleType == "tvSeries")
            .Skip(page * pageSize)
            .Take(pageSize)
            .Include(t => t.PlotAndPoster)
            .ToList();
    }


    public Title? GetTitle(string id)
    {
        using (var db = new imdbContext())
        {

            return db.Titles
                .Include(t => t.PlotAndPoster)
                .Include(t => t.TitleRating)
                .Include(t => t.TitleGenre)
                .ThenInclude(tg => tg.Genre)
                .Include(t => t.KnownForTitles)
                .ThenInclude(kft => kft.Name)
                .FirstOrDefault(t => t.Id == id);
        }
    }
    /*
    public Title CreateTitle(int userid, string primarytitle, string originaltitle, bool isadult, string startyear)
    {
        var db = new imdbContext();

        if (db.Users.FirstOrDefault(x => x.Id == userid) == null)
        {
            throw new ArgumentException("User not found");
        }

        //make new id
        var lastTitle = db.Titles
           .OrderByDescending(t => t.Id)
           .FirstOrDefault();

        int newIdNumber = 1;

        string lastId = lastTitle.Id;
        if (int.TryParse(lastId.Substring(2), out int parsedId))
        {
            newIdNumber = parsedId + 1;
        }

        string newId = $"tt{newIdNumber}";

        var title = new Title
        {
            Id = newId,
            PrimaryTitle = primarytitle,
            OriginalTitle = originaltitle,
            IsAdult = isadult,
            StartYear = startyear
        };

        db.Titles.Add(title);
        db.SaveChanges();
        return title;
    }
    */

    /*

    public bool UpdateTitle(int userid, Title title)
    {
        var db = new imdbContext();

        if (db.Users.FirstOrDefault(x => x.Id == userid) == null)
        {
            throw new ArgumentException("User not found");
        }

        db.Update(title);

        return db.SaveChanges() > 0;

    }
    */

    /*
    public bool DeleteTitle(int userid, string id)
    {
        var db = new imdbContext();

        if (db.Users.FirstOrDefault(x => x.Id == userid) == null)
        {
            throw new ArgumentException("User not found");
        }

        var title = db.Titles.Find(id);

        if (title == null)
        {
            return false;
        }

        db.Titles.Remove(title);
        return db.SaveChanges() > 0;

    }
    */

    public int NumberOfTitles()
    {
        var db = new imdbContext();
        return db.Titles.Count();
    }


}
