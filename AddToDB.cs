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
    public partial class AddToDB : Form
    {
        DBManager dbManager = new DBManager();
        DB db = new DB();
        
        public AddToDB()
        {
            DBManager.connectTo();
            InitializeComponent();
        }

        private void AddToDB_Load(object sender, EventArgs e)
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
                dataGridView1.Rows.Clear();
                for (int i = 0; i < CmdExecute.Variable.Count; i++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = i;
                    dataGridView1.Rows[i].Cells[1].Value = CmdExecute.Variable[i].value.ToString();
                    dataGridView1.Rows[i].Cells[2].Value = CmdExecute.Variable[i].desc.ToString();
                }
            }
            catch { MessageBox.Show("Некоректное подключение к базе данных");this.Close(); }
            
        }       

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if(dataGridView2.RowCount != 0)
            {
                if (dataGridView2.CurrentCell.ColumnIndex == 0)
                {
                    dataGridView2.CurrentCell.Value = treeView1.SelectedNode.Text.ToString();
                }
                else if (dataGridView2.CurrentCell.ColumnIndex == 1)
                {
                    if (dataGridView2.CurrentCell.Value == null)
                    {
                        dataGridView2.CurrentCell.Value += treeView1.SelectedNode.Text.ToString();
                    }
                    else { dataGridView2.CurrentCell.Value += "," + treeView1.SelectedNode.Text.ToString(); }
                }
                if(dataGridView2.CurrentCell.ColumnIndex == 1)
                {
                    string[] colCount = dataGridView2.CurrentCell.Value.ToString().Split(new char[] { ',' });
                    dataGridView2.CurrentRow.Cells[3].Value = colCount.Length.ToString();
                }                
            }
            else { MessageBox.Show("Добавьте строку"); }           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Add();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView2.Rows.RemoveAt(dataGridView2.CurrentRow.Index);
            }
            catch { MessageBox.Show("Выберите строку, которую хотите удалить"); } 
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            db.ShowDialog();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(dataGridView2.RowCount != 0)
            {
                if (dataGridView2.CurrentRow.Cells[2].Value == null)
                {
                    dataGridView2.CurrentRow.Cells[2].Value += dataGridView1.CurrentRow.Cells[0].Value.ToString();
                }
                else
                {
                    dataGridView2.CurrentRow.Cells[2].Value += "," + dataGridView1.CurrentRow.Cells[0].Value.ToString();
                }
                if (dataGridView2.CurrentCell.ColumnIndex == 2)
                {
                    string[] valueCount = dataGridView2.CurrentCell.Value.ToString().Split(new char[] { ',' });
                    dataGridView2.CurrentRow.Cells[4].Value = valueCount.Length.ToString();
                }
            }
            else { MessageBox.Show("Добавьте строку"); }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
        }
    }
}
