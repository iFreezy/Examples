using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tolkoviy
{
    class Streamfs
    {
        public string stream_file(string file_name) //выбор файла и считывание его в переменную
        {
            string reg;
            System.IO.StreamReader streamReader;
            streamReader = new System.IO.StreamReader(file_name,
              System.Text.Encoding.GetEncoding("windows-1251"));
            reg = streamReader.ReadToEnd();
            streamReader.Close();

            return reg;
        }
    }
}
