
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class NameDataService : INameDataService
    {
        public Name? GetName(string id)
        {
            var db = new imdbContext();

            return db.Names
                .Include(n => n.KnownForTitles)
                .ThenInclude(kft => kft.Title)
                .Include(n => n.NameProfession)
                .ThenInclude(np => np.Profession)
                .FirstOrDefault(n => n.NameId == id);
        }
        /*
        public IList<Name> GetNames()
        {
            var db = new imdbContext();
            return db.Names.ToList();
        }
        */

        public IList<Name> GetNames(int page, int pageSize, string? primaryName = null)
        {
            var db = new imdbContext();

            var query = db.Names.AsQueryable();

            if (!string.IsNullOrEmpty(primaryName))
            {
                query = query.Where(n => n.PrimaryName.ToLower().Contains(primaryName.ToLower()));
            }

            return query
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList();
        }


        /*

        public Name CreateName(int userid, string primaryname, string birthyear, string deathyear)
        {
            var db = new imdbContext();

            if (db.Users.FirstOrDefault(x => x.Id == userid) == null)
            {
                throw new ArgumentException("User not found");
            }

            var lastName = db.Names
               .OrderByDescending(t => t.NameId)
               .FirstOrDefault();

            int newIdNumber = 1;

            string lastId = lastName.NameId;
            if (int.TryParse(lastId.Substring(2), out int parsedId))
            {
                newIdNumber = parsedId + 1;
            }

            string newId = $"nm{newIdNumber}";

            var name = new Name
            {
                NameId = newId,
                PrimaryName = primaryname,
                BirthYear = birthyear,
                DeathYear = deathyear,
            };

            db.Names.Add(name);
            db.SaveChanges();
            return name;
        }

        */

        /*
        public bool UpdateName(int userid, Name name)
        {
            var db = new imdbContext();

            if (db.Users.FirstOrDefault(x => x.Id == userid) == null)
            {
                throw new ArgumentException("User not found");
            }

            db.Update(name);

            return db.SaveChanges() > 0;

        }

        */

        /*
        public bool DeleteName(int userid, string id)
        {
            var db = new imdbContext();

            if (db.Users.FirstOrDefault(x => x.Id == userid) == null)
            {
                throw new ArgumentException("User not found");
            }

            var name = db.Names.Find(id);

            if (name == null)
            {
                return false;
            }

            db.Names.Remove(name);
            return db.SaveChanges() > 0;

        }
        */

        public int NumberOfNames()
        {
            var db = new imdbContext();
            return db.Names.Count();
        }

    }
}
