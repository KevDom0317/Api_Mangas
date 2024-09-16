using MangaApi.Services;  // Para importar MangaService
using MangaApi.Models;    // Para importar Manga
using MangaApi.Infraestructure.Repositories; // Para importar MangaRepository

var builder = WebApplication.CreateBuilder(args);

// Registrar MangaRepository y MangaService en el contenedor de dependencias
builder.Services.AddScoped<MangaRepository>();
builder.Services.AddScoped<MangaService>();

// Agregar servicios para la generación de documentación Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar la generación de Swagger si estamos en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

// Endpoint para obtener todos los mangas
app.MapGet("/mangas", (MangaService mangaService) =>
{
    return Results.Ok(mangaService.GetAll());
})
.WithName("GetAllMangas")
.WithOpenApi();

// Endpoint para obtener un manga por ID
app.MapGet("/mangas/{id}", (int id, MangaService mangaService) =>
{
    var manga = mangaService.GetById(id);
    return manga is not null ? Results.Ok(manga) : Results.NotFound();
})
.WithName("GetMangaById")
.WithOpenApi();

// Endpoint para agregar un nuevo manga
app.MapPost("/mangas", (Manga manga, MangaService mangaService) =>
{
    mangaService.Add(manga);
    return Results.Created($"/mangas/{manga.Id}", manga);
})
.WithName("AddManga")
.WithOpenApi();

// Endpoint para actualizar un manga existente
app.MapPut("/mangas/{id}", (int id, Manga updatedManga, MangaService mangaService) =>
{
    var existingManga = mangaService.GetById(id);
    if (existingManga is not null)
    {
        mangaService.Update(updatedManga);
        return Results.NoContent();
    }
    return Results.NotFound();
})
.WithName("UpdateManga")
.WithOpenApi();

// Endpoint para eliminar un manga por ID
app.MapDelete("/mangas/{id}", (int id, MangaService mangaService) =>
{
    var existingManga = mangaService.GetById(id);
    if (existingManga is not null)
    {
        mangaService.Delete(id);
        return Results.NoContent();
    }
    return Results.NotFound();
})
.WithName("DeleteManga")
.WithOpenApi();

// Iniciar la aplicación
app.Run();
