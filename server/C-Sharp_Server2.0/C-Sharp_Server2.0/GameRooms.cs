using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace C_Sharp_Server
{
    class GameRooms
    {
        /// <summary>
        /// Igår kl 16:30
        /// </summary>
        public class GameRoom
        {
            private bool gameStarted = false;
            private int gameSlots = 2;
            public int GameSlots
            {
                get { return gameSlots; }
                set
                {
                    if (gameSlots == 0)
                        gameSlots = value;
                }
            }
            private int countLastMonsterDie = 0;
            private List<Player> listOfPlayers = new List<Player>();
            public List<Player> ListOfPlayers { get { return listOfPlayers; } }
            private string name;
            public string Name { get { return name; } }
            /// <summary>
            /// Sets the name of the game room.
            /// </summary>
            /// <param name="name">The name of the room</param>
            public GameRoom(string name)
            {
                this.name = name;
            }
            public GameRoom(string name, int gameSlots)
            {
                this.name = name;
                this.gameSlots = gameSlots;
            }
            /// <summary>
            /// Calls everytime you kills one monster
            /// </summary>
            /// <returns>Returns true if all players killed thier last monster</returns>
            public bool LastMonsterKilled()
            {
                int counter = 0;
                foreach (Player p in listOfPlayers)
                {
                    Console.WriteLine("LastmonsterKilled: " + counter);
                    if (!p.Gamee.lastMonstersKilled)
                        return false;
                    counter++;
                }
                return true;
            }
            /// <summary>
            /// Joins this gameroom
            /// </summary>
            /// <param name="pl">The Player thats wants to join</param>
            public void Join(Player pl)
            {
                if (listOfPlayers.Count <= gameSlots)
                {
                    listOfPlayers.Add(pl);
                    pl.MyCurrentGameRoom = this;
                }
            }
            /// <summary>
            /// Cheack if the gamme room is full
            /// </summary>
            /// <returns>Returns true if game-room is full</returns>
            public bool isFull()
            {
                if (gameStarted)
                    return true;
                else
                    if (listOfPlayers.Count >= gameSlots)
                    {
                        Console.WriteLine(this.Name + " is filled and started");
                        gameStarted = true;
                        return true;
                    }
                    else
                        return false;
            }
            /// <summary>
            /// Quits the game room
            /// </summary>
            /// <param name="pl">Player quit this game room</param>
            public bool Quit(Player pl)
            {
                listOfPlayers.Remove(pl);
                pl.MyCurrentGameRoom = null;
                if (this.ListOfPlayers.Count == 0)
                    return true;
                else
                    return false;
            }
            /// <summary>
            /// Returns a string of players in this gameRoom.
            /// </summary>
            /// <returns></returns>
            public string PlayersInThisGameRoom()
            {
                string tmp = "";
                if (listOfPlayers.Count > 0)
                {
                    foreach (Player p in listOfPlayers)
                        tmp += "," + p.Name + ":" + p.Id;
                }
                else
                    tmp = "There are no others in this room";
                return tmp;
            }
            /// <summary>
            /// Gives you the next player you should send on!
            /// </summary>
            /// <param name="p">You!</param>
            /// <returns>Next player</returns>
            public Player NextPlayer(Player p)
            {
                if (listOfPlayers.Count > 0)
                {
                    Player nextPlayer = (Player)listOfPlayers[(ListOfPlayers.IndexOf(p) + 1) % listOfPlayers.Count];
                    if (nextPlayer.Gamee.PlayerHP <= 0)
                        return NextPlayer(nextPlayer);
                    else
                        return nextPlayer;
                }
                else
                    throw new Exception("There are to few player in this room to start it!");
            }
            /// <summary>
            /// Clear the gameRoom.
            /// </summary>
            public void Dispose()
            {
                listOfPlayers.Clear();
            }
        }

        private List<GameRoom> gameRooms = new List<GameRoom>();
        public List<GameRoom> GameRoomsList { get { return gameRooms; } }
        private Hashtable nameToRooms = new Hashtable();
        private Random ran = new Random();

        /// <summary>
        /// Add a new game room.
        /// </summary>
        /// <param name="name">The name of the new Gameroom</param>
        public void AddGameRoom(string name)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Room: " + name + " was created");
            GameRoom tmp = new GameRoom(name);
            gameRooms.Add(tmp);
            nameToRooms.Add(name, tmp);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public void AddGameRoom(string name, int ammountOfPlayers)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Room: " + name + " was created");
            GameRoom tmp = new GameRoom(name, ammountOfPlayers);
            gameRooms.Add(tmp);
            nameToRooms.Add(name, tmp);
            Console.ForegroundColor = ConsoleColor.White;
        }
        /// <summary>
        /// Join a game-room
        /// </summary>
        /// <param name="name">The name of the game-room you wanna join.</param>
        /// <param name="pl">The Player thats going to join</param>
        public void JoinRoom(string name, Player pl)
        {
            if (nameToRooms.ContainsKey(name))
            {
                GameRoom gr = (GameRoom)nameToRooms[name];
                gr.Join(pl);
            }
        }
        /// <summary>
        /// Quit the game room
        /// </summary>
        /// <param name="name">The name of the game-room you wanna quit</param>
        /// <param name="pl">The Player thats going to quit.</param>
        public void QuitRoom(string name, Player pl)
        {
            if (nameToRooms.ContainsKey(name))
            {
                GameRoom gr = (GameRoom)nameToRooms[name];
                gr.Quit(pl);
                if (gr.ListOfPlayers.Count == 0)
                {
                    gameRooms.Remove(gr);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Gameroom: " + gr.Name + " got removed bc eveybody left it");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else
                throw new Exception("No such game room");
        }
        public void CloseGameRoom(GameRoom gr)
        {
            List<Player> playersToRemove = new List<Player>();
            foreach (Player p in gr.ListOfPlayers)
                playersToRemove.Add(p);
            foreach (Player p in playersToRemove)
            {
                gr.ListOfPlayers.Remove(p);
                p.MyCurrentGameRoom = null;
                p.Gamee.IsInGame = false;

            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Gameroom: " + gr.Name + " is removed bc game ended");
            Console.ForegroundColor = ConsoleColor.White;
            RemoveGameRoom(gr.Name);
            playersToRemove.Clear();
        }

        /// <summary>
        /// Remove a game room and remove all players inside it.
        /// </summary>
        /// <param name="name">Remove a gameroom with this name</param>
        public void RemoveGameRoom(string name)
        {

            if (nameToRooms.ContainsKey(name))
            {
                GameRoom gr = (GameRoom)nameToRooms[name];
                gr.Dispose();
                gameRooms.Remove(gr);
                nameToRooms.Remove(name);
            }
            else
                throw new Exception("No such game room");
        }
        /// <summary>
        /// Gives back a room if its Not Full, if there are non your giving a new game Room.
        /// </summary>
        /// <returns>Gives back the room thats not full</returns>
        public GameRoom FindRandomRoom(int slots)
        {
            foreach (GameRoom g in gameRooms)
            {
                if (g.GameSlots == 0)
                {
                    if (slots == 0)
                        g.GameSlots = 2;
                    else
                        g.GameSlots = slots;
                    return g;
                }
                if (!g.isFull() && (g.GameSlots == slots || slots == 0))
                    return g;
            }
            while (true)
            {
                string tmp = Convert.ToString(ran.Next(10000000));
                if (!nameToRooms.ContainsKey(tmp))
                {
                    GameRoom tmpgr = new GameRoom(tmp, slots);
                    gameRooms.Add(tmpgr);
                    nameToRooms.Add(tmp, tmpgr);
                    return tmpgr;
                }
            }
        }
        /// <summary>
        /// Returns a gameRoom object from a name.
        /// </summary>
        /// <param name="name">The name of the game room object you want</param>
        /// <returns>The gameroom object</returns>
        public GameRoom GetGameRoom(string name)
        {
            if (nameToRooms.ContainsKey(name))
                return (GameRoom)nameToRooms[name];
            else
                throw new Exception("There is no such gameRoom");
        }
        /// <summary>
        /// Get a string with all the gamerooms.
        /// </summary>
        /// <returns></returns>
        public string GetGameRooms()
        {
            string tmp = "";
            if (gameRooms.Count > 0)
                foreach (GameRoom gr in gameRooms)
                    tmp += gr.Name + "\r\n";
            else
                tmp = "There is no gamerooms";
            return tmp;
        }
    }
}
