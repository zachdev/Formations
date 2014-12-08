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

public class ConnectionManager
{
    private static ConnectionManager cm;
    private const String SERVER_IP = "96.42.67.194";
    private const int PORT = 15000;
    private GameLobby gameLobby;


    public Boolean isConnected = false;

    private TcpClient server;
    //private TcpClient client;
    private NetworkStream serverSenderNS;
    //private NetworkStream serverListenNS;

    private Task serverSenderThread;
    //private Task serverListenThread;

    #region - Constructors

    private ConnectionManager()
    {
        // Get the Person info before going further
    }

    #endregion - Contructors

    #region - Public Methods

    // Call to getting the Connection Manager
    public static ConnectionManager getInstance()
    {
        if (cm == null)
        {
            cm = new ConnectionManager();
        }
        return cm;
    }

    // Chat Textbox pass in
    public void setUpChat(TextBox chatHistoryTextbox)
    {
        if (gameLobby == null)
        {
            gameLobby = GameLobby.getInstance();
        }
        serverSenderThread = Task.Factory.StartNew(() => Sender());
    }

    public void sendChallengeRequect(ChallengeRequest request)
    {
        if (server.Connected)
        {

            ConnectionMessage obj = Serialize(request);

            serverSenderNS.Write(obj.Data, 0, obj.Data.Length);
            serverSenderNS.Flush();
        }
    }
    // Method for sending a String, specifially the chat method.
    public void sendMessage(String message)
    {
        if (server.Connected)
        {
            // Need to add the current player's name to the message
            //message = "<" + player.Name + "> " + message;

            ConnectionMessage obj = Serialize(message);

            serverSenderNS.Write(obj.Data, 0, obj.Data.Length);
            serverSenderNS.Flush();
        }
    }
    public void sendPerson(Person person)
    {
        if (server.Connected)
        {
            ConnectionMessage obj = Serialize(person);

            serverSenderNS.Write(obj.Data, 0, obj.Data.Length);
            serverSenderNS.Flush();
        }
    }
    // Close the damn connection, the .NET framework seems to be taking care of it, so... that's good.
    public void closeConnection()
    {
            serverSenderNS.Close();
            //serverListenNS.Close();
            server.Close();
            //client.Close();
    }

    #endregion - Public Methods

    #region - Private Methods

    // Build's the sender datastream, uses the server TcpClient
    private void Sender()
    {
        try
        {
            server = new TcpClient(SERVER_IP, PORT);
        }
        catch (SocketException)
        {
            gameLobby.chatHistoryTextbox.Text += "Unable to connect.\n";
            return;
        }
        serverSenderNS = server.GetStream();

        byte[] buffer = new byte[server.ReceiveBufferSize];

        //---read incoming stream--- Will place the data into the buffer
        int bytesRead = serverSenderNS.Read(buffer, 0, server.ReceiveBufferSize);

        char[] chars = new char[buffer.Length / sizeof(char)];
        System.Buffer.BlockCopy(buffer, 0, chars, 0, buffer.Length);
        String ip = "";
        for (int i = 0; i < 16; i++)
        {
            System.Console.WriteLine(chars[i]);
            if (chars[i] == '\0') { break; }
            ip += chars[i];
            
        }
        gameLobby.person.ipAddress = ip;

        gameLobby.chatHistoryTextbox.Text += ip + "\n";

        Listener();

        // Set-up the listener for the server.
        // serverListenThread = Task.Factory.StartNew(() => Listener());
    }

    // Listener method will be placed on its own thread, uses the client TcpClient
    private void Listener()
    {
        //---listen at the specified IP and port no.---
        //IPAddress localAdd = IPAddress.Any;
        //TcpListener listener = new TcpListener(localAdd, PORT);
        //listener.Start();

        //---incoming client connected---
        //client = listener.AcceptTcpClient();

        //---get the incoming data through a network stream---
        //serverListenNS = client.GetStream();

        gameLobby.chatHistoryTextbox.Text += "Connection to server established.\n";
        isConnected = true;

        while (true)
        {
            listen();
            Thread.Sleep(10);
        }
    }



    // The constant listening function
    private void listen()
    {
        // Something is available
        if (server.Available != 0)
        {
            byte[] buffer = new byte[server.ReceiveBufferSize];

            //---read incoming stream--- Will place the data into the buffer
            int bytesRead = serverSenderNS.Read(buffer, 0, server.ReceiveBufferSize);

            ConnectionMessage message = new ConnectionMessage { Data = buffer };

            // Deserialize then figure out what the object we got was.
            object obj = Deserialize(message);
            if (obj is String)
            {
                gameLobby.chatHistoryTextbox.Text += (obj as String) + "\n"; //---write back the text to the client---
            }
            if (obj is Person)
            {
                //gameLobby.chatHistoryTextbox.Text += (obj as Person) + "\n"; //---write back the text to the client---
                gameLobby.updatePlayersList((Person)obj);
            }
            if (obj is ChallengeRequest)
            {
                gameLobby.AcceptChallengeWindowOpen((ChallengeRequest)obj);
                System.Console.WriteLine("ChallengeRequest");

            }
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

    #endregion - Private Methods

}
