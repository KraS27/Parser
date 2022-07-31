using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelDataReader;

namespace Parser3._0
{
    class ExcelReader
    {
        public DataTableCollection tableCollection = null;       
        
        //Сам до конца не понимаю как оно работает требует using ExcelDataReader
        //Требует using ExcelDataReader, подклчённого с помощью пакетов NuGet
        public void OpenExcelFile(string path)
        {
            FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read);

            IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);

            DataSet db = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (configure) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = false
                }
            });
            tableCollection = db.Tables;            
        }       
    }
}
