using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using SD = System.Data;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;
using SecQ = SecurityQueryMySQL;
using System.Diagnostics;
using Google.Protobuf.WellKnownTypes;
using Org.BouncyCastle.Ocsp;
using Org.BouncyCastle.Asn1.Ocsp;
using static Mysqlx.Expect.Open.Types.Condition.Types;
using Mysqlx.Crud;

namespace MySQLOnline
{
    public partial class Form1 : Form
    {
        private static TcpClient? client;
        private static NetworkStream? stream;
        //private static extern string dbName = Settings1.Default.dbName; //mytestcompany
        //private static string login = Settings1.Default.login; //root
        //private static string password = Settings1.Default.password; //GLEBPC-X666
        public static MySqlConnection? mycon;
        public static MySqlCommand? mycom;
        //public static readonly string connect = $"Server=localhost;Database={dbName};Uid={login};pwd={password};charset=utf8;";
        public static SD.DataSet? ds;
        public readonly string key = "38769316";

        public static SD.DataTable CSVtoTable(string data)
        {
            SD.DataTable dataTable = new SD.DataTable();
            bool columnsAdded = false;
            string[] rows = data.Split('\n');
            string[] headers = rows[0].Split(",");

            for (int i = 1; i < rows.Length; i++)
            {
                SD.DataRow dataRow = dataTable.NewRow();
                int j = 0;
                foreach (string cell in rows[i].Split(','))
                {
                    if (!columnsAdded)
                    {
                        SD.DataColumn dataColumn = new SD.DataColumn(cell);
                        dataTable.Columns.Add(dataColumn);
                    }
                    dataRow[cell] = headers[j];
                    j++;
                }
                columnsAdded = true;
                dataTable.Rows.Add(dataRow);
            }
            return dataTable;
        }
        public static SD.DataTable ConvertCsvToDataTable(string str)
        {
            //reading all the lines(rows) from the file.
            string[] rows = str.Split('\n');

            DataTable dtData = new DataTable();
            string[] rowValues = null;
            DataRow dr = dtData.NewRow();

            //Creating columns
            if (rows.Length > 0)
            {
                foreach (string columnName in rows[0].Split(','))
                    dtData.Columns.Add(columnName);
            }

            //Creating row for each line.(except the first line, which contain column names)
            for (int row = 1; row < rows.Length; row++)
            {
                rowValues = rows[row].Split(',');
                dr = dtData.NewRow();
                dr.ItemArray = rowValues;
                dtData.Rows.Add(dr);
            }

            return dtData;
        }
        public static SD.DataTable ConvertCsvToDataTable(string answer, string request)
        {
            SD.DataTable dataTable = ConvertCsvToDataTable(answer);
            if (request.Contains('*')) return dataTable;

            string[] allHeaders = answer.Split('\n')[0].Split(',');
            string[] requestSubstr = request.Split(' ');
            foreach (string cell in requestSubstr)
            {
                cell.Trim(',');
            }
            if (requestSubstr[0].ToUpper() == "SELECT")
            {
                int i = 1;
                // ищем столбцы, которые мы хотим оставить
                List<string> headers = new List<string>();
                while (requestSubstr[i].ToUpper() != "FROM")
                {

                    headers.Add(requestSubstr[i].Trim(','));
                    i++;
                }

                // удалить ненужные столбцы
                DataColumnCollection columns = dataTable.Columns;
                foreach (string headName in allHeaders)
                {
                    if (!headers.Contains(headName))
                    {
                        if (columns.Contains(headName))
                            if (columns.CanRemove(columns[headName]))
                            {
                                dataTable.Columns.Remove(columns[headName]);
                                //columns.Remove(headName);
                            }
                    }

                }
            }

            return dataTable;
        }

        public Form1()
        {
            InitializeComponent();
            InitializeDataGridView();
        }
        private void InitializeDataGridView()
        {
            try
            {
                // Set up the DataGridView.
                //dataGridView1.Dock = DockStyle.Fill;

                // Automatically generate the DataGridView columns.
                dataGridView1.AutoGenerateColumns = true;

                // Automatically resize the visible rows.
                dataGridView1.AutoSizeRowsMode =
                    DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;

                // Set the DataGridView control's border.
                dataGridView1.BorderStyle = BorderStyle.Fixed3D;

                // Put the cells in edit mode when user enters them.
                dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            toolStripTextBox1.Text = Settings1.Default.login;
            toolStripTextBox2.Text = Settings1.Default.password;
            toolStripTextBox2.TextBox.UseSystemPasswordChar = true;
            toolStripTextBox3.Text = Settings1.Default.dbName;
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox2_TextChanged(object sender, EventArgs e)
        {
            SecQ.SecurityQueryMySQL.password = toolStripTextBox2.Text;

        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            SecQ.SecurityQueryMySQL.login = toolStripTextBox1.Text;
        }

        private void toolStripTextBox3_Click(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox3_TextChanged(object sender, EventArgs e)
        {
            SecQ.SecurityQueryMySQL.dbName = toolStripTextBox3.Text;
        }

        public void PerfomSecQueryForm(string request)
        {
            string answer = SecQ.SecurityQueryMySQL.ToCSV(SecQ.SecurityQueryMySQL.performSecQuery(request, key));
            if (answer != null && answer != "")
            {
                label2.Visible = false;
                if (SecQ.SecurityQueryMySQL.checkMACFromAnswer(answer, key))
                {
                    Console.WriteLine("god");
                    //textBox1.BackColor = Color.White;
                    dataGridView1.DataSource = ConvertCsvToDataTable(answer, request);
                }
                else
                {
                    //textBox1.Text = "ERROR! Integrity has been violated!";
                    label2.Visible = true;
                    //textBox1.BackColor = Color.Red;
                    Console.WriteLine("evil");
                    dataGridView1.DataSource = ConvertCsvToDataTable(answer, request);
                }
            }
        }
        public void PerfomQueryForm(string request)
        {
            dataGridView1.DataSource = SecQ.SecurityQueryMySQL.performQuery(request);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                string request = textBox1.Text;
                if (string.IsNullOrEmpty(request) || string.IsNullOrWhiteSpace(request)) return;
                PerfomSecQueryForm(request);
                //string answer = SecQ.SecurityQueryMySQL.ToCSV(SecQ.SecurityQueryMySQL.performQuery(request));
                //string answer = SecQ.SecurityQueryMySQL.ToCSV(SecQ.SecurityQueryMySQL.performSecQuery(request, key));
                //if (answer != null && answer != "")
                //{
                //    dataGridView1.DataSource = ConvertCsvToDataTable(answer, request);
                //    if (SecQ.SecurityQueryMySQL.checkMACFromAnswer(answer, key))
                //    {
                //        Console.WriteLine("god");
                //        textBox1.BackColor = Color.White;
                //    }
                //    else
                //    {
                //        //textBox1.Text = "ERROR! Integrity has been violated!";
                //        textBox1.BackColor = Color.Red;
                //        Console.WriteLine("evil");
                //    }
                //}

            }
            catch (Exception ex)
            {
                if (stream != null) stream.Close();

                MessageBox.Show("Error connection or request:" + ex.Message, "Error", MessageBoxButtons.OK);
                //Console.WriteLine(ex.Message);
                //Console.WriteLine("\nNull request");
                //Console.WriteLine("\nClient closed");
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            Random random = new Random();
            Stopwatch stopwatch = new Stopwatch();

            //SELECT

            // // засекаем врем€ начала SecQuery операции
            // stopwatch.Start();     
            // for (int i = 1; i < 50; i++)
            // {
            //     // функци€ с модификацией запроса и проверкой целостности
            //     PerfomSecQueryForm($"select user_login, user_pwd from userlog where id < {i}");
            // }
            // // останавливаем счЄтчик
            // stopwatch.Stop();
            // var sec1 = stopwatch.ElapsedMilliseconds;
            // 
            // // засекаем врем€ начала Query операции
            // stopwatch = Stopwatch.StartNew();
            // for (int i = 1; i < 50; i++)
            // {
            //     // функци€ без модификаций запросов и каких-либо проверок
            //     PerfomQueryForm($"select user_login, user_pwd from userlog where id < {i}");
            // }
            // // останавливаем счЄтчик
            // stopwatch.Stop();
            // var sec2 = stopwatch.ElapsedMilliseconds;
            // 
            // // вывод результатов в MessageBox
            // MessageBox.Show($"SELECT: \nSecQuery = {sec1}ms. \nQuery = {sec2}ms.", "Test completed", MessageBoxButtons.OK);


            // INSERT

            // засекаем врем€ начала SecQuery операции
            // stopwatch = Stopwatch.StartNew();
            // 
            // string name = NameData.Names[random.Next(NameData.Names.Count)] + NameData.Surnames[random.Next(NameData.Surnames.Count)];
            // string login = name + random.NextInt64(10000).ToString();
            // string pwd = login + "QWERTY" + random.Next(1000).ToString();
            // 
            // PerfomSecQueryForm($"insert into userlog (user_name, user_login, user_pwd) " +
            //     $"values (\"{name}\", \"{login}\", \"{pwd}\");");
            // 
            // //останавливаем счЄтчик
            // stopwatch.Stop();
            // var sec1 = stopwatch.ElapsedMilliseconds;
            // 
            // // засекаем врем€ начала Query операции
            // stopwatch = Stopwatch.StartNew();
            // 
            // name = NameData.Names[random.Next(NameData.Names.Count)] + NameData.Surnames[random.Next(NameData.Surnames.Count)];
            // login = name + random.NextInt64(10000).ToString();
            // pwd = login + "QWERTY" + random.Next(1000).ToString();
            // 
            // PerfomQueryForm($"insert into userlog (user_name, user_login, user_pwd, row_hash) " +
            //     $"values (\"{name}\", \"{login}\", \"{pwd}\", \"some mac i\");");
            // 
            // //останавливаем счЄтчик
            // stopwatch.Stop();
            // var sec2 = stopwatch.ElapsedMilliseconds;
            // 
            // // вывод результатов в MessageBox
            // MessageBox.Show($"INSERT: \nSecQuery = {sec1}ms. \nQuery = {sec2}ms.", "Test completed", MessageBoxButtons.OK);

            //UPDATE

            // засекаем врем€ начала SecQuery операции
            // stopwatch.Start();
            // for (int i = 30; i < 50; i++)
            // {
            //     // изменим €чейку с паролем
            //     string pwd = "123qwerty" + (i*10).ToString();
            //     // функци€ с модификацией запроса и проверкой целостности
            //     PerfomSecQueryForm($"update userlog set user_pwd = \"{pwd}\" where id = {i};");
            // }
            // // останавливаем счЄтчик
            // stopwatch.Stop();
            // var sec1 = stopwatch.ElapsedMilliseconds;
            // 
            // // засекаем врем€ начала Query операции
            // stopwatch = Stopwatch.StartNew();
            // for (int i = 30; i < 50; i++)
            // {
            //     // изменим €чейку с паролем
            //     string pwd = "123qwerty" + (i * 10).ToString();
            //     // функци€ без модификаций запросов и каких-либо проверок
            //     PerfomQueryForm($"update userlog set user_pwd = \"{pwd}\" where id = {i};");
            // }
            // // останавливаем счЄтчик
            // stopwatch.Stop();
            // var sec2 = stopwatch.ElapsedMilliseconds;
            // MessageBox.Show($"UPDATE: \nSecQuery = {sec1}ms. \nQuery = {sec2}ms.", "Test completed", MessageBoxButtons.OK);

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                SecQ.SecurityQueryMySQL.refreshTable(key);
                stopwatch.Stop();
                var sec1 = stopwatch.ElapsedMilliseconds;
                MessageBox.Show($"Table has been successfuly refreshed!\n Time = {sec1}ms", "Refresh table", MessageBoxButtons.OK);

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}



