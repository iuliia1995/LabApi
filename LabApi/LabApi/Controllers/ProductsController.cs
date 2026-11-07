using Microsoft.AspNetCore.Mvc;
using LabApi.Models;

namespace LabApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private static List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Ноутбук", Category = "Электроника", Price = 50000, Quantity = 10 },
            new Product { Id = 2, Name = "Смартфон", Category = "Электроника", Price = 30000, Quantity = 25 },
            new Product { Id = 3, Name = "Наушники", Category = "Электроника", Price = 2000, Quantity = 50 },
            new Product { Id = 4, Name = "Клавиатура", Category = "Аксессуары", Price = 1500, Quantity = 100 },
            new Product { Id = 5, Name = "Мышь", Category = "Аксессуары", Price = 800, Quantity = 200 },
            new Product { Id = 6, Name = "Монитор", Category = "Электроника", Price = 15000, Quantity = 15 },
            new Product { Id = 7, Name = "Стол", Category = "Мебель", Price = 7000, Quantity = 5 },
            new Product { Id = 8, Name = "Стул", Category = "Мебель", Price = 3000, Quantity = 20 },
            new Product { Id = 9, Name = "Лампа", Category = "Мебель", Price = 1200, Quantity = 30 },
            new Product { Id = 10, Name = "Коврик для мыши", Category = "Аксессуары", Price = 500, Quantity = 150 }
        };

        [HttpGet]
        public ActionResult<List<Product>> GetAll()
        {
            return Ok(_products);
        }

        [HttpGet("{id}")]
        public ActionResult<Product> GetById(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpGet("category/{category}")]
        public ActionResult<List<Product>> GetByCategory(string category)
        {
            var result = _products.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
            return Ok(result);
        }

        [HttpGet("cheap")]
        public ActionResult<List<Product>> GetCheap([FromQuery] decimal maxPrice)
        {
            var result = _products.Where(p => p.Price < maxPrice).ToList();
            return Ok(result);
        }

        [HttpGet("search/{name}")]
        public ActionResult<List<Product>> SearchByName(string name)
        {
            var result = _products.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
            return Ok(result);
        }

        [HttpPost]
        public ActionResult<Product> Create(Product product)
        {
            if (product.Price < 0) return BadRequest("Цена не может быть отрицательной");
            if (product.Quantity < 0) return BadRequest("Количество не может быть отрицательным");

            product.Id = _products.Count > 0 ? _products.Max(p => p.Id) + 1 : 1;
            _products.Add(product);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Product updatedProduct)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            if (updatedProduct.Price < 0) return BadRequest("Цена не может быть отрицательной");
            if (updatedProduct.Quantity < 0) return BadRequest("Количество не может быть отрицательным");

            product.Name = updatedProduct.Name;
            product.Category = updatedProduct.Category;
            product.Price = updatedProduct.Price;
            product.Quantity = updatedProduct.Quantity;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            _products.Remove(product);
            return NoContent();
        }
    }
}