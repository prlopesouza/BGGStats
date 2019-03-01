using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace BGG_PlayStats
{
    public partial class FormSearch : Form
    {
        List<Dictionary<string, string>> searchResults = new List<Dictionary<string, string>>();

        public FormSearch()
        {
            InitializeComponent();
        }

        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            bggSearch(txtSearch.Text);

            dgSearchResults.Rows.Clear();
            foreach (Dictionary<string, string> item in searchResults)
            {
                dgSearchResults.Rows.Add(item["ID"], item["Name"], item["Year"]);
            }
        }

        private void bggSearch(string text)
        {
            RestClient client = new RestClient("https://boardgamegeek.com");

            IRestRequest r = new RestRequest("/xmlapi2/search", Method.GET);
            r.AddQueryParameter("query", text);
            r.AddQueryParameter("type", "boardgame");
            
            IRestResponse response = client.Execute(r);

            searchResults.Clear();

            XDocument content = new XDocument();
            content = XDocument.Parse(response.Content);

            XElement el = content.Root;
            foreach (XElement item in el.Elements())
            {
                Dictionary<string, string> searchItem = new Dictionary<string, string>();
                searchItem.Add("Name", item.Element("name").Attribute("value").Value);
                if (item.Element("yearpublished") != null)
                {
                    searchItem.Add("Year", item.Element("yearpublished").Attribute("value").Value);
                }
                else
                {
                    searchItem.Add("Year", "");
                }
                searchItem.Add("ID", item.Attribute("id").Value);

                searchResults.Add(searchItem);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            FormStats formStats = new FormStats(dgSearchResults.SelectedRows[0].Cells["Id"].Value.ToString());
            formStats.ShowDialog();
            //this.Hide();
        }
    }
}
