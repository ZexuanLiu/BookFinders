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
       private IDatabase _userLocationsDatabase;

        public UserLocationsController(ProductionDatabase productionDatabase, TestDatabase testDatabase)
        {
            bool isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            if (isDev)
            {
                _userLocationsDatabase = testDatabase;
            }
            else
            {
                _userLocationsDatabase = productionDatabase;
            }

            if (_userLocationsDatabase == null)
            {
                throw new Exception("User Locations database was not initialized!");
            }
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

                var addLocationTask = _userLocationsDatabase.AddLocation(filteredlocations);
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
                var getLocationsTask = _userLocationsDatabase.GetLocations();
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