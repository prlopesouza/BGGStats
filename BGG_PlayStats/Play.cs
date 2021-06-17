using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGG_PlayStats
{
    public class Play
    {
        public long id;
        public long userId;
        public bool incomplete;
        public bool noWinStats;
        public string location;
        public long gameId;

        public List<Player> players;

        public DateTime date;

        public int quantity;

        public int playerCount;

        public int length;


        public Play()
        {
            players = new List<Player>();
            date = new DateTime();
            quantity = 1;
            playerCount = 0;
        }

        public bool AddPlayer(string color, int win, float score = 0)
        {
            Player p = new Player();
            p.color = color;
            p.win = win;
            p.score = score;
            this.players.Add(p);

            return true;
        }
    }
}
