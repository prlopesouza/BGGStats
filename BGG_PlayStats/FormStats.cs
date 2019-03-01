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

        private void btnLoadPlays_Click(object sender, EventArgs e)
        {
            RestClient client = new RestClient("https://boardgamegeek.com");

            IRestRequest r = new RestRequest("/xmlapi2/plays", Method.GET);
            r.AddQueryParameter("id", gameId);
            r.AddQueryParameter("type", "thing");

            int page = 1;
            int total = -1;
            AllPlays = new List<Play>();
            pbLoadBar.Value = 0;
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
                    if (play.Element("players") == null) continue;
                    if (play.Element("players").Elements().Count() < 1) continue;
                    //if (play.Element("players").Elements().Count() < int.Parse(cbMinPlayers.SelectedValue.ToString())) continue;
                    //if (play.Element("players").Elements().Count() > int.Parse(cbMaxPlayers.SelectedValue.ToString())) continue;
                    if (play.Attribute("incomplete").Value == "1") continue;
                    if (play.Attribute("nowinstats").Value == "1") continue;
                    bool ok = true;
                    foreach (XElement player in play.Element("players").Elements())
                    {
                        if (player.Attribute("color").Value == "")
                        {
                            ok = false;
                            break;
                        }
                    }
                    if (!ok) continue;

                    Play objPlay = new Play();

                    objPlay.quantity = int.Parse(play.Attribute("quantity").Value);
                    foreach (XElement player in play.Element("players").Elements())
                    {
                        string color = player.Attribute("color").Value.ToUpper().Trim();

                        //if (excluded.Contains(color)) continue;
                        //if (aggregated.ContainsKey(color)) color = aggregated[color];

                        float score = 0;
                        if (player.Attribute("score").Value != "")
                        {
                            try
                            {
                                score = float.Parse(player.Attribute("score").Value);
                            }
                            catch (Exception) { }
                        }
                        int win = int.Parse(player.Attribute("win").Value);

                        objPlay.AddPlayer(color, win, score);

                        /*FactionStats fs = new FactionStats();
                        fs.name = color;
                        if (AllStats.Keys.Contains(color))
                        {
                            fs.winRatio = (AllStats[color].winRatio * AllStats[color].playCount + win * objPlay.quantity) / (AllStats[color].playCount + objPlay.quantity);
                            fs.playCount = AllStats[color].playCount + objPlay.quantity;
                            if (score != 0)
                            {
                                fs.avgScore = (AllStats[color].avgScore * AllStats[color].scoreCount + score * objPlay.quantity) / (AllStats[color].scoreCount + objPlay.quantity);
                                fs.scoreCount = AllStats[color].scoreCount + objPlay.quantity;
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
                            fs.avgScore = score;
                            if (score != 0)
                            {
                                fs.scoreCount = objPlay.quantity;
                            }
                            else
                            {
                                fs.scoreCount = 0;
                            }
                            fs.winRatio = win;
                            fs.playCount = objPlay.quantity;
                            AllStats.Add(color, fs);
                        }*/
                    }
                    AllPlays.Add(objPlay);
                }
                page++;
                Thread.Sleep(2100);
            } while ((page - 1) * 100 < total);

            UpdateAll();
        }

        private void UpdateAll(List<string> includedColors = null, bool exclusiveColors = false, int minPlayers = 0, int maxPlayers = 999)
        {
            AllStats.Clear();
            foreach (Play play in AllPlays)
            {
                if (play.playerCount < minPlayers || play.playerCount > maxPlayers) continue;
                
                List<string> colorNames = new List<string>();
                foreach (Play.Player player in play.players)
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

                foreach (Play.Player player in play.players) {
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
            AllStats.Clear();
            foreach (Play play in AllPlays)
            {
                if (play.playerCount < minPlayers || play.playerCount > maxPlayers) continue;

                List<string> colorNames = new List<string>();
                foreach (Play.Player player in play.players)
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

                foreach (Play.Player player in play.players)
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
            AllStats.OrderBy(k => k.Value.playCount);
            dgStats.Rows.Clear();
            foreach (string faction in AllStats.Keys.OrderBy(k => AllStats[k].playCount))
            {
                dgStats.Rows.Add(faction, AllStats[faction].winRatio, AllStats[faction].avgScore, AllStats[faction].playCount, AllStats[faction].scoreCount);
            }
        }

        private void UpdateFactionsShow()
        {
            lbFactions.Items.Clear();
            foreach (string faction in AllFactions)
            {
                lbFactions.Items.Add(faction);
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
            Filter(include, cbApenasSelecionados.Checked);
        }
    }
}
