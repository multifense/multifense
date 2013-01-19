using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace C_Sharp_Server
{
    [TestFixture]
    class Test
    {
        [Test]
        public void TestDecodeIncomingMessage()
        {
            Protocol pro = new Protocol();
            Protocol.Package pkg = pro.GetPackage("ReverS" + "\x27\x27" + "1124" + "\x27\x27" + "This is the body");
            Assert.AreEqual(pkg.Name, "ReverS", "Coult not find name");
            Assert.AreEqual(pkg.ID, 1124, "Could not get ID");
            Assert.AreEqual(pkg.Body, "This is the body", "Could not retrive body");
        }
        [Test]
        public void TestGameRoom()
        {
            GameRooms grs = new GameRooms();
            Player p1 = new Player(1);
            p1.Name = "Simon";
            Player p2 = new Player(2);
            p2.Name = "Kalle";
            grs.AddGameRoom("p1");
            Assert.AreEqual(grs.GetGameRooms(), "p1\r\n");
            p1.JoinGameRoom(grs.GetGameRoom("p1"));
            p2.JoinGameRoom(grs.GetGameRoom("p1"));
            Assert.AreEqual(grs.GetGameRoom("p1").ListOfPlayers.Count, 2);
            p2.QuitGameRoom();
            Assert.AreEqual(grs.GetGameRoom("p1").ListOfPlayers.Count, 1);
            Assert.AreEqual(grs.GetGameRoom("p1").PlayersInThisGameRoom(), ",Simon");
        }
        [Test]
        public void TestFindRandomRoom()
        {
            GameRooms grs = new GameRooms();
            Player p1 = new Player(1);
            p1.Name = "Simon";
            Player p2 = new Player(2);
            p2.Name = "Kalle";
            Player p3 = new Player(3);
            p3.Name = "Zimon";

            grs.FindRandomRoom(2).Join(p1);
            Assert.AreEqual(p1.MyCurrentGameRoom.PlayersInThisGameRoom(), ",Simon");
            grs.FindRandomRoom(2).Join(p2);
            Assert.AreEqual(p2.MyCurrentGameRoom.PlayersInThisGameRoom(), ",Simon,Kalle");
            grs.FindRandomRoom(2).Join(p3);
            Assert.AreNotEqual(p3.MyCurrentGameRoom.Name, p1.MyCurrentGameRoom.Name);
        }
        [Test]
        public void TestGameCommands()
        {
            GameRooms gr = new GameRooms();
            gr.AddGameRoom("s1");
            Player p = new Player(1);
            p.Name = "Simon";

            gr.FindRandomRoom(2).Join(p);
            p.Gamee.IsInGame = true;
            Commands com = new Commands(p, gr);
            com.Body("GOLD,100");
            Assert.AreEqual(p.Gamee.PlayerGold, 100);
            Assert.IsEmpty(p.Gamee.TheTowers);
            Assert.IsEmpty(p.Gamee.TheMonsters);
            com.Body("BUILDTOWER,12,SuperTower,10,10");
            Assert.AreEqual(p.Gamee.TheTowers[0].type, "SuperTower");
            com.Body("SELLTOWER,12");
            Assert.IsEmpty(p.Gamee.TheTowers);
            com.Body("BUILDTOWER,10,SuperTower,10,10");
            com.Body("UPGRADE, 10, 12,NewTower,10,10");
            Assert.AreEqual(p.Gamee.TheTowers[0].type, "NewTower");
        }
        [Test]
        public void TestFindNextPlayer()
        {
            GameRooms.GameRoom gr = new GameRooms.GameRoom("New");
            Player p1 = new Player(1);
            p1.Name = "Simon";
            Player p2 = new Player(2);
            p2.Name = "Kalle";
            gr.Join(p1);
            gr.Join(p2);
            Assert.AreEqual(gr.NextPlayer(p1).Name, "Kalle");
            Assert.AreEqual(gr.NextPlayer(p2).Name, "Simon");
        }
    }
}
