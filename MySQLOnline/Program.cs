using SD = System.Data;
using MySql.Data.MySqlClient;
using System.Text;

namespace MySQLOnline
{
    internal static class Program
    {
        

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}

/*static void Main(string[] args)
        {
            try
            {
                TcpClient client = new TcpClient("127.0.0.1", 7000);
                Console.WriteLine("Client connected");

                NetworkStream stream = client.GetStream();

                while (true)
                {

                    Console.WriteLine("Hello, enter code:\n1 to send request\n0 to close connection");
                    string flag = Console.ReadLine().ToString();

                    if (flag == "1")
                    {
                        Console.Write("MySql: ");
                        string request = Console.ReadLine().ToString();
                        request = Class1.modifyQuery(request);

                        if (request != null)
                        {

                            byte[] bytesWrite = Encoding.ASCII.GetBytes(request); // конвертировали запрос для отправки

                            stream.Write(bytesWrite, 0, bytesWrite.Length);  // отправляем, с 0 эл-та, длинны bytesWrite.Length
                            //stream.Flush();
                            //Console.WriteLine("Client sent request.");

                            byte[] bytesRead = new byte[1024];
                            int length = stream.Read(bytesRead, 0, bytesRead.Length); // запись полученного ответа в bytesRead, подсчет длинны bytesRead


                            string answer = Encoding.UTF8.GetString(bytesRead, 0, length);

                            //if (verifyHash(answer)) Console.WriteLine("Answer is:\n" + answer);
                            //else
                            //{
                            //    Console.WriteLine("Repit request");
                            //    //Console.WriteLine("Enter 3 to repit request: ");
                            //    //string repit = Console.ReadLine().ToString();
                            //
                            //    client = new TcpClient("127.0.0.1", 7000);
                            //    stream = client.GetStream();
                            //
                            //    stream.Write(bytesWrite, 0, bytesWrite.Length);  // отправляем, с 0 эл-та, длинны bytesWrite.Length
                            //                                                     //stream.Flush();
                            //
                            //    bytesRead = new byte[1024];
                            //    length = stream.Read(bytesRead, 0, bytesRead.Length); // запись полученного ответа в bytesRead, подсчет длинны bytesRead
                            //    answer = Encoding.UTF8.GetString(bytesRead, 0, length);
                            //
                            //    if (verifyHash(answer)) Console.WriteLine(answer + "\nGOOOOD");
                            //    else Console.WriteLine("bad");
                            //
                            //}

                        }
                        else
                        {
                            stream.Close();
                            Console.WriteLine("\nNull request");
                            Console.WriteLine("\nClient closed");
                        }
                    }
                    if (flag == "0" || flag == null || flag != "1" || flag != "2" || flag != "3")
                        break;
                }
                client.Close();
                Console.WriteLine("\nClient closed");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }
*/