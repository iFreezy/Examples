using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tolkoviy
{
    class Gridarray
    {
        public ArrayList grid_view(string query,string constring) //запрос на отображение в grid
        {
            ArrayList grid = new ArrayList();
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            SqlDataReader myReader = null;
            SqlCommand myCommand = new SqlCommand(query, con);
            myReader = myCommand.ExecuteReader();
            if (myReader.HasRows)
                foreach (var result in myReader)
                    grid.Add(result);
            con.Dispose();
            return grid;
        }
    }
}
