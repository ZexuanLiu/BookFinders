using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using BookFindersLibrary.Models;
using BookFindersLibrary.Models.OnCampus;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BookFindersAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookSearchController : ControllerBase
    {
        private HttpClient client;
        public BookSearchController()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            client = new HttpClient(handler);
        }

        [HttpGet("{searchText}/{offset}")]
        public async Task<IActionResult> SearchBooks(string searchText, int offset)
        {   
            string? apiKey = Environment.GetEnvironmentVariable("bookfindersLibraryAPIKey");
            
            var response = await client.GetAsync("https://api-ca.hosted.exlibrisgroup.com/primo/v1/search?vid=01OCLS_SHER%3ASHER&tab=Everything&scope=MyInst_and_CI&q=q%3Dany%2Ccontains%2C"+searchText+"&lang=eng&offset="+offset+"&limit=10&sort=rank&pcAvailability=true&getMore=0&conVoc=true&inst=01OCLS_SHER&skipDelivery=true&disableSplitFacets=true&apikey="+apiKey);
            List<book> books = new List<book>();

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var bookObjLists = JsonConvert.DeserializeObject<BookResponse>(content);

                foreach (var doc in bookObjLists.docs)
                {
                    BookFindersLibrary.Models.PnxSort sort = doc.pnx.sort;
                    PnxSearch search = doc.pnx.search;
                    PnxLinks links = doc.pnx.links;
                    BookFindersLibrary.Models.BestLocation bestlocation = doc.delivery.bestlocation;
                    
                    var bookObj = new book
                    {
                        Id = "1",
                        Name = sort.title?.Count > 0 ? sort.title[0] : "Unknown Title",
                        Author = sort.author?.Count > 0 ? sort.author[0] : "Unknown Author",
                        Description = search.description?.Count > 0 ? search.description[0] : "Unknown Description",
                        ImageLink = await CheckImageValidity(links.thumbnail[0].Replace("$$T", "")),
                        LocationCode = bestlocation?.callNumber ?? "Unknown Location Code",
                        LibraryCode = "None",
                        LocationBookShelfNum = "1",
                        LocationBookShelfSide = "A",
                        LocationBookShelfRow = -1,
                        LocationBookShelfColumn = -1
                    };

                    books.Add(bookObj);
                }

                return Ok(books);
            }
            else
            {
                return BadRequest("Failed to fetch the books");
            }
        }
        [HttpGet("OnCampus/{searchText}/{offset}")]
        public async Task<IActionResult> SearchLibraryBooks(string searchText, int offset)
        {   
            string? apiKey = Environment.GetEnvironmentVariable("bookfindersLibraryAPIKey");
            var response = await client.GetAsync("https://api-ca.hosted.exlibrisgroup.com/primo/v1/search?vid=01OCLS_SHER%3ASHER&tab=Library_Physical&scope=MyInstitution_Physical&q=any%2Ccontains%2C"+searchText+"&multiFacets=multiFacets%3Dfacet_tlevel%2Cinclude%2Cavailable_p&lang=eng&offset="+offset+"&limit=10&sort=rank&pcAvailability=true&getMore=0&conVoc=true&inst=01OCLS_SHER&skipDelivery=false&disableSplitFacets=true&apikey="+apiKey);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var bookObjLists = JsonConvert.DeserializeObject<OnCampusBookResponse>(content);

                List<book> books = new List<book>();

                foreach (var doc in bookObjLists.docs)
                {
                    BookFindersLibrary.Models.OnCampus.PnxSort sort = doc.pnx.sort;
                    PnxDisplay display = doc.pnx.display;
                    BookFindersLibrary.Models.OnCampus.BestLocation bestlocation = doc.delivery.bestlocation;
                    string bookShelfInfo = await CheckBookShelfNum(bestlocation?.callNumber ?? "Unknown Location Code");
                    if (bookShelfInfo != "Unknown Location Code"){
                        int length = bookShelfInfo.Length;
                        string locationBookShelfNum = bookShelfInfo.Substring(0,length-1);
                        string locationBookShelfSide = bookShelfInfo.Substring(length-1);
                        var bookObj = new book
                        {
                        Id = "1",
                        Name = sort.title?.Count > 0 ? sort.title[0] : "Unknown Title",
                        Author = sort.author?.Count > 0 ? sort.author[0] : "Unknown Author",
                        Description = display.description?.Count > 0 ? display.description[0] : "Unknown Description",
                        ImageLink = "defaultBook.png",
                        LocationCode = bestlocation?.callNumber ?? "Unknown Location Code",
                        LibraryCode = bestlocation?.libraryCode ?? "Unknown Library Code",
                        LocationBookShelfNum = locationBookShelfNum,
                        LocationBookShelfSide = locationBookShelfSide,
                        LocationBookShelfRow = 0,
                        LocationBookShelfColumn = 0
                        };
                         books.Add(bookObj);
                    }
                    else{
                        var bookObj = new book
                        {
                        Id = "1",
                        Name = sort.title?.Count > 0 ? sort.title[0] : "Unknown Title",
                        Author = sort.author?.Count > 0 ? sort.author[0] : "Unknown Author",
                        Description = display.description?.Count > 0 ? display.description[0] : "Unknown Description",
                        ImageLink = "defaultBook.png",
                        LocationCode = bestlocation?.callNumber ?? "Unknown Location Code",
                        LibraryCode = bestlocation?.libraryCode ?? "Unknown Library Code",
                        LocationBookShelfNum = "Unknown Location Code",
                        LocationBookShelfSide = "Unknown Location Code",
                        LocationBookShelfRow = -1,
                        LocationBookShelfColumn = -1
                        };
                         books.Add(bookObj);
                    }
                        
                   
                }

                return Ok(books);
            }
            else
            {
                return BadRequest("Failed to fetch the books");
            }
        }
        private async Task<string> CheckImageValidity(string uri)
        {
            try
            {
                var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, uri));

                if (response.IsSuccessStatusCode)
                {
                    var contentType = response.Content.Headers.ContentType;
                    if (contentType != null && contentType.MediaType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                    {
                        // It is a Image
                        return uri;
                    }
                    else
                    {
                        // not a image
                        return "defaultBook.png";
                    }
                }
                else
                {
                    // request failed invaild url
                    return "defaultBook.png";
                }
            }
            catch (Exception ex)
            {
                //if the url is not a image console will print An Invalid request URL was provided.
                Console.WriteLine("Exception: " + ex.Message);
                return "defaultBook.png";
                
            }
        }
        private async Task<string> CheckBookShelfNum(string locationCode)
        {
            //use splite to detect blank
            string[] parts = locationCode.Split(' ');
            var result = await SeparateLettersAndNumbers(parts[0]);
                    // Access the result properties
            var letters = result.Letters;
            var numbers = result.Numbers;
            if((letters == "TK"&&numbers>=5105.888)||(letters=="TR"&&numbers<681)){
                return "7A";
            }
            else if ((letters == "TT"&&numbers>=212)||(letters=="ZA")){
                return "6A";
            }
            else if ((letters == "TR"&&numbers>=681)||(letters=="TT"&&numbers<=205)){
                return "7B";
            }
            else if ((letters=="Q"&&numbers>=175)||(letters=="R"&&numbers<=26)){
                return "8A";
            }
            else if ((letters=="R"&&numbers>=726)||(letters=="TK"&&numbers<5105.888)){
                return "8B";
            }
            else if ((letters=="PN"&&numbers>=6710)||(letters=="PS"&&numbers<3503)){
                return "9A";
            }
            else if ((letters=="PS"&&numbers>=3501)||(letters=="Q"&&numbers<175)){
                return "9B";
            }
            else if ((letters=="PN"&&numbers>=1994)&&(letters=="PN"&&numbers<2091)){
                return "10A";
            }
            else if ((letters=="PN"&&numbers>=2091)&&(letters=="PN"&&numbers<6700)){
                return "10B";
            }
            else if ((letters=="NK"&&numbers>=4335)||(letters=="P"&&numbers<112)){
                return "11A";
            }
            else if ((letters=="P"&&numbers>=116)||(letters=="PN"&&numbers<1994)){
                return "11B";
            }
            else if ((letters=="ND"&&numbers>=461)||(letters=="NE"&&numbers<1300)){
                return "12A";
            }
            else if ((letters=="NE"&&numbers>=1300)||(letters=="NK"&&numbers<4335)){
                return "12B";
            }
            else if ((letters=="NC"&&numbers>=760)||(letters=="NC"&&numbers<1002)){
                return "13A";
            }
            else if ((letters=="NC"&&numbers>=1002)||(letters=="ND"&&numbers<458)){
                return "13B";
            }
            else if ((letters=="N"&&numbers>=5300)&&(letters=="N"&&numbers<7405)){
                return "14A";
            }
            else if ((letters=="N"&&numbers>=7405)||(letters=="NC"&&numbers<758)){
                return "14B";
            }
            else if ((letters=="HQ"&&numbers>=1080)||(letters=="M"&&numbers<1630)){
                return "15A";
            }
            else if ((letters=="M"&&numbers>=1630)||(letters=="N"&&numbers<5300)){
                return "15B";
            }
            else if ((letters=="GT"&&numbers>=540)||(letters=="HE"&&numbers<7775)){
                return "16A";
            }
            else if ((letters=="HE"&&numbers>=7815)||(letters=="HQ"&&numbers<1075)){
                return "16B";
            }
            else if ((letters=="BL"&&numbers>=1200)||(letters=="E"&&numbers<99)){
                return "17A";
            }
            else if ((letters=="E"&&numbers>=99)||(letters=="GT"&&numbers<528)){
                return "17B";
            }
            else if ((letters=="AC"&&numbers>=1)||(letters=="BL"&&numbers<1175)){
                return "18B";
            }
            else{
                return letters.ToString();
            }
        }

        private async Task<(string Letters, double Numbers)> SeparateLettersAndNumbers(string input)
        {
        
        if (input.Length < 2)
        {
            return (string.Empty, 0);
        }

        if(input[0].ToString()=="Q"||input[0].ToString()=="R"||input[0].ToString()=="M"||(input[0].ToString()=="P"&&char.IsDigit(input[1]))||(input[0].ToString()=="N"&&char.IsDigit(input[1]))||(input[0].ToString()=="E"&&char.IsDigit(input[1]))){
            string letters = input.Substring(0, 1);

       
            string numberPart = input.Substring(1);

       
             if (double.TryParse(numberPart, out double numbers))
            {
                return (letters, numbers);
            }
            else
            {
            
                return (letters, 0);
            }
        }
        else{
            string letters = input.Substring(0, 2);

            string numberPart = ExtractNumbers(input.Substring(2));
       
            if (double.TryParse(numberPart, out double numbers))
            {
                return (letters, numbers);
            }
            else
            {
            
                return (letters, 0);
            }
        }
        
        }
        static string ExtractNumbers(string input)
        {
        
            string numbersOnly = new string(input.Where(c => char.IsDigit(c) || c == '.').ToArray());

            return numbersOnly;
        }
    }
}
