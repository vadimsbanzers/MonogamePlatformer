using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class LeaderboardManager // Game Engine class that modifies,saves and loads leadboard information from .json files
{
    private static string leaderboardFilePath = "leaderboard.json";
    private List<LeaderboardEntry> entries;

    public LeaderboardManager()
    {
        LoadLeaderboard();
    }

    private void LoadLeaderboard()  // loading leaderboard,
    {
        if (File.Exists(leaderboardFilePath)) // if a json file already exists, it saves entries in the existing file
        {
            string json = File.ReadAllText(leaderboardFilePath);
            entries = JsonConvert.DeserializeObject<List<LeaderboardEntry>>(json);
        }
        else // creates new json file if it doesn't already exist
        {
            entries = new List<LeaderboardEntry>();
        }
    }

    public void SaveLeaderboard() // Saves entries list into the Json file
    {
        string json = JsonConvert.SerializeObject(entries);
        File.WriteAllText(leaderboardFilePath, json);
    }

    public void AddEntry(LeaderboardEntry entry) // adds an entry to the entry list
    {
        entries.Add(entry);
        SaveLeaderboard();
    }

    public List<LeaderboardEntry> GetEntries() // returns entries list
    {
        return entries;
    }
}