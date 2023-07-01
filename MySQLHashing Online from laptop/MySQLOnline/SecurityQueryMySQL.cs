using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SD = System.Data;
using System.Security.Cryptography;
using MySQLOnline;

namespace SecurityQueryMySQL
{
    internal class SecurityQueryMySQL
    {
        public static MySqlConnection? mycon;
        public static MySqlCommand? mycom;
        public static string dbName = Settings1.Default.dbName; //mytestcompany
        public static string login = Settings1.Default.login; //root
        public static string password = Settings1.Default.password; //GLEBPC-X666
        public static string connect = string.Empty;
        public static SD.DataSet? ds;

        private static string tableToString(SD.DataTable table)
        {
            if (table == null)
            {
                return "null table";
            }

            string str = new("");

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
        public static string ToCSV(SD.DataTable table)
        {
            string str = string.Empty;
            //headers    
            for (int i = 0; i < table.Columns.Count; i++)
            {
                str += table.Columns[i];
                if (i < table.Columns.Count - 1)
                {
                    str += ",";
                }
            }
            str += '\n';

            foreach (SD.DataRow dr in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(','))
                        {
                            value = String.Format("\"{0}\"", value);
                            str += value;
                        }
                        else
                        {
                            str += dr[i].ToString();
                        }
                    }
                    if (i < table.Columns.Count - 1)
                    {
                        str += ",";
                    }
                }
                str += '\n';
            }
            return str;
        }

        public static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }


        public static string GenKey()
        {
            //generates 16chars of digits (64bits)
            Random rnd = new Random();
            string key = "";
            int seed = rnd.Next(1112, 9999);
            int rounds = rnd.Next(4, 12);

            for (int i = 0; i < rounds; i++)
            {
                seed = seed * seed;
                seed = (seed % 1000000) / 100;

                key = (seed).ToString() + key;
                if (key.Length > 16)
                    key = key.Remove(8); // удаляем все символы начиная с 8го
            }

            return key;
        }

        private static string GetMAC(HashAlgorithm hashAlgorithm, string input, string key) //H(k1,H(k2,m))
        {
            //H(k1,H(k2,m))
            string k1 = "";
            string k2 = "";
            for (int i = 0; i < key.Length / 2; i++)
            {
                k1 = k1 + key[i];
                k2 = k2 + key[i + key.Length / 2];
            }

            input = k2 + input;
            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            string hash = k1 + sBuilder.ToString();

            byte[] data2 = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(hash));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var suilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data2.Length; i++)
            {
                suilder.Append(data2[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return suilder.ToString();
        }

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
                connect = $"Server=GLEBPC;Database={dbName};Port=3306;Uid={login};pwd={password};charset=utf8;";
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

        public static SD.DataTable performSecQuery(string query)
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
                //mycon = new MySqlConnection(connect);
                //mycon.Open();

                SD.DataTable table = new SD.DataTable();

                query = modifyQuery(query);
                table = performQuery(query);

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

        public static SD.DataTable performSecQuery(string query, string key)
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
                //mycon = new MySqlConnection(connect);
                //mycon.Open();

                SD.DataTable table = new SD.DataTable();

                query = modifyQuery(query, key);
                table = performQuery(query);

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

        public static string modifyQuery(string query)
        {
            if (query == null) return null;
            try
            {
                string newQuery = new("");
                string[] substr = query.Split(' ');
                // 0 - select/insert/update/delete
                // 1 - column name
                // 2 - from 
                // 3 - table name
                // 4 - where 
                // 5,6,7 - condition (id = 3;)
                short flag = 0;

                switch (substr[0].ToUpper())
                {
                    case "SELECT":
                        {
                            //flag = 1;
                            //newQuery = newQuery + flag.ToString();
                            newQuery = newQuery + substr[0] + ' ';
                            newQuery = newQuery + '*' + ' ';
                            for (int i = 2; i < substr.Length; i++)
                                newQuery = newQuery + substr[i] + ' ';
                            //newQuery = newQuery + '~';
                            return newQuery;
                        }

                    case "INSERT":
                        {
                            //flag = 2;
                            //newQuery = newQuery + flag.ToString();

                            int position = 6, len = substr.Length - 1;
                            for (int i = 0; i < substr.Length; i++)
                            {
                                if (substr[i].ToUpper() == "VALUES")
                                    position = i - 1;
                                // - "user_pwd)"
                            }
                            string hash = "";

                            substr[position] = substr[position].TrimEnd(')') + ',' + " row_hash)"; // удаляем скобку, ставим запятую и продолжаем до row_hash)

                            substr[len] = substr[len].TrimEnd(')', ';') + ',';

                            for (int i = 0; i < substr.Length; i++)
                            {
                                newQuery = newQuery + substr[i] + ' ';
                                if (i > position + 1) hash += substr[i] + ' ';
                            }

                            hash = hash.Trim();
                            string[] subs = hash.Split(',');
                            hash = "";
                            for (int i = 0; i < subs.Length; i++)
                            {
                                hash = hash + subs[i].Trim(',', '(', ')', '\\', '"', ' ');
                            }
                            //Console.WriteLine(hash);
                            using (SHA256 sha256Hash = SHA256.Create())
                            {
                                hash = GetHash(sha256Hash, hash);
                            }

                            newQuery = newQuery + "\"" + hash + "\");";
                            return newQuery;
                        }

                    case "UPDATE":
                        {
                            //"UPDATE userlog SET user_pwd = "parol" WHERE id = 1;"
                            //"UPDATE userlog SET user_pwd = \"parol\", row_hash = \"~\" WHERE id = 1;"
                            //1) обновить данные
                            //2) пересчитать хэш для новой строки 
                            //3) обновить "row_hash = new hash"
                            //flag = 3;
                            string name = "";
                            string login = "";
                            string pwd = "";
                            newQuery = newQuery + flag.ToString();

                            int position = 6, len = substr.Length - 1;
                            for (int i = 0; i < substr.Length; i++)
                            {
                                switch (substr[i].ToUpper().Trim('"'))
                                {
                                    case "WHERE":
                                        {
                                            position = i - 1;
                                            break;
                                        }
                                    case "USER_NAME":
                                        {
                                            int j;
                                            for (j = i + 2; j < substr.Length; j++)
                                            {
                                                name = name + substr[j].Trim('"') + ' ';
                                                if (substr[j].EndsWith(',')) break;
                                            }
                                            i = j;
                                            name = name.Trim('"', '\\', ' ', ',');

                                            break;
                                        }
                                    case "USER_LOGIN":
                                        {
                                            int j;
                                            for (j = i + 2; j < substr.Length; j++)
                                            {
                                                login = login + substr[j].Trim('"') + ' ';
                                                if (substr[j].EndsWith(',')) break;
                                            }
                                            i = j;
                                            login = login.Trim('"', '\\', ' ', ',');

                                            break;
                                        }
                                    case "USER_PWD":
                                        {
                                            int j;
                                            for (j = i + 2; j < substr.Length; j++)
                                            {
                                                pwd = pwd + substr[j].Trim('"') + ' ';
                                                if (substr[j].EndsWith(',') || substr[j].EndsWith('"')) break;
                                            }
                                            i = j;
                                            pwd = pwd.Trim('"', '\\', ' ', ',');

                                            break;
                                        }
                                    default:
                                        break;
                                }
                                //if (substr[i].ToUpper() == "WHERE")
                                //    position = i - 1;

                                // - "parol"
                            }

                            string condition = new string("");
                            for (int i = position + 1; i < substr.Length; i++)
                            {
                                condition = condition + substr[i] + ' ';
                            }
                            //Console.WriteLine(condition);

                            string hash = "mynewhash";
                            string resTable = tableToString(performSecQuery("select * from userlog " + condition));
                            // тут надо проверить целостность
                            //14	Elena Maskova	elenam0110	strongqwerty123	966058a65fbfab345a8a3388274b24f4c318a10e320307600ce7279c0cf95642
                            string[] chop = resTable.Split('\t');

                            resTable = chop[1] + chop[2] + chop[3]; //name + login + password
                            if (name == "") name = chop[1];
                            if (login == "") login = chop[2];
                            if (pwd == "") pwd = chop[3];

                            using (SHA256 sha256Hash = SHA256.Create())
                            {
                                hash = GetHash(sha256Hash, resTable);
                            }

                            for (int i = 0; i < substr.Length; i++)
                            {
                                newQuery = newQuery + substr[i] + ' ';
                            }

                            newQuery = $"UPDATE userlog SET user_name = \"{name}\", user_login = \"{login}\", user_pwd = \"{pwd}\", row_hash = \"{hash}\" {condition}";
                            //Console.WriteLine(query);
                            //Console.WriteLine(newQuery);

                            return newQuery;
                        }

                    case "DELETE":
                        {
                            //flag = 4;
                            //newQuery = newQuery + flag.ToString();
                            for (int i = 0; i < substr.Length; i++)
                                newQuery = newQuery + substr[i] + ' ';
                            return newQuery;
                        }


                    default: return query;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return query;
            }
        }

        public static string modifyQuery(string query, string key)
        {
            if (query == null) return null;
            try
            {
                string newQuery = new("");
                string[] substr = query.Split(' ');
                // 0 - select/insert/update/delete
                // 1 - column name
                // 2 - from 
                // 3 - table name
                // 4 - where 
                // 5,6,7 - condition (id = 3;)
                short flag = 0;

                switch (substr[0].ToUpper())
                {
                    case "SELECT":
                        {
                            //flag = 1;
                            //newQuery = newQuery + flag.ToString();
                            newQuery = newQuery + substr[0] + ' ';
                            newQuery = newQuery + '*' + ' ';
                            int i = 2;
                            while(substr[i].ToUpper() != "FROM")
                            {
                                i++;
                            }
                            for (; i < substr.Length; i++)
                                newQuery = newQuery + substr[i] + ' ';
                            //newQuery = newQuery + '~';
                            return newQuery;
                        }

                    case "INSERT":
                        {
                            //flag = 2;
                            //newQuery = newQuery + flag.ToString();

                            int position = 6, len = substr.Length - 1;
                            for (int i = 0; i < substr.Length; i++)
                            {
                                if (substr[i].ToUpper() == "VALUES")
                                    position = i - 1;
                                // - "user_pwd)"
                            }
                            string hash = "";

                            substr[position] = substr[position].TrimEnd(')') + ',' + " row_hash)"; // удаляем скобку, ставим запятую и продолжаем до row_hash)

                            substr[len] = substr[len].TrimEnd(')', ';') + ',';

                            for (int i = 0; i < substr.Length; i++)
                            {
                                newQuery = newQuery + substr[i] + ' ';
                                if (i > position + 1) hash += substr[i] + ' ';
                            }

                            hash = hash.Trim();
                            string[] subs = hash.Split(',');
                            hash = "";
                            for (int i = 0; i < subs.Length; i++)
                            {
                                hash = hash + subs[i].Trim(',', '(', ')', '\\', '"', ' ');
                            }
                            //Console.WriteLine(hash);
                            using (SHA256 sha256Hash = SHA256.Create())
                            {
                                hash = GetMAC(sha256Hash, hash, key);
                            }

                            newQuery = newQuery + "\"" + hash + "\");";
                            return newQuery;
                        }

                    case "UPDATE":
                        {
                            //"UPDATE userlog SET user_pwd = "parol" WHERE id = 1;"
                            //"UPDATE userlog SET user_pwd = \"parol\", row_hash = \"~\" WHERE id = 1;"
                            //1) обновить данные
                            //2) пересчитать хэш для новой строки 
                            //3) обновить "row_hash = new hash"
                            //flag = 3;
                            string name = "";
                            string login = "";
                            string pwd = "";
                            newQuery = newQuery + flag.ToString();

                            int position = 6, len = substr.Length - 1;
                            for (int i = 0; i < substr.Length; i++)
                            {
                                switch (substr[i].ToUpper().Trim('"'))
                                {
                                    case "WHERE":
                                        {
                                            position = i - 1;
                                            break;
                                        }
                                    case "USER_NAME":
                                        {
                                            int j;
                                            for (j = i + 2; j < substr.Length; j++)
                                            {
                                                name = name + substr[j].Trim('"') + ' ';
                                                if (substr[j].EndsWith(',')) break;
                                            }
                                            i = j;
                                            name = name.Trim('"', '\\', ' ', ',');

                                            break;
                                        }
                                    case "USER_LOGIN":
                                        {
                                            int j;
                                            for (j = i + 2; j < substr.Length; j++)
                                            {
                                                login = login + substr[j].Trim('"') + ' ';
                                                if (substr[j].EndsWith(',')) break;
                                            }
                                            i = j;
                                            login = login.Trim('"', '\\', ' ', ',');

                                            break;
                                        }
                                    case "USER_PWD":
                                        {
                                            int j;
                                            for (j = i + 2; j < substr.Length; j++)
                                            {
                                                pwd = pwd + substr[j].Trim('"') + ' ';
                                                if (substr[j].EndsWith(',') || substr[j].EndsWith('"')) break;
                                            }
                                            i = j;
                                            pwd = pwd.Trim('"', '\\', ' ', ',');

                                            break;
                                        }
                                    default:
                                        break;
                                }
                                //if (substr[i].ToUpper() == "WHERE")
                                //    position = i - 1;

                                // - "parol"
                            }

                            string condition = new string("");
                            for (int i = position + 1; i < substr.Length; i++)
                            {
                                condition = condition + substr[i] + ' ';
                            }
                            //Console.WriteLine(condition);

                            string hash = "mynewhash";
                            string resTable = tableToString(performSecQuery("select * from userlog " + condition));
                            //14	Elena Maskova	elenam0110	strongqwerty123	966058a65fbfab345a8a3388274b24f4c318a10e320307600ce7279c0cf95642
                            string[] chop = resTable.Split('\t');

                            resTable = chop[1] + chop[2] + chop[3]; //name + login + password
                            if (name == "") name = chop[1];
                            if (login == "") login = chop[2];
                            if (pwd == "") pwd = chop[3];

                            using (SHA256 sha256Hash = SHA256.Create())
                            {
                                hash = GetMAC(sha256Hash, resTable, key);
                            }

                            for (int i = 0; i < substr.Length; i++)
                            {
                                newQuery = newQuery + substr[i] + ' ';
                            }

                            newQuery = $"UPDATE userlog SET user_name = \"{name}\", user_login = \"{login}\", user_pwd = \"{pwd}\", row_hash = \"{hash}\" {condition}";
                            //Console.WriteLine(query);
                            //Console.WriteLine(newQuery);

                            return newQuery;
                        }

                    case "DELETE":
                        {
                            //flag = 4;
                            //newQuery = newQuery + flag.ToString();
                            for (int i = 0; i < substr.Length; i++)
                                newQuery = newQuery + substr[i] + ' ';
                            return newQuery;
                        }


                    default: return query;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return query;
            }
        }

        private static string getHashFromAnswer(string table)
        {
            string hash = "";
            string[] item = table.Split('\t');
            for (int i = 1; i < item.Length - 2; i++)
            {
                hash += item[i];
            }
            using (SHA256 sha256Hash = SHA256.Create())
            {
                hash = GetHash(sha256Hash, hash);
            }
            return hash;
        }

       
        public static bool checkHashFromAnswer(string table)
        {
            string[] item = table.Split('\t');
            string hash = getHashFromAnswer(table);
            string hashS = item[item.Length - 2];
            if (hash == hashS)
                return true;
            return false;
        }

        public static bool checkMACFromAnswer(string table, string key)
        {
            string[] rows = table.Split('\n');
            string hashFromTable = string.Empty;
            string hashNewComp = string.Empty;
            for (int i = 1; i < rows.Length - 1; i++)
            {
                string[] cols = rows[i].Split(',');
                hashFromTable = cols[cols.Length - 1];
                string res = string.Empty;
                for(int j = 0; j < cols.Length - 1; j++)
                {
                    res += cols[j];
                }
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    hashNewComp = GetMAC(sha256Hash, res, key); //H(k1,H(k2,m))
                }
                if (hashFromTable == hashNewComp)
                    continue;
                else return false;
            }
            return true;
        }

        public static void refreshTable(string key)
        {
            string rowsInTable = new("SELECT COUNT(*) FROM userlog");
            string maxId = new("SELECT MAX(id) FROM userlog");

            int n = Convert.ToInt32(tableToString(performQuery(maxId)));

            for (int id = 1; id < n + 1; id++)
            {
                string query = new($"UPDATE userlog SET WHERE id = {id};");
                string check = tableToString(performQuery($" SELECT EXISTS(SELECT * FROM userlog WHERE id = {id})"));
                if (check == "1\t\n")
                    performSecQuery(query, key); // making update
            }
        }

    }

}

