namespace BookFindersLibrary.Models
{
    public class ResponseDTO
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }
        public object? Errors { get; set; }
    }
}