using System.Collections.ObjectModel;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace Assignment2;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private TcpClient client;
    private NetworkStream stream;
    private static string user;
    private ObservableCollection<string> Messages;
    public ObservableCollection<string> ReceiveMessageList;

    public MainWindow()
    {
        InitializeComponent();
        user = "";
        Messages = new ObservableCollection<string>();
        ReceiveMessageList = new ObservableCollection<string>();
        //listBoxMessages.ItemsSource = Messages;
        listBoxMessages.ItemsSource = ReceiveMessageList;

    }

    private async void ConnectButton_Click(object sender, RoutedEventArgs e)
    {
        string server = "127.0.0.1";
        int port = 13000;

        try
        {
            client = new TcpClient(server, port);
            stream = client.GetStream();

            string username = UsernameTextBox.Text;
            string password = PasswordTextBox.Text;
            User u = new User(username, password);
            await SendDataAsync(stream, u);

            string loginResp = await ReadDataAsync();
            var checkLogin = loginResp.Contains("successful");

            LogActivity($"Login Response: {loginResp}");
            lbLogin.Content = checkLogin ? "Connected Successfully !" : "Connected Failed !";
            if (checkLogin)
            {
                //_ = ReceiveMessagesAsync();
                user = username;
                MessageTextBox.IsEnabled = checkLogin;
                btnSend.IsEnabled = checkLogin;
                btnLoad.IsEnabled = checkLogin;
                await LoadMessages();
            }
            LogActivity(lbLogin.Content.ToString() ?? "");
        }
        catch (Exception ex)
        {
            LogActivity($"Exception: {ex.Message}");
        }
    }

    private async void SendButton_Click(object sender, RoutedEventArgs e)
    {
        if (client == null || !client.Connected) return;

        string message = $"{user}: {MessageTextBox.Text}";
        if (message.ToLower().Equals("logout"))
        {
            LogActivity("Logging out...");
            client.Close();
            return;
        }
        await SendDataAsync(stream, message);
        LogActivity($"Sent= {message}");
        Messages.Add(message);
        await LoadMessages();
    }

    private async Task LoadMessages()
    {
        await ReceiveMessagesAsync();
    }
    private async void Load_Click(object sender, RoutedEventArgs e)
    {
        await LoadMessages();
    }
    private static async Task SendDataAsync(NetworkStream stream, object data)
    {
        byte[] messageBytes = Encoding.ASCII.GetBytes(data.ToString());
        await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
        LogActivity(data.ToString() ?? "");
    }


    private async Task<string> ReadDataAsync()
    {
        byte[] buffer = new byte[1024];
        int bytes = await stream.ReadAsync(buffer, 0, buffer.Length);
        return Encoding.ASCII.GetString(buffer, 0, bytes);
    }
    private async Task ReceiveMessagesAsync()
    {
        while (client.Connected)
        {
            try
            {
                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string receivedMessages = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                ReceiveMessageList.Add(receivedMessages);

            }
            catch (Exception ex)
            {
                LogActivity(ex.Message);
                break;
            }
        }
    }
    private static void LogActivity(string message)
    {
        using (var writer = new StreamWriter($"{user}_log.txt", true))
        {
            writer.WriteLine($"{DateTime.Now}: {message}");
        }
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

        public override string? ToString()
        {
            return $"{Username}/{Password}";
        }
    }
}
