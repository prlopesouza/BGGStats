using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BGG_PlayStats
{
    public partial class FormNameDialog : Form
    {
        public string selectedName = "";

        public FormNameDialog(List<string> names)
        {
            InitializeComponent();
            foreach (string name in names)
            {
                string factionName = Regex.Replace(name, "\\[\\d+\\]", "").Trim();
                cbFactionNames.Items.Add(factionName);
            }
            cbFactionNames.SelectedIndex = 0;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            selectedName = cbFactionNames.SelectedItem.ToString().ToUpper().Trim();
            this.Close();
        }
    }
}
