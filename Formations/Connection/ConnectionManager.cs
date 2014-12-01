using Formations;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TomShane.Neoforce.Controls;

public class ConnectionManger
{
    const int PORT = 15000;
    string ip;

    private TextBox chatHistoryTextbox;

    Boolean isHost = true;
    Boolean isConnected = true;

    TcpClient server;
    TcpClient client;
    NetworkStream ns;

    public ConnectionManger(TextBox chatHistoryTextbox, String ip)
    {
        // Default, will be the host and will prep for the listening side

        this.chatHistoryTextbox = chatHistoryTextbox;
        this.ip = ip;
        var t = Task.Factory.StartNew(() => Listener());

        try
        {
            server = new TcpClient(ip, PORT);
        }
        catch (SocketException)
        {
            chatHistoryTextbox.Text += "\nUnable to connect to server";
            return;
        }
        ns = server.GetStream();

        chatHistoryTextbox.Text += "\nConnection established...";
    }

    private void Listener()
    {
        //---listen at the specified IP and port no.---
        IPAddress localAdd = IPAddress.Any;
        TcpListener listener = new TcpListener(localAdd, PORT);
        listener.Start();

        //---incoming client connected---
        client = listener.AcceptTcpClient();

        while (true)
        {
            listen(listener);
        }
    }

    private void listen(TcpListener listener)
    {
        //---get the incoming data through a network stream---
        NetworkStream nwStream = client.GetStream();
        byte[] buffer = new byte[client.ReceiveBufferSize];

        //---read incoming stream---
        int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

        //---convert the data received into a string---
        var dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);

        //---write back the text to the client---
        chatHistoryTextbox.Text += "\n<Received> " + dataReceived;
    }

    public void sendMessage(String message)
    {
        if (isConnected)
        {
            ns.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
            //ns.Flush();

            //byte[] data = new byte[1024];
            //int recv = ns.Read(data, 0, data.Length);
            //string stringData = Encoding.ASCII.GetString(data, 0, recv);
            //chatHistoryTextbox.Text += "\n" + stringData;
        }
    }

    // Close the damn connection
    public void closeConnection()
    {
        ns.Close();
        server.Close();
        client.Close();
    }

    // Encode message for use
    /*
    private Message Serialize(object someObject)
    {
        using (var memoryStream = new MemoryStream())
        {
            (new BinaryFormatter()).Serialize(memoryStream, someObject);
            return new Message { Data = memoryStream.ToArray() };
        }
    }

    // Decode object for use
    private object Deserialize(Message message)
    {
        using (var memoryStream = new MemoryStream(message.Data))
        {
            return (new BinaryFormatter()).Deserialize(memoryStream);
        }
    }
     */
}
