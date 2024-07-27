using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AvaloniaSpaceInvaders.Models
{
    internal class HighScoreModel
    {
        [JsonPropertyName("high_score")]
        public int HighScore { get; set; }
    }
}
