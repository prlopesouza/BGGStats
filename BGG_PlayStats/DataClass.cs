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
                (new FormError(ex.Message, ex.StackTrace)).ShowDialog();
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

            sql = "CREATE TABLE IF NOT EXISTS Players (userid INTEGER, name TEXT, startposition TEXT, color TEXT, score NUMERIC, new INTEGER, rating INTEGER, win INTEGER, playId INTEGER NOT NULL, UNIQUE(userid, name, startposition, color, score, new, rating, win, playId))";
            sqlite_cmd.CommandText = sql;
            sqlite_cmd.ExecuteNonQuery();

            sql = "CREATE TABLE IF NOT EXISTS Plays (id INTEGER PRIMARY KEY, userid INTEGER NOT NULL, date TEXT, quantity INTEGER, length INTEGER, incomplete INTEGER, nowinstats INTEGER, location TEXT, boardgameId INTEGER NOT NULL, playerCount INTEGER)";
            sqlite_cmd.CommandText = sql;
            sqlite_cmd.ExecuteNonQuery();

            sql = "CREATE TABLE IF NOT EXISTS Colors (gameId INTEGER NOT NULL, registeredColor TEXT NOT NULL, aggregatedColor TEXT, excluded INTEGER NOT NULL, PRIMARY KEY(\"gameId\",\"registeredColor\"))";
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
                string sql = "INSERT OR IGNORE INTO Plays (id, userid, date, quantity, length, incomplete, nowinstats, location, boardgameId, playerCount) " +
                    $"VALUES({id}, {userId}, '{date}', {quantity}, {length}, {incomplete}, {nowinstats}, '{location.Replace("'", "")}', {boardgameId}, {playerCount})";
                sqlite_cmd.CommandText = sql;
                sqlite_cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                (new FormError(ex.Message, ex.StackTrace)).ShowDialog();
                return false;
            }

            return true;
        }

        public bool insertPlayer(string userId, string name, string startPosition, string color, string score, string isNew, string rating, string win, string playId)
        {
            if (score == null || score == "NULL" || score == "null" || score == "") score = "0";

            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();

            int s = 0;
            int.TryParse(score, out s);

            try
            {
                string sql = "INSERT OR IGNORE INTO Players (userid, name, startposition, color, score, new, rating, win, playId) " +
                    $"VALUES({userId}, '{name.Replace("'", "")}', '{startPosition.Replace("'", "")}', '{color.Replace("'", "")}', {s}, {isNew}, {rating}, {win}, {playId})";
                sqlite_cmd.CommandText = sql;
                sqlite_cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                (new FormError(ex.Message, ex.StackTrace)).ShowDialog();
                return false;
            }

            return true;
        }

        public bool insertUpdateColor(string gameId, string registeredColor, string aggregatedColor, int excluded)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();

            try
            {
                string sql = $"SELECT * FROM Colors WHERE gameId=={gameId} AND registeredColor=='{registeredColor.Replace("'", "")}'";
                sqlite_cmd.CommandText = sql;
                SQLiteDataReader dataReader = sqlite_cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    dataReader.Close();
                    sql = $"UPDATE Colors SET aggregatedColor='{aggregatedColor.Replace("'", "")}', excluded={excluded} " +
                    $"WHERE gameId=={gameId} AND registeredColor=='{registeredColor.Replace("'", "")}'";
                    sqlite_cmd.CommandText = sql;
                    sqlite_cmd.ExecuteNonQuery();
                }
                else
                {
                    dataReader.Close();
                    sql = "INSERT INTO Colors (gameId, registeredColor, aggregatedColor, excluded) " +
                    $"VALUES({gameId}, '{registeredColor.Replace("'", "")}', '{aggregatedColor.Replace("'", "")}', {excluded})";
                    sqlite_cmd.CommandText = sql;
                    sqlite_cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                (new FormError(ex.Message, ex.StackTrace)).ShowDialog();
                return false;
            }

            return true;
        }

        public List<Play> selectPlays(string gameId)
        {
            List<Play> plays = new List<Play>();

            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();

            //VERSION 1 
            /*
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
            */
            //END VERSION 1

            //VERSION 2
            try
            {
                string sql = "SELECT * FROM Plays as P, Players as Pl " +
                    $"WHERE boardgameId = {gameId} AND P.id == Pl.playId AND " + 
                    "Pl.color != \"\" AND P.playerCount != 0 AND " +
                    "P.incomplete == 0 AND P.nowinstats == 0 " +
                    "ORDER BY P.id";
                sqlite_cmd.CommandText = sql;
                SQLiteDataReader dataReader = sqlite_cmd.ExecuteReader();

                long id = 0;
                long currentId = -999;

                if (dataReader.Read())
                {
                    id = dataReader.GetInt64(0);
                    currentId = id;
                }

                while (currentId != -999)
                {
                    id = currentId;
                    int winTotal = 0;

                    Play play = new Play();
                    play.id = id;
                    play.userId = dataReader.GetInt64(1);
                    try
                    {
                        play.date = DateTime.ParseExact(dataReader.GetString(2), "yyyy-MM-dd", null);
                    }
                    catch (Exception e)
                    {
                        play.date = new DateTime();
                    }
                    play.quantity = dataReader.GetInt32(3);
                    play.length = dataReader.GetInt32(4);
                    play.incomplete = (dataReader.GetInt32(5) == 1);
                    play.noWinStats = (dataReader.GetInt32(6) == 1);
                    play.location = dataReader.GetString(7);
                    play.gameId = dataReader.GetInt64(8);
                    play.playerCount = dataReader.GetInt32(9);

                    while (currentId==id)
                    {
                        Player player = new Player();
                        player.color = dataReader.GetString(13);
                        player.score = dataReader.GetFloat(14);
                        player.win = dataReader.GetInt32(17);

                        play.players.Add(player);
                        winTotal += player.win;

                        if (dataReader.Read())
                        {
                            currentId = dataReader.GetInt64(0);
                        }
                        else
                        {
                            currentId = -999;
                        } 
                    }

                    if (winTotal>0) plays.Add(play);
                }
            }
            catch (Exception ex)
            {
                (new FormError(ex.Message, ex.StackTrace)).ShowDialog();
                return plays;
            }
            //END VERSION 2

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
                (new FormError(ex.Message, ex.StackTrace)).ShowDialog();
                return players;
            }

            return players;
        }

        public Dictionary<String, String> selectAggregated(string gameId)
        {
            Dictionary<String, String> aggregated = new Dictionary<String, String>();

            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();

            try
            {
                string sql = $"SELECT * FROM Colors WHERE gameId = {gameId} AND excluded==0";
                sqlite_cmd.CommandText = sql;
                SQLiteDataReader dataReader = sqlite_cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    aggregated.Add(dataReader.GetString(1), dataReader.GetString(2));
                }
            }
            catch (Exception ex)
            {
                (new FormError(ex.Message, ex.StackTrace)).ShowDialog();
                return aggregated;
            }

            return aggregated;
        }

        public List<String> selectExcluded(string gameId)
        {
            List<String> excluded = new List<String>();

            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();

            try
            {
                string sql = $"SELECT * FROM Colors WHERE gameId = {gameId} AND excluded==1";
                sqlite_cmd.CommandText = sql;
                SQLiteDataReader dataReader = sqlite_cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    excluded.Add(dataReader.GetString(1));
                }
            }
            catch (Exception ex)
            {
                (new FormError(ex.Message, ex.StackTrace)).ShowDialog();
                return excluded;
            }

            return excluded;
        }
    }
}
