package com.team4.mptd;

import java.util.ArrayList;

public class MobWave {
	private int spawnCountdown;
	private int currentSpawnPointer = 0;
	private int afterNext;
	private boolean done;
	private boolean complete = false;
	private MobWaveEntry first;
	private GameSession session;
	private ArrayList<GameObject> wave = new ArrayList<GameObject>();
	private ArrayList<MobWaveEntry> entrys = new ArrayList<MobWaveEntry>();

	public MobWave(GameSession ses){
		session = ses;
	}
	public void addEntry(MobWaveEntry entry) {
		entrys.add(entry);	//adds a waveentry to the entrys list
	}
	public boolean isWaveOver(){
		return complete;	//returns if the wave is complete or not
	}

	public void createWave(){

		while(!(done)){ //while not all entrys are done we'll keep looping
			first = null;
			for(MobWaveEntry entry : entrys) {	//iterate though the entry list
				if(first == null && !(entry.isFinished())) {
					first = entry;
				}
				if((first != null) && (entry.getNextSpawn() < first.getNextSpawn()) && (!(entry.isFinished()))){	//if the entry we check with is smaller then the one we have,
					//we replace the one we have with the smaller one
					
					first = entry;
				}
			}
			afterNext = first.getNextSpawn();
	
			wave.add(new WaitObject(afterNext));	//adds a wait object with the time we choose above
			
			for(MobWaveEntry entry : entrys) {	//iterates through the list reducing the time to next 
				entry.spendTime(afterNext);//spawn by the amount added in the wait object
			}

			wave.add(first.spawnNext());	//adds the chosen monster to the arrayList

			

			done = true;	//checks if all entrys are done if not, sets "done" to false
			for(MobWaveEntry entry : entrys) {
				if(!(entry.isFinished())){
					done = false;
				}
			}
		}
	}
	//runs the created monster list: goes through the list of monsters and waitObjects spawning and waiting 
	public void update(){
		if(currentSpawnPointer < wave.size()){
			spawnCountdown--;
			//if the counter is done and the object to handle is a waitObject 
			if((spawnCountdown <= 0) && (wave.get(currentSpawnPointer).getTypeID() == 99)) {
				//sets the counter to the time found in our wait object
				spawnCountdown =((WaitObject) wave.get(currentSpawnPointer)).getWaitTime();
				currentSpawnPointer++;
				//if the counter is done and the object to be handled is a monster
			}else if(spawnCountdown <= 0){
				session.addMonster((Monster)(wave.get(currentSpawnPointer)));
				currentSpawnPointer++;
			}
		}else {
			complete = true;
		}
	}
	//+++++++++++++++++TEST STUFF++++++++++++++++++
	public ArrayList<GameObject> getWaveList(){
		return wave;
	}
}
