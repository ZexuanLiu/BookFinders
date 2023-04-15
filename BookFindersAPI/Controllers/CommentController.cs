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
                   UserId = comment.UserId,
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
        [HttpGet("getComments/{bookId}")]
        public async Task<IActionResult> GetComments(string bookId)
        {
            try
            {
                var getCommentsTask = _pushNotificationDatabase.GetBookComments(bookId);
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
        [HttpGet("getAllComments")]
        public async Task<IActionResult> GetAllComments()
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
        [HttpPut("addthumbsUp/{commentId}")]
        public async Task<IActionResult> addThumbsUp(int commentId)
        {
            try
            {
                var addThumbsUpTask = _pushNotificationDatabase.addThumbsUp(commentId);
                await addThumbsUpTask;

                bool result = addThumbsUpTask.Result;

                ResponseDTO responseDTOOk = new ResponseDTO()
                {
                    Status = 200,
                    Message = "Successfully add Thumbs Up",
                    Data = result
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
        [HttpDelete("removeComment/{commentId}")]
        public async Task<IActionResult> removeComment(int commentId)
        {
            try
            {
                var removeCommentTask = _pushNotificationDatabase.removeComment(commentId);
                await removeCommentTask;

                bool result = removeCommentTask.Result;

                ResponseDTO responseDTOOk = new ResponseDTO()
                {
                    Status = 200,
                    Message = "Successfully remove comment",
                    Data = result
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
        [HttpPut("editComment/{commentId}/{newComment}")]
        public async Task<IActionResult> EditComment(int commentId, string newComment)
        {
            try
            {
                var EditCommentTask = _pushNotificationDatabase.EditComment(commentId,newComment);
                await EditCommentTask;

                bool result = EditCommentTask.Result;

                ResponseDTO responseDTOOk = new ResponseDTO()
                {
                    Status = 200,
                    Message = "Successfully add Thumbs Up",
                    Data = result
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