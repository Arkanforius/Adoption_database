﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoptionDatabase
{
    public partial class LogInForm : Form
    {
        public LogInForm()
        {
            InitializeComponent();
        }

        private void logInBtn(object sender, EventArgs e)
        {
            //need if else statement first to check if credentials correct
            this.Hide();
            AdminPortal adminport = new AdminPortal();
            adminport.ShowDialog();
            this.Close();
        }
    }
}