using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using SD = System.Data;
using MySql.Data.MySqlClient;
using System.Text;

namespace MySQLOnline
{
    public partial class Form1 : Form
    {
        private static TcpClient? client;
        private static NetworkStream? stream;
        private static string dbName = Settings1.Default.dbName; //mytestcompany
        private static string login = Settings1.Default.login; //root
        private static string password = Settings1.Default.password; //GLEBPC-X666
        public static MySqlConnection? mycon;
        public static MySqlCommand? mycom;
        public static readonly string connect = $"Server=localhost;Database={dbName};Uid={login};pwd={password};charset=utf8;";
        public static SD.DataSet? ds;

        public static SD.DataTable performQuery(string query)
        {
            if (query == null)
            {
                SD.DataTable errorTable = new SD.DataTable();
                SD.DataColumn dataColumn = new("error", typeof(string));
                errorTable.Columns.Add(dataColumn);
                SD.DataRow dataRow = errorTable.NewRow();
                dataRow[0] = "Error: null query";
                errorTable.Rows.Add(dataRow);
                return errorTable;
            }
            try
            {
                mycon = new MySqlConnection(connect);
                mycon.Open();

                SD.DataTable table = new SD.DataTable();
                MySqlDataAdapter msql_dt = new MySqlDataAdapter(query, mycon);
                msql_dt.Fill(table);

                mycon.Close();
                return table;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                SD.DataTable errorTable = new SD.DataTable();
                SD.DataColumn dataColumn = new("error", typeof(string));
                errorTable.Columns.Add(dataColumn);
                SD.DataRow dataRow = errorTable.NewRow();
                dataRow[0] = "Error: DB connection error";
                errorTable.Rows.Add(dataRow);
                return errorTable;
            }
        }

        private static string tableToString(SD.DataTable table)
        {
            if (table == null)
            {
                return "null table";
            }

            string str = new string("");

            foreach (SD.DataRow dataRow in table.Rows)
            {
                foreach (var item in dataRow.ItemArray)
                {
                    str += (item + "\t");
                }
                str += "\n";
            }

            return str;
        }
        public static SD.DataTable convertStringToDataTable(string data)
        {
            SD.DataTable dataTable = new SD.DataTable();
            bool columnsAdded = false;
            foreach (string row in data.Split('$'))
            {
                SD.DataRow dataRow = dataTable.NewRow();
                foreach (string cell in row.Split('|'))
                {
                    string[] keyValue = cell.Split('~');
                    if (!columnsAdded)
                    {
                        SD.DataColumn dataColumn = new SD.DataColumn(keyValue[0]);
                        dataTable.Columns.Add(dataColumn);
                    }
                    dataRow[keyValue[0]] = keyValue[1];
                }
                columnsAdded = true;
                dataTable.Rows.Add(dataRow);
            }
            return dataTable;
        }

        public static SD.DataTable stringToTable(string data)
        {
            SD.DataTable dataTable = new SD.DataTable("tableName");

            SD.DataColumn ID = new("ID", typeof(int));
            SD.DataColumn user_name = new SD.DataColumn("user_name", typeof(string));
            SD.DataColumn user_login = new SD.DataColumn("user_login", typeof(string));
            SD.DataColumn user_pwd = new SD.DataColumn("user_pwd", typeof(string));
            SD.DataColumn row_hash = new SD.DataColumn("row_hash", typeof(string));

            var rows = data.Split('\n');
            int rowsNumber = rows.Length;

            //var res = rows[0];
            //int columnNumber = res.Split('\t').Length;

            SD.DataRow[] dataRows = new SD.DataRow[rowsNumber];
            for (int i = 0; i < rowsNumber; i++)
            {
                dataRows[i] = dataTable.NewRow();
                //SD.DataRow row1 = dataTable.NewRow();
                var columns = rows[i].Split('\t');

                dataRows[i]["ID"] = columns[0];
                dataRows[i]["user_name"] = columns[1];
                dataRows[i]["user_login"] = columns[2];
                dataRows[i]["user_pwd"] = columns[3];
                dataRows[i]["row_hash"] = columns[4];
                
                dataTable.Rows.Add(dataRows[i]);
            }
            
            return dataTable;
        }
        public static SD.DataSet createDataSet(string data)
        {
            SD.DataSet dataBase = new("dbName");

            SD.DataTable dataTable = new("tableName");

            SD.DataColumn ID = new("ID", typeof(int));
            ID.Unique = true;
            ID.AutoIncrement = true;
            ID.AutoIncrementSeed = 1;
            ID.AutoIncrementStep = 1;
            SD.DataColumn Name = new("Name", typeof(string));
            SD.DataColumn Surname = new("Surname", typeof(string));
            SD.DataColumn Age = new("Age", typeof(int));

            SD.DataRow row1 = dataTable.NewRow();
            row1["Name"] = "John";
            row1["Surname"] = "Hamilton";
            row1["Age"] = 23;

            SD.DataRow row2 = dataTable.NewRow();
            row2[1] = "Mary";
            row2[2] = "Smirko";
            row2[3] = 19;

            dataTable.Rows.Add(row1);
            dataTable.Rows.Add(row2);

            dataBase.Tables.Add(dataTable);

            return dataBase;
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            toolStripTextBox1.Text = login;
            toolStripTextBox2.Text = password;
            toolStripTextBox3.Text = dbName;
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {


        }

        private void toolStripTextBox2_TextChanged(object sender, EventArgs e)
        {
            password = toolStripTextBox2.Text;
            
        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            login = toolStripTextBox1.Text;
        }

        private void toolStripTextBox3_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripTextBox3_TextChanged(object sender, EventArgs e)
        {
            dbName = toolStripTextBox3.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (stream == null || client == null)
            {
                return;
            }
            // отправить логи для входа в бд также
            string request = textBox1.Text;
            byte[] bytesWrite = Encoding.ASCII.GetBytes(request); // конвертировали запрос для отправки

            stream.Write(bytesWrite, 0, bytesWrite.Length);  // отправляем, с 0 эл-та, длинны bytesWrite.Length
                                                             //stream.Flush();
                                                             //Console.WriteLine("Client sent request.");
            string answer = string.Empty;
   
               if (request != null)
               {
                                     
                   byte[] bytesRead = new byte[1024];
                   int length = stream.Read(bytesRead, 0, bytesRead.Length); 
                   // запись полученного ответа в bytesRead, подсчет длинны bytesRea

                   answer = Encoding.UTF8.GetString(bytesRead, 0, length);
               }
               else
               {
                   stream.Close();
                   Console.WriteLine("\nNull request");
                   Console.WriteLine("\nClient closed");
               }
            
            client.Close();

            dataGridView1.DataSource = convertStringToDataTable(answer);
            textBox2.Text = answer;
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {            
            //try to connect
            try
            {
                client = new TcpClient("127.0.0.1", 7000);
                //Console.WriteLine("Client connected");
                stream = client.GetStream();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}