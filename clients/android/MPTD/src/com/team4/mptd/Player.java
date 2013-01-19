package com.team4.mptd;

public class Player {
	private int id;
	private double playerHP;
	private int playerGold;
	private int mobWaveCount;
	private int defeatedMonsters;
	//private int income; // commented out by Kalle; unused by code
	private boolean isAlive = true;
	private String nick = "Unnamed Player";

	public Player() {
		playerHP = 10;
		playerGold = 100;
		mobWaveCount = 1;
		defeatedMonsters = 0;
	}

	public void defeatedMonster() {
		defeatedMonsters++;
	}

	public int getDefeatedMonsters() {
		return defeatedMonsters;
	}

	public double getPlayerHP() {
		return playerHP;
	}

	public int getPlayerGold() {
		return playerGold;
	}

	public int getMobWaveCount() {
		return mobWaveCount;
	}

	public void adjustPlayerHP(double value) {
		playerHP += value;
		if (playerHP <= 0) {
			isAlive = false;
		}
	}

	public boolean isAlive() {
		return isAlive;
	}

	public void adjustPlayerGold(int value) {
		playerGold += value;
	}

	public void incrementMobWaveCount() {
		mobWaveCount++;
	}

	public int getId() {
		return id;
	}
	
	public void setId(int newID) {
		id = newID;
	}
	
	public String getNick() {
		return nick;
	}
	
	public void setNick(String newNick) {
		nick = newNick;
	}
}
