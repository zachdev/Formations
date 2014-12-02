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
    private const int PORT = 15000;

    private TextBox chatHistoryTextbox;

    private Boolean isConnected = false;

    private TcpClient server;
    private TcpClient client;
    private NetworkStream ns;
    private NetworkStream ns2;

    private Task sendThread;
    private Task listenThread;

    // Constructors

    public ConnectionManger(TextBox chatHistoryTextbox, String ip)
    {
        // Default, will be the host and will prep for the listening side
        this.chatHistoryTextbox = chatHistoryTextbox;
        chatHistoryTextbox.Text += "\nAttempt connection...";

        // Start up a thread to connect
        sendThread = Task.Factory.StartNew(() => Sender(ip));
    }

    public ConnectionManger(TextBox chatHistoryTextbox)
    {
        // Default, will be the host and will prep for the listening side
        this.chatHistoryTextbox = chatHistoryTextbox;
        chatHistoryTextbox.Text += "\nListening for connection...";

        // Start up a thread to listen
        listenThread = Task.Factory.StartNew(() => Listener());
    }

    // -- Public Methods --

    // Method for sending a String, specifially the chat method.
    public void sendMessage(String message)
    {
        if (isConnected)
        {
            ConnectionMessage obj = Serialize(message);

            ns.Write(obj.Data, 0, obj.Data.Length);
            ns.Flush();

            chatHistoryTextbox.Text += "\n<You> " + message;
        }
    }

    // Close the damn connection, the .NET framework seems to be taking care of it, so... that's good.
    public void closeConnection()
    {
        if (isConnected)
        {
            ns.Close();
            ns2.Close();
            server.Close();
            client.Close();
        }
    }


    // -- Private Methods --

    // Listener method will be placed on its own thread, uses the client TcpClient
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

        // In the case of the host, needs to have this data done now.
        if (server == null)
        {
            // Convert the connection we just recieved into an outgoing connection for two way sending.
            IPEndPoint ep = client.Client.RemoteEndPoint as IPEndPoint;
            IPAddress ipa = ep.Address;

            sendThread = Task.Factory.StartNew(() => Sender(ipa.ToString()));
        }

        chatHistoryTextbox.Text += "\nConnection established...";
        isConnected = true;

        while (true)
        {
            listen(listener);
        }
    }

    // Build's the sender datastream, uses the server TcpClient
    private void Sender(String ip)
    {
        try
        {
            server = new TcpClient(ip, PORT);
        }
        catch (SocketException)
        {
            chatHistoryTextbox.Text += "\nUnable to connect.";
            return;
        }
        ns = server.GetStream();

        if (client == null)
        {
            listenThread = Task.Factory.StartNew(() => Listener());
        }
    }


    // The constant listening function
    private void listen(TcpListener listener)
    {
        byte[] buffer = new byte[client.ReceiveBufferSize];

        //---read incoming stream--- Will place the data into the buffer
        int bytesRead = ns2.Read(buffer, 0, client.ReceiveBufferSize);

        ConnectionMessage message = new ConnectionMessage { Data = buffer };

        // Deserialize then figure out what the object we got was.
        object obj = Deserialize(message);
        if (obj is String)
        {
            chatHistoryTextbox.Text += "\n<Received> " + (obj as String); //---write back the text to the client---
        }
        //else if (obj is SerialClass)
        //    Class.someMethodToUse(obj as SerialClass);
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
