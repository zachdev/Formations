using Formations;
using Formations.Connection;
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

    Boolean isConnected = false;

    TcpClient server;
    TcpClient client;
    NetworkStream ns;
    NetworkStream ns2;

    public ConnectionManger(TextBox chatHistoryTextbox, String ip)
    {
        // Default, will be the host and will prep for the listening side

        this.chatHistoryTextbox = chatHistoryTextbox;
        this.ip = ip;
        //var t = Task.Factory.StartNew(() => Listener());

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

        var t = Task.Factory.StartNew(() => Listener());

        chatHistoryTextbox.Text += "\nConnection established...";
        isConnected = true;
    }

    public ConnectionManger(TextBox chatHistoryTextbox)
    {
        // Default, will be the host and will prep for the listening side

        this.chatHistoryTextbox = chatHistoryTextbox;
        var t = Task.Factory.StartNew(() => Listener());

        chatHistoryTextbox.Text += "\nListening for connection...";
    }

    private void Listener()
    {
        //---listen at the specified IP and port no.---
        IPAddress localAdd = IPAddress.Any;
        TcpListener listener = new TcpListener(localAdd, PORT);
        listener.Start();

        //---incoming client connected---
        client = listener.AcceptTcpClient();

        //---get the incoming data through a network stream---
        ns2 = client.GetStream();

        IPEndPoint ep = client.Client.RemoteEndPoint as IPEndPoint;
        IPAddress ipa = ep.Address;
        ip = ipa.ToString();

        if (server == null)
        {
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
        }

        chatHistoryTextbox.Text += "\nConnection established...";
        isConnected = true;

        while (true)
        {
            listen(listener);
        }
    }

    private void listen(TcpListener listener)
    {
        byte[] buffer = new byte[client.ReceiveBufferSize];

        //---read incoming stream---
        int bytesRead = ns2.Read(buffer, 0, client.ReceiveBufferSize);

        ConnectionMessage message = new ConnectionMessage { Data = buffer };

        object obj = Deserialize( message );
        if (obj is String)
        {
            //---convert the data received into a string---
            var dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            chatHistoryTextbox.Text += "\n<Received> " + dataReceived; //---write back the text to the client---
        }
        //else if (obj is Player)
        //    Client.ProcessOtherPlayersStatusUpdates(obj as Player);

    }

    public void sendMessage(String message)
    {
        if (isConnected)
        {
            ConnectionMessage obj = Serialize(message);

            ns.Write(obj.Data, 0, obj.Data.Length);
            ns.Flush();
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
    private ConnectionMessage Serialize(object someObject)
    {
        using (var memoryStream = new MemoryStream())
        {
            (new BinaryFormatter()).Serialize(memoryStream, someObject);
            return new ConnectionMessage { Data = memoryStream.ToArray() };
        }
    }

    // Decode object for use
    private object Deserialize(ConnectionMessage message)
    {
        using (var memoryStream = new MemoryStream(message.Data))
        {
            return (new BinaryFormatter()).Deserialize(memoryStream);
        }
    }

}
