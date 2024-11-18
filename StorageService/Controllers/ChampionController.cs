using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using StorageService.Models.Account;
using StorageService.Models.Champions;

namespace StorageService.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ChampionController : ControllerBase
    {
        private readonly IMongoCollection<Champion> _championCollection;
        private readonly IMongoCollection<Profile> _profileCollection;

        public ChampionController(IMongoDatabase database)
        {
            _championCollection = database.GetCollection<Champion>("Champions");
            _profileCollection = database.GetCollection<Profile>("Profiles");
        }

        [HttpPost("insert")]
        public async Task<IActionResult> InsertData()
        {
            try
            {
                string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "champion.json");
                var jsonData = System.IO.File.ReadAllText(jsonFilePath);

                JObject champions = JObject.Parse(jsonData);
                var championsList = new List<Champion>();

                if (champions["data"] is null)
                    return BadRequest($"Wrong file location: {jsonFilePath}");

                foreach (var champion in champions["data"])
                {
                    var championModel = champion.First.ToObject<Champion>();
                    championsList.Add(championModel);
                }
                await _championCollection.InsertManyAsync(championsList);

                return Ok("Data inserted into MongoDB.");
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetChampionByKey/{key}")]
        public async Task<IActionResult> GetChampionByKey(long key)
        {
            var champ = await _championCollection.Find(x => x.Key == key).FirstOrDefaultAsync();
            return Ok(champ);
        }

        [HttpPost("InsertProfileData")]
        public async Task<IActionResult> InsertProfileData([FromBody] Profile profile)
        {
            profile.Id = profile.Account.Puuid;
            await _profileCollection.InsertOneAsync(profile);
            return Ok(profile);
        }

        [HttpGet("GetProfileData/{gameName}/{tag}")]
        public async Task<IActionResult> GetProfileData(string gameName, string tag)
        {
            var person = await _profileCollection.Find(x => x.Account.GameName == gameName && x.Account.TagLine == tag).FirstOrDefaultAsync();
            return Ok(person);
        }

        [HttpGet("GetProfilesLike/{gameName}")]
        public async Task<IActionResult> GetProfilesLike(string gameName)
        {
            var profiles = await _profileCollection.Find(x => x.Account.GameName.Contains(gameName)).ToListAsync();
            return Ok(profiles);
        }


    }

}
