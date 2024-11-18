namespace StorageService.Models.Account
{
    public class AccountModel
    {
        public string Puuid { get; set; }
        public string GameName { get; set; }
        public string TagLine { get; set; }
        public string? AccountId { get; set; }
        public int ProfileIconId { get; set; }
        public long RevisionDate { get; set; }
        public string SummonerId { get; set; }
        public long SummonerLevel { get; set; }
        public string Tier { get; set; }
        public string Rank { get; set; }
        public string LeaguePoints { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
    }
}
