using MangaApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace MangaApi.Infraestructure.Repositories
{
    public class MangaRepository
    {
        private List<Manga> mangas = new List<Manga>(); // Simulación de base de datos en memoria

        public IEnumerable<Manga> GetAll()
        {
            return mangas;
        }

        public Manga GetById(int id)
        {
            return mangas.FirstOrDefault(m => m.Id == id);
        }

        public void Add(Manga manga)
        {
            manga.Id = mangas.Count > 0 ? mangas.Max(m => m.Id) + 1 : 1;
            mangas.Add(manga);
        }

        public void Update(Manga manga)
        {
            var existingManga = GetById(manga.Id);
            if (existingManga != null)
            {
                existingManga.Title = manga.Title;
                existingManga.Author = manga.Author;
                existingManga.PublicationDate = manga.PublicationDate;
            }
        }

        public void Delete(int id)
        {
            var manga = GetById(id);
            if (manga != null)
            {
                mangas.Remove(manga);
            }
        }
    }
}
