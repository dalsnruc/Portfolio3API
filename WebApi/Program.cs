using DataLayer;
using Mapster;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<IUserDataService, UserDataService>();
builder.Services.AddSingleton<ITitleDataService, TitleDataService>();
builder.Services.AddSingleton<INameDataService, NameDataService>();
builder.Services.AddSingleton<IGenreDataService, GenreDataService>();
builder.Services.AddSingleton<IBookmarkNameDataService, BookmarkNameDataService>();
builder.Services.AddSingleton<IBookmarkTitleDataService, BookmarkTitleDataService>();
builder.Services.AddSingleton<IUserRatingDataService, UserRatingDataService>();
builder.Services.AddSingleton<ISearchesDataService, SearchesDataService>();



//TypeAdapterConfig.GlobalSettings.Default.PreserveReference(true);

builder.Services.AddMapster();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();