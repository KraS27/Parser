using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Parser3._0
{
    static public class BinConvert
    {        
        static public void WriteToFile(Settings settings)
        {
            BinaryWriter writer = new BinaryWriter(new FileStream(@"dbsettings.bin", FileMode.Create));
            
            writer.Write(settings.Server);
            writer.Write(settings.Database);
            writer.Write(settings.Uid);
            writer.Write(settings.Pwd);
            writer.Write(settings.charset);
            writer.Close();
        }
        static public Settings ReadFromFile()
        {
            try
            {
                Settings settings = new Settings();
                BinaryReader reader = new BinaryReader(new FileStream(@"dbsettings.bin", FileMode.Open));

                settings.Server = reader.ReadString();
                settings.Database = reader.ReadString();
                settings.Uid = reader.ReadString();
                settings.Pwd = reader.ReadString();
                settings.charset = reader.ReadString();

                reader.Close();
                return settings;
            }
            catch 
            {
                Menu menu;
                Settings settings = new Settings();
                dbSettings dbSettings = new dbSettings();
                if(dbSettings.ShowDialog() == DialogResult.OK)
                {
                    return DBManager.setting;                    
                }
                else { return settings; menu.Close(); }
            }
        }
    }
}
