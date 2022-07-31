using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parser3._0
{
    public partial class dbSettings : Form
    {                
        public dbSettings()
        {           
            InitializeComponent();
            textBox1.Text = DBManager.setting.Server;
            textBox2.Text = DBManager.setting.Database;
            textBox3.Text = DBManager.setting.Uid;
            textBox4.Text = DBManager.setting.Pwd;
            textBox5.Text = DBManager.setting.charset;
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {                    
            DBManager.setting.Server = textBox1.Text;
            DBManager.setting.Database = textBox2.Text;
            DBManager.setting.Uid = textBox3.Text;
            DBManager.setting.Pwd = textBox4.Text;
            DBManager.setting.charset = textBox5.Text;
            DBManager.connectTo();

            BinConvert.WriteToFile(DBManager.setting);           
        }
    }
}
