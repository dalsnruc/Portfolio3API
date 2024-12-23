﻿using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace DataLayer;

public class TitleDataService : ITitleDataService
{


    public IList<Title> GetTitles(int page, int pageSize, string? genre = null, double? minRating = null, string? primaryTitle = null)
    {
        var db = new imdbContext();

        var query = db.Titles
            .Where(t => t.TitleType == "movie")
            .Include(t => t.PlotAndPoster)
            .Include(t => t.TitleRating)
            .Include(t => t.TitleGenre)
            .ThenInclude(tg => tg.Genre)
            .AsQueryable();

        if (!string.IsNullOrEmpty(genre))
        {
            query = query.Where(t => t.TitleGenre.Any(tg => tg.Genre.Name == genre));
        }

        if (minRating.HasValue)
        {
            query = query.Where(t => t.TitleRating != null && t.TitleRating.AverageRating >= minRating.Value);
        }

        if (!string.IsNullOrEmpty(primaryTitle))
        {
            query = query.Where(t => t.PrimaryTitle.ToLower().Contains(primaryTitle.ToLower()));
        }


        return query
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToList();
    }


    public IList<Title> GetTvSeries(int page, int pageSize, string? genre = null, double? minRating = null, string? primaryTitle = null)
    {
        var db = new imdbContext();

        var query = db.Titles
            .Where(t => t.TitleType == "tvSeries")
            .Include(t => t.PlotAndPoster)
            .Include(t => t.TitleRating)
            .Include(t => t.TitleGenre)
            .ThenInclude(tg => tg.Genre)
            .AsQueryable();

        if (!string.IsNullOrEmpty(genre))
        {
            query = query.Where(t => t.TitleGenre.Any(tg => tg.Genre.Name == genre));
        }

        if (minRating.HasValue)
        {
            query = query.Where(t => t.TitleRating != null && t.TitleRating.AverageRating >= minRating.Value);
        }

        if (!string.IsNullOrEmpty(primaryTitle))
        {
            query = query.Where(t => t.PrimaryTitle.ToLower().Contains(primaryTitle.ToLower()));
        }
        return query
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToList();
    }


    public IList<Title> GetTopRatedMovies(int page, int pageSize, int minVotes)
    {
        using var db = new imdbContext();

        return db.Titles
            .Where(t => t.TitleType == "movie" && t.TitleRating.NumVotes > minVotes) 
            .OrderByDescending(t => t.TitleRating.AverageRating) 
            .Skip(page * pageSize) 
            .Take(pageSize) 
            .Include(t => t.PlotAndPoster) 
            .Include(t => t.TitleRating)
            .Include(t => t.TitleGenre)
                .ThenInclude(tg => tg.Genre)
            .ToList();
    }

    public IList<Title> GetTopRatedTvSeries(int page, int pageSize, int minVotes)
    {
        using var db = new imdbContext();

        return db.Titles
            .Where(t => t.TitleType == "tvSeries" && t.TitleRating.NumVotes > minVotes)
            .OrderByDescending(t => t.TitleRating.AverageRating)
            .Skip(page * pageSize)
            .Take(pageSize)
            .Include(t => t.PlotAndPoster)
            .Include(t => t.TitleRating)
            .Include(t => t.TitleGenre)
                .ThenInclude(tg => tg.Genre)
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
