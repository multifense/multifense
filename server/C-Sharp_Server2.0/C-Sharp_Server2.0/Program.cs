using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_Sharp_Server
{
    class Program
    {
        /// <summary>
        /// kl 15:17
        /// </summary>
        static Server serv = new Server();
        static void Main(string[] args)
        {
            Console.Title = "The Server";
            while (true)
            {
                string input = Console.ReadLine();
                if (input == "CLEAN")
                    serv.Clean();
                if (input.Split(' ')[0] == "BRD")
                {
                    serv.BroadCast(input.Substring(4));
                }
                if (input.Split(' ')[0] == "DEL")
                {
                    serv.gameRooms.RemoveGameRoom(input.Substring(4));
                }
                if (input.Split(' ')[0] == "KICK")
                {
                    Player tmp = serv.GetPlayerByID(Convert.ToInt32(input.Substring(5)));
                    serv.onlinePlayer.Remove(tmp);
                    tmp.ForceKicK();
                }
                if (input == "CLS")
                    Console.Clear();
                if (input == "ONLINE")
                {
                    foreach (Player p in serv.onlinePlayer)
                    {
                        string s = " player is not in game room";
                        if (p.MyCurrentGameRoom != null) 
                            s = p.MyCurrentGameRoom.Name;
                         Console.WriteLine("Name: " + p.Name + " ID: " + p.Id + " In gameroom: " + s);
                    }
                }
                if (input.Split(' ').Length > 1 && input.Split(' ')[0] == "GR")
                    serv.gameRooms.AddGameRoom(input.Split(' ')[1], Convert.ToInt32(input.Split(' ')[2]));
                if (input == "ROOMS")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    foreach (GameRooms.GameRoom g in serv.gameRooms.GameRoomsList)
                        Console.WriteLine("Name: " + g.Name + " in room: " + g.ListOfPlayers.Count);
                    Console.ForegroundColor = ConsoleColor.White;

                }
            }
        }
    }
}
