using Microsoft.AspNetCore.Mvc;
using LabApi.Models;

namespace LabApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private static List<TaskItem> _tasks = new List<TaskItem>
        {
            new TaskItem { Id = 1, Title = "Купить хлеб", Description = "Сходить в магазин", IsCompleted = false, CreatedAt = DateTime.Now },
            new TaskItem { Id = 2, Title = "Сделать лабу", Description = "Написать код для API", IsCompleted = true, CreatedAt = DateTime.Now.AddDays(-1) }
        };

        [HttpGet]
        public ActionResult<List<TaskItem>> GetAll()
        {
            return Ok(_tasks);
        }

        [HttpGet("completed")]
        public ActionResult<List<TaskItem>> GetCompleted()
        {
            var result = _tasks.Where(t => t.IsCompleted).ToList();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<TaskItem> GetById(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpPost]
        public ActionResult<TaskItem> Create(TaskItem task)
        {
            if (string.IsNullOrWhiteSpace(task.Title))
                return BadRequest("Название задачи не может быть пустым");

            task.Id = _tasks.Count > 0 ? _tasks.Max(t => t.Id) + 1 : 1;
            task.CreatedAt = DateTime.Now;

            _tasks.Add(task);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, TaskItem updatedTask)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound();

            if (string.IsNullOrWhiteSpace(updatedTask.Title))
                return BadRequest("Название задачи не может быть пустым");

            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.IsCompleted = updatedTask.IsCompleted;

            return NoContent();
        }

        [HttpPatch("{id}/complete")]
        public IActionResult Complete(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound();

            task.IsCompleted = true;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound();

            _tasks.Remove(task);
            return NoContent();
        }
    }
}