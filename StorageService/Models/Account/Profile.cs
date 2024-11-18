using MongoDB.Bson.Serialization.Attributes;

namespace StorageService.Models.Account
{
    public class Profile
    {
        [BsonId]
        public string? Id { get; set; }
        public AccountModel Account { get; set; }
        public List<ChampionMastery> Masteries { get; set; }
    }
}
