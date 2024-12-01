using BookFindersAPI.Interfaces;
using BookFindersAPI.Services;
using BookFindersLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using BookFindersLibrary.Enums;

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
                   NavigationMethod = bookSearchHistory.NavigationMethod,
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
        [HttpGet("getTop5BookSearchHistory")]
        public async Task<IActionResult> GetTop5BookSearchHistory()
        {
            try
            {
                var getBookSearchHistoryTask = _bookSearchHistoryDatabase.GetAllBookSearchHistory();
                await getBookSearchHistoryTask;

                IEnumerable<BookSearchHistory> bookSearchHistoryList = getBookSearchHistoryTask.Result;

                var subjectFrequency = bookSearchHistoryList
                    .GroupBy(obj => obj.Subject) 
                    .Select(group => new
                    {
                        Subject = group.Key,
                        Count = group.Count()
                    })
                    .OrderByDescending(x => x.Count) 
                    .Take(5) 
                    .ToList();
                if(subjectFrequency.Count != 0)
                {
                    var response = new BookSearchHistoryResponse
                    {
                        TopSubjects = subjectFrequency.Select(x => x.Subject).ToList(),
                        SubjectCounts = subjectFrequency.Select(x => x.Count).ToList()
                    };
                    ResponseDTO responseDTOOk = new ResponseDTO()
                    {
                        Status = 200,
                        Message = "Successfully fetched all book search history",
                        Data = response
                    };

                    return Ok(responseDTOOk);
                }
                else
                {
                    throw new Exception("subjectFrequency list is empty");
                }
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
        [HttpPost("getTop5BookSearchHistoryWithCondition")]
        public async Task<IActionResult> GetTop5BookSearchHistory(DataAnalystCondition dataAnalystCondition)
        {
            try
            {
                var getBookSearchHistoryTask = _bookSearchHistoryDatabase.GetAllBookSearchHistory();
                await getBookSearchHistoryTask;

                IEnumerable<BookSearchHistory> bookSearchHistoryList = getBookSearchHistoryTask.Result;

                //if !dataAnalystCondition.Campus.HasValue is true will ignore check the Campus field.
                var filteredList = bookSearchHistoryList.Where(history =>
                    (!dataAnalystCondition.Campus.HasValue || dataAnalystCondition.Campus == SheridanCampusEnum.All || history.Campus == dataAnalystCondition.Campus) &&
                    (!dataAnalystCondition.NavigationMethod.HasValue || dataAnalystCondition.NavigationMethod == NavigationMethodEnmu.All || history.NavigationMethod == dataAnalystCondition.NavigationMethod) &&
                    (!dataAnalystCondition.StartDate.HasValue || history.SearchDate >= dataAnalystCondition.StartDate) && 
                    (dataAnalystCondition.StartDate.HasValue || !dataAnalystCondition.EndDate.HasValue || (history.SearchDate >= dataAnalystCondition.StartDate && history.SearchDate <= dataAnalystCondition.EndDate))
                ).ToList();
                var subjectFrequency = filteredList
                    .GroupBy(obj => obj.Subject) 
                    .Select(group => new
                    {
                        Subject = group.Key,
                        Count = group.Count()
                    })
                    .OrderByDescending(x => x.Count) 
                    .Take(5) 
                    .ToList();

                    var response = new BookSearchHistoryResponse
                    {
                        TopSubjects = subjectFrequency.Select(x => x.Subject).ToList(),
                        SubjectCounts = subjectFrequency.Select(x => x.Count).ToList()
                    };
                    ResponseDTO responseDTOOk = new ResponseDTO()
                    {
                        Status = 200,
                        Message = "Successfully fetched all book search history",
                        Data = response
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
        [HttpDelete("removeBookSearchHistory/{historyId}")]
        public async Task<IActionResult> RemoveBookSearchHistory(int historyId)
        {
            try
            {
                var removeHistoryTask = _bookSearchHistoryDatabase.RemoveBookSearchHistory(historyId);
                await removeHistoryTask;

                bool result = removeHistoryTask.Result;

                ResponseDTO responseDTOOk = new ResponseDTO()
                {
                    Status = 200,
                    Message = "Successfully remove bookSearchHistory",
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
        [HttpPut("editBookSearchHistory/{historyId}/{newMethod}")]
        public async Task<IActionResult> EditBookSearchHistoryNavigationMethod(int historyId, NavigationMethodEnmu newMethod)
        {
            try
            {
                var EditBookSearchHistoryTask = _bookSearchHistoryDatabase.EditBookSearchHistoryNavigationMethod(historyId,newMethod);
                await EditBookSearchHistoryTask;

                bool result = EditBookSearchHistoryTask.Result;

                ResponseDTO responseDTOOk = new ResponseDTO()
                {
                    Status = 200,
                    Message = "Successfully UpdateB BookSearchHistory",
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