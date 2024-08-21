using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;

class Program
{
    private static readonly string SERVER = "127.0.0.1";
    private static readonly int PORT = 13000;

    private static void ConnectServer(String server, int port)
    {
        Console.WriteLine("Connecting to Server....");
        Console.WriteLine("====================================");
        TcpClient client = new TcpClient(server, port);
        Console.Title = "Client Application";
        NetworkStream stream = client.GetStream();
        string message, responseData;
        var buffer = new byte[1024];
        int bytes;
        try
        {
            Console.Write("Username: ");
            var username = Console.ReadLine();
            Console.Write("Password: ");
            var password = Console.ReadLine();

            User u = new User()
            {
                Username = username,
                Password = password
            };
            SendData(stream, u, username);

            bytes = stream.Read(buffer, 0, buffer.Length);
            var loginResp = Encoding.ASCII.GetString(buffer, 0, bytes);
            Console.WriteLine(loginResp);
            LogActivity(username, loginResp);
            if (loginResp.Contains("successful"))
            {
                while (true)
                {
                    Console.Write("Input message (Press LogOut to log out): ");
                    message = Console.ReadLine();
                    if (message.ToLower().Equals("logout"))
                    {
                        Console.WriteLine("Logging out...");
                        Console.ReadLine();
                        break;
                    }
                    Byte[] data = Encoding.ASCII.GetBytes($"{message}");
                    stream = client.GetStream();
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine("{0} Sent: {1}", username, message);
                    LogActivity(username, $"Sent {message}");
                    //data = new Byte[1024];
                    //bytes = stream.Read(data, 0, data.Length);
                    //responseData = Encoding.ASCII.GetString(data, 0, bytes);
                    //Console.WriteLine("Received: {0}", responseData);                    L
                    //LogActivity(username, $"Received {message}");

                }
            }
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
            client.Close();

        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: {0}", e.Message);
        }
    }
    private static void SendData(NetworkStream stream, object data, string username)
    {
        var messageBytes = Encoding.ASCII.GetBytes(data.ToString());
        stream.Write(messageBytes, 0, messageBytes.Length);
        LogActivity(username, $"Sent: {data}");
    }
    private static void LogActivity(string username, string message)
    {
        string filename = $"{username}_log.txt";
        using (var writer = new StreamWriter(filename, true))
        {
            writer.WriteLine($"{DateTime.Now}: {message}");
        }
    }
    private static Connection? ConfigureConnectionCheck()
    {
        Console.Write("Enter Server: ");
        string server = Console.ReadLine();
        int port;
        while (true)
        {
            Console.Write("Enter Port: ");
            string input = Console.ReadLine();

            try
            {
                port = Convert.ToInt32(input);
                break;
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Input is too large or too small. Please enter a number within the valid range.");
            }
        }
        if (server.Equals(SERVER) && port == PORT)
        {

            return new Connection(server, port);
        }
        return null;
    }
    public static void Main(string[] args)
    {
        while (true)
        {
            var connection = ConfigureConnectionCheck();
            if (connection != null)
            {
                ConnectServer(connection.server, connection.port);
            }
            else
            {
                Console.WriteLine("Cannot connect to Server");
                Console.WriteLine("======================================");
            }
        }
    }
    public class Connection
    {
        public string server;
        public int port;

        public Connection(string server, int port)
        {
            this.server = server;
            this.port = port;
        }
    }
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
