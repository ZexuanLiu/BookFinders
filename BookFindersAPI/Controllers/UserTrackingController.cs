using BookFindersAPI.Interfaces;
using BookFindersAPI.Services;
using BookFindersLibrary.Models;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace BookFindersAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserTrackingController : ControllerBase
    {
        private IDatabase _userTrackingDatabase;

        public UserTrackingController(ProductionDatabase productionDatabase, TestDatabase testDatabase)
        {
            bool isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

            if (isDev)
            {
                _userTrackingDatabase = testDatabase;

            }
            else
            {
                _userTrackingDatabase = productionDatabase;
            }

            if (_userTrackingDatabase == null)
            {
                throw new Exception("User Tracking database was not initialized!");
            }
        }

        [HttpPost("sendUserTracking")]
        public async Task<IActionResult> SendUserTracking(UserTrackingSession userTrackingSession)
        {
            try
            {
                UserTrackingSession filteredUserTrackingSession = new UserTrackingSession()
                {
                    Campus = userTrackingSession.Campus,
                    Locations = userTrackingSession.Locations,
                    TimeStarted = userTrackingSession.Locations.First().PostDateTime,
                    TimeEnded = userTrackingSession.Locations.Last().PostDateTime,
                };

                var userTrackingSessionTask = _userTrackingDatabase.SendUserTrackingSession(filteredUserTrackingSession);
                await userTrackingSessionTask;

                UserTrackingSession newSentUserTrackingSession = userTrackingSessionTask.Result;

                ResponseDTO responseDTOOk = new ResponseDTO()
                {
                    Status = 200,
                    Message = "Successfully sent user tracking session",
                    Data = newSentUserTrackingSession
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
