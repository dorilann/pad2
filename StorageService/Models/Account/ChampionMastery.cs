namespace StorageService.Models.Account
{
    public class ChampionMastery
    {
        public string PuuId { get; set; }
        public long ChampionPointsUntilNextLevel { get; set; }
        public bool ChestGranted { get; set; }
        public long ChampionId { get; set; }
        public long LastPlayTime { get; set; }
        public int ChampionLevel { get; set; }
        public int ChampionPoints { get; set; }
    }
}
