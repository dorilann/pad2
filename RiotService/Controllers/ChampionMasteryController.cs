using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RiotService.Models.Account;
using RiotService.Models.ChampionMastery;

namespace RiotService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ChampionMasteryController : ControllerBase
    {

        private readonly ILogger<ChampionMasteryController> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public ChampionMasteryController(ILogger<ChampionMasteryController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
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
        [HttpGet("{region}/{puuid}", Name = "GetChampionMastery")]
        public async Task<IActionResult> GetChampionMastery(string region, string puuid)
        {
            var response = await _httpClient.GetAsync($"https://{region}.api.riotgames.com/lol/champion-mastery/v4/champion-masteries/by-puuid/{puuid}");
            var body = await response.Content.ReadAsStringAsync();
            var masteries = JsonConvert.DeserializeObject<List<ChampionMastery>>(body);
            return Ok(masteries);
        }

        /// <summary>
        /// Получите топ записей о мастерстве чемпионов, отсортированные по убыванию количества очков чемпиона.
        /// </summary>
        /// <param name="region">Регион Сервера (например, "euw1", "ru")</param>
        /// <param name="puuid">Идентификатор PUUID игрока.</param>
        /// <param name="top">Количество записей.</param>
        /// <returns>Получите топ записей о мастерстве чемпионов, отсортированные по убыванию количества очков чемпиона.</returns>
        [HttpGet("{region}/{puuid}/{top}", Name = "GetChampionMasteryTop")]
        public async Task<IActionResult> GetChampionMasteryTop(string region, string puuid, int top)
        {
            var response = await _httpClient.GetAsync($"https://{region}.api.riotgames.com/lol/champion-mastery/v4/champion-masteries/by-puuid/{puuid}/top?count={top}");
            var body = await response.Content.ReadAsStringAsync();
            var masteries = JsonConvert.DeserializeObject<List<ChampionMastery>>(body);
            return Ok(masteries);
        }


    }
}
