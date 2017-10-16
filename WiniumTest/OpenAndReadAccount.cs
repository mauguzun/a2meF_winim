using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WiniumTest
{
    public class OpenAndReadAccount
    {
        private List<Account> _acc;
        string _filename = "acc.xml";

        public List<Account> Load()
        {
            try
            {
                using (FileStream fs = new FileStream(this._filename, FileMode.OpenOrCreate))
                {
                    XmlSerializer formatter = new XmlSerializer(typeof(List<Account>));
                    _acc = (List<Account>)formatter.Deserialize(fs);
                    return _acc;
                }
            }
            catch
            {
                return null;
            }
           
        }
        public void Save()
        {
            try
            {
                using (FileStream fs = new FileStream(this._filename, FileMode.OpenOrCreate))
                {
                    XmlSerializer formatter = new XmlSerializer(typeof(List<Account>));
                    formatter.Serialize(fs, this._acc);
                }
            }
            catch
            {

            }
          
        }

        public void Save(List<Account> list)
        {
            try
            {
                using (FileStream fs = new FileStream(this._filename, FileMode.OpenOrCreate))
                {
                    XmlSerializer formatter = new XmlSerializer(typeof(List<Account>));
                    formatter.Serialize(fs, list);
                }
            }
            catch
            {

            }

        }
    }
}
