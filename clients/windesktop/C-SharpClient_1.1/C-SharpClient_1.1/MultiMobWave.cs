using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace C_SharpClient_1._1
{
    class MultiMobWave
    {
        private bool isWaveOver = false;
        private bool clockIsSet = false;
        public bool IsWaveOver { get { return isWaveOver; }}
        //Cheack if you have got the money
        public class MonsterINFO
        {
            public int id; public int hp; public int owner;
            public MonsterINFO(int id, int hp, int owner){
                this.id = id;
                this.hp = hp;
                this.owner = owner;
            }
        }

        private List<MonsterINFO> NextMonsterWave = new List<MonsterINFO>();
        private GameSession gameSession;

        private int timer = 1;
        public int waveNumber = 0;
        public int monstersInThisWave = 0;

        public MultiMobWave(GameSession gameSession)
        {
            this.gameSession = gameSession;
        }

        public void AddMonsterToNextMobWave(int monsterID, int hp, int owner)
        {
            System.Diagnostics.Debug.WriteLine("Monster ID: " + monsterID + " CulpritID: " + owner);
            NextMonsterWave.Add(new MonsterINFO(monsterID, hp, owner));
        }
        public void SpawnNextMobWave()
        {
            int counter = 0;
            foreach (MonsterINFO moInfo in NextMonsterWave)
            {
                monstersInThisWave++;
                counter++;
                Monster mo = gameSession.SpawnMonster(moInfo.id);
                mo.hp = moInfo.hp;
                mo.ownerID = moInfo.owner;
                mo.moveDistance(-counter * 60);
            }
            NextMonsterWave.Clear();
        }

        /// <summary>
        /// Cheack How long to next mobspawn.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        public int TimeToNextWave(GameTime gameTime)
        {
            if (timer - (gameTime.TotalGameTime.Seconds + gameTime.TotalGameTime.Minutes * 60 + gameTime.TotalGameTime.Hours * 3600) < 1)
                return 0;
            return timer - (gameTime.TotalGameTime.Seconds + gameTime.TotalGameTime.Minutes * 60 + gameTime.TotalGameTime.Hours * 3600);
        }

        public void SetSpawnTimer(int timeToNextWave)
        {
            isWaveOver = true;
            timer = timeToNextWave;
            clockIsSet = true;
            waveNumber++;
            if (waveNumber != 1 && gameSession.player.hp > 0)
                gameSession.player.gold += gameSession.sharedModule.MpIncome();
        }


        public bool Update(GameTime gameTime)
        {
            if (clockIsSet)
            {
                timer += gameTime.TotalGameTime.Seconds + gameTime.TotalGameTime.Minutes * 60 + gameTime.TotalGameTime.Hours * 3600;
                clockIsSet = false;
            }
            if (isWaveOver)
            {
                if ((gameTime.TotalGameTime.Seconds + gameTime.TotalGameTime.Minutes * 60 + gameTime.TotalGameTime.Hours * 3600) > timer)
                {
                    isWaveOver = false;
                    SpawnNextMobWave();
                    if (monstersInThisWave <= 0)
                    {
                        monstersInThisWave = 0;
                        gameSession.sharedModule.KilledLastMonster();
                    }
                }
            }
            return false;
        }
    }
}
