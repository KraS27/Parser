using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parser3._0
{   
    static public class CmdExecute
    {
        static public object cmdclarification;

        static Clarifications clarifications = new Clarifications();
        static DBManager dB = new DBManager();
        static CmdExecute() { DBManager.connectTo(); }

        static public List<Variable> Variable = new List<Variable>();       

        static private string[] DataSplit(string data)
        {
            return data.Split(new char[] { ';' });
        }

        static public void InVariable(Variable data)
        {
            if(data.value != "")
            Variable.Add(data);
        }

        static public void Find(string Options, string Results, string value)
        {
            var options = DataSplit(Options);
            var results = DataSplit(Results);
            var tmp = dB.DBFind(options[0], results[1], options[1], "'"+value+"'");       
            if(tmp.Count == 1)
            {
                Variable.Add(new Variable(tmp[0].ToString(), value + "; " + results[1]));
            }
            else if(tmp.Count == 0) 
            { 
                MessageBox.Show($"В базе данных нету записей со значением: {value}.");
            }
            else if(tmp.Count > 1)
            {
                MessageBox.Show($"В базе данных есть несколько записей со значением: {value}.\nВыберите какое вам нужно");
                clarifications.FillGrid(tmp);
                clarifications.ShowDialog();
                Variable.Add(new Variable(cmdclarification.ToString(), value + "; " + results[1]));
            }
        }

        static public void Add(string Tables, string Columns, string Values)
        {
            dB.DBAdd(Tables, Columns, Values);
        }
    }
}
