using BookFindersAPI.Interfaces;
using BookFindersLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookFindersAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class HelloWorldController : ControllerBase
    {
        [HttpGet("helloWorld")]
        public IActionResult HelloWWorld()
        {
            return Ok("Hello World!");
        }
    }
}
