using Microsoft.AspNetCore.Mvc;

namespace BookFindersAPI.Controllers
{
    public class BookLocationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
