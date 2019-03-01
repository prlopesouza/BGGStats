using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGG_PlayStats
{
    public class Play
    {
        public struct Player
        {
            public string color;
            public int win;
            public float score;
        }

        public List<Player> players;

        public DateTime date;

        public int quantity;

        public int playerCount;

        public Play()
        {
            players = new List<Player>();
            date = new DateTime();
            quantity = 1;
            playerCount = 0;
        }

        public bool AddPlayer(string color, int win, float score)
        {
            this.players.Add(new Player { color = color, win = win, score = score });
            playerCount++;
            return true;
        }

        public bool AddPlayer(string color, int win)
        {
            this.players.Add(new Player { color = color, win = win});
            playerCount++;
            return true;
        }
    }
}
