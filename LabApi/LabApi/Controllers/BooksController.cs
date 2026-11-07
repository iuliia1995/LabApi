using Microsoft.AspNetCore.Mvc;
using LabApi.Models;

namespace LabApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private static List<Book> _books = new List<Book>
        {
            new Book { Id = 1, Title = "Война и мир", Author = "Лев Толстой", Year = 1869, IsAvailable = true },
            new Book { Id = 2, Title = "Преступление и наказание", Author = "Федор Достоевский", Year = 1866, IsAvailable = false },
            new Book { Id = 3, Title = "Мастер и Маргарита", Author = "Михаил Булгаков", Year = 1967, IsAvailable = true },
            new Book { Id = 4, Title = "1984", Author = "Джордж Оруэлл", Year = 1949, IsAvailable = true },
            new Book { Id = 5, Title = "Гарри Поттер", Author = "Джоан Роулинг", Year = 1997, IsAvailable = false },
            new Book { Id = 6, Title = "Властелин колец", Author = "Джон Толкин", Year = 1954, IsAvailable = true },
            new Book { Id = 7, Title = "Маленький принц", Author = "Антуан де Сент-Экзюпери", Year = 1943, IsAvailable = true }
        };

        [HttpGet]
        public ActionResult<List<Book>> GetAll()
        {
            return Ok(_books);
        }

        [HttpGet("{id}")]
        public ActionResult<Book> GetById(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();
            return Ok(book);
        }

        [HttpGet("search/{title}")]
        public ActionResult<List<Book>> SearchByTitle(string title)
        {
            var result = _books.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();
            return Ok(result);
        }

        [HttpGet("available")]
        public ActionResult<List<Book>> GetAvailable()
        {
            var result = _books.Where(b => b.IsAvailable).ToList();
            return Ok(result);
        }

        [HttpPost]
        public ActionResult<Book> Create(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Title) || string.IsNullOrWhiteSpace(book.Author))
                return BadRequest("Название и автор не могут быть пустыми");

            book.Id = _books.Count > 0 ? _books.Max(b => b.Id) + 1 : 1;
            _books.Add(book);
            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Book updatedBook)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();

            if (string.IsNullOrWhiteSpace(updatedBook.Title) || string.IsNullOrWhiteSpace(updatedBook.Author))
                return BadRequest("Название и автор не могут быть пустыми");

            book.Title = updatedBook.Title;
            book.Author = updatedBook.Author;
            book.Year = updatedBook.Year;
            book.IsAvailable = updatedBook.IsAvailable;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();

            _books.Remove(book);
            return NoContent();
        }
    }
}