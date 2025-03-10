using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllMotorcycles()
        {
            string [] motorcycles = new string []{ "Kawasaki", "Ktm", "Yamaha", "Honda", "Aprilla" };

            return Ok(motorcycles);
        }
    }
}
