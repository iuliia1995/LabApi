using Microsoft.AspNetCore.Mvc;
using LabApi.Models;

namespace LabApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestsController : ControllerBase
    {
        private static LinkedList<SupportRequest> _requests = new LinkedList<SupportRequest>();

        [HttpGet]
        public ActionResult<List<SupportRequest>> GetAll()
        {
            return Ok(_requests.ToList());
        }

        [HttpGet("next")]
        public ActionResult<SupportRequest> GetNext()
        {
            if (_requests.Count == 0) return NotFound("Очередь пуста");
            return Ok(_requests.First.Value);
        }

        [HttpGet("count")]
        public ActionResult<int> GetCount()
        {
            return Ok(_requests.Count);
        }

        [HttpPost]
        public ActionResult<SupportRequest> Create(SupportRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ClientName) || string.IsNullOrWhiteSpace(request.Description))
                return BadRequest("Имя клиента и описание проблемы не могут быть пустыми");

            request.Id = _requests.Count > 0 ? _requests.Max(r => r.Id) + 1 : 1;
            request.CreatedAt = DateTime.Now;

            _requests.AddLast(request);

            return CreatedAtAction(nameof(GetNext), new { id = request.Id }, request);
        }

        [HttpPost("process")]
        public IActionResult Process()
        {
            if (_requests.Count == 0) return NotFound("Очередь пуста");

            _requests.RemoveFirst();
            return Ok("Заявка обработана и удалена из очереди");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var request = _requests.FirstOrDefault(r => r.Id == id);
            if (request == null) return NotFound("Заявка не найдена");

            _requests.Remove(request);
            return NoContent();
        }
    }
}