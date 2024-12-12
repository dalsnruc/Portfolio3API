using Microsoft.EntityFrameworkCore;

namespace DataLayer;

internal class imdbContext : DbContext
{
    // Represents the titles_basics table in the database
    public DbSet<Title> Titles {  get; set; }

    //public DbSet<PlotAndPoster> PlotAndPosters { get; set; }
    public DbSet<User> Users { get; set; }

    public DbSet<Genre> Genres { get; set; }
    public DbSet<TitleGenre> TitleGenres { get; set; }
    public DbSet<Name> Names { get; set; }

    public DbSet<BookmarkName> BookmarkNames { get; set; }
    public DbSet<BookmarkTitle> BookmarkTitles { get; set; }
    public DbSet<Searches> Searches { get; set; }
    public DbSet<UserRating> UserRatings { get; set; }
    public DbSet<GlobalSearches> GlobalSearches { get; set; }

    // Configures the database connection and logging for this DbContext
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        optionsBuilder.UseNpgsql("host=localhost;db=database;uid=postgres;pwd=postgres");
    }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Titles
        modelBuilder.Entity<Title>().ToTable("title_basics");
        modelBuilder.Entity<Title>().Property(x => x.Id).HasColumnName("title_id");
        modelBuilder.Entity<Title>().Property(x => x.TitleType).HasColumnName("title_type");
        modelBuilder.Entity<Title>().Property(x => x.PrimaryTitle).HasColumnName("primarytitle");
        modelBuilder.Entity<Title>().Property(x => x.OriginalTitle).HasColumnName("originaltitle");
        modelBuilder.Entity<Title>().Property(x => x.IsAdult).HasColumnName("isadult");
        modelBuilder.Entity<Title>().Property(x => x.StartYear).HasColumnName("startyear");

        //PlotAndPosters
        modelBuilder.Entity<PlotAndPoster>().ToTable("title_plot_and_poster");
        modelBuilder.Entity<PlotAndPoster>().Property(x => x.TitleId).HasColumnName("title_id");
        modelBuilder.Entity<PlotAndPoster>().Property(x => x.Plot).HasColumnName("plot");
        modelBuilder.Entity<PlotAndPoster>().Property(x => x.Poster).HasColumnName("poster");

        //Users
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<User>().Property(x => x.Id).HasColumnName("user_id");
        modelBuilder.Entity<User>().Property(x => x.Username).HasColumnName("username");
        modelBuilder.Entity<User>().Property(x => x.Password).HasColumnName("password");
        modelBuilder.Entity<User>().Property(x => x.Email).HasColumnName("email");
        modelBuilder.Entity<User>().Property(x => x.Birthday).HasColumnName("birthday");
        modelBuilder.Entity<User>().Property(x => x.Phonenumber).HasColumnName("phonenumber");
        modelBuilder.Entity<User>().Property(x => x.Salt).HasColumnName("salt");

        //Genres
        modelBuilder.Entity<Genre>().ToTable("genres");
        modelBuilder.Entity<Genre>().Property(x => x.Id).HasColumnName("genre_id");
        modelBuilder.Entity<Genre>().Property(x => x.Name).HasColumnName("genre_name");

        //TitleGenre
        modelBuilder.Entity<TitleGenre>().ToTable("title_genres");
        modelBuilder.Entity<TitleGenre>().Property(x => x.TitleId).HasColumnName("title_id");
        modelBuilder.Entity<TitleGenre>().Property(x => x.GenreId).HasColumnName("genre_id");
        modelBuilder.Entity<TitleGenre>()
            .HasKey(tg => new { tg.TitleId, tg.GenreId });

        //TitleRating
        modelBuilder.Entity<TitleRating>().ToTable("title_ratings");
        modelBuilder.Entity<TitleRating>().Property(x => x.TitleId).HasColumnName("title_id");
        modelBuilder.Entity<TitleRating>().Property(x => x.AverageRating).HasColumnName("averagerating");
        modelBuilder.Entity<TitleRating>().Property(x => x.NumVotes).HasColumnName("numvotes");
        modelBuilder.Entity<TitleRating>()
            .HasKey(r => new { r.TitleId });

        //UserRating
        modelBuilder.Entity<UserRating>().ToTable("user_ratings");
        modelBuilder.Entity<UserRating>().Property(x => x.UserId).HasColumnName("user_id");
        modelBuilder.Entity<UserRating>().Property(x => x.TitleId).HasColumnName("title_id");
        modelBuilder.Entity<UserRating>().Property(x => x.Rating).HasColumnName("rating");
        modelBuilder.Entity<UserRating>().Property(x => x.Date).HasColumnName("date");
        modelBuilder.Entity<UserRating>()
            .HasKey(r => new { r.UserId, r.TitleId });

        //BookmarkName
        modelBuilder.Entity<BookmarkName>().ToTable("bookmark_names");
        modelBuilder.Entity<BookmarkName>().Property(x => x.UserId).HasColumnName("user_id");
        modelBuilder.Entity<BookmarkName>().Property(x => x.NameId).HasColumnName("name_id");
        modelBuilder.Entity<BookmarkName>().Property(x => x.Date).HasColumnName("date");
        modelBuilder.Entity<BookmarkName>()
            .HasKey(bn => new { bn.UserId, bn.NameId });



        //BookmarkTitle
        modelBuilder.Entity<BookmarkTitle>().ToTable("bookmark_titles");
        modelBuilder.Entity<BookmarkTitle>().Property(x => x.UserId).HasColumnName("user_id");
        modelBuilder.Entity<BookmarkTitle>().Property(x => x.TitleId).HasColumnName("title_id");
        modelBuilder.Entity<BookmarkTitle>().Property(x => x.Date).HasColumnName("date");
        modelBuilder.Entity<BookmarkTitle>()
            .HasKey(bt => new { bt.UserId, bt.TitleId });

        //Name
        modelBuilder.Entity<Name>().ToTable("name_basics");
        modelBuilder.Entity<Name>().Property(x => x.NameId).HasColumnName("name_id");
        modelBuilder.Entity<Name>().Property(x => x.PrimaryName).HasColumnName("primaryname");
        modelBuilder.Entity<Name>().Property(x => x.BirthYear).HasColumnName("birthyear");
        modelBuilder.Entity<Name>().Property(x => x.DeathYear).HasColumnName("deathyear");
        modelBuilder.Entity<Name>().Property(x => x.AverageRating).HasColumnName("average_rating");
        modelBuilder.Entity<Name>()
            .HasKey(n => n.NameId);


        //Searches 
        modelBuilder.Entity<Searches>().ToTable("searches");
        modelBuilder.Entity<Searches>().Property(x => x.SearchId).HasColumnName("search_id");
        modelBuilder.Entity<Searches>().Property(x => x.UserId).HasColumnName("user_id");
        modelBuilder.Entity<Searches>().Property(x => x.Content).HasColumnName("content");
        modelBuilder.Entity<Searches>().Property(x => x.Date).HasColumnName("timestamp");
        modelBuilder.Entity<Searches>()
            .HasKey(s => new { s.SearchId});

        //KnownForTitles
        modelBuilder.Entity<KnownForTitles>().ToTable("known_for_titles");
        modelBuilder.Entity<KnownForTitles>().Property(x => x.TitleId).HasColumnName("title_id");
        modelBuilder.Entity<KnownForTitles>().Property(x => x.NameId).HasColumnName("name_id");
        modelBuilder.Entity<KnownForTitles>()
            .HasKey(kft => new { kft.NameId, kft.TitleId });

        //Profession
        modelBuilder.Entity<Profession>().ToTable("professions");
        modelBuilder.Entity<Profession>().Property(x => x.ProfessionId).HasColumnName("profession_id");
        modelBuilder.Entity<Profession>().Property(x => x.ProfessionName).HasColumnName("profession_name");

        //NameProfession
        modelBuilder.Entity<NameProfession>().ToTable("name_professions");
        modelBuilder.Entity<NameProfession>().Property(x => x.NameId).HasColumnName("name_id");
        modelBuilder.Entity<NameProfession>().Property(x => x.ProfessionId).HasColumnName("profession_id");
        modelBuilder.Entity<NameProfession>()
            .HasKey(np => new { np.NameId, np.ProfessionId });

        //GlobalSearches
        modelBuilder.Entity<GlobalSearches>().ToTable("global_searches");
        modelBuilder.Entity<GlobalSearches>().Property(gs => gs.Id).HasColumnName("global_search_id");
        modelBuilder.Entity<GlobalSearches>().Property(gs => gs.Content).HasColumnName("content");
        modelBuilder.Entity<GlobalSearches>().Property(gs => gs.Count).HasColumnName("counter");

    }


}