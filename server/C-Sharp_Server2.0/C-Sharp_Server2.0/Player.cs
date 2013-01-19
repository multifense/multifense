using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;


namespace C_Sharp_Server
{
    class Player
    {
        public class Game
        {
            public bool lastMonstersKilled = false;
            //A bool to tell if hes in or out of a game;
            private bool isInGame = false;
            public bool IsInGame { get { return isInGame; } set 
            {
                if (value)
                    isInGame = value;
                else
                {
                    theMonsters.Clear();
                    theTowers.Clear();
                    isInGame = value;
                }
            } }
            /// <summary>
            /// A sub class used to store monster
            /// </summary>
            public class Monster
            {
                public int id;
                public double hp;
                public int type;
                public Monster(int id, int type, double hp)
                {
                    this.id = id;
                    this.hp = hp;
                    this.type = type;
                }
                public void DamageMonster(double damage)
                {
                    hp -= damage;
                }
            }

            /// <summary>
            /// A subclass used to store Tower
            /// </summary>
            public class Tower
            {
                public int id;
                public double x;
                public double y;
                public string type;

                public Tower(int id, string type, double x, double y)
                {
                    this.id = id;
                    this.x = x;
                    this.y = y;
                    this.type = type;
                }
            }

            private List<Tower> theTowers = new List<Tower>();
            public List<Tower> TheTowers { get { return theTowers; } }
            private Hashtable idToMonster = new Hashtable();

            private List<Monster> theMonsters = new List<Monster>();
            public List<Monster> TheMonsters { get { return theMonsters; } }
            private Hashtable idToTowers = new Hashtable();

            /// <summary>
            /// Adds a tower to this players towerlist
            /// </summary>
            /// <param name="id">The ID of the tower</param>
            /// <param name="type">What tower is it</param>
            /// <param name="x">Position X</param>
            /// <param name="y">Position Y</param>
            public void AddTower(int id, string type, double x, double y)
            {
                Tower to = new Tower(id, type, x, y);
                theTowers.Add(to);
                idToTowers.Add(id, to);
            }
            /// <summary>
            /// Removes a tower from the tower list depending on ID
            /// </summary>
            /// <param name="id">The tower id you want to remove</param>
            public void RemoveTower(int id)
            {
                theTowers.Remove((Tower)idToTowers[id]);
                idToTowers.Remove(id);
            }
            /// <summary>
            /// Let you add a monster the this players monster list!
            /// </summary>
            /// <param name="id">Id for the monster</param>
            /// <param name="type">Which monster is it</param>
            /// <param name="hp">How much helth points do it have.</param>
            public void AddMonster(int id, int type, int hp)
            {
                Monster mo = new Monster(id, type, hp);
                theMonsters.Add(mo);
                Random ran = new Random();
                while (true)
                {
                    if (!idToMonster.Contains(id))
                    {
                        idToMonster.Add(id, mo);
                        break;
                    }
                    id = ran.Next();
                }
            }
            /// <summary>
            /// Remove monster with id
            /// </summary>
            /// <param name="id">The id of the monster you want to remove</param>
            public void RemoveMonster(int id)
            {
                theMonsters.Remove((Monster)idToMonster[id]);
                idToMonster.Remove(id);
            }
            /// <summary>
            /// Damage a monster.
            /// </summary>
            /// <param name="id">the id of the monster</param>
            /// <param name="damage">How much damage you hurt it!</param>
            public void DamageMonster(int id, double damage)
            {
                Monster mo = (Monster)idToMonster[id];
                mo.DamageMonster(damage);
            }

            private int playerHP = 10;
            public int PlayerHP { get {return playerHP;} set { playerHP = value;} }

            private double playerGold;
            public double PlayerGold { get { return playerGold; } set { playerGold = value; } }

        }
        private Game game = new Game();
        /// <summary>
        /// Stores inforamtion about the player and his current game.
        /// </summary>
        public Game Gamee { get { return game; } set { game = value; } }

        //Stores your current game Room so its easly can modifyied.
        private GameRooms.GameRoom myCurrentGameRoom;
        public GameRooms.GameRoom MyCurrentGameRoom { get { return myCurrentGameRoom; } set { myCurrentGameRoom = value; } }

        private ASCIIEncoding encoder = new ASCIIEncoding();
        private StreamWriter stw;

        //Player name
        private string name = "";
        public string Name { get { return name; } set { name = value; } }

        //Current Thread ID
        private int id;
        public int Id { get { return id; } set { id = value; } }

        //Currents NetStream
        private NetworkStream netStream;
        public NetworkStream NetStream { get { return netStream; } set { netStream = value; stw = new StreamWriter(netStream); } }

        //Tcp Client the user connected on!
        private TcpClient tcpClient;
        public TcpClient TcpClien { get { return tcpClient; } set { tcpClient = value; } }

        //The Thread is running on!
        private Thread myCurrentThread;

        /// <summary>
        /// Just for testing!!!!
        /// </summary>
        /// <param name="id"></param>
        public Player(int id)
        {
            this.id = id;
        }

        /// <summary>
        /// Creates a player with a special tcpClient and id
        /// </summary>
        /// <param name="tcpClient">The connected players tcp Client</param>
        /// <param name="id">The players thread ID</param>
        public Player(TcpClient tcpClient, Thread myThread, int id)
        {
            this.myCurrentThread = myThread;
            this.tcpClient = tcpClient;
            this.NetStream = tcpClient.GetStream();
            this.id = id;
        }
        /// <summary>
        /// Sends the string s to the client
        /// </summary>
        /// <param name="s">The string to be sent!</param>
        public void Send(string s)
        {
            try
            {
                stw.WriteLine(s);
                stw.Flush();
            }
            catch
            {
                Console.WriteLine("Cannot send: " + s + " to " + id);
            }
        }
        /// <summary>
        /// Lets this player Join a game room.
        /// </summary>
        /// <param name="gameRooms">The game room</param>
        /// <param name="name">the name of the game room</param>
        public void JoinGameRoom(GameRooms.GameRoom gameRoom)
        {
            if (myCurrentGameRoom == null)
            {
                gameRoom.Join(this);
                myCurrentGameRoom = gameRoom;
            }
            else
                throw new Exception("Your already in a game room");
        }
        /// <summary>
        /// Quit this players gameroom.
        /// </summary>
        public bool QuitGameRoom()
        {
            if (myCurrentGameRoom != null)
            {
                if (myCurrentGameRoom.Quit(this))
                    return true;
                else
                    return false;
            }
            else
                Console.WriteLine(this.Name + " not in a gameroom ERROR");
            return false;
        }
        public void ForceKicK()
        {
            Console.WriteLine(this.Id + " is forced kick");
            Send("You have been forced kicked");
            tcpClient.Close();
            myCurrentThread.Abort();
        }
    }
}
