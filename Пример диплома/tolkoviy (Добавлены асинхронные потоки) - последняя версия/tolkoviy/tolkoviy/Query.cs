using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tolkoviy
{
    class Query
    {
        public void put_value(string file_path,string constring, string file_name) //запись данных в таблицы
        {
            Streamfs strf = new Streamfs();
            Regular rg = new Regular();
            List<string> insert_list_word = new List<string>();
            List<string> insert_list_desc = new List<string>();
            var stream_f = strf.stream_file(file_path);
            foreach (var i in rg.regular(stream_f))
            {
                insert_list_word.Add(i.word);
                insert_list_desc.Add(i.description);
            }

            SqlConnection con = new SqlConnection(constring);
            con.Open();
            try
            {
                string select_idw = "select count (*) from table_word";
                SqlCommand sc_idw = new SqlCommand(select_idw, con);
                int idw = Convert.ToInt32(sc_idw.ExecuteScalar());

                if (idw == 0) // поиск id таблицы word
                    idw = 1;
                else idw += 1;
                int temp_idw = idw; //чтобы не было инкремента на следующие таблицы
                string select_idd = "select count (*) from table_dictionary";
                SqlCommand sc_idd = new SqlCommand(select_idd, con);
                int idd = Convert.ToInt32(sc_idd.ExecuteScalar());
                int temp_idd = idd; // чтобы не было инкремента на следующие таблицы
                if (idd == 0) // поиск id таблицы dictionary
                    temp_idd = 1;
                string select_iddw = "select count (*) from table_description_word";
                SqlCommand sc_iddw = new SqlCommand(select_iddw, con);
                int iddw = Convert.ToInt32(sc_iddw.ExecuteScalar());
                if (iddw == 0) // поиск id таблицы table_discription_word
                    iddw = 1;
                else iddw += 1;

                foreach (var i in insert_list_word)
                {
                    string str = "INSERT INTO[dbo].[table_word]"
                                + "([id]"
                                + ",[dict_id]"
                                + ",[word])"
                                + "VALUES"
                                + "(" + temp_idw
                                + "," + temp_idd
                                + ",'" + i.ToString() + "')";
                    SqlCommand myCommand = new SqlCommand(str, con);
                    try
                    {
                        myCommand.ExecuteNonQuery();
                    }
                    catch(SqlException ex)
                    {
                        Console.WriteLine(str + ":::: " + ex.ToString());
                    }
                    temp_idw++;
                }
                foreach (var i in insert_list_desc)
                {
                    string str = "INSERT INTO[dbo].[table_description_word]"
                                + "([id]"
                                + ",[word_id]"
                                + ",[dict_id]"
                                + ",[description])"
                                + "VALUES"
                                + "(" + iddw
                                + "," + idw
                                + "," + temp_idd
                                + ",'" + i.ToString() + "')";
                    SqlCommand myCommand = new SqlCommand(str, con);
                    myCommand.ExecuteNonQuery();
                    idw++;
                    iddw++;
                }
                string select_name_dict = "select name from table_dictionary";  //сравнение с существующим именем словаря
                List<string> name_dict = new List<string>();
                bool mycheck_filename = true;
                SqlCommand sc_name_dict = new SqlCommand(select_name_dict, con);
                using (SqlDataReader reader = sc_name_dict.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        name_dict.Add(reader.GetString(0).Trim());
                    }
                }
                foreach (var i in name_dict)
                {
                    if (i.ToString() == file_name.ToString())
                    {
                        mycheck_filename = false;
                        break;
                    }
                }
                if (mycheck_filename || idd == 0)
                {
                    //if (idd != 1)
                        idd++;
                    string ins_dic = "INSERT INTO[dbo].[table_dictionary]"
                                    + "([id]"
                                    + ",[name])"
                                     + "VALUES"
                                    + "(" + idd
                                    + ",'" + file_name + "')";
                    SqlCommand cmd_ins_dic = new SqlCommand(ins_dic, con);
                    cmd_ins_dic.ExecuteNonQuery();
                }
                con.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        public void clear_tables(string constring) //очистка содержимого таблиц
        {
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            try
            {
                string delete_word = 
                    "TRUNCATE TABLE table_word; ";
                string delete_dict =
                    " TRUNCATE TABLE table_dictionary; ";
                string delete_desc_word =
                    " TRUNCATE TABLE table_description_word; ";
                SqlCommand cmd_del = new SqlCommand(delete_word, con);
                cmd_del.ExecuteNonQuery();
                cmd_del = new SqlCommand(delete_dict, con);
                cmd_del.ExecuteNonQuery();
                cmd_del = new SqlCommand(delete_desc_word, con);
                cmd_del.ExecuteNonQuery();
                con.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
