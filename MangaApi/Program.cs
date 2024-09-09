using MangaApi.Services;  // Para importar MangaService
using MangaApi.Models;    // Para importar Manga

var builder = WebApplication.CreateBuilder(args);

// Registrar MangaService como un singleton en el contenedor de dependencias
builder.Services.AddSingleton<MangaService>();

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

app.UseHttpsRedirection();

// Obtener el servicio MangaService a través de inyección de dependencias
var mangaService = app.Services.GetRequiredService<MangaService>();

// Endpoint para obtener todos los mangas
app.MapGet("/mangas", () =>
{
    return Results.Ok(mangaService.GetAll());
})
.WithName("GetAllMangas")
.WithOpenApi();

// Endpoint para obtener un manga por ID
app.MapGet("/mangas/{id}", (int id) =>
{
    var manga = mangaService.GetById(id);
    return manga is not null ? Results.Ok(manga) : Results.NotFound();
})
.WithName("GetMangaById")
.WithOpenApi();

// Endpoint para agregar un nuevo manga
app.MapPost("/mangas", (Manga manga) =>
{
    mangaService.Add(manga);
    return Results.Created($"/mangas/{manga.Id}", manga);
})
.WithName("AddManga")
.WithOpenApi();

// Endpoint para actualizar un manga existente
app.MapPut("/mangas/{id}", (int id, Manga updatedManga) =>
{
    var success = mangaService.Update(id, updatedManga);
    return success ? Results.NoContent() : Results.NotFound();
})
.WithName("UpdateManga")
.WithOpenApi();

// Endpoint para eliminar un manga por ID
app.MapDelete("/mangas/{id}", (int id) =>
{
    var success = mangaService.Delete(id);
    return success ? Results.NoContent() : Results.NotFound();
})
.WithName("DeleteManga")
.WithOpenApi();

// Iniciar la aplicación
app.Run();
