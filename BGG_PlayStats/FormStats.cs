using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace BGG_PlayStats
{
    public partial class FormStats : Form
    {
        string gameId = "";

        List<string> excluded = new List<string>();
        Dictionary<string, string> aggregated = new Dictionary<string, string>();
        List<Play> AllPlays = new List<Play>();
        long totalPlayTime = 0;
        long playTimeCount = 0;
        int minPlayCount = 2;

        //BindingList<PlayerCount> playerCounts = new BindingList<PlayerCount>(); 

        private struct FactionStats
        {
            public string name;
            public float avgScore;
            public int scoreCount;
            public float winRatio;
            public int playCount;
        }
        private Dictionary<string, FactionStats> AllStats = new Dictionary<string, FactionStats>();
        private List<string> AllFactions = new List<string>();

        public FormStats(string id)
        {
            gameId = id;

            InitializeComponent();

            AllPlays = Program.data.selectPlays(gameId);

            aggregated = Program.data.selectAggregated(gameId);
            excluded = Program.data.selectExcluded(gameId);

            UpdateAll();

            for (int i=2; i<=10; i++)
            {
                //playerCounts.Add(new PlayerCount() { intPlayerCount = i, strPlayerCount = i + " jogadores" });
                //playerCounts.Add(i + " jogadores");
            }

            //cbMinPlayers.DataSource = playerCounts;
            //cbMinPlayers.DisplayMember = "strPlayerCount";
            //cbMinPlayers.ValueMember = "intPlayerCount";
            //cbMinPlayers.DataBindings.

            //cbMaxPlayers.DataSource = playerCounts;
            //cbMaxPlayers.DisplayMember = "strPlayerCount";
            //cbMaxPlayers.ValueMember = "intPlayerCount";
        }

        private void btnSyncPlays_Click(object sender, EventArgs e)
        {
            RestClient client = new RestClient("https://boardgamegeek.com");

            IRestRequest r = new RestRequest("/xmlapi2/plays", Method.GET);
            r.AddQueryParameter("id", gameId);
            r.AddQueryParameter("type", "thing");
            //username
            //mindate
            //maxdate
            //subtype
            //page

            int page = 1;
            int total = -1;
            pbLoadBar.Value = 0;

            txtDebug.AppendText("START SYNC: " + DateTime.Now);

            do
            {
                r.AddOrUpdateParameter("page", page.ToString(), ParameterType.QueryString);
                IRestResponse response = client.Execute(r);

                XDocument content = new XDocument();
                content = XDocument.Parse(response.Content);

                if (total < 0)
                {
                    total = int.Parse(content.Element("plays").Attribute("total").Value);
                    pbLoadBar.Maximum = (int)Math.Ceiling((double)total / 100);
                    pbLoadBar.Step = 1;
                }
                pbLoadBar.PerformStep();

                foreach (XElement play in content.Element("plays").Elements())
                {
                    int playerCount = 0;
                    if (play.Element("players") != null) playerCount = play.Element("players").Elements().Count();
                    bool ok = Program.data.insertPlay(play.Attribute("id").Value, play.Attribute("userid").Value, play.Attribute("date").Value, play.Attribute("quantity").Value, play.Attribute("length").Value, play.Attribute("incomplete").Value, play.Attribute("nowinstats").Value, play.Attribute("location").Value, gameId, playerCount);
                    if (ok && play.Element("players") != null)
                    {
                        foreach (XElement player in play.Element("players").Elements())
                        {
                            Program.data.insertPlayer(player.Attribute("userid").Value, player.Attribute("name").Value, player.Attribute("startposition").Value, player.Attribute("color").Value.ToUpper().Trim(), player.Attribute("score").Value, player.Attribute("new").Value, player.Attribute("rating").Value, player.Attribute("win").Value, play.Attribute("id").Value);
                        }
                    }
                }

                page++;
                Thread.Sleep(2100);
            } while ((page - 1) * 100 < total);

            txtDebug.AppendText("END SYNC, START DB READ: " + DateTime.Now);

            AllPlays = Program.data.selectPlays(gameId); //TEMPORÁRIO

            txtDebug.AppendText("END DB READ: " + DateTime.Now);

            UpdateAll();
        } 
        
        private void UpdateAll(List<string> includedColors = null, bool exclusiveColors = false, int minPlayers = 0, int maxPlayers = 999)
        {
            AllStats.Clear();
            totalPlayTime = 0;
            playTimeCount = 0;

            foreach (Play play in AllPlays)
            {
                if (play.playerCount < minPlayers || play.playerCount > maxPlayers) continue;
                
                if (play.length>10)
                {
                    totalPlayTime += play.length;
                    playTimeCount++;
                }

                List<string> colorNames = new List<string>();
                foreach (Player player in play.players)
                {
                    string color = player.color;
                    if (aggregated.ContainsKey(color)) color = aggregated[color];
                    if (excluded.Contains(color)) continue;
                    colorNames.Add(color);
                    if (!AllFactions.Contains(color)) AllFactions.Add(color);
                }
                if (includedColors!=null)
                {
                    List<string> except1 = includedColors.Except(colorNames).ToList();
                    List<string> except2 = colorNames.Except(includedColors).ToList();
                    if (except1.Count>0 || (except2.Count>0 && exclusiveColors))
                    {
                        continue;
                    }
                }

                List<string> countedColor = new List<string>();

                foreach (Player player in play.players) {
                    string color = player.color;
                    if (excluded.Contains(color)) continue;
                    if (aggregated.ContainsKey(color)) color = aggregated[color];

                    if (countedColor.Contains(color)) continue;
                    else countedColor.Add(color);

                    FactionStats fs = new FactionStats();
                    fs.name = color;
                    if (AllStats.Keys.Contains(color))
                    {
                        fs.winRatio = (AllStats[color].winRatio * AllStats[color].playCount + player.win * play.quantity) / (AllStats[color].playCount + play.quantity);
                        fs.playCount = AllStats[color].playCount + play.quantity;
                        if (player.score != 0)
                        {
                            fs.avgScore = (AllStats[color].avgScore * AllStats[color].scoreCount + player.score * play.quantity) / (AllStats[color].scoreCount + play.quantity);
                            fs.scoreCount = AllStats[color].scoreCount + play.quantity;
                        }
                        else
                        {
                            fs.avgScore = AllStats[color].avgScore;
                            fs.scoreCount = AllStats[color].scoreCount;
                        }
                        AllStats[color] = fs;
                    }
                    else
                    {
                        fs.avgScore = player.score;
                        if (player.score != 0)
                        {
                            fs.scoreCount = play.quantity;
                        }
                        else
                        {
                            fs.scoreCount = 0;
                        }
                        fs.winRatio = player.win;
                        fs.playCount = play.quantity;
                        AllStats.Add(color, fs);
                    }
                }
            }

            UpdateStatsShow();
            UpdateFactionsShow();
        }

        private void Filter(List<string> includedColors = null, bool exclusiveColors = false, int minPlayers = 0, int maxPlayers = 999)
        {
            totalPlayTime = 0;
            playTimeCount = 0;
            AllStats.Clear();
            foreach (Play play in AllPlays)
            {
                if (play.playerCount < minPlayers || play.playerCount > maxPlayers) continue;

                if (play.length > 10)
                {
                    totalPlayTime += play.length;
                    playTimeCount++;
                }

                List<string> colorNames = new List<string>();
                foreach (Player player in play.players)
                {
                    string color = player.color;
                    if (aggregated.ContainsKey(color)) color = aggregated[color];
                    if (excluded.Contains(color)) continue;
                    colorNames.Add(color);
                }
                if (includedColors != null)
                {
                    List<string> except1 = includedColors.Except(colorNames).ToList();
                    List<string> except2 = colorNames.Except(includedColors).ToList();
                    if (except1.Count > 0 || (except2.Count > 0 && exclusiveColors))
                    {
                        continue;
                    }
                }

                List<string> countedColor = new List<string>();

                foreach (Player player in play.players)
                {
                    string color = player.color;
                    if (aggregated.ContainsKey(color)) color = aggregated[color];
                    if (excluded.Contains(color)) continue;

                    if (countedColor.Contains(color)) continue;
                    else countedColor.Add(color);

                    FactionStats fs = new FactionStats();
                    fs.name = color;
                    if (AllStats.Keys.Contains(color))
                    {
                        fs.winRatio = (AllStats[color].winRatio * AllStats[color].playCount + player.win * play.quantity) / (AllStats[color].playCount + play.quantity);
                        fs.playCount = AllStats[color].playCount + play.quantity;
                        if (player.score != 0)
                        {
                            fs.avgScore = (AllStats[color].avgScore * AllStats[color].scoreCount + player.score * play.quantity) / (AllStats[color].scoreCount + play.quantity);
                            fs.scoreCount = AllStats[color].scoreCount + play.quantity;
                        }
                        else
                        {
                            fs.avgScore = AllStats[color].avgScore;
                            fs.scoreCount = AllStats[color].scoreCount;
                        }
                        AllStats[color] = fs;
                    }
                    else
                    {
                        fs.avgScore = player.score;
                        if (player.score != 0)
                        {
                            fs.scoreCount = play.quantity;
                        }
                        else
                        {
                            fs.scoreCount = 0;
                        }
                        fs.winRatio = player.win;
                        fs.playCount = play.quantity;
                        AllStats.Add(color, fs);
                    }
                }
            }

            UpdateStatsShow();
        }

        private void UpdateStatsShow()
        {
            int AvgPlayTime = 0;
            if (playTimeCount > 0) AvgPlayTime = (int)(totalPlayTime / playTimeCount);
            lbAvgPlayTime.Text = $"{AvgPlayTime} minutes (from {playTimeCount} plays).";

            AllStats.OrderBy(k => k.Value.playCount);
            dgStats.Rows.Clear();
            foreach (string faction in AllStats.Keys.OrderBy(k => AllStats[k].playCount))
            {
                if (AllStats[faction].playCount > minPlayCount)
                {
                    dgStats.Rows.Add(faction, AllStats[faction].winRatio, AllStats[faction].avgScore, AllStats[faction].playCount, AllStats[faction].scoreCount);
                }
            }
        }

        private void UpdateFactionsShow()
        {
            lbFactions.Items.Clear();
            foreach (string faction in AllFactions)
            {
                if (AllStats[faction].playCount > minPlayCount)
                {
                    lbFactions.Items.Add(faction);
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            /*foreach (string selectedFaction in lbFactions.SelectedItems)
            {
                string faction = Regex.Replace(selectedFaction, "\\[\\d+\\]", "").Trim();
                AllStats.Remove(faction);
            }*/
            foreach (DataGridViewRow selectedRow in dgStats.SelectedRows)
            {
                string faction = selectedRow.Cells["Faccoes"].Value.ToString();
                AllStats.Remove(faction);
                AllFactions.Remove(faction);
                excluded.Add(faction);
                Program.data.insertUpdateColor(gameId, faction, "", 1);
            }

            UpdateStatsShow();
            UpdateFactionsShow();
        }

        private void btnAggregate_Click(object sender, EventArgs e)
        {
            //FormNameDialog nameDialog = new FormNameDialog(lbFactions.SelectedItems.Cast<string>().ToList());

            List<string> selectedNames = new List<string>();
            foreach (DataGridViewRow selectedRow in dgStats.SelectedRows)
            {
                selectedNames.Add(selectedRow.Cells["Faccoes"].Value.ToString());
            }
            FormNameDialog nameDialog = new FormNameDialog(selectedNames);

            nameDialog.ShowDialog();
            string newName = nameDialog.selectedName;

            FactionStats fs = new FactionStats();
            fs.name = newName;
            fs.avgScore = 0;
            fs.scoreCount = 0;
            fs.winRatio = 0;
            fs.playCount = 0;

            //foreach (string selectedFaction in lbFactions.SelectedItems)
            foreach (string selectedFaction in selectedNames)
            {
                string faction = Regex.Replace(selectedFaction, "\\[\\d+\\]", "").Trim();
                if (AllStats[faction].scoreCount + fs.scoreCount > 0)
                {
                    fs.avgScore = (AllStats[faction].avgScore * AllStats[faction].scoreCount + fs.avgScore * fs.scoreCount) / (AllStats[faction].scoreCount + fs.scoreCount);
                }
                fs.scoreCount = AllStats[faction].scoreCount + fs.scoreCount;
                if (AllStats[faction].playCount + fs.playCount > 0)
                {
                    fs.winRatio = (AllStats[faction].winRatio * AllStats[faction].playCount + fs.winRatio * fs.playCount) / (AllStats[faction].playCount + fs.playCount);
                }
                fs.playCount = AllStats[faction].playCount + fs.playCount;

                AllStats.Remove(faction);
                AllFactions.Remove(faction);
                if (!aggregated.ContainsKey(faction)) aggregated.Add(faction, newName);

                Program.data.insertUpdateColor(gameId, faction, newName, 0);
            }

            AllStats.Add(newName, fs);
            AllFactions.Add(newName);

            UpdateStatsShow();
            UpdateFactionsShow();
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            List<string> include = new List<string>();
            include = lbFactions.SelectedItems.Cast<string>().ToList();
            Filter(include, cbApenasSelecionados.Checked, int.Parse((String)cbMinPlayers.SelectedItem), int.Parse((String)cbMaxPlayers.SelectedItem));
        }
    }
}
