using BookFindersWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookFindersWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Home()
        {
            return View();
        }

        public IActionResult PushNotificationForm()
        {
            return View();
        }

        public IActionResult PushNotificationHistory()
        {
            return View();
        }

        public IActionResult ConfirmationLogin(LoginForm form)
        {
            if (ModelState.IsValid)
            {
                if (form.Username.Equals("Librarian") && form.Password.Equals("password"))
                {
                    return View("Home");
                }
                else if(form.Username.Equals("Faculty") && form.Password.Equals("password"))
                {
                    ModelState.AddModelError("Unauthorized Login", "This tool is for Librarians only.");
                    return View("Index");
                }
                else
                {
                    ModelState.AddModelError("Unknown Login", "Unknown Username or Password, please try again...");
                    return View("Index");
                }
                
            }
            return View("Index");
        }

        [HttpPost]
        public IActionResult Confirmation(PushNotificationForm form)
        {

            if (ModelState.IsValid)
            {
                PushNotification tempPushNotification = new PushNotification() {
                    Title = form.Title,
                    Description = form.Description,
                    StartDateTime = form.StartDateTime,
                    EndDateTime = form.EndDateTime
                };

                PushNotificationRepository.AddPushNotification(tempPushNotification);
                return View("Confirmation");
            }
            return View("PushNotificationForm");

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}