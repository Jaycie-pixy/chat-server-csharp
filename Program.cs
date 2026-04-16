using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Server
{
    static TcpListener server;
    static List<TcpClient> clients = new List<TcpClient>();

    static async Task Main()
    {
        server = new TcpListener(IPAddress.Any, 5000);
        server.Start();

        Console.WriteLine("🚀 Server started on port 5000...");

        while (true)
        {
            TcpClient client = await server.AcceptTcpClientAsync();
            clients.Add(client);

            Console.WriteLine("✅ Client connected");

            _ = Task.Run(() => HandleClient(client));
        }
    }

    static async Task HandleClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[4096];

        try
        {
            while (true)
            {
                int bytes = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytes == 0) break;

                string userMessage = Encoding.UTF8.GetString(buffer, 0, bytes);
                Console.WriteLine("👤 User: " + userMessage);

                string reply = GetResponse(userMessage);

                string finalMessage = "AI: " + reply;

                byte[] data = Encoding.UTF8.GetBytes(finalMessage);
                await stream.WriteAsync(data, 0, data.Length);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Error: " + ex.Message);
        }
        finally
        {
            clients.Remove(client);
            client.Close();
        }
    }

    // 🤖 SIMPLE NORMAL OFFLINE CHATBOT
    static string GetResponse(string input)
    {
        string msg = input.ToLower().Trim();

        if (msg.Contains("hello") || msg.Contains("hi") || msg.Contains("hey"))
            return "Hello! How can I help you?";

        if (msg.Contains("how are you"))
            return "I'm fine, thank you! How can I assist you?";

        if (msg.Contains("your name"))
            return "My name is JudBot.";

        if (msg.Contains("time"))
            return "Current time is " + DateTime.Now.ToString("HH:mm:ss");

        if (msg.Contains("date"))
            return "Today's date is " + DateTime.Now.ToString("dd-MM-yyyy");

        if (msg.Contains("joke"))
            return "Why did the computer go to doctor? Because it had a virus!";

        if (msg.Contains("help"))
            return "You can ask me about time, date, jokes, or greetings.";

        if (msg.Contains("bye"))
            return "Goodbye! Have a nice day!";
        if (msg.Contains("thank you") || msg.Contains("thanks"))
            return "You're welcome!";
        if (msg.Contains("who are you"))
            return "I'm JudBot, your friendly chatbot!";
        if (msg.Contains("what can you do"))
            return "I can chat with you, tell jokes, and provide information about time and date.";
        if (msg.Contains("how are you"))
            return "l don't have feelings and you";
        if (msg.Contains("what's up") || msg.Contains("whats up"))
            return "Not much, just chatting with you!";
        if (msg.Contains("do you like music"))
            return "Yes, I love music! What's your favorite genre?";
        if (msg.Contains("do you have feelings"))
            return "I don't have feelings, but I'm here to help you!";

        return "Sorry, I don't understand that. Try asking something simple.";
    }
}