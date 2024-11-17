using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using BookFindersLibrary.Models;
using BookFindersLibrary.Models.OnCampus;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

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
                    BookFindersLibrary.Models.BestLocation bestlocation = doc.delivery.bestlocation;
                    BookFindersLibrary.Models.PnxAdData addata = doc.pnx.addata;
                    BookFindersLibrary.Models.Delivery delivery = doc.delivery;
                    BookFindersLibrary.Models.PnxDisplay display = doc.pnx.display;
                    var bookObj = new book
                    {
                        Id = "1",
                        Name = sort.title?.Count > 0 ? sort.title[0] : "Unknown Title",
                        Author = sort.author?.Count > 0 ? sort.author[0] : "Unknown Author",
                        Description = search.description?.Count > 0 ? search.description[0] : "Unknown Description",
                        ImageLink = await GetImageByISBN(addata.isbn?.Count > 0 ? addata.isbn[0] : "defaultBook.png"),
                        Publisher = display.publisher?.Count > 0 ? display.publisher[0] : "Unknown Publisher",
                        PublishYear = search.creationdate?.Count > 0 ? search.creationdate[0] : "Unknown Publish Year",
                        LocationCode = bestlocation?.callNumber ?? "Unknown Location Code",
                        LibraryCode = "None",
                        LocationBookShelfNum = "1",
                        LocationBookShelfSide = "A",
                        // LocationBookShelfRow = -1,
                        // LocationBookShelfColumn = -1,
                        OnlineResourceURL = delivery.almaOpenurl
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
                    BookFindersLibrary.Models.OnCampus.PnxDisplay display = doc.pnx.display;
                    BookFindersLibrary.Models.OnCampus.PnxAdData addata = doc.pnx.addata;
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
                        ImageLink = await GetImageByISBN(addata.isbn?.Count > 0 ? addata.isbn[0] : "defaultBook.png"),
                        Publisher = display.publisher?.Count > 0 ? display.publisher[0] : "Unknown Publisher",
                        PublishYear = display.creationdate?.Count > 0 ? display.creationdate[0] : "Unknown Publish Year",
                        LocationCode = bestlocation?.callNumber ?? "Unknown Location Code",
                        LibraryCode = bestlocation?.libraryCode ?? "Unknown Library Code",
                        LocationBookShelfNum = locationBookShelfNum,
                        LocationBookShelfSide = locationBookShelfSide,
                        // LocationBookShelfRow = 0,
                        // LocationBookShelfColumn = 0,
                        OnlineResourceURL = ""
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
                        ImageLink = await GetImageByISBN(addata.isbn?.Count > 0 ? addata.isbn[0] : "defaultBook.png"),
                        Publisher = display.publisher?.Count > 0 ? display.publisher[0] : "Unknown Publisher",
                        PublishYear = display.creationdate?.Count > 0 ? display.creationdate[0] : "Unknown Publish Year",
                        LocationCode = bestlocation?.callNumber ?? "Unknown Location Code",
                        LibraryCode = bestlocation?.libraryCode ?? "Unknown Library Code",
                        LocationBookShelfNum = "Unknown Location Code",
                        LocationBookShelfSide = "Unknown Location Code",
                        // LocationBookShelfRow = -1,
                        // LocationBookShelfColumn = -1
                        OnlineResourceURL = ""
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
        private async Task<string> GetImageByISBN (string isbn)
        {
            string baseUrl = $"https://proxy-ca.hosted.exlibrisgroup.com/exl_rewrite/syndetics.com/index.php?client=primo&isbn={isbn}/lc.jpg";
            return await CheckImageValidity(baseUrl);
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

            Match match = Regex.Match(parts[0], @"^([A-Za-z]+)(\d+(?:\.\d+)?)(?:\.[A-Za-z0-9]+)?$");
            if (!match.Success)
                return "Unknown Location Code";

            string bookPrefix = match.Groups[1].Value.ToUpper();
            double bookNumber = double.Parse(match.Groups[2].Value); 

            if (match.Groups[3].Success && double.TryParse(match.Groups[3].Value, out double decimalPart))
            {
                bookNumber += decimalPart / Math.Pow(10, match.Groups[3].Value.Length);
            }

            if(IsInBookShelfRange(bookPrefix,bookNumber,"U",20,"Z",2448)){
                return "6B";
            }
            else if (IsInBookShelfRange(bookPrefix,bookNumber,"TS",23,"TX",991)){
                return "7A";
            }
            else if (IsInBookShelfRange(bookPrefix,bookNumber,"TR",263,"TR",1010)){
                return "7B";
            }
            else if (IsInBookShelfRange(bookPrefix,bookNumber,"SB",453,"TR",263)){
                return "8A";
            }
            else if (IsInBookShelfRange(bookPrefix,bookNumber,"QH",46,"SB",451)){
                return "8B";
            }
            else if (IsInBookShelfRange(bookPrefix,bookNumber,"PS",8523,"QH",45)){
                return "9A";
            }
            else if (IsInBookShelfRange(bookPrefix,bookNumber,"PR",3325,"PS",8545)){
                return "9B";
            }
            else if (IsInBookShelfRange(bookPrefix,bookNumber,"PN",1998,"PN",2598)){
                return "10A";
            }
            else if (IsInBookShelfRange(bookPrefix,bookNumber,"PN",2599,"PR",3316)){
                return "10B";
            }
            else if (IsInBookShelfRange(bookPrefix,bookNumber,"NK",5440,"PL",8013)){
                return "11A";
            }
            else if (IsInBookShelfRange(bookPrefix,bookNumber,"PL",8014,"PN",1997)){
                return "11B";
            }
            else if (IsInBookShelfRange(bookPrefix,bookNumber,"ND",624,"NK",1397)){
                return "12A";
            }
            else if (IsInBookShelfRange(bookPrefix,bookNumber,"NK",1403,"NK",5343)){
                return "12B";
            }
            else if (IsInBookShelfRange(bookPrefix,bookNumber,"NC",976,"NC",1765)){
                return "13A";
            }
            else if (IsInBookShelfRange(bookPrefix,bookNumber,"NC",1766,"ND",623)){
                return "13B";
            }
            else if (IsInBookShelfRange(bookPrefix,bookNumber,"N",5301,"N",7405)){
                return "14A";
            }
            else if (IsInBookShelfRange(bookPrefix,bookNumber,"N",7407,"NC",975)){
                return "14B";
            }
            else if (IsInBookShelfRange(bookPrefix,bookNumber,"HQ",1122,"M",1630)){
                return "15A";
            }
            else if (IsInBookShelfRange(bookPrefix,bookNumber,"M",1631,"N",5300)){
                return "15B";
            }
            else if (IsInBookShelfRange(bookPrefix,bookNumber,"GT",540,"HE",7775)){
                return "16A";
            }
            else if (IsInBookShelfRange(bookPrefix,bookNumber,"HE",7815,"HQ",1075)){
                return "16B";
            }
            else if (IsInBookShelfRange(bookPrefix,bookNumber,"BL",1202,"E",99)){
                return "17A";
            }
            else if (IsInBookShelfRange(bookPrefix,bookNumber,"E",100,"GT",525)){
                return "17B";
            }
            else if (IsInBookShelfRange(bookPrefix,bookNumber,"AC",1,"BL",1112)){
                return "18B";
            }
            else{
                return "Unknown Location Code";
            }
        }

        static bool IsInBookShelfRange(string bookPrefix, double bookNumber, string startPrefix, double startNumber, string endPrefix, double endNumber)
        {
            string shelfStartPrefix = startPrefix;
            double shelfStartNumber = startNumber;
            string shelfEndPrefix = endPrefix;
            double shelfEndNumber = endNumber;

            double prefixStartComparison = String.Compare(bookPrefix, shelfStartPrefix);
            double prefixEndComparison = String.Compare(bookPrefix, shelfEndPrefix);

            if (prefixStartComparison > 0 && prefixEndComparison < 0)
            {
                return true;
            }
            else if (prefixStartComparison == 0)
            {
                return bookNumber >= shelfStartNumber;
            }
            else if (prefixEndComparison == 0)
            {
                return bookNumber <= shelfEndNumber;
            }

            return false;
        }
    }
}
