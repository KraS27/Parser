using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Parser3._0
{
    class JsonConverter
    {
        CustomTabControl tabControl;
        public int currentRow = 0;
        private string jsonData { get; set; }        
        
        public JsonConverter(CustomTabControl t)
        {
            this.tabControl = t;
        }
        public void readFromGrid(DataGridView dgv, string path)
        {
            JsonProvider[] jsonProvider = new JsonProvider[dgv.RowCount];
            for(int i = 0;i < dgv.RowCount; i++)
            {
                jsonProvider[i].InputData = dgv.Rows[i].Cells[0].Value?.ToString();
                if(dgv.Rows[i].Cells[0].Value.ToString() == "Local Data")
                {
                    jsonProvider[i].Value = dgv.Rows[i].Cells[1].Value?.ToString();
                }               
                jsonProvider[i].OutputData = dgv.Rows[i].Cells[2].Value?.ToString();
                jsonProvider[i].Command = dgv.Rows[i].Cells[3].Value?.ToString();
                jsonProvider[i].Options = dgv.Rows[i].Cells[4].Value?.ToString();
                jsonProvider[i].Result = dgv.Rows[i].Cells[5].Value?.ToString();
            }                       
            jsonData = JsonConvert.SerializeObject(jsonProvider);
            File.WriteAllText(path, jsonData);
        }
        
        public void writeToGrid(DataGridView dgv, string path)
        {           
            jsonData = File.ReadAllText(path);
            var jsonProvider = JsonConvert.DeserializeObject<JsonProvider[]>(jsonData);
            dgv.Rows.Clear();
            for (int i = 0; i < jsonProvider.Length; i++)
            {
                dgv.Rows.Add();
                dgv.Rows[i].Cells[0].Value = jsonProvider[i].InputData;
                if (dgv.Rows[i].Cells[0].Value != null && dgv.Rows[i].Cells[0].Value.ToString() != "Local Data")
                {
                    var tmp = dgv.Rows[i].Cells[0].Value.ToString().Split(new char[] { '[', ';', ']' });
                    dgv.Rows[i].Cells[1].Value = tabControl.CurrentdataGrid.Rows[Convert.ToInt32(tmp[1])].Cells[Convert.ToInt32(tmp[2])].Value.ToString();
                }
                else if(dgv.Rows[i].Cells[0].Value.ToString() == "Local Data")
                {
                    dgv.Rows[i].Cells[1].Value = jsonProvider[i].Value;
                }
                dgv.Rows[i].Cells[2].Value = jsonProvider[i].OutputData;
                dgv.Rows[i].Cells[3].Value = jsonProvider[i].Command;
                dgv.Rows[i].Cells[4].Value = jsonProvider[i].Options;
                dgv.Rows[i].Cells[5].Value = jsonProvider[i].Result;
            }           
                     
        }
        public void ChangeValue(DataGridView recipient, DataGridView sender)
        {
            try
            {
                for (int i = 0; i < recipient.RowCount; i++)
                {
                    if (recipient.Rows[i].Cells[0].Value != null && recipient.Rows[i].Cells[0].Value.ToString() != "Local Data")
                    {
                        var tmp = recipient.Rows[i].Cells[0].Value.ToString().Split(new char[] { '[', ';', ']' });
                        recipient.Rows[i].Cells[1].Value = sender.Rows[Convert.ToInt32(tmp[1])].Cells[Convert.ToInt32(tmp[2])].Value.ToString();
                    }
                }
            }
            catch { MessageBox.Show("Некорректный индекс ячейки"); }
        }

    }
}
