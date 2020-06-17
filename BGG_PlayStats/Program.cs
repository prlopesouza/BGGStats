using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BGG_PlayStats
{
    static class Program
    {
        static public DataClass data = new DataClass();
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            data.CreateConnection("database.db");
            data.InitializeDB();
            Application.Run(new FormSearch());
        }
    }
}
