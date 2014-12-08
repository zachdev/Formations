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
    // Connection info
    private const String SERVER_IP = "96.42.67.194";
    private const int PORT = 15000;

    // This
    private static ConnectionManager cm;

    // Server connection objects
    private TcpClient server;
    private NetworkStream serverNS;
    private Task serverSenderThread;

    // Other player connection objects
    private TcpClient playerClient;
    private NetworkStream playerClientNS;

    private GameLobby gameLobby;

    #region - Constructors

    private ConnectionManager()
    {
        if (gameLobby == null)
        {
            gameLobby = GameLobby.getInstance();
        }

        serverSenderThread = Task.Factory.StartNew(() => ServerConnection());
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

    public void sendChallengeRequect(ChallengeRequest request)
    {
        if (server.Connected)
        {
            ConnectionMessage obj = Serialize(request);

            serverNS.Write(obj.Data, 0, obj.Data.Length);
            serverNS.Flush();
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

            serverNS.Write(obj.Data, 0, obj.Data.Length);
            serverNS.Flush();
        }
    }

    public void sendPerson(Person person)
    {
        if (server.Connected)
        {
            ConnectionMessage obj = Serialize(person);

            serverNS.Write(obj.Data, 0, obj.Data.Length);
            serverNS.Flush();
        }
    }

    // Close the damn connection, the .NET framework seems to be taking care of it, so... that's good.
    public void closeConnection()
    {
        serverNS.Close();
        server.Close();
    }

    #endregion - Public Methods

    #region - Private Methods

    // Build's the sender datastream, uses the server TcpClient
    private void ServerConnection()
    {
        try
        {
            // This is the connection to the server
            server = new TcpClient(SERVER_IP, PORT);
        }
        catch (SocketException)
        {
            gameLobby.chatHistoryTextbox.Text += "Unable to connect.\n";
            return;
        }

        // Set the NS stream reference
        serverNS = server.GetStream();

        // Create a byte array to store the incoming IP address
        byte[] buffer = new byte[server.ReceiveBufferSize];

        // Read the data that's come in
        int bytesRead = serverNS.Read(buffer, 0, server.ReceiveBufferSize);
        // Getting only the IP
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

        gameLobby.chatHistoryTextbox.Text += "Connection to server established.\n";

        ServerListener();

        // Set-up the listener for the server.
        // serverListenThread = Task.Factory.StartNew(() => Listener());
    }

    // Listener method will be placed on its own thread, uses the client TcpClient
    private void ServerListener()
    {
        while (server.Connected)
        {
            // This will make the server listen
            listen(server);
            Thread.Sleep(10);
        }
    }

    // The constant listening function
    private void listen(TcpClient client)
    {
        // Something is available
        if (client.Available != 0)
        {
            byte[] buffer = new byte[client.ReceiveBufferSize];

            //---read incoming stream--- Will place the data into the buffer
            int bytesRead = client.GetStream().Read(buffer, 0, client.ReceiveBufferSize);

            ConnectionMessage message = new ConnectionMessage { Data = buffer };

            // Deserialize then figure out what the object we got was.
            object obj = Deserialize(message);
            if (obj is String)
            {
                gameLobby.chatHistoryTextbox.Text += (obj as String) + "\n"; //---write back the text to the client---
            }
            else if (obj is Person)
            {
                //gameLobby.chatHistoryTextbox.Text += (obj as Person) + "\n"; //---write back the text to the client---
                gameLobby.updatePlayersList((Person)obj);
            }
            else if (obj is ChallengeRequest)
            {
                gameLobby.AcceptChallengeWindowOpen((ChallengeRequest)obj);
                System.Console.WriteLine("ChallengeRequest");

            }
        }
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
