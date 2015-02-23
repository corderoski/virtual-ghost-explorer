using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VGExplorer.Framework.Entities;
using VGExplorer.Framework.Helpers;

namespace VGExplorerTool.Forms
{
    public partial class Configurations : Form
    {

        Configuration _appConfiguration;

        public Configurations()
        {
            InitializeComponent();
            _appConfiguration = FileHelper.GetAppConfiguration(Environment.CurrentDirectory);
        }

        private void Configuration_Load(object sender, EventArgs e)
        {
            this.Text = Program.APP_NAME + " / Configurations";
            this.Icon = Helpers.ResourcesHelper.GetAppIcon();
            //
            chckBox.Text = "Include/Create files in schema a creation"; 
        }
    }
}
