using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RiotService.Models.Account;
using RiotService.Models.ChampionMastery;
using RiotService.Models.NewFolder;

namespace RiotService.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class SummonerController : ControllerBase
    {

        private readonly ILogger<SummonerController> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public SummonerController(ILogger<SummonerController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;

            var token = Environment.GetEnvironmentVariable("X-RIOT-TOKEN")
                           ?? _configuration["X-Riot-Token"];

            _httpClient.DefaultRequestHeaders.Add("X-Riot-Token", token);
        }


        /// <summary>
        /// Получите все записи о мастерстве чемпионов, отсортированные по убыванию количества очков чемпиона.
        /// </summary>
        /// <param name="region">Регион Сервера (например, "euw1", "ru")</param>
        /// <param name="puuid">Идентификатор PUUID игрока.</param>
        /// <returns>Получите все записи о мастерстве чемпионов, отсортированные по убыванию количества очков чемпиона.</returns>
        [HttpGet("{region}/{puuid}", Name = "GetSummonerData")]
        public async Task<IActionResult> GetSummonerData(string region, string puuid)
        {
            var response = await _httpClient.GetAsync($"https://{region}.api.riotgames.com/lol/summoner/v4/summoners/by-puuid/{puuid}");
            var body = await response.Content.ReadAsStringAsync();
            var masteries = JsonConvert.DeserializeObject<SummonerModel>(body);
            return Ok(masteries);
        }



    }
}
