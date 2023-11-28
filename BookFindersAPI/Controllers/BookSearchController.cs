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
            var response = await client.GetAsync("https://api-ca.hosted.exlibrisgroup.com/primo/v1/search?vid=01OCLS_SHER%3ASHER&tab=Everything&scope=MyInst_and_CI&q=q%3Dany%2Ccontains%2C"+searchText+"&lang=eng&offset="+offset+"&limit=10&sort=rank&pcAvailability=true&getMore=0&conVoc=true&inst=01OCLS_SHER&skipDelivery=true&disableSplitFacets=true&apikey=l8xxbd240191e506439380215edab4ec4d85");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var bookObjLists = JsonConvert.DeserializeObject<BookResponse>(content);

                List<book> books = new List<book>();

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
                        LibraryCode = "None"
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
            var response = await client.GetAsync("https://api-ca.hosted.exlibrisgroup.com/primo/v1/search?vid=01OCLS_SHER%3ASHER&tab=Library_Physical&scope=MyInstitution_Physical&q=any%2Ccontains%2C"+searchText+"&multiFacets=multiFacets%3Dfacet_tlevel%2Cinclude%2Cavailable_p&lang=eng&offset="+offset+"&limit=10&sort=rank&pcAvailability=true&getMore=0&conVoc=true&inst=01OCLS_SHER&skipDelivery=false&disableSplitFacets=true&apikey=l8xxbd240191e506439380215edab4ec4d85");
            
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

                    var bookObj = new book
                    {
                        Id = "1",
                        Name = sort.title?.Count > 0 ? sort.title[0] : "Unknown Title",
                        Author = sort.author?.Count > 0 ? sort.author[0] : "Unknown Author",
                        Description = display.description?.Count > 0 ? display.description[0] : "Unknown Description",
                        ImageLink = "defaultBook.png",
                        LocationCode = bestlocation?.callNumber ?? "Unknown Location Code",
                        LibraryCode = bestlocation?.libraryCode ?? "Unknown Library Code"
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
    }
}
