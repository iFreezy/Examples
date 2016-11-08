using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace tolkoviy
{
    class Regular
    {
        public List<Regularlist> regular(string reg) //регулярка
        {
            List<Regularlist> reglist = new List<Regularlist>();
            string pattern = @"([А-ЯA-Z]+),\s*(.*)\n";

            Regex cut_word = new Regex(pattern);
            MatchCollection matches = cut_word.Matches(reg);
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    reglist.Add(new Regularlist(match.Groups[1].ToString(), match.Groups[2].ToString()));
                }
            }
            else
            {
                reglist.Add(new Regularlist("Нет совпадений", ""));
            }

            return reglist;
        }
    }
}
