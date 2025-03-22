using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using TunaAPI.Models;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// allows passing datetimes without time zone data 
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


// allows our api endpoints to access the database through Entity Framework Core
builder.Services.AddNpgsql<TunaAPIDbContext>(builder.Configuration["TunaAPIDbConnectionString"]);

// Set the JSON serializer options
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

//  This configures how the backend handles requests from the frontend
// It's essentially giving permission to other applications to access your backend.
// It will only handle requests from the address in the WithOrgins function
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});


var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigin");
app.UseHttpsRedirection();

//calls
app.MapPost("/songs", (TunaAPIDbContext db, Song newSong) =>
{
    db.Songs.Add(newSong);
    db.SaveChanges();
    return Results.Created($"/songs/{newSong.Id}", newSong);
});

app.MapDelete("/songs/{songId}", (TunaAPIDbContext db, int songId) =>
{
    var song = db.Songs.Find(songId);
    if (song == null)
    {
        return Results.NotFound();
    }

    db.Songs.Remove(song);
    db.SaveChanges();
    return Results.NoContent(); // 204 No Content
});

app.MapPut("/songs/{songId}", (TunaAPIDbContext db, int songId, Song updatedSong) =>
{
    var song = db.Songs.Find(songId);
    if (song == null)
    {
        return Results.NotFound();
    }

    song.Title = updatedSong.Title;
    song.ArtistId = updatedSong.ArtistId;
    song.Album = updatedSong.Album;
    song.Length = updatedSong.Length;

    db.SaveChanges();
    return Results.Ok(song);
});

app.MapGet("/songs", (TunaAPIDbContext db) =>
{
    var songs = db.Songs.ToList();
    return Results.Ok(songs);
});

app.MapGet("/songs/{songId}", (TunaAPIDbContext db, int songId) =>
{
    var song = db.Songs.Find(songId);
    if (song == null)
    {
        return Results.NotFound();
    }

    var artist = db.Artists.Find(song.ArtistId);

    var genres = db.SongGenres
        .Where(sg => sg.SongId == songId)
        .Join(db.Genres, sg => sg.GenreId, g => g.Id, (sg, g) => new
        {
            g.Id,
            g.Description
        })
        .ToList();

    var response = new
    {
        song.Id,
        song.Title,
        Artist = artist != null ? new 
        {
            artist.Id,
            artist.Name,
            artist.Age,
            artist.Bio
        } : null,
        song.Album,
        song.Length,
        Genres = genres
    };

    return Results.Ok(response);
});

app.MapPost("/genres", (TunaAPIDbContext db, Genre newGenre) =>
{
    db.Genres.Add(newGenre);
    db.SaveChanges();
    return Results.Created($"/genres/{newGenre.Id}", newGenre);
});

app.MapDelete("/genres/{genreId}", (TunaAPIDbContext db, int genreId) =>
{
    var genre = db.Genres.Find(genreId);
    if (genre == null)
    {
        return Results.NotFound();
    }

    db.Genres.Remove(genre);
    db.SaveChanges();
    return Results.NoContent(); // 204 No Content
});

app.MapPut("/genres/{genreId}", (TunaAPIDbContext db, int genreId, Genre updatedGenre) =>
{
    var genre = db.Genres.Find(genreId);
    if (genre == null)
    {
        return Results.NotFound();
    }

    genre.Description = updatedGenre.Description;

    db.SaveChanges();
    return Results.Ok(genre);
});

app.MapGet("/genres", (TunaAPIDbContext db) =>
{
    var genres = db.Genres.ToList();
    return Results.Ok(genres);
});

app.MapGet("/genres/{genreId}", (TunaAPIDbContext db, int genreId) =>
{
    var genre = db.Genres.Find(genreId);
    if (genre == null)
    {
        return Results.NotFound();
    }

    var songs = db.SongGenres
        .Where(sg => sg.GenreId == genreId)
        .Join(db.Songs, sg => sg.SongId, s => s.Id, (sg, s) => new
        {
            s.Id,
            s.Title,
            s.Album,
            s.Length,
            ArtistId = s.ArtistId
        })
        .ToList();

    var response = new
    {
        genre.Id,
        genre.Description,
        Songs = songs
    };

    return Results.Ok(response);
});

app.MapPost("/artists", (TunaAPIDbContext db, Artist newArtist) =>
{
    db.Artists.Add(newArtist);
    db.SaveChanges();
    return Results.Created($"/artists/{newArtist.Id}", newArtist);
});

app.MapDelete("/artists/{artistId}", (TunaAPIDbContext db, int artistId) =>
{
    var artist = db.Artists.Find(artistId);
    if (artist == null)
    {
        return Results.NotFound();
    }

    db.Artists.Remove(artist);
    db.SaveChanges();
    return Results.NoContent(); // 204 No Content
});

app.MapPut("/artists/{artistId}", (TunaAPIDbContext db, int artistId, Artist updatedArtist) =>
{
    var artist = db.Artists.Find(artistId);
    if (artist == null)
    {
        return Results.NotFound();
    }

    artist.Name = updatedArtist.Name;
    artist.Age = updatedArtist.Age;
    artist.Bio = updatedArtist.Bio;

    db.SaveChanges();

    return Results.Ok(artist);
});

app.MapGet("/artists", (TunaAPIDbContext db) =>
{
    var artists = db.Artists.ToList();
    return Results.Ok(artists);
});

app.MapGet("/artists/{artistId}", (TunaAPIDbContext db, int artistId) =>
{
    var artist = db.Artists.Find(artistId);
    if (artist == null)
    {
        return Results.NotFound();
    }

    var songs = db.Songs
        .Where(s => s.ArtistId == artistId)
        .Select(s => new 
        {
            s.Id,
            s.Title,
            s.Album,
            s.Length,
            Genres = db.SongGenres
                .Where(sg => sg.SongId == s.Id)
                .Join(db.Genres, sg => sg.GenreId, g => g.Id, (sg, g) => new
                {
                    g.Id,
                    g.Description
                })
                .ToList() 
        })
        .ToList();

    var response = new
    {
        artist.Id,
        artist.Name,
        artist.Age,
        artist.Bio,
        SongCount = songs.Count,
        Songs = songs
    };

    return Results.Ok(response);
});

app.MapPost("/songgenres", (TunaAPIDbContext db, SongGenre songGenre) =>
{
    db.SongGenres.Add(songGenre);
    db.SaveChanges();
    return Results.Created($"/songgenres/{songGenre.SongId}/{songGenre.GenreId}", songGenre);
});

app.Run();
