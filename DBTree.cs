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
    public partial class DBTree : Form
    {
        DBManager dbManager = new DBManager();
        
        public DBTree()
        {
            InitializeComponent();
            DBManager.connectTo();
        }

        public string GetName()
        {
            if(treeView1.SelectedNode.Parent != null)
            {
                return treeView1.SelectedNode.Parent.Text + ";" + treeView1.SelectedNode.Text;
            }
            else { return treeView1.SelectedNode.Text; }
        }

        private void DBTree_Load(object sender, EventArgs e)
        {
            try
            {                            
                treeView1.Nodes.Clear();
                var Tables = dbManager.ShowTables();
                for (int i = 0; i < Tables.Count; i++)
                { 
                    treeView1.Nodes.Add(Tables[i].ToString());
                    var column = dbManager.ShowColumns(Tables[i].ToString());

                    for (int j = 0; j < column.Count; j++)
                    {
                        treeView1.Nodes[i].Nodes.Add(column[j].ToString());
                    }
                }               
            }
            catch { MessageBox.Show("Некоректное подключение к базе данных");this.Close();}
                      
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }
    }
}
