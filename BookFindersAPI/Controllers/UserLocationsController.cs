using BookFindersAPI.Interfaces;
using BookFindersAPI.Services;
using BookFindersLibrary.Models;
using Microsoft.AspNetCore.Mvc;


namespace BookFindersAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserLocationsController : ControllerBase
    {
       private IDatabase _pushNotificationDatabase;

        public UserLocationsController(ProductionDatabase productionDatabase, TestDatabase testDatabase)
        {
            bool isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            ControllerStartUpTracker controllerStartUpTracker = ControllerStartUpTracker.GetInstance();

            if (isDev)
            {

                if (controllerStartUpTracker.IsInitialRunPushNotificationController())
                {
                    controllerStartUpTracker.SetIsInitialRunPushNotificationController(false);
                    var loadingDefaultPushNotificationsTask = testDatabase.LoadDefaultPushNotificationHistoryAndReset();

                    while (!loadingDefaultPushNotificationsTask.IsCompleted) { Thread.Sleep(500); }

                    if (loadingDefaultPushNotificationsTask.Result != true)
                    {
                        throw new Exception("Test database could not load default push notifications!");
                    }
                    
                }
                _pushNotificationDatabase = testDatabase;

            }
            else
            {
                _pushNotificationDatabase = productionDatabase;
            }

            if (_pushNotificationDatabase == null)
            {
                throw new Exception("Push Notification database was not initialized!");
            }

            controllerStartUpTracker.SetIsInitialRunPushNotificationController(false);
        }

        [HttpPost("addlocations")]
        public async Task<IActionResult> AddLocations(UserLocations locations)
        {
            try
            {
                UserLocations filteredlocations = new UserLocations()
                {
                   Id = locations.Id,
                   UserId = locations.UserId,
                   DestinationId = "1",
                   XCoordinate = locations.XCoordinate,
                   YCoordinate = locations.YCoordinate,
                };

                var addLocationTask = _pushNotificationDatabase.AddLocation(filteredlocations);
                await addLocationTask;

                UserLocations newlyAddedLocation = addLocationTask.Result;

                 ResponseDTO responseDTOOk = new ResponseDTO()
                 {
                     Status = 200,
                     Message = "Successfully add new comment",
                     Data = newlyAddedLocation
                 };

                return Ok(responseDTOOk);
               
            }
            catch (Exception e)
            {
                ResponseDTO responseDTOError = new ResponseDTO
                {
                    Status = 400,
                    Message = "An unexpected server error occurred",
                    Errors = e
                };

                return BadRequest(responseDTOError);
            }
        }
        [HttpGet("getAllLocations")]
        public async Task<IActionResult> GetAllLocations()
        {
            try
            {
                var getLocationsTask = _pushNotificationDatabase.GetLocations();
                await getLocationsTask;

                IEnumerable<UserLocations> locations = getLocationsTask.Result;

                ResponseDTO responseDTOOk = new ResponseDTO()
                {
                    Status = 200,
                    Message = "Successfully fetched push comments",
                    Data = locations
                };

                return Ok(responseDTOOk);
            }
            catch (Exception e)
            {
                ResponseDTO responseDTOError = new ResponseDTO
                {
                    Status = 400,
                    Message = "An unexpected server error occurred",
                    Errors = e
                };
                
                return BadRequest(responseDTOError);
            }
        }
    }
}