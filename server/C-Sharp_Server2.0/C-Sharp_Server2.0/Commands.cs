using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_Sharp_Server
{
    /// <summary>
    /// A class that recives and sends command from server/Client.
    /// </summary>
    class Commands
    {
        Player p;
        GameRooms grs;
        Protocol pro;
        /// <summary>
        /// Make the a refrense to player p.
        /// </summary>
        /// <param name="p"></param>
        public Commands(Player p, GameRooms grs)
        {
            pro = new Protocol();
            this.p = p;
            this.grs = grs;
        }

        public void HasLeft(GameRooms.GameRoom gm)
        {
            if (gm.ListOfPlayers.Count == 1)
            {
                gm.ListOfPlayers[0].Send(pro.MakePackage("VICTOR," + gm.ListOfPlayers[0].Name));
                Console.WriteLine("In gameroom: " + gm.Name + " the game has ended and the winner is: " + gm.ListOfPlayers[0].Name + ":" + gm.ListOfPlayers[0].Id);
                grs.CloseGameRoom(gm);
            }
            else
            {
                if (gm.LastMonsterKilled())
                {
                    foreach (Player player in gm.ListOfPlayers)
                    {
                        player.Send(pro.MakePackage("MOBWAVE,10"));
                        player.Gamee.lastMonstersKilled = false;
                    }
                }
            }
        }
        /// <summary>
        /// Reads everycommand and call the apriate method.
        /// </summary>
        /// <param name="s">The Input string</param>
        public void Body(string s)
        {
            if (s.Split(',').Length > 0 && s.Split(',')[0] == "NAME")
            {
                p.Name = s.Split(',')[1];
                p.Send(pro.MakePackage("REGISTERED," + p.Id));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(p.Name + " has id: " + p.Id);
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }
            if (p.Gamee.IsInGame)
            {
                string[] inpt = s.Split(',');
                //Add monster to next players monster stack
                //Tells the other CLient to spawn a monster

                if (inpt[0] == "RECMONSTER")
                {
                    p.MyCurrentGameRoom.NextPlayer(p).Gamee.AddMonster(Convert.ToInt32(inpt[1]),Convert.ToInt32(inpt[2]), Convert.ToInt32(inpt[3]));
                    p.MyCurrentGameRoom.NextPlayer(p).Send(pro.MakePackage("SPAWN," + p.Id + "," + inpt[2] + "," + inpt[3]));
                }
                if (inpt[0] == "BUILDTOWER")
                    p.Gamee.AddTower(Convert.ToInt32(inpt[1]), inpt[2], Convert.ToDouble(inpt[3]), Convert.ToDouble(inpt[4]));
                if (inpt[0] == "SELLTOWER")
                    p.Gamee.RemoveTower(Convert.ToInt32(inpt[1]));
                if (inpt[0] == "UPGRADE")
                {
                    p.Gamee.RemoveTower(Convert.ToInt32(inpt[1]));
                    p.Gamee.AddTower(Convert.ToInt32(inpt[2]), inpt[3], Convert.ToDouble(inpt[4]), Convert.ToDouble(inpt[5]));
                }
                if (inpt[0] == "GOLD")
                    p.Gamee.PlayerGold = Convert.ToDouble(inpt[1]);
                if (inpt[0] == "MONSTERKILLED")
                    p.Gamee.RemoveMonster(Convert.ToInt32(inpt[1]));
                if (inpt[0] == "MONSTERDAMAGE")
                    p.Gamee.DamageMonster(Convert.ToInt32(inpt[1]), Convert.ToDouble(inpt[2]));
                if (inpt[0] == "LIFE")
                {
                    int counter = 0;
                    if (p.Gamee.PlayerHP > 0)
                        p.Gamee.PlayerHP = Convert.ToInt32(inpt[1]);

                    int culpritID = Convert.ToInt32(inpt[2]);
                    foreach (Player player in p.MyCurrentGameRoom.ListOfPlayers)
                    {
                        if (player.Gamee.PlayerHP > 0)
                            counter++;
                        if (player.Id == culpritID && player.Gamee.PlayerHP > 0)
                            player.Gamee.PlayerHP++;
                        if (player != p)
                        {
                            if (culpritID != 0)
                                player.Send(pro.MakePackage("DAMAGE,"+p.Id+","+culpritID+",1"));
                            player.Send(pro.MakePackage("LIFE," + p.Id + "," + p.Gamee.PlayerHP));
                        }
                    }

                    if (counter == 1)
                    {
                        Player winner = new Player(0);
                        foreach (Player player in p.MyCurrentGameRoom.ListOfPlayers)
                            if (player.Gamee.PlayerHP > 0)
                                winner = player;
                        foreach (Player player in p.MyCurrentGameRoom.ListOfPlayers)
                            player.Send(pro.MakePackage("VICTOR," + winner.Id));
                        Console.WriteLine("In gameroom: " + winner.MyCurrentGameRoom.Name + " the game has ended and the winner is: " + winner.Name + ":" + winner.Id);
                        grs.CloseGameRoom(winner.MyCurrentGameRoom);
                        
                    }
                }
                if (inpt[0] == "QUIT")
                {
                    GameRooms.GameRoom playerRoom = p.MyCurrentGameRoom;
                    grs.QuitRoom(p.MyCurrentGameRoom.Name, p);
                    p.Gamee.IsInGame = false;
                    Console.WriteLine(p.Name + " Quit his Gameroom");
                    foreach (Player player in playerRoom.ListOfPlayers)
                        if (player != p)
                        {
                            player.Send(pro.MakePackage("GAMEROOM" + playerRoom.PlayersInThisGameRoom()));
                        }
                    HasLeft(playerRoom);
                }
                if (inpt[0] == "LASTMONSTERKILLED")
                {
                    this.p.Gamee.lastMonstersKilled = true;
                    if (p.MyCurrentGameRoom.LastMonsterKilled())
                    {
                        foreach (Player player in p.MyCurrentGameRoom.ListOfPlayers)
                        {
                            player.Send(pro.MakePackage("MOBWAVE,10"));
                            player.Gamee.lastMonstersKilled = false;
                        }
                    }
                }
            }
            else
            {
                if (s.Split(',')[0] == "JOINRAN")
                {
                    int gameslots = Convert.ToInt32(s.Split(',')[1]);

                    p.Gamee.IsInGame = true;
                    grs.FindRandomRoom(gameslots).Join(p);
                    Console.WriteLine(p.Name + " joined gameroom: " + p.MyCurrentGameRoom.Name);
                    p.Send(pro.MakePackage("your Joined game room nr: " + p.MyCurrentGameRoom.Name));
                    Console.WriteLine("GAMEROOM" + p.MyCurrentGameRoom.PlayersInThisGameRoom());
                    p.Send(pro.MakePackage("GAMEROOM" + p.MyCurrentGameRoom.PlayersInThisGameRoom()));
                    foreach (Player player in p.MyCurrentGameRoom.ListOfPlayers)
                        if (player != p)
                        {
                            player.Send(pro.MakePackage("GAMEROOM" + p.MyCurrentGameRoom.PlayersInThisGameRoom()));
                            //player.Send(pro.MakePackage(p.Name + " joined your game room"));
                        }
                    if (p.MyCurrentGameRoom.isFull() && p.MyCurrentGameRoom.GameSlots != 0)
                        foreach (Player player in p.MyCurrentGameRoom.ListOfPlayers)
                        {
                            player.Send(pro.MakePackage("START,10"));
                            player.Send(pro.MakePackage("MOBWAVE,10"));
                            player.Gamee.lastMonstersKilled = false;
                        }
                }
            }
        }
    }
}
