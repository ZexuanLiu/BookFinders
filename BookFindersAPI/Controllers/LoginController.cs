using BookFindersAPI.Interfaces;
using BookFindersAPI.Services;
using BookFindersLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace BookFindersAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private IDatabase _loginDatabase;

        private string _passwordSalt;

        public LoginController(ProductionDatabase productionDatabase, TestDatabase testDatabase)
        {
            bool isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            if (isDev)
            {
                _loginDatabase = testDatabase;
            }
            else
            {
                _loginDatabase = productionDatabase;
            }

            if (_loginDatabase == null)
            {
                throw new Exception("User Locations database was not initialized!");
            }

            _passwordSalt = Environment.GetEnvironmentVariable("bookFindersAPISalt");
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(User user)
        {
            #region Checking for null properties

            if (user.UserLogin == null)
            {
                ResponseDTO responseDTOError = new ResponseDTO
                {
                    Status = 400,
                    Message = "UserLogin object not specified",
                };

                return BadRequest(responseDTOError);
            }
            else if (string.IsNullOrEmpty(user.UserLogin.Username))
            {
                ResponseDTO responseDTOError = new ResponseDTO
                {
                    Status = 400,
                    Message = "User's username cannot be null or empty",
                };

                return BadRequest(responseDTOError);
            }
            else if (string.IsNullOrEmpty(user.UserLogin.Password))
            {
                ResponseDTO responseDTOError = new ResponseDTO
                {
                    Status = 400,
                    Message = "User's password cannot be null or empty",
                };

                return BadRequest(responseDTOError);
            }
            else if(string.IsNullOrEmpty(user.Role))
            {
                ResponseDTO responseDTOError = new ResponseDTO
                {
                    Status = 400,
                    Message = "User's role cannot be null or empty",
                };

                return BadRequest(responseDTOError);
            }
            else if(string.IsNullOrEmpty(user.Fullname))
            {
                ResponseDTO responseDTOError = new ResponseDTO
                {
                    Status = 400,
                    Message = "User's full name cannot be null or empty",
                };

                return BadRequest(responseDTOError);
            }

            #endregion

            IEnumerable<string> currentUsernames = await _loginDatabase.GetUsernames();
            if (currentUsernames.Contains(user.UserLogin.Username, StringComparer.OrdinalIgnoreCase))
            {
                ResponseDTO responseDTOError = new ResponseDTO
                {
                    Status = 400,
                    Message = "User with this username already exists",
                };

                return BadRequest(responseDTOError);
            }

            user.UserLogin.Password = SaltAndHashPassword(user.UserLogin.Password);

            User createdUser = await _loginDatabase.SignUpUser(user);

            User createdUserNoPassword = GetUserNoPassword(createdUser);
            ResponseDTO responseDTOOk = new ResponseDTO()
            {
                Status = 200,
                Message = "Successfully signed up a new user",
                Data = createdUserNoPassword
            };
            return Ok(responseDTOOk);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            if (string.IsNullOrEmpty(userLogin.Username))
            {
                ResponseDTO responseDTOError = new ResponseDTO
                {
                    Status = 400,
                    Message = "Username cannot be null or empty",
                };

                return BadRequest(responseDTOError);
            }
            else if (string.IsNullOrEmpty(userLogin.Password))
            {
                ResponseDTO responseDTOError = new ResponseDTO
                {
                    Status = 400,
                    Message = "User's password cannot be null or empty",
                };

                return BadRequest(responseDTOError);
            }

            userLogin.Password = SaltAndHashPassword(userLogin.Password);

            User? loggedInUser = await _loginDatabase.GetUserFromUserLogin(userLogin);

            if (loggedInUser == null)
            {
                ResponseDTO responseDTOError = new ResponseDTO
                {
                    Status = 400,
                    Message = "Username or password incorrect",
                };

                return BadRequest(responseDTOError);
            }

            User loggedInUserNoPassword = GetUserNoPassword(loggedInUser);
            ResponseDTO responseDTOOk = new ResponseDTO()
            {
                Status = 200,
                Message = "Successfully logged in",
                Data = loggedInUserNoPassword
            };
            return Ok(responseDTOOk);

        }

        private string SaltAndHashPassword(string password)
        {
            password += _passwordSalt;
            password = GetSHA256Hash(password);

            return password;
        }

        private User GetUserNoPassword(User user)
        {
            User userNoPassword = new User()
            {
                Id = user.Id,
                Role = user.Role,
                Fullname = user.Fullname,
                UserLogin = new UserLogin()
                {
                    Id = user.UserLogin.Id,
                    Username = user.UserLogin.Username
                }
            };
            return userNoPassword;
        }

        private static string GetSHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convert input string to byte array and compute hash
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Convert the byte array to a hexadecimal string
                StringBuilder hashBuilder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    hashBuilder.Append(b.ToString("x2"));  // Format as hexadecimal
                }

                return hashBuilder.ToString();
            }
        }
    }
}
