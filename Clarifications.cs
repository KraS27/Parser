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
    public partial class Clarifications : Form
    {
        public Clarifications()
        {
            InitializeComponent();
        }
     
        public void FillGrid(List<object> data)
        {
            dataGridView1.Rows.Clear();
            for(int i = 0; i < data.Count; i++)
            {
                dataGridView1.Rows.Add(i, data[i]);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CmdExecute.cmdclarification = dataGridView1.CurrentRow.Cells[1].Value;
        }

        private void Clarifications_Load(object sender, EventArgs e)
        {

        }
    }
}
