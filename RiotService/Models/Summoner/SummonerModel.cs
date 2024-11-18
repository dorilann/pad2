namespace RiotService.Models.NewFolder
{
    public class SummonerModel
    {
        public string AccountId { get; set; }
        public int ProfileIconId { get; set; }
        public long RevisionDate { get; set; }
        //Encrypted summoner ID
        public string Id { get; set; }
        public string PuuId { get; set; }
        public long SummonerLevel { get; set; }
    }
}
