using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using static Program;

class Program
{
    private static readonly Dictionary<string, string> Users = new Dictionary<string, string>()
    {
        {"test","test"},
        {"test1","test1"},
        {"test2","test2"},
        {"test3","test3"},
        {"test4","test4"},
        {"test5","test5"},
        {"test6","test6"},
        {"cap","123" },

    };
    private static List<Message> messages = new();
    static int countUser = 0;

    private static async Task<bool> AuthenticateAsync(NetworkStream stream)
    {
        byte[] bytes = new byte[1024];
        string username = "";
        string password = "";
        User user;
        int countByte;
        try
        {
            countByte = await stream.ReadAsync(bytes, 0, bytes.Length);
            var userString = Encoding.ASCII.GetString(bytes, 0, countByte);
            user = GetUser(userString);
            username = user.Username;
            password = user.Password;
            if (Users.ContainsKey(username) && Users[username] == password)
            {
                LogActivity(username, $"User {username} logged in successfully.");
                var successMessage = Encoding.ASCII.GetBytes("Login successful!\n");
                await stream.WriteAsync(successMessage, 0, successMessage.Length);
                Console.WriteLine("Login Successful !");
                Console.WriteLine($"Number of client connected: {++countUser}");
                Console.WriteLine(new string('=', 40));
                return true;
            }
            LogActivity(username, $"Login attempt failed for user {username}.");
            Console.WriteLine("Login Failed");
            var msg = Encoding.ASCII.GetBytes("Login failed!\n");
            await stream.WriteAsync(msg, 0, msg.Length);
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Authentication error: {ex.Message}");
            LogActivity("", $"Authentication error: {ex.Message}");
            return false;
        }
    }


    private static async Task ProcessMessageAsync(TcpClient client)
    {
        var stream = client.GetStream();
        byte[] bytes = new byte[1024];
        string data;
        int count;
        try
        {
            if (!await AuthenticateAsync(stream))
            {
                client.Close();
                return;
            }
            await SendMessagesAsync(stream, messages);
            while ((count = await stream.ReadAsync(bytes, 0, bytes.Length)) != 0)
            {
                data = Encoding.ASCII.GetString(bytes, 0, count);
                Console.WriteLine($"{DateTime.Now} Received = {data} ");
                var message = SplitMessage(data);
                messages.Add(message);
                byte[] msg = Encoding.ASCII.GetBytes(data);
                await stream.WriteAsync(msg, 0, msg.Length);
                Console.WriteLine($" {DateTime.Now} Sent =  {data}");
            }
            client.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
            LogActivity("", $"Exception: {ex.Message}");
        }
    }
    private static async Task SendMessagesAsync(NetworkStream stream, List<Message> messages)
    {
        string serializedMessages = string.Join("\n", messages.ConvertAll(m => $"{m.user}: {m.message}"));
        byte[] messageBytes = Encoding.ASCII.GetBytes(serializedMessages);
        await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
    }
    private static void SendMessages(NetworkStream stream, List<Message> messages)
    {
        string serializedMessages = string.Join("\n", messages.ConvertAll(m => $"{m.user}: {m.message}"));
        using (var writer = new StreamWriter("server_messages.txt", true))
        {
            foreach (var message in messages)
            {
                writer.WriteLine($"{DateTime.Now}: {message}");
            }
        }
    }
    public static void Main()
    {
        string host = "127.0.0.1";
        int port = 13000;
        ExecuteServer(host, port).GetAwaiter().GetResult();
    }

    private static async Task ExecuteServer(string host, int port)
    {
        int count = 0;
        TcpListener server = null;
        try
        {
            Console.Title = "Server Application";
            IPAddress localAddr = IPAddress.Parse(host);
            server = new TcpListener(localAddr, port);
            server.Start();
            Console.WriteLine(new string('=', 40));
            Console.WriteLine("Waiting for a connection... ");
            while (true)
            {
                TcpClient client = await server.AcceptTcpClientAsync();

                ProcessMessageAsync(client);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            LogActivity("", $"Exception: {ex.Message}");
        }
        finally
        {
            server?.Stop();
            LogActivity("", "Server stopped.");
            Console.WriteLine("Server stopped. Press any key to exit.");
            Console.ReadKey();
        }
    }

    private static void LogActivity(string username, string message)
    {
        using (var writer = new StreamWriter("server_log.txt", true))
        {
            writer.WriteLine($"{DateTime.Now}: {username} {message}");
        }
    }
    private static User GetUser(string userString)
    {
        string s = userString;
        string[] parts = s.Split('/');
        string username = "";
        string password = "";
        if (parts.Length == 2)
        {
            username = parts[0];
            password = parts[1];
            //Console.WriteLine($"Username: {username}, Password: {password}");
        }
        else
        {
            Console.WriteLine("Invalid format");
        }
        return new User(username, password);
    }
    private static Message SplitMessage(string input)
    {
        string[] parts = input.Split(new[] { ':' }, 2);
        string username = "";
        string msg = "";
        if (parts.Length == 2)
        {
            username = parts[0].Trim();
            msg = parts[1].Trim();
        }
        else
        {
            Console.WriteLine("Invalid format");
        }
        return new Message(username, msg);
    }
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
    public class Message
    {
        public string user { get; set; }
        public string message { get; set; }
        public Message(string user, string message)
        {
            this.user = user;
            this.message = message;
        }
    }
}