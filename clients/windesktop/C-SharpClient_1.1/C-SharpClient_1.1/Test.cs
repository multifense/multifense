/*
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using NUnit.Framework;

namespace C_SharpClient_1._1
{
    [TestFixture]
    class Test
    {
        [Test]
        public void TestMobWave()
        {
            GameSession gameSession = new GameSession("Simon");
            Hashtable hash = new Hashtable();
            Monster mo1 = new Monster(new Rectangle(0,0,50,50), 100, 10);
            Monster mo2 = new Monster(new Rectangle(0,0,50,50), 100, 10);
            List<GameObject> listToPrint = new List<GameObject>();
            hash.Add(1, mo1);
            hash.Add(2, mo2);
            MultiMobWave mobwave = new MultiMobWave(gameSession);
            mobwave.AddMonsterToNextMobWave(1,100,2);
            mobwave.AddMonsterToNextMobWave(2,100,2);
            mobwave.SpawnNextMobWave();
            Assert.AreEqual(listToPrint.Count, 2);
        }
        //Testing if Mouse works
        [Test]
        public void TestMouseInputOutPut()
        {
            Mouse.SetPosition(0, 0);
            Assert.AreEqual(Mouse.GetState().X, 0);
            Assert.AreEqual(Mouse.GetState().Y, 0);
        }
        //Test if Graphic Egine Works
        [Test]
        public void TestGrafikEgine()
        {

        }

        [Test]
        public void SpriteAtCorrectPlace()
        {
            Rectangle rec = new Rectangle(0, 0, 10, 10);
            Assert.AreEqual(rec.X, 0);
            Assert.AreEqual(rec.Y, 0);
            Assert.AreEqual(rec.Width, 10);
            Assert.AreEqual(rec.Height, 10);
        }
        /// <summary>
        /// Provar Om monster spritesen får den position och
        /// storlek vi givit dem
        /// </summary>
        [Test]
        public void MonsterTest()
        {
            Monster Monster1 = new Monster(new Rectangle(0, 0, 10, 10), 100, 10);
            Assert.AreEqual(Monster1.Speed, 10);
            Monster1.ReduceHP(10);
            Assert.AreEqual(Monster1.ReduceHP(0), 90);
            Assert.AreEqual(Monster1.ReduceHP(90), 0);
            Assert.AreEqual(Monster1.Speed, 10);
        }
        /// <summary>
        /// A monster can die and be removed
        /// based on hp.
        /// </summary>
        [Test]
        public void MonsterDeath()
        {
            List<GameObject> list = new List<GameObject>();
            Monster mo = new Monster(new Rectangle(0, 0, 10, 10), 100, 10);
            list.Add(mo);
            mo.ReduceHP(10);
            Assert.AreEqual(mo.ReduceHP(0), 90, "Hp problem");
            Assert.AreEqual(mo.Alive, true, "Is it dead? " + mo.Alive);
            mo.ReduceHP(110);
            Assert.AreEqual(mo.Alive, false, "Still not dead? " + mo.Alive);
            if (!mo.Alive)
                list.Remove(mo);
            Assert.IsEmpty(list);
        }
        /// <summary>
        /// Testing the towers
        /// </summary>
        [Test]
        public void PlaceTower()
        {
            //Svårt att testa utan draw... testet görs i draw.
        }

        /// <summary>
        /// Ritar ut monster sprites
        /// </summary>
        [Test]
        public void SpriteListNMonsterWalk()
        {
            Economy eco = new Economy();
            Player p = new Player("Simon",1);
            List<GameObject> listSprites = new List<GameObject>();
            List<GameObject> garbageSprites = new List<GameObject>();

            for (int i = 0; i != 10; i++)
            {
                listSprites.Add(new Monster(new Rectangle(0, 0, 50, 50), 100, 10));
            }
            foreach (Monster s in listSprites)
            {
                Assert.AreEqual(0, s.Rec.X);
                Assert.AreEqual(0, s.Rec.Y);
                Assert.AreEqual(50, s.Rec.Width);
                Assert.AreEqual(50, s.Rec.Height);
                Assert.AreEqual(s.Speed, 10);
                foreach (GameObject g in listSprites)
                    g.Update(ref listSprites);
            }
        }
        [Test]
        public void TestMonsterPath()
        {
            GameSession gameSession = new GameSession("Simon");
            GameSession.GamePath.GamePathPoint gpp = gameSession.gamePath.PathStart;
            int counter = 0;
            while (gpp != null)
            {
                counter++;
                if (counter != 12)
                    Assert.AreEqual(gpp.dir.Length(), 1);
                else
                    Assert.AreEqual(gpp.dir.Length(), 0);

                gpp = gpp.next;
            }
        }
        /// <summary>
        /// Test proctile have right damage and hit monster!
        /// </summary>
        [Test]
        public void TestProjectile()
        {
            List<GameObject> listSprites = new List<GameObject>();
            Monster mo = new Monster(new Rectangle(0, 0, 20, 20), 100, 10);
            TrackingProjectile track = new TrackingProjectile(mo, 0, 0, 15, 10);
            while (true)
            {

                if (track.Update(ref listSprites))
                    break;
            }
            Assert.AreEqual(mo.ReduceHP(0), 90);
        }
        [Test]
        public void TestConenction()
        {
            GameSession gs = new GameSession("ReverS", "www.google.se", 80);
            gs.tcpGame.Send("GET /");            
        }
    }
}
*/