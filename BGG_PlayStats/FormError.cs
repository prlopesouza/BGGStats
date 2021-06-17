﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BGG_PlayStats
{
    public partial class FormError : Form
    {
        public FormError(String msg, String stack)
        {
            InitializeComponent();
            txtErrorMsg.Text = msg;
            txtStackTrace.Text = stack;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
