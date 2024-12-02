using BookFindersWebApp.Models;
using BookFindersLibrary.Models;
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

        public async Task<IActionResult> DataAnalytics()
        {
            DataAnalyticsCondition tempDataAnalyticsCondition = new DataAnalyticsCondition()
            {
                Campus = BookFindersLibrary.Enums.SheridanCampusEnum.All,
                NavigationMethod = BookFindersLibrary.Enums.NavigationMethodEnmu.All,
                StartDate = DateTime.MinValue,
                EndDate = DateTime.Now
            };
            DataAnalyticsRepository dataAnalyticsRepository = new DataAnalyticsRepository();
            DataAnalyticsModel dataAnalyticsModel = new DataAnalyticsModel();
            await dataAnalyticsRepository.FilterByDataAnalyticsCondition(tempDataAnalyticsCondition);
            dataAnalyticsModel.DataModel = dataAnalyticsRepository;
            return View("DataAnalytics", dataAnalyticsModel);
        }

        public IActionResult ConfirmationLogin(LoginForm form)
        {
            if (ModelState.IsValid)
            {
                UserLogin userLogin = new UserLogin()
                {
                    Username = form.Username,
                    Password = form.Password
                };
                User loggedInUser = LoginRepository.Login(userLogin).Result;

                if (loggedInUser != null)
                {
                    ViewBag.UserFullname = loggedInUser.Fullname;
                    ViewBag.UserUsername = loggedInUser.UserLogin.Username;
                    ViewBag.UserRole = loggedInUser.Role;
                    return View("Home");
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
        public async Task<IActionResult> FilterDataAnalytics(DataAnalyticsCondition condition)
        {

            if (ModelState.IsValid)
            {
                DataAnalyticsCondition tempDataAnalyticsCondition = new DataAnalyticsCondition()
                {
                    Campus = condition.Campus,
                    NavigationMethod = condition.NavigationMethod,
                    StartDate = condition.StartDate,
                    EndDate = condition.EndDate
                };

                DataAnalyticsRepository dataAnalyticsRepository = new DataAnalyticsRepository();
                await dataAnalyticsRepository.FilterByDataAnalyticsCondition(tempDataAnalyticsCondition);
                DataAnalyticsModel dataAnalyticsModel = new DataAnalyticsModel();
                dataAnalyticsModel.DataModel = dataAnalyticsRepository;
                return View("DataAnalytics", dataAnalyticsModel);
            }
            return View("DataAnalytics");

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