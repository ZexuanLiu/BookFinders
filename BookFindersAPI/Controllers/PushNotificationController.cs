using BookFindersAPI.Interfaces;
using BookFindersAPI.Services;
using BookFindersLibrary.Models;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

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
                if (controllerStartUpTracker.IsInitialRunOfControllers())
                {
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

            if (controllerStartUpTracker.IsInitialRunOfControllers())
            {
                // //https://github.com/jfversluis/XFFCMPushNotificationsSample

                JObject credentialJson = new JObject();
                string? bookFindersProjectId = Environment.GetEnvironmentVariable("bookfindersFirebaseProjectId");
                string? bookFindersPrivateKeyId = Environment.GetEnvironmentVariable("bookfindersFirebasePrivateKeyId");
                string? bookFindersPrivateKey = Environment.GetEnvironmentVariable("bookfindersFirebasePrivateKey");
                string? bookFindersClientId = Environment.GetEnvironmentVariable("bookfindersFirebaseClientId");
                string? bookFindersClientEmail = Environment.GetEnvironmentVariable("bookfindersFirebaseClientEmail");
                string? bookFindersClientCertURL = Environment.GetEnvironmentVariable("bookfindersFirebaseClientCertURL");

                if (bookFindersPrivateKey != null)
                {
                    bookFindersPrivateKey = bookFindersPrivateKey.Replace(@"\\n", $"{Environment.NewLine}");
                    bookFindersPrivateKey = bookFindersPrivateKey.Replace(@"\n", $"{Environment.NewLine}");
                }

                credentialJson.Add("type", "service_account");
                credentialJson.Add("project_id", bookFindersProjectId);
                credentialJson.Add("private_key_id", bookFindersPrivateKeyId);
                credentialJson.Add("private_key", bookFindersPrivateKey);
                credentialJson.Add("client_email", bookFindersClientEmail);
                credentialJson.Add("client_id", bookFindersClientId);
                credentialJson.Add("auth_uri", "https://accounts.google.com/o/oauth2/auth");
                credentialJson.Add("token_uri", "https://oauth2.googleapis.com/token");
                credentialJson.Add("auth_provider_x509_cert_url", "https://www.googleapis.com/oauth2/v1/certs");
                credentialJson.Add("client_x509_cert_url", bookFindersClientCertURL);

                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromJson(credentialJson.ToString())
                });
            }

            if (_pushNotificationDatabase == null)
            {
                throw new Exception("Push Notification database was not initialized!");
            }

            controllerStartUpTracker.SetIsInitialRunOfControllers(false);
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
                    StartDateTime = pushNotification.StartDateTime,
                    EndDateTime = pushNotification.EndDateTime
                };

                var addPushNotificationTask = _pushNotificationDatabase.AddPushNotification(filteredPushNotification);
                await addPushNotificationTask;

                PushNotification newlyAddedPushNotification = addPushNotificationTask.Result;

                // //https://github.com/jfversluis/XFFCMPushNotificationsSample
                var message = new Message()
                {
                    Topic = "all",
                    Notification = new Notification()
                    {
                        Title = newlyAddedPushNotification.Title,
                        Body = newlyAddedPushNotification.Description,
                    }
                };

                try
                {
                    // //https://github.com/jfversluis/XFFCMPushNotificationsSample
                    Task<string> sendPushNotificationTask = FirebaseMessaging.DefaultInstance.SendAsync(message);
                    await sendPushNotificationTask;
                    string resultOfNotification = sendPushNotificationTask.Result;
                }
                catch (Exception e)
                {
                    ResponseDTO responseDTOError = new ResponseDTO
                    {
                        Status = 500,
                        Message = "An unexpected server error occurred",
                        Errors = e
                    };
                    return BadRequest(responseDTOError);
                }

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
