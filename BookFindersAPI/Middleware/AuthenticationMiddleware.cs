using BookFindersLibrary.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BookFindersAPI.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _nextRequest;
        private readonly string _staticBearerToken = Environment.GetEnvironmentVariable("bookfindersAPIBearerToken"); 

        public AuthenticationMiddleware(RequestDelegate nextRequest)
        {
            _nextRequest = nextRequest;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var authHeader = httpContext.Request.Headers["X-Authorization"].FirstOrDefault();

            if (httpContext.Request.Path.StartsWithSegments("/api/helloWorld") || httpContext.Request.Path.StartsWithSegments("/swagger") || httpContext.Request.Path.StartsWithSegments("/favicon.ico"))
            {
                await _nextRequest(httpContext);

                PrintLog(httpContext, authHeader, "Authorization Not Needed");

                return;
            }

            if (authHeader != null && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();

                if (_staticBearerToken != token)
                {
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

                    ResponseDTO responseDTOUnauthorized = new ResponseDTO()
                    {
                        Status = 401,
                        Message = $"Unauthorized: Bad token - '{token}'",
                    };

                    JObject responseAsJson = JObject.FromObject(responseDTOUnauthorized);

                    using (var writer = new StreamWriter(httpContext.Response.Body))
                    await using (var jsonWriter = new JsonTextWriter(writer))
                    {
                        await responseAsJson.WriteToAsync(jsonWriter);
                    }

                    PrintLog(httpContext, authHeader, "Unauthorized");

                    return;
                }
            }
            else
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

                ResponseDTO responseDTOUnauthorized = new ResponseDTO()
                {
                    Status = 401,
                    Message = "Unauthorized: No token provided",
                };

                JObject responseAsJson = JObject.FromObject(responseDTOUnauthorized);

                using (var writer = new StreamWriter(httpContext.Response.Body))
                await using (var jsonWriter = new JsonTextWriter(writer))
                {
                    await responseAsJson.WriteToAsync(jsonWriter);
                }

                PrintLog(httpContext, authHeader, "Unauthorized");

                return;
            }

            PrintLog(httpContext, authHeader, "Authorized");

            await _nextRequest(httpContext);
            return;
        }

        private void PrintLog(HttpContext httpContext, string authorizationHeader, string authorizationStatus)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("info: ");
            Console.ResetColor();
            Console.WriteLine($"{DateTime.Now} ----- '{httpContext.Connection.RemoteIpAddress?.ToString()}' -- '{httpContext.Request.Path}' -- '{authorizationHeader}' ({authorizationStatus})");
        }
    }
}
