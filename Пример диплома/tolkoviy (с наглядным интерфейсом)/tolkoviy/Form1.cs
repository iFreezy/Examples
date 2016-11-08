using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

//TO DO
//Сделать асинхронный поток для добавления данных в бд
namespace tolkoviy
{
    public partial class Form1 : Form
    {
        private string file_path;
        private string file_name;
        private string constring = "Data Source=DESKTOP-VSSRJ2K;Initial Catalog=tolkoviy_db;Integrated Security=True";
        public Form1()
        {
            InitializeComponent();
            textBox3.Text = "select * from table_word"; //текстовое поле с запросами
            List<Querybox> querybox = new List<Querybox>
            {
                new Querybox { Name="Таблица Слов", Query="select * from table_word"},
                new Querybox { Name="Таблица Описания Слов", Query="select * from table_description_word"},
                new Querybox { Name="Таблица Словарей", Query="select * from table_dictionary"},
           
            };
            comboBox1.DataSource = querybox;
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "Query";
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
        } 
        private void button1_Click(object sender, EventArgs e) //выполнить запрос к БД
        {
            string query = textBox3.Text; 
            Gridarray gridarr = new Gridarray();
            try
            {
                dataGridView1.DataSource = gridarr.grid_view(query, constring);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void button2_Click(object sender, EventArgs e) //загрузить файл
        {
            Streamfs strf = new Streamfs();
            Regular rg = new Regular();
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    file_path = openFileDialog1.FileName; //путь к файлу
                    file_name = openFileDialog1.SafeFileName; //имя файла
                    string stream_f = strf.stream_file(file_path);
                    textBox1.Text = stream_f; //изначальный текст
                    foreach (var i in rg.regular(stream_f))
                    {
                        textBox2.Text += i.Getinfo(); //отформатированный текст
                    }
                }
                button3.Enabled = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            button2.Enabled = true;
        }
        private void button3_Click(object sender, EventArgs e) // загрузить в БД
        {
            Query que = new Query();
            que.put_value(file_path, constring, file_name);
        }
        
        private void button4_Click(object sender, EventArgs e) //очистить таблицы
        {
            Query que = new Query();
            que.clear_tables(constring);
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Querybox querybox = (Querybox)comboBox1.SelectedItem;
            textBox3.Text = querybox.Query;
        }
    }
}
