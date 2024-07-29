using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AvaloniaSpaceInvaders.Models
{
    public class HighScoreModel
    {
        public List<HighScoreEntry> HighScores { get; set; } = new List<HighScoreEntry>();
    }

    public class HighScoreEntry
    {
        public int HighScore { get; set; }
        public string Name { get; set; }
    }
}
