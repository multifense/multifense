package com.team4.mptd;

import java.util.ArrayList;

class MonsterINFO{
	public int id;
	public int hp;
	public int owner;
	public MonsterINFO(int id, int hp, int owner){
		this.id = id;
		this.hp = hp;
		this.owner = owner;
	}
}
public class MPMobWave {
	private boolean isWaveOver;
	private boolean didKillLast;
	private ArrayList<MonsterINFO> nextMonsterWave = new ArrayList<MonsterINFO>();
	private MultiplayerGameSession session;
	private int timer = 1;
	private int timeToWaveCounter = 0;
	public int waveNumber = 0;
	public int monstersInThisWave = 0;

	public int getTimeToWave() {
		return (timer - timeToWaveCounter) / 30;
	}

	public MPMobWave(MultiplayerGameSession session){
		this.session = session;
	}

	public void addMonsterToNextWave(int monsterID, int hp, int owner) {
		nextMonsterWave.add(new MonsterINFO(monsterID,hp,owner));
	}
	public void spawnNextMobWave(){
		waveNumber++;
		int counter = 0;
		for(MonsterINFO moInfo : nextMonsterWave){
			counter++;
			Monster mo = session.getMonsterFromType(moInfo.id);
			mo.takeDamage(mo.getHealth() - moInfo.hp);
			mo.setOwner(moInfo.owner);
			mo.moveDistance(-counter * 60);
			session.addMonster(mo);
		}
		nextMonsterWave.clear();
	}

	public void setSpawnTimer(int timeToNextWave){
		isWaveOver = true;
		didKillLast = false;
		timer = timeToNextWave;
		waveNumber++;
		timeToWaveCounter = 0;
		session.getPlayer().adjustPlayerGold(session.getKernel().mpIncome());
	}
	public void update(){
		timeToWaveCounter++;
		if(isWaveOver){
			if(timeToWaveCounter > timer){
				isWaveOver = false;
				if (nextMonsterWave.size() == 0) {
					session.kernel.didKillLastMonster();
					didKillLast = true;
				}
				spawnNextMobWave();
				timeToWaveCounter = 0;
			}
		} else if (! didKillLast) {
	        if (session.getMonsters().size() == 0) {
	            session.kernel.didKillLastMonster();
	            didKillLast = true;
	        }
		}
	}

	public boolean isWaveOver(){
		return isWaveOver;
	}
}
