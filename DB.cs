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
    public partial class DB : Form
    {
        DBManager dbManager = new DBManager();

        public DB()
        {
            InitializeComponent();
            DBManager.connectTo();
        }

        private void DB_Load(object sender, EventArgs e)
        {
            try
            {
                foreach (object x in dbManager.ShowTables())
                {
                    listBox1.Items.Add(x);
                }
                listBox1.SelectedIndex = 0;
               
                dbManager.SelectAll(listBox1.SelectedItem.ToString(), dataGridView1);               
            }
            catch { MessageBox.Show("Некоректное подключение к базе данных");this.Close(); }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dbManager.SelectAll(listBox1.SelectedItem.ToString(), dataGridView1);
        }
    }
}
