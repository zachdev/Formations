using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace Formations
{
    class PlayerContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<ScoreCard> ScoreCards { get; set; }
    }
}
