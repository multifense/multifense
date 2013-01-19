package com.team4.mptd;

public class MobWaveEntry {
	private int nextSpawn;
	private int burstCounter;
	private int waveCounter;
	private int timeBetweenBursts;
	private int monstersPerBurst;
	private Monster monster;
	private boolean finished = false;

	// param list (startTime the time for the first burst to start, _numberOfBursts the number of monster bursts in the wave,
	//_monster the monster object to be sent, _timeBetweenBursts the time between the bursts in the wave)
	public MobWaveEntry(Monster _monster, int startTime, int _timeBetweenBursts, int _numberOfBursts, int _monstersPerBurst){
		nextSpawn = startTime;
		burstCounter = 0;
		waveCounter = _numberOfBursts;
		timeBetweenBursts = _timeBetweenBursts;
		monstersPerBurst = _monstersPerBurst;
		monster = _monster;

		prepareBurst();

	}
	//returns the time to next spawn
	public int getNextSpawn(){
		return nextSpawn;
	}
	
	//reduce the time to nextSpawn with the argument
	public void spendTime(int time){ 
		nextSpawn -= time;
	}

	//spawns a monster 
	public Monster spawnNext(){
		burstCounter--;	//reduces burstCounter by one
		if(burstCounter == 0) {	//if the whole burst has been sent do  following
			prepareBurst();	//calls the prepareBurst method
			nextSpawn = timeBetweenBursts;	//set the time to next spawn to the time between bursts
			finished = (waveCounter < 0);	//if the waveCounter is less then zero the finished boolean is set to true
		}else {
			nextSpawn = (int) (60/monster.getSpeed());	//sets the time to next spawn to 60 frames divided by the monsters speed
		}
		return monster.clone();	//clones the monster and returns the clone
	}

	public void prepareBurst(){
		burstCounter = monstersPerBurst;	//sets burst counter to the number of monsters in a single burst
		waveCounter--;	//reduces the number of remaining bursts with one
	}
	//returns if the wave is complete or not
	public boolean isFinished(){
		return finished;
	}

}
