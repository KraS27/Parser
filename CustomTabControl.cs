using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parser3._0
{
    class CustomTabControl
    {             
        public DataGridView dataGridView;
        public DataGridView CurrentdataGrid;        
        public ListBox listBox1;
        public ListBox listBox2;
        public ExcelReader reader;
        
        //Add dataGrid for the current page and fill it data from  tableCollection
        public void AddGridToPage(TabPage Page, string path, DataTableCollection tableCollection)
        {            
            DataGridView dgv = new DataGridView();
            string fileName = Path.GetFileNameWithoutExtension(path);                       
            dgv.Left = 0;
            dgv.Top = 0;
            dgv.Dock = DockStyle.Fill;
            dgv.DataSource = tableCollection[0];
            Page.Controls.Add(dgv);
            CurrentdataGrid = dgv;
            dgv.CellClick += new DataGridViewCellEventHandler(this.CellClick);            
        }

        //Добавляет ToolStrip на указаную страницу, также добавляет на ToolStrip 2 елемента: label и comboBox
        //comboBox нужен для выбора листа из таблицы Excel (если их несколько)
        public void AddToolToPage(TabPage Page, DataTableCollection tableCollection)
        {
            ToolStripLabel label = new ToolStripLabel();
            ToolStripComboBox comboBox = new ToolStripComboBox();
            ToolStrip tool = new MenuStrip();

            label.Text = "Лист: ";

            foreach(DataTable table in tableCollection)
            {
                comboBox.Items.Add(table.TableName);     
            }
            comboBox.SelectedIndex = 0;
            comboBox.SelectedIndexChanged += new EventHandler(this.SelectedIndexChanged);

            tool.Dock = DockStyle.Top;
            tool.Items.Add(label);
            tool.Items.Add(comboBox);
            Page.Controls.Add(tool);           
        }

        //Обработчик события при клике на ячейку dataGrid рассположеном в tabControl (обозначу его как gridControl)
        //Заполняет ячейки dataGridView1 информацией из ячеек gridControl, а так же выбранными елементами из listBox1 и listBox2
        private void CellClick(object sender, DataGridViewCellEventArgs e)
        {           
            if (Menu.choice == 0) 
            {
                int cr = dataGridView.RowCount;
                dataGridView.Rows.Add();
                dataGridView.Rows[cr].Cells[0].Value = "[" + e.RowIndex.ToString() + ";" + e.ColumnIndex.ToString() + "]";
                dataGridView.Rows[cr].Cells[1].Value = CheckFloat(((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                dataGridView.Rows[cr].Cells[2].Value = listBox1.SelectedItem.ToString();
                dataGridView.Rows[cr].Cells[3].Value = listBox2.SelectedItem.ToString();                
            }
            else if(Menu.choice == 1)
            { 
                if(dataGridView.CurrentRow != null)
                {
                    dataGridView.CurrentRow.Cells[0].Value = "[" + e.RowIndex.ToString() + " ; " + e.ColumnIndex.ToString() + "]";
                    dataGridView.CurrentRow.Cells[1].Value = ((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    dataGridView.CurrentRow.Cells[2].Value = listBox1.SelectedItem.ToString();
                    dataGridView.CurrentRow.Cells[3].Value = listBox2.SelectedItem.ToString();
                }               
            }
        }
        
        private void SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable table = reader.tableCollection[((ToolStripComboBox)sender).SelectedItem.ToString()];           
            CurrentdataGrid.DataSource = table;
        }

        public string CheckFloat(string s)
        {
            try
            {
                float f = float.Parse(s);
                return s.Replace(",", ".");
            }
            catch
            {
                return s;
            }
        }
    }
}
