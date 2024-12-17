namespace DataLayer
{
    public interface INameDataService
    {
        //IList<Name> GetNames();
        IList<Name> GetNames(int page, int pageSize, string? primaryName = null);
        Name? GetNameByPrimaryName(string primaryName);
        Name? GetName(string id);

        //Name CreateName(int userid, string primaryname, string birthyear, string deathyear);
        //bool UpdateName(int userid, Name name);

        //bool DeleteName(int userid, string id);

        int NumberOfNames();


    }
}
