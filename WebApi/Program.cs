using DataLayer;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.kklk

builder.Services.AddSingleton<IUserDataService, UserDataService>();
builder.Services.AddSingleton(new Hashing());
builder.Services.AddSingleton<ITitleDataService, TitleDataService>();
builder.Services.AddSingleton<INameDataService, NameDataService>();
builder.Services.AddSingleton<IGenreDataService, GenreDataService>();
builder.Services.AddSingleton<IBookmarkNameDataService, BookmarkNameDataService>();
builder.Services.AddSingleton<IBookmarkTitleDataService, BookmarkTitleDataService>();
builder.Services.AddSingleton<IUserRatingDataService, UserRatingDataService>();
builder.Services.AddSingleton<ISearchesDataService, SearchesDataService>();

var secret = builder.Configuration.GetSection("Auth:Secret").Value;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ClockSkew = TimeSpan.Zero
        }


    );


// CORS Policy to allow all origins
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()  // Allow any origin
              .AllowAnyMethod()  // Allow any HTTP method
              .AllowAnyHeader(); // Allow any header
    });
});


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
// Apply CORS policy
app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();