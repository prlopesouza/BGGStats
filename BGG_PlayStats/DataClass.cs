using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Globalization;

namespace BGG_PlayStats
{
    public class DataClass
    {
        private SQLiteConnection sqlite_conn;

        public bool CreateConnection(string fileName)
        {
            sqlite_conn = new SQLiteConnection($"Data Source={fileName}; Version = 3; New = True; Compress = True;");
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool InitializeDB()
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();

            string sql = "CREATE TABLE IF NOT EXISTS Boardgames (primaryname TEXT NOT NULL, thumbnail BLOB, id INTEGER PRIMARY KEY, description TEXT, yearpublished INTEGER NOT NULL, minplayers INTEGER NOT NULL, maxplayers INTEGER NOT NULL, playingtime INTEGER, minplaytime INTEGER, maxplaytime INTEGER, minage INTEGER)";
            sqlite_cmd.CommandText = sql;
            sqlite_cmd.ExecuteNonQuery();

            sql = "CREATE TABLE IF NOT EXISTS Designers (id INTEGER PRIMARY KEY, name TEXT NOT NULL)";
            sqlite_cmd.CommandText = sql;
            sqlite_cmd.ExecuteNonQuery();

            sql = "CREATE TABLE IF NOT EXISTS \"Boardgames - Designers\" (boardgameId INTEGER NOT NULL, designerId INTEGER NOT NULL)";
            sqlite_cmd.CommandText = sql;
            sqlite_cmd.ExecuteNonQuery();

            sql = "CREATE TABLE IF NOT EXISTS Players (userid INTEGER, name TEXT, startposition TEXT, color TEXT, score NUMERIC, new INTEGER, rating INTEGER, win INTEGER, playId INTEGER NOT NULL)";
            sqlite_cmd.CommandText = sql;
            sqlite_cmd.ExecuteNonQuery();

            sql = "CREATE TABLE IF NOT EXISTS Plays (id INTEGER PRIMARY KEY, userid INTEGER NOT NULL, date TEXT, quantity INTEGER, length INTEGER, incomplete INTEGER, nowinstats INTEGER, location TEXT, boardgameId INTEGER NOT NULL, playerCount INTEGER)";
            sqlite_cmd.CommandText = sql;
            sqlite_cmd.ExecuteNonQuery();

            sql = "CREATE TABLE IF NOT EXISTS Colors (gameId INTEGER NOT NULL, registeredColor TEXT NOT NULL, aggregatedColor TEXT, excluded INTEGER NOT NULL)";
            sqlite_cmd.CommandText = sql;
            sqlite_cmd.ExecuteNonQuery();

            return true;
        }

        public bool insertPlay(string id, string userId, string date, string quantity, string length, string incomplete, string nowinstats, string location, string boardgameId, int playerCount)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();

            try
            {
                string sql = "INSERT INTO Plays (id, userid, date, quantity, length, incomplete, nowinstats, location, boardgameId, playerCount) " +
                    $"VALUES({id}, {userId}, '{date}', {quantity}, {length}, {incomplete}, {nowinstats}, '{location.Replace("'", "")}', {boardgameId}, {playerCount})";
                sqlite_cmd.CommandText = sql;
                sqlite_cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public bool insertPlayer(string userId, string name, string startPosition, string color, string score, string isNew, string rating, string win, string playId)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();

            try
            {
                string sql = "INSERT INTO Players (userid, name, startposition, color, score, new, rating, win, playId) " +
                    $"VALUES({userId}, '{name.Replace("'", "")}', '{startPosition.Replace("'", "")}', '{color.Replace("'", "")}', {score}, {isNew}, {rating}, {win}, {playId})";
                sqlite_cmd.CommandText = sql;
                sqlite_cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public List<Play> selectPlays(string gameId)
        {
            List<Play> plays = new List<Play>();

            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();

            try
            {
                string sql = "SELECT * FROM Plays as P " +
                    $"WHERE boardgameId = {gameId} AND playerCount != 0 AND " +
                    "incomplete == 0 AND nowinstats == 0 AND " +
                    "(SELECT count(color) FROM Players as PL WHERE PL.playId = P.id AND PL.color != \"\") == playerCount";
                sqlite_cmd.CommandText = sql;
                SQLiteDataReader dataReader = sqlite_cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    Play play = new Play();
                    play.id = dataReader.GetInt64(0);
                    play.userId = dataReader.GetInt64(1);
                    play.date = DateTime.ParseExact(dataReader.GetString(2), "yyyy-MM-dd", null);
                    play.quantity = dataReader.GetInt32(3);
                    play.incomplete = (dataReader.GetInt32(5) == 1);
                    play.noWinStats = (dataReader.GetInt32(6) == 1);
                    play.location = dataReader.GetString(7);
                    play.gameId = dataReader.GetInt64(8);
                    play.playerCount = dataReader.GetInt32(9);

                    play.players = selectPlayers(play.id);

                    plays.Add(play);
                }
            }
            catch (Exception ex)
            {
                return plays;
            }

            return plays;
        }

        public List<Player> selectPlayers(long playId)
        {
            List<Player> players = new List<Player>();

            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();

            try
            {
                string sql = $"SELECT * FROM Players WHERE playId = {playId} AND color != \"\"";
                sqlite_cmd.CommandText = sql;
                SQLiteDataReader dataReader = sqlite_cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    Player player = new Player();
                    player.color = dataReader.GetString(3);
                    player.win = dataReader.GetInt32(7);
                    player.score = dataReader.GetFloat(4);

                    players.Add(player);
                }
            }
            catch (Exception ex)
            {
                return players;
            }

            return players;
        }
    }
}
