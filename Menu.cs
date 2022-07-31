using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows.Forms;
using ExcelDataReader;
using System.Collections.Generic;

namespace Parser3._0 
{
    public partial class Menu : Form
    {
        DBTree dbTree;
        ExcelReader excelReader;
        CustomTabControl customTabControl;
        JsonConverter JsonConverter;
        AddToDB addToDB;       
       
        private string[] InVarriable = new string[1] {"Save"};       
        private string[] ToDataBase =  new string[1] {"Find"};
        public static int choice = 0;

        public Menu()
        {                       
            dbTree = new DBTree();
            excelReader = new ExcelReader();
            customTabControl = new CustomTabControl();
            addToDB = new AddToDB();

            InitializeComponent();           
            customTabControl.dataGridView = dataGridView1;
            customTabControl.listBox1 = listBox1;
            customTabControl.listBox2 = listBox2;
            customTabControl.reader = excelReader;
            listBox1.SelectedIndex = 0;
            listBox2.SelectedIndex = 0;
            JsonConverter = new JsonConverter(customTabControl);
            DBManager.setting = BinConvert.ReadFromFile();
        }
        
        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {                                                                                                
              if(openFileDialog1.ShowDialog() == DialogResult.OK)
              {
                  string path = openFileDialog1.FileName;
                  TabPage Page = new TabPage();
                  string fileName = Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                  Page.Text = fileName;                    
                  excelReader.OpenExcelFile(path);
                  customTabControl.AddGridToPage(Page, path, excelReader.tableCollection);
                  customTabControl.AddToolToPage(Page, excelReader.tableCollection);
                  tabControl1.TabPages.Add(Page);
                  Page.Tag = excelReader.tableCollection;
              }
              else { MessageBox.Show("Файл не выбран"); }                                    
        }
        
        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {         
            if(tabControl1.TabCount != 0)
            {
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
            }
            else { MessageBox.Show("Таблица не открыта!"); }
        }
        
        private void listBox1_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            switch (listBox1.SelectedIndex)
            {
                case 0:                    
                    listBox2.Items.AddRange(InVarriable);
                    break;
                case 1:                    
                    listBox2.Items.AddRange(ToDataBase);
                    break;
            }                       
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CmdExecute.Variable.Clear();
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (dataGridView1.Rows[i].Cells[3].Value.ToString() == "Save")
                {
                    CmdExecute.InVariable(new Variable(dataGridView1.Rows[i].Cells[1].Value.ToString(),""));
                }
                else if (dataGridView1.Rows[i].Cells[3].Value.ToString() == "Find")
                {
                    string options = dataGridView1.Rows[i].Cells[4].Value.ToString();
                    string results = dataGridView1.Rows[i].Cells[5].Value.ToString();
                    string value = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    CmdExecute.Find(options, results, value);
                }
            }
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.CurrentRow.Cells[2].Value = listBox1.SelectedItem.ToString();
                dataGridView1.CurrentRow.Cells[3].Value = listBox2.SelectedItem.ToString();
            }
            catch { MessageBox.Show("Выберите строку которую хотите изменить"); }
        }
        
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                
                dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
                              
            }
            catch { MessageBox.Show("Выберите строку которую хотите удалить"); }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {            
            TabPage tabPage = tabControl1.TabPages[((TabControl)sender).SelectedIndex];
            excelReader.tableCollection = (DataTableCollection)tabPage.Tag;
            foreach (Control con in tabPage.Controls)
            {
                if(con.GetType() == typeof(DataGridView))
                {
                    customTabControl.CurrentdataGrid = ((DataGridView)con);
                }
            }            
        }

        private void button5_Click(object sender, EventArgs e)
        {            
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = saveFileDialog1.FileName;
                JsonConverter.readFromGrid(dataGridView1, path);
            }                 
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(customTabControl.CurrentdataGrid != null)
            { 
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string path = openFileDialog1.FileName;
                    JsonConverter.writeToGrid(dataGridView1, path);

                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[0].Value.ToString() == "Local Data")
                        {
                            addToDB.dataGridView2.Rows.Add();
                            addToDB.dataGridView2.Rows[addToDB.dataGridView2.RowCount - 1].Cells[0].Value = dataGridView1.Rows[i].Cells[4].Value.ToString();
                            addToDB.dataGridView2.Rows[addToDB.dataGridView2.RowCount - 1].Cells[1].Value = dataGridView1.Rows[i].Cells[5].Value.ToString();
                            addToDB.dataGridView2.Rows[addToDB.dataGridView2.RowCount - 1].Cells[2].Value = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        }
                    }
                    button2_Click(sender, e);
                }
            }
            else { MessageBox.Show("Откройте таблицу"); }            
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            choice = 0;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            choice = 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {          
            if (addToDB.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < addToDB.dataGridView2.RowCount; i++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = "Local Data";
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = addToDB.dataGridView2.Rows[i].Cells[2].Value.ToString();
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = "ToDataBase";
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[3].Value = "Add";
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[4].Value = addToDB.dataGridView2.Rows[i].Cells[0].Value.ToString();
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[5].Value = addToDB.dataGridView2.Rows[i].Cells[1].Value.ToString();
                }
            }                    
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox2.SelectedIndex = 0;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex == 4 || e.ColumnIndex == 5) && dataGridView1.CurrentRow.Cells[3].Value.ToString() == "Find")
            {
                dbTree.ShowDialog();
                if (dbTree.DialogResult == DialogResult.OK)
                {
                    dataGridView1.CurrentCell.Value = dbTree.GetName();
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            CmdExecute.Variable.Clear();
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (dataGridView1.Rows[i].Cells[3].Value.ToString() == "Save")
                {
                    CmdExecute.InVariable(new Variable(dataGridView1.Rows[i].Cells[1].Value.ToString(), ""));
                }
                else if (dataGridView1.Rows[i].Cells[3].Value.ToString() == "Find")
                {
                    string options = dataGridView1.Rows[i].Cells[4].Value.ToString();
                    string results = dataGridView1.Rows[i].Cells[5].Value.ToString();
                    string value = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    CmdExecute.Find(options, results, value);
                }
                else if(dataGridView1.Rows[i].Cells[3].Value.ToString() == "Add")
                {
                    string[] tmpvalues = dataGridView1.Rows[i].Cells[1].Value.ToString().Split(new char[] { ',' });
                    string values = "";
                    for (int j = 0; j < tmpvalues.Length; j++)
                    {
                        if(j == 0)
                        {
                            values += "'" + CmdExecute.Variable[Convert.ToInt32(tmpvalues[j])].value.ToString() + "'";
                        }
                        else
                        {
                            values += "," + "'" + CmdExecute.Variable[Convert.ToInt32(tmpvalues[j])].value.ToString() + "'";
                        }                            
                    }
                    string tableName = dataGridView1.Rows[i].Cells[4].Value.ToString();
                    string columns = dataGridView1.Rows[i].Cells[5].Value.ToString();

                    CmdExecute.Add(tableName, columns, values);
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        private void dataBaseToolStripMenuItem_Click(object sender, EventArgs e)
        {          
             DB db = new DB();           
             db.Show();                     
        }

        private void dbSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dbSettings dbSettings = new dbSettings();
            dbSettings.ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Insert(dataGridView1.CurrentRow.Index);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try 
            {
                JsonConverter.ChangeValue(dataGridView1, customTabControl.CurrentdataGrid);
            }
            catch { MessageBox.Show("Не удалось обновить значения"); }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            for (int i = customTabControl.CurrentdataGrid.SelectedCells.Count - 1; i >= 0; i--)
            {
                if (choice == 0)
                {
                    int cr = dataGridView1.RowCount;
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[cr].Cells[0].Value = "[" + customTabControl.CurrentdataGrid.SelectedCells[i].RowIndex + ";" + customTabControl.CurrentdataGrid.SelectedCells[i].ColumnIndex + "]";
                    dataGridView1.Rows[cr].Cells[1].Value = customTabControl.CheckFloat(customTabControl.CurrentdataGrid.SelectedCells[i].Value.ToString());                    
                    dataGridView1.Rows[cr].Cells[2].Value = listBox1.SelectedItem.ToString();
                    dataGridView1.Rows[cr].Cells[3].Value = listBox2.SelectedItem.ToString();
                }                
            }
        }

        private string RowIncrease(string row)
        {
            string[] tmp = row.Split('[',';',']');
            string resRow =  "[" + (Convert.ToInt32(tmp[1]) + 1) + ';' + tmp[2] + "]";
            return resRow;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = RowIncrease(dataGridView1.Rows[i].Cells[0].Value.ToString());             
            }
            JsonConverter.ChangeValue(dataGridView1, customTabControl.CurrentdataGrid);
            customTabControl.CurrentdataGrid.CurrentCell = customTabControl.CurrentdataGrid.Rows[customTabControl.CurrentdataGrid.CurrentRow.Index + 1].Cells[0];
        }

        private void Menu_Load(object sender, EventArgs e)
        {

        }
    }
}
