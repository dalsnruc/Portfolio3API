﻿namespace DataLayer;

public interface ITitleDataService
{

    //IList<Title> GetAllMovies();
    IList<Title> GetTitles(int page, int pageSize, string? genre = null, double? minRating = null);

    IList<Title> GetTvSeries(int page, int pageSize, string? genre = null, double? minRating = null);

    Title? GetTitle(string id);

    //Title CreateTitle(int userid, string primarytitle, string originaltitle, bool isadult, string startyear);
    //bool UpdateTitle(int userid, Title title);
    //bool DeleteTitle(int userid, string id);
    
    int NumberOfTitles();
    






}
