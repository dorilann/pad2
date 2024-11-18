using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RiotService.Models.Account;
using RiotService.Models.ChampionMastery;
using RiotService.Models.NewFolder;
using RiotService.Models.Summoner;
using System;

namespace RiotService.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {

        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public AccountController(ILogger<AccountController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;

            var token = Environment.GetEnvironmentVariable("X-RIOT-TOKEN")
                           ?? _configuration["X-Riot-Token"];

            _httpClient.DefaultRequestHeaders.Add("X-Riot-Token", token);
        }


        /// <summary>
        /// Получает информацию об аккаунте по PUUID.
        /// </summary>
        /// <param name="region">Регион Riot Games API (например, "europe", "americas")</param>
        /// <param name="puuid">Идентификатор PUUID игрока.</param>
        /// <returns>Информация об аккаунте в JSON формате.</returns>
        [HttpGet("{region}/{puuid}",Name = "GetAccountByPuuId")]
        public async Task<IActionResult> GetByPuuId(string region, string puuid)
        {
            var response = await _httpClient.GetAsync($"https://{region}.api.riotgames.com/riot/account/v1/accounts/by-puuid/{puuid}");
            var body = await response.Content.ReadAsStringAsync();
            var person = JsonConvert.DeserializeObject<AccountModel>(body);
            return Ok(person);
        }

        /// <summary>
        /// Получает информацию о Riot ID.
        /// </summary>
        /// <param name="region">Регион Riot Games API (например, "europe", "americas")</param>
        /// <param name="gameName">Имя игрока в игре</param>
        /// <param name="tagLine">Тег игрока</param>
        /// <returns>Информация об аккаунте в JSON формате</returns>
        [HttpGet("{region}/{gameName}/{tagLine}",Name = "GetByRiotId")]
        public async Task<IActionResult> GetByRiotId(string region,string gameName,string tagLine)
        {
            var response = await _httpClient.GetAsync($"https://{region}.api.riotgames.com/riot/account/v1/accounts/by-riot-id/{gameName}/{tagLine}");
            var body = await response.Content.ReadAsStringAsync();
            var person = JsonConvert.DeserializeObject<AccountModel>(body);
            return Ok(person);
        }


        /// <summary>
        /// Получает информацию об Аккаунта.
        /// </summary>
        /// <param name="region">Регион Riot Games API (например, "europe", "americas")</param>
        /// <param name="server">Сервер Riot Games API (например, "euw1", "ru")</param>
        /// <param name="gameName">Имя игрока в игре</param>
        /// <param name="tagLine">Тег игрока</param>
        /// <returns>Информация об аккаунте в JSON формате</returns>
        [HttpGet("{region}/{server}/{gameName}/{tagLine}", Name = "GetFullProfile")]
        public async Task<Profile> GetFullProfile (string region, string server, string gameName, string tagLine)
        {
            var account = await GetAccount(region, gameName, tagLine);
            var masteries = await GetMastery(server, account.Puuid, 5);
            var profile = new Profile();
            profile.Account = account;
            profile.Masteries = masteries;
            profile = await GetSummonerData(profile, server, profile.Account.Puuid);
            profile = await GetRankedStatistics(profile, server);
            return profile;
        }


        private async Task<AccountModel?> GetAccount(string region, string gameName, string tagLine)
        {
            var response = await _httpClient.GetAsync($"https://{region}.api.riotgames.com/riot/account/v1/accounts/by-riot-id/{gameName}/{tagLine}");
            var body = await response.Content.ReadAsStringAsync();
            var person = JsonConvert.DeserializeObject<AccountModel>(body);
            return person;
        }

        private async Task<List<ChampionMastery>?> GetMastery(string server, string puuid, int top)
        {
            var response = await _httpClient.GetAsync($"https://{server}.api.riotgames.com/lol/champion-mastery/v4/champion-masteries/by-puuid/{puuid}/top?count={top}");
            var body = await response.Content.ReadAsStringAsync();
            var masteries = JsonConvert.DeserializeObject<List<ChampionMastery>>(body);
            return masteries;
        }

        private async Task<Profile> GetSummonerData(Profile profile,string server, string puuid)
        {
            var response = await _httpClient.GetAsync($"https://{server}.api.riotgames.com/lol/summoner/v4/summoners/by-puuid/{puuid}");
            var body = await response.Content.ReadAsStringAsync();
            var summonerModel = JsonConvert.DeserializeObject<SummonerModel>(body);
            if(summonerModel != null)
            {
                profile.Account.SummonerId = summonerModel.Id;
                profile.Account.AccountId = summonerModel.AccountId;
                profile.Account.SummonerLevel = summonerModel.SummonerLevel;
                profile.Account.ProfileIconId = summonerModel.ProfileIconId;
            }
            return profile;
        }

        private async Task<Profile> GetRankedStatistics(Profile profile, string server)
        {
            var response = await _httpClient.GetAsync($"https://{server}.api.riotgames.com/lol/league/v4/entries/by-summoner/{profile.Account.SummonerId}");
            var body = await response.Content.ReadAsStringAsync();
            var statistics = JsonConvert.DeserializeObject<List<Statistics>>(body);
            var ranked = statistics.Where(x => x.QueueType == "RANKED_SOLO_5x5").FirstOrDefault();
            if(ranked != null)
            {
                profile.Account.Tier = ranked.Tier;
                profile.Account.Rank = ranked.Rank;
                profile.Account.LeaguePoints = ranked.LeaguePoints;
                profile.Account.Wins = ranked.Wins;
                profile.Account.Losses = ranked.Losses;
            }

            return profile;
        }
    }
}
