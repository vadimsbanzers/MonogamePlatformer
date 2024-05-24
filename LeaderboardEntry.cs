public class LeaderboardEntry
{
    public string PlayerName { get; set; }
    public int Score { get; set; }

    public LeaderboardEntry(string playerName, int score) //data structure for a single entry
    {
        PlayerName = playerName;
        Score = score;
    }
}

