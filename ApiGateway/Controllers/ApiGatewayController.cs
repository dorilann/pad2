using ApiGateway.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace ApiGateway.Controllers
{
    [ApiController]
	[Route("")]
	public class ApiGatewayController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly ILogger<ApiGatewayController> _logger;

        public ApiGatewayController(ILogger<ApiGatewayController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [Authorize]
        [HttpGet("account/{region}/{server}/{gameName}/{tag}")]
        public async Task<IActionResult> GetProfile(string region, string server, string gameName, string tag)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString();

                var client = _httpClientFactory.CreateClient();

                client.DefaultRequestHeaders.Add("Authorization", token);

                var data = await client.GetStringAsync($"http://storageservice:8080/Champion/GetProfileData/{gameName}/{tag}");

                if (string.IsNullOrEmpty(data))
                {

                    data = await client.GetStringAsync($"http://riotservice:8080/Account/{region}/{server}/{gameName}/{tag}");

                    var content = new StringContent(data, Encoding.UTF8, "application/json");
                    await client.PostAsync("http://storageservice:8080/Champion/InsertProfileData", content);
                }

                // Log the retrieved profile data
                _logger.LogInformation("Profile data: {Data}", data);

                var profile = JsonConvert.DeserializeObject<Profile>(data);
                return Ok(profile);
            }
            catch (Exception ex)
            {
                // Log an error if something goes wrong during the request
                _logger.LogError(ex, "Error processing request");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var client = _httpClientFactory.CreateClient();

            var content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("http://authorizationservice:8080/api/Auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                return Ok(new { token });
            }

            return BadRequest("Invalid login credentials.");


        }

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterModel register)
		{
			var client = _httpClientFactory.CreateClient();
			var content = new StringContent(JsonConvert.SerializeObject(register), Encoding.UTF8, "application/json");

			try
			{
				var response = await client.PostAsync("http://authorizationservice:8080/api/Auth/register", content);

				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();
					return Ok(new { message = "Registration successful", data = result });
				}

				var errorContent = await response.Content.ReadAsStringAsync();
				return BadRequest(new { message = "Error from AuthorizationService", details = errorContent });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "Internal server error", error = ex.Message });
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
}
