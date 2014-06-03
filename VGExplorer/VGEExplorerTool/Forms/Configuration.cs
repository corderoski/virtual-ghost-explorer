using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VGExplorerTool.Forms
{
    public partial class Configuration : Form
    {

        Entities.Configuration _appConfiguration;

        public Configuration()
        {
            InitializeComponent();
            _appConfiguration = Helpers.FileHelper.GetAppConfiguration();
        }

        private void Configuration_Load(object sender, EventArgs e)
        {
            this.Text = Program.AppName + " / Configurations";
            this.Icon = Helpers.ResourcesHelper.GetAppIcon();
            //
            chckBox.Text = "Include/Create files in schema a creation"; 
        }
    }
}
