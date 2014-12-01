using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace Formations
{
    class ScoreCard
    {
        private int scorecardId { get; set; }
        private int playerId { get; set; }
        private int gamesWon { get; set; }
        private int gamesLost { get; set; }
        private int gamesAbandoned { get; set; }
    }
}
