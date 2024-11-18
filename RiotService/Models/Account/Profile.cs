namespace RiotService.Models.Account
{
    using RiotService.Models.ChampionMastery;

    public class Profile
    {
        public AccountModel Account { get; set; }
        public List<ChampionMastery> Masteries { get; set; }
    }
}
