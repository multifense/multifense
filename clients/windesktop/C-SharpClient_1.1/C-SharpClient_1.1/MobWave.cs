using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace C_SharpClient_1._1
{
    class MobWave
    {
        public class MobWaveEntry
        {
            private int nextSpawn; // counter until the next object is to be added to the array
            private int burstCounter = 0; // number of monsters left in current burst
            private int timeBetweenBursts; // number of frames between each burst
            private int monstersPerBurst; // number of monsters to spawn in a single burst
            private int waveCounter; // number of waves left of this entry

            private Monster monster; // monster to spawn (master clone)
            private bool finished = false;

            //initialise a monster wave 
            public MobWaveEntry(Monster monster, int startTime, int timeBetweenBursts, int numberOfBursts, int monstersPerBurst)
            {
                this.monster = monster;
                this.nextSpawn = startTime;
                burstCounter = 0;
                waveCounter = numberOfBursts;
                this.timeBetweenBursts = timeBetweenBursts;
                this.monstersPerBurst = monstersPerBurst;
                PrepareBurst();
            }
            //check time till nextSpawn
            public int GetNextSpawn()
            {
                return nextSpawn;
            }

            //Decreases the time untill the next group of monsters are deployd by subtracting time from nextSpawn.
            public void SpendTime(int time)
            {
                nextSpawn -= time;
            }
            //kolla över inte 100%
            public Monster SpawnNext()
            {
                burstCounter--; //reduces burstCounter by one
                if (burstCounter == 0) //if the whole burst has been sent get next burst
                {
                    PrepareBurst(); // calling PrepareBurst method to create next burst. 
                    nextSpawn = timeBetweenBursts;//  set the time to next burst to timeBetweenBursts
                    finished = (waveCounter < 0);//if the waveCounter is less then zero the finished boolean is set to true
                }
                else
                {
                    nextSpawn = (int)(60 / monster.Speed);
                }
                return (Monster)monster.Clone(); // clone the monster 
            }
            //Prepares the next burst by increasing the waveCount and setting the burstcounter 
            //to the appropriate number of monsters for that burst.
            public void PrepareBurst()
            {
                burstCounter = monstersPerBurst;
                waveCounter--; //reduces counter untill it reaches 0
            }
            //returns if the wave is complete or not
            public bool isFinished()
            {
                return finished;
            }
        }

        // HÄR BÖRJAR MonsterWave
        private int spawnCountDown;
        private int currentSpawnPointer = 0;
        private int afterNext;
        private bool done;
        private bool complete = false;
        private MobWaveEntry first;
        private GameSession session;
        private List<GameObject> wave = new List<GameObject>();
        private List<MobWaveEntry> Entrys = new List<MobWaveEntry>();

        //kolla över inte 100%
        public MobWave (GameSession ses)
        {
            session = ses;
        }
        //adds a waveentry to the entrys list
        public void addEntry (MobWaveEntry entrys)
        {
            Entrys.Add(entrys);
        }
        // check if wave is complete (over)
        public bool IsWaveOver()
        {
            return complete;
        }
        //kolla övet inte 100%
        public void CreateWave()
        {
            while (!done) //while all entries are not done we'll keep looping
            {
                first = null;
                foreach (MobWaveEntry entry in Entrys) //iterate though the entry list
                {
                    if (first == null && !(entry.isFinished()))
                    {
                        first = entry;
                    }
                    if ((first != null) && (entry.GetNextSpawn() < first.GetNextSpawn()) && (!(entry.isFinished())))
                    //if the entry we check with is smaller then the one we have, we replace the one we have with the smaller one
                    {
                        first = entry;
                    }
                }
                afterNext = first.GetNextSpawn();

                wave.Add(new WaitObject(afterNext)); //adds a wait object with the time we choose above
                foreach (MobWaveEntry entry in Entrys)//iterates through the list reducing the time to next 
                {
                    entry.SpendTime(afterNext);//spawn by the amount added in the wait object
                }
                wave.Add(first.SpawnNext());//adds the chosen monster to the arrayList
                done = true; //checks if all entrys are done if not, sets "done" to false
                foreach (MobWaveEntry entry in Entrys)
                {
                    if (!(entry.isFinished()))
                    {
                        done = false;
                    }
                }
            }
        }
        //runs the created monster list: goes through the list of monsters and waitObjects spawning and waiting
        public void Update()
        {
            if (currentSpawnPointer < wave.Count)
            {
                spawnCountDown--;
                //if the counter is done and the object to handle is a waitObject 
                if ((spawnCountDown <= 0) && wave[currentSpawnPointer].typeID == 99)
                {

                    //sets the counter to the time found in our wait object
                    WaitObject tmp = (WaitObject)wave[currentSpawnPointer];
                    spawnCountDown = tmp.waitTime;
                    currentSpawnPointer++;
                    //if the counter is done and the object to be handled is a monster
                }
                else if (spawnCountDown <= 0)
                {
                    this.session.listToPrint.Add((Monster)wave[currentSpawnPointer]);
                    currentSpawnPointer++;
                }
            }
            else
            {
                complete = true;
            }
        }
    }
}
