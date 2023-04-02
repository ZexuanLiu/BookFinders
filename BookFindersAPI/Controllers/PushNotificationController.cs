using BookFindersAPI.Interfaces;
using BookFindersAPI.Services;
using BookFindersLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookFindersAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PushNotificationController : ControllerBase
    {
        private IDatabase _pushNotificationDatabase;

        public PushNotificationController(ProductionDatabase productionDatabase, TestDatabase testDatabase)
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

        [HttpGet("getPushNotifications")]
        public async Task<IActionResult> GetPushNotifications()
        {
            try
            {
                var getPushNotificationsTask = _pushNotificationDatabase.GetPushNotifications();
                await getPushNotificationsTask;

                IEnumerable<PushNotification> pushNotifications = getPushNotificationsTask.Result;

                ResponseDTO responseDTOOk = new ResponseDTO()
                {
                    Status = 200,
                    Message = "Successfully fetched push notifications",
                    Data = pushNotifications
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

        [HttpPost("sendPushNotification")]
        public async Task<IActionResult> SendPushNotification(PushNotification pushNotification)
        {
            try
            {
                PushNotification filteredPushNotification = new PushNotification()
                {
                    Description = pushNotification.Description,
                    Title = pushNotification.Title,
                    StartDateTime = pushNotification.StartDateTime.ToUniversalTime(),
                    EndDateTime = pushNotification.EndDateTime.ToUniversalTime()
                };

                var addPushNotificationTask = _pushNotificationDatabase.AddPushNotification(filteredPushNotification);
                await addPushNotificationTask;

                PushNotification newlyAddedPushNotification = addPushNotificationTask.Result;

                // TODO: Add logic to actually send the push notification

                ResponseDTO responseDTOOk = new ResponseDTO()
                {
                    Status = 200,
                    Message = "Successfully sent out push notification",
                    Data = newlyAddedPushNotification
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
