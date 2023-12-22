namespace FootballResultsApi.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public List<League> Leagues { get; set; } = new List<League>();
        public List<Team> Teams { get; set; } = new List<Team>();
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
