using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser3._0
{
    public class Variable
    {
        public Variable(string value, string desc)
        {
            this.value = value;
            this.desc = desc;
        }
        public object value;
        public string desc;
    }
}
