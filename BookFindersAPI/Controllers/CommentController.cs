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
         private IDatabase _commentsDatabase;

        public CommentController(ProductionDatabase productionDatabase, TestDatabase testDatabase)
        {
            bool isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

            if (isDev)
            {
                _commentsDatabase = testDatabase;
            }
            else
            {
                _commentsDatabase = productionDatabase;
            }

            if (_commentsDatabase == null)
            {
                throw new Exception("Comment database was not initialized!");
            }
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

                var addCommentTask = _commentsDatabase.AddComment(filteredComment);
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
                var getCommentsTask = _commentsDatabase.GetBookComments(bookId);
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
                var getCommentsTask = _commentsDatabase.GetComments();
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
        [HttpGet("addthumbsUp/{commentId}")]
        public async Task<IActionResult> addThumbsUp(int commentId)
        {
            try
            {
                var addThumbsUpTask = _commentsDatabase.addThumbsUp(commentId);
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
        [HttpGet("subthumbsUp/{commentId}")]
        public async Task<IActionResult> subThumbsUp(int commentId)
        {
            try
            {
                var subThumbsUpTask = _commentsDatabase.subThumbsUp(commentId);
                await subThumbsUpTask;

                bool result = subThumbsUpTask.Result;

                ResponseDTO responseDTOOk = new ResponseDTO()
                {
                    Status = 200,
                    Message = "Successfully sub Thumbs Up",
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
                var removeCommentTask = _commentsDatabase.removeComment(commentId);
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
                var EditCommentTask = _commentsDatabase.EditComment(commentId,newComment);
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