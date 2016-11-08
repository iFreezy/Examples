using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace tolkoviy
{
    class Regularlist //для списка, где будут храниться слова и их описание
    {
        public string word { get; set; }
        public string description { get; set; }
        public Regularlist(string _word, string _description)
        {
            this.word = _word;
            this.description = _description;
        }
        public string Getinfo()
        {
            return String.Format(word + description);
        }
        
    }
}
