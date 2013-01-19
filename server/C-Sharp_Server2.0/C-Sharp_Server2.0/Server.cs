using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace C_Sharp_Server
{
    class Server
    {
        //The first tcp Lister, listen incoming connections
        private TcpListener tcpListener;
        //The thread that listen.
        private Thread listenThread;
        public List<Player> onlinePlayer = new List<Player>();
        private Hashtable nameToPlayer = new Hashtable();

        //A Object that contains gamerooms.
        public GameRooms gameRooms = new GameRooms();

        /// <summary>
        /// Starts the server
        /// </summary>
        public Server()
        {
            this.tcpListener = new TcpListener(IPAddress.Any, 1337);
            this.listenThread = new Thread(new ThreadStart(ListenForClients));
            this.listenThread.Start();
        }
        public Player GetPlayerByID(int id)
        {
            foreach (Player p in onlinePlayer)
                if (p.Id == id)
                    return p;
            return new Player(1);
        }
        /// <summary>
        /// Listen for Incoming connection on port 1337
        /// </summary>
        private void ListenForClients()
        {
            this.tcpListener.Start();

            while (true)
            {
                //blocks until a client has connected to the server
                TcpClient client = this.tcpListener.AcceptTcpClient();

                //create a thread to handle communication 
                //with connected client
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                clientThread.IsBackground = true;
                clientThread.Start(client);
            }
        }
        /// <summary>
        /// Broacast to every online client
        /// </summary>
        /// <param name="s">String to be broadcasted</param>
        public void BroadCast(string s)
        {
            foreach (Player p in onlinePlayer)
                p.Send(s);
        }

        public void Clean()
        {
            foreach (Player p in onlinePlayer)
            {
                if (p.TcpClien.Connected == false)
                {
                    Console.WriteLine(p.Name + ":" +p.Id +" is no longer online, time to kick...");
                    GameRooms.GameRoom gameRoom = p.MyCurrentGameRoom;
                    p.MyCurrentGameRoom.Quit(p);
                    if (gameRoom.ListOfPlayers.Count == 0)
                        gameRooms.RemoveGameRoom(gameRoom.Name);
                    p.ForceKicK();
                }
            }
        }


        /// <summary>
        /// Handles all the Tcp Clients
        /// </summary>
        /// <param name="client">TcpClient</param>
        private void HandleClientComm(object client)
        {
            //Creates an player object with information about player
            Player thisPlayer = new Player((TcpClient)client, Thread.CurrentThread, Thread.CurrentThread.ManagedThreadId);
            //Commands is a class to recive and send Commands.
            Commands commands = new Commands(thisPlayer, gameRooms);
            onlinePlayer.Add(thisPlayer);
            Protocol protocolHandler = new Protocol();

            //Tells someone have connected
            Console.WriteLine(thisPlayer.TcpClien.Client.RemoteEndPoint + " Has Connected with id: " + thisPlayer.Id);
            byte[] message = new byte[4096];
            int bytesRead;

            //Client Loop start
            while (true)
            {
                bytesRead = 0;

                try
                {
                    bytesRead = thisPlayer.NetStream.Read(message, 0, 4096);
                }
                catch
                {
                    //a socket error has occured
                    break;
                }
                if (bytesRead == 0 || !onlinePlayer.Contains(thisPlayer))
                {
                    //the client has disconnected from the server
                    break;
                }
                //Recvies information of pakgage
                string[] kalle = Encoding.ASCII.GetString(message, 0, bytesRead).Split("\r\n".ToCharArray());
                Protocol.Package pkg = null;
                for (int i = 0; i != kalle.Length; i++)
                {
                    pkg = protocolHandler.GetPackage(kalle[i]);
                    if (kalle[i] != "")
                    {
                        commands.Body(pkg.Body);
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("User->Server:" + pkg.Name + " " + pkg.Body);
                        //Console.WriteLine("UserID: " + pkg.Name + "\r\nID: " + pkg.ID + "\r\nBody: " + pkg.Body);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
            //Kills Conection
            Console.WriteLine(thisPlayer.Id + " Has left the server");
            onlinePlayer.Remove(thisPlayer);
            thisPlayer.TcpClien.Close();
            if (thisPlayer.MyCurrentGameRoom != null)
            {
                GameRooms.GameRoom gm = thisPlayer.MyCurrentGameRoom;
                if (thisPlayer.QuitGameRoom())
                {
                    gameRooms.RemoveGameRoom(gm.Name);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(thisPlayer.Name + " force left and his gameroom: " + gm.Name + " was taken down");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(thisPlayer.Name + " force left and quit his gameroom: " + gm.Name);
                    Console.ForegroundColor = ConsoleColor.White;
                    commands.HasLeft(gm);
                }
            }
        }
    }
}
