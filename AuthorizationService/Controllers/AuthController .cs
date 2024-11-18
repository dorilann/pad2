using AuthorizationService.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace AuthorizationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenService _jwtTokenService;
        private readonly IMongoCollection<User> _userCollection;

        public AuthController(JwtTokenService jwtTokenService, IMongoDatabase database)
        {
            _jwtTokenService = jwtTokenService;
            _userCollection = database.GetCollection<User>("Users");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var user = await _userCollection.Find(x => x.Username == login.Username && x.Password == login.Password).FirstOrDefaultAsync();
            if(user != null)
            {
                try
                {
                    var token = _jwtTokenService.GenerateToken(user);
                    return Ok(token);
                }
                catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }        
            }
            return BadRequest("Wrong credentials.");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (model.Password != model.ConfPassword)
            {
                return BadRequest("Passwords do not match.");
            }

            var existingUser = await _userCollection.Find(u => u.Username == model.Username).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                return BadRequest("Username already exists.");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = model.Username,
                Password = model.Password,
                Role = "User" 
            };

            await _userCollection.InsertOneAsync(user);

            return Ok("User registered successfully.");
        }

    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RegisterModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfPassword { get; set; }
    }

}
