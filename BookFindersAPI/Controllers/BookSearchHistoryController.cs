using BookFindersAPI.Interfaces;
using BookFindersAPI.Services;
using BookFindersLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookFindersAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookSearchHistoryController : ControllerBase
    {
        private IDatabase _bookSearchHistoryDatabase;

        public BookSearchHistoryController(ProductionDatabase productionDatabase, TestDatabase testDatabase)
        {
            bool isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

            if (isDev)
            {
                _bookSearchHistoryDatabase = testDatabase;
            }
            else
            {
                _bookSearchHistoryDatabase = productionDatabase;
            }

            if (_bookSearchHistoryDatabase == null)
            {
                throw new Exception("BookSearchHistory database was not initialized!");
            }
        }
        [HttpPost("InsertBookSearchHistory")]
        public async Task<IActionResult> InsertBookSearchHistory(BookSearchHistory bookSearchHistory)
        {
            try
            {
                BookSearchHistory tempBookSearchHistory = new BookSearchHistory()
                {
                   Campus = bookSearchHistory.Campus,
                   Subject = bookSearchHistory.Subject,
                   SearchDate = DateTime.UtcNow
                };

                var addBookSearchHistory = _bookSearchHistoryDatabase.AddBookSearchHistory(tempBookSearchHistory);
                await addBookSearchHistory;
                BookSearchHistory newlyAddBookSearchHistory = addBookSearchHistory.Result;

                 ResponseDTO responseDTOOk = new ResponseDTO()
                 {
                     Status = 200,
                     Message = "Successfully add an new book search record",
                     Data = newlyAddBookSearchHistory
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
        [HttpGet("getAllBookSearchHistory")]
        public async Task<IActionResult> GetAllBookSearchHistory()
        {
            try
            {
                var getBookSearchHistoryTask = _bookSearchHistoryDatabase.GetAllBookSearchHistory();
                await getBookSearchHistoryTask;

                IEnumerable<BookSearchHistory> bookSearchHistoryList = getBookSearchHistoryTask.Result;

                ResponseDTO responseDTOOk = new ResponseDTO()
                {
                    Status = 200,
                    Message = "Successfully fetched all book search history",
                    Data = bookSearchHistoryList
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
        [HttpGet("getAllBookSearchHistory/{year}/{month}")]
        public async Task<IActionResult> GetAllBookSearchHistoryByYearAndMonth(int year, int month)
        {
            try
            {
                var getBookSearchHistoryTask = _bookSearchHistoryDatabase.GetAllBookSearchHistory();
                await getBookSearchHistoryTask;

                IEnumerable<BookSearchHistory> bookSearchHistoryList = getBookSearchHistoryTask.Result;
                bookSearchHistoryList = bookSearchHistoryList.Where(obj => obj.SearchDate.Year == year && obj.SearchDate.Month == month).ToList();
                ResponseDTO responseDTOOk = new ResponseDTO()
                {
                    Status = 200,
                    Message = "Successfully fetched all book search history by year with month",
                    Data = bookSearchHistoryList
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