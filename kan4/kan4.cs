﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kan4
{
    public partial class kan4 : Form
    {
        private Kan4DB db;

        public kan4()
        {
            InitializeComponent();
            db = new Kan4DB();
        }
    }
}
