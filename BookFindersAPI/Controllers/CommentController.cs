using BookFindersAPI.Interfaces;
using BookFindersAPI.Services;
using BookFindersLibrary.Models;
using Microsoft.AspNetCore.Mvc;


namespace BookFindersAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
         private IDatabase _pushNotificationDatabase;

        public CommentController(ProductionDatabase productionDatabase, TestDatabase testDatabase)
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

        [HttpPost("postcomment")]
        public async Task<IActionResult> PostComment(Comment comment)
        {
            try
            {
                Comment filteredComment = new Comment()
                {
                   UserId = "001",
                   BookId = comment.BookId,
                   ThumbsUp = 0,
                   UserName = comment.UserName,
                   Description = comment.Description,
                   PostDateTime = DateTime.Now
                };

                var addCommentTask = _pushNotificationDatabase.AddComment(filteredComment);
                await addCommentTask;

                Comment newlyAddedComment = addCommentTask.Result;

                 ResponseDTO responseDTOOk = new ResponseDTO()
                 {
                     Status = 200,
                     Message = "Successfully add new comment",
                     Data = newlyAddedComment
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
        [HttpGet("getComments")]
        public async Task<IActionResult> GetComments()
        {
            try
            {
                var getCommentsTask = _pushNotificationDatabase.GetComments();
                await getCommentsTask;

                IEnumerable<Comment> comments = getCommentsTask.Result;

                ResponseDTO responseDTOOk = new ResponseDTO()
                {
                    Status = 200,
                    Message = "Successfully fetched push comments",
                    Data = comments
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