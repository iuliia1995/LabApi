using Microsoft.AspNetCore.Mvc;
using LabApi.Models;

namespace LabApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private static List<Student> _students = new List<Student>
        {
            new Student { Id = 1, FullName = "Иванов Иван", Age = 19, GroupName = "ИС-21", AverageGrade = 4.5 },
            new Student { Id = 2, FullName = "Петрова Анна", Age = 20, GroupName = "ИС-21", AverageGrade = 4.8 },
            new Student { Id = 3, FullName = "Сидоров Петр", Age = 18, GroupName = "ИС-22", AverageGrade = 3.9 },
            new Student { Id = 4, FullName = "Козлова Мария", Age = 19, GroupName = "ИС-22", AverageGrade = 4.2 },
            new Student { Id = 5, FullName = "Смирнов Алексей", Age = 21, GroupName = "ИС-23", AverageGrade = 4.7 }
        };

        [HttpGet]
        public ActionResult<List<Student>> GetAll()
        {
            return Ok(_students);
        }

        [HttpGet("{id}")]
        public ActionResult<Student> GetById(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student == null) return NotFound();
            return Ok(student);
        }

        [HttpGet("group/{groupName}")]
        public ActionResult<List<Student>> GetByGroup(string groupName)
        {
            var result = _students.Where(s => s.GroupName.Equals(groupName, StringComparison.OrdinalIgnoreCase)).ToList();
            return Ok(result);
        }

        [HttpPost]
        public ActionResult<Student> Create(Student student)
        {
            if (student.Age < 0) return BadRequest("Возраст не может быть отрицательным");
            if (student.AverageGrade < 0 || student.AverageGrade > 5) return BadRequest("Средний балл должен быть от 0 до 5");

            student.Id = _students.Count > 0 ? _students.Max(s => s.Id) + 1 : 1;
            _students.Add(student);
            return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Student updatedStudent)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student == null) return NotFound();

            if (updatedStudent.Age < 0) return BadRequest("Возраст не может быть отрицательным");
            if (updatedStudent.AverageGrade < 0 || updatedStudent.AverageGrade > 5) return BadRequest("Средний балл должен быть от 0 до 5");

            student.FullName = updatedStudent.FullName;
            student.Age = updatedStudent.Age;
            student.GroupName = updatedStudent.GroupName;
            student.AverageGrade = updatedStudent.AverageGrade;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student == null) return NotFound();

            _students.Remove(student);
            return NoContent();
        }
    }
}