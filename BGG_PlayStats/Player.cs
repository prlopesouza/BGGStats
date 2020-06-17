using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGG_PlayStats
{
    public class Player
    {
        public long userId;
        public string userName;
        public int startPosition;
        public string color;
        public int win;
        public float score;
        public bool isNew;
        public int rating;
        public long playId;

        public Player ()
        {
            userId = 0;
            userName = "";
            startPosition = 0;
            color = "";
            score = 0;
            isNew = false;
            rating = 0;
            playId = 0;
        }
    }
}
