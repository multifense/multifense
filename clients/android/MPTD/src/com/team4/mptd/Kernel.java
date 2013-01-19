package com.team4.mptd;

import android.graphics.Point;
import android.util.Log;

public class Kernel {

    static {
    	try{
    		System.loadLibrary("kernel");
    	} catch (java.lang.ExceptionInInitializerError e) {
    		System.out.println("unable to load shared module: " + e);
    		throw e;
    	}
	}
    
    public Kernel(GameSession session, int mapID) {
    	this.session = session;
    	loadMapData(mapID);
    	this.session.getMap().preparePath();
    	loadData();
    }
    
	// delegate methods
    public native void loadData();
    public native void setNickname(String nick);
    public native void didRead(String s);
    public native void didCreateTower(int tower, int type, int x, int y);
    public native void didSpawnMonster(int monster, int type);
    public native void didKillMonster(int monster);
    public native void didKillLastMonster();
    public native void didRecruitMonster(int type, int health);
    public native void didSurrender();
    public native void didTakeDamage(int damage, int culprit);
    public native void didDie();
    public native void findGame(int players);
    public native void sharedModuleSelfTest();
    public native void newGame();
    
    private GameSession session;
    
    // kernel 1.2
    public native void loadMapData(int mapid);
    public native String getNameForMap(int mapid);
    public native String getImageNameForMap(int mapid);
    public native int updateSPIncome(int waveCount);// (economy) update single player wave count 
    public native int getBountyForMonster(int type);// (economy) how much $ should player get for killing mob
    public native int updateMPIncomeForBuyingMonster(int type);// (economy) increase and return new income for buying mob
    public native int mpIncome();
    public native int spIncome();
    
    // delegate callbacks
    public void monsterDefinition(int type, String sprite, int health, int speed, int costToSend, int incomeIncrease, int coloring) {
    	
    }

    public void towerDefinition(int type, String sprite, int damage, int timeBetweenShots, int range, int cost, String projectileSprite, int projectileSpeed, String projectileSound) {
    }

    public void victorWasDecided(int player) {
    	// playerid == 0 if the victor was me, otherwise it is the id of the winner
    	if (player == 0) {
            // WE WON! FOR GREAT JUSTICE!!!!
    		session.addGameText("WE WIN! FOR GREAT JUSTICE!");
    		session.pauseGame(TypeID.WIN_SCREEN);
        } else {
            // DEFEAT!!!!
        	session.addGameText("WE HAVE LOST!");
        	session.pauseGame(TypeID.LOSE_SCREEN);
        }
    	// TODO: this code should update the screen to show the appropriate win/lose screen
    }

    public void towerWasCreated(int player, int tower, int type, int x, int y) {
	
    }
    
    public void monsterWasCreated(int player, int monster, int type) {
    	
    }
    
    public void monsterWasKilled(int monster) {
    	
    }
    
    public void waveWasKilledForPlayer(int player) {
    	Player p = ((MultiplayerGameSession)session).getOpponentById(player);
    	session.addGameText(p.getNick() + " killed off their wave.");
    }
    
    public void playerWasDamagedByPlayer(int player, int opponent, int damage) {
    	// opponent == 0 if the culprit (the person causing the grief) was me; in which case I should get health ++
        Player taker = ((MultiplayerGameSession)session).getOpponentById(player);
        
        // if opponent is zero, it means WE dealt damage to the player; we thus get player if that is the case, otherwise we grab the opponent in question
        Player giver = opponent == 0 ? session.getPlayer() : ((MultiplayerGameSession)session).getOpponentById(opponent);

        // same thing here, we switch the nick for  "you"  if it's us dealing the hurt
        session.addGameText(taker.getNick() + " took " + damage + " damage because of " + (opponent == 0 ? "you" : giver.getNick()));

        if (opponent == 0) {
        	// we up our HP as we pwned someone

        	giver.adjustPlayerHP(damage);
        }
        
        // now we loop through all the remaining players and adjust their HP accordingly
        for (Player p : ((MultiplayerGameSession)session).getOpponentList()) {
            if (p.getId() == player) {
                // p took damage
            	p.adjustPlayerHP(- damage);
            }
            if (p.getId() == opponent) {
                // p dealt damage
                p.adjustPlayerHP( + damage);
            }
        }
    }
    
    public void playerDied(int player) {
    	Player p = ((MultiplayerGameSession)session).getOpponentById(player);
    	session.addGameText(p.getNick() + " has died!");
    }
    
    public void playerSurrendered(int player) {
    	Player p = ((MultiplayerGameSession)session).getOpponentById(player);
    	session.addGameText(p.getNick() + " has surrendered!");
    }
    
    public void monsterWithHealthWasSentByPlayer(int monster, int health, int player) {
    	Player p = ((MultiplayerGameSession)session).getOpponentById(player);
    	session.addGameText(p.getNick() + " sent a monster with health " + health + " to you!");
    	((MultiplayerGameSession)session).spawnMonster(monster, health, player);
    }
    
    public void netWrite(String s) {
    	Log.i("Output","writing to network : " + s);
    	((MultiplayerGameSession)session).sendData(s);
    }
    
    public void playerJoined(int player, String name) {
    	Log.i("Output", "Player joined: " + name + " (id " + player + ")");
    	session.addGameText(name + " joined the game.");
    	Player p = new Player();
    	p.setNick(name);
    	p.setId(player);
    	((MultiplayerGameSession)session).addOpponent(p);
    }
    
    public void playerLeft(int player) {
    	Player p = ((MultiplayerGameSession)session).getOpponentById(player);
    	session.addGameText(p.getNick() + " left the game.");
    	((MultiplayerGameSession)session).removeOpponentByPlayerID(player);
    }
    
    public void didFindGame(int myPlayerId, int startsInSeconds) {
    	Log.i("Output", "Found a game (starts in " + startsInSeconds + " seconds.");
    	// start 10 sec countdown and then start game.
    	session.getGameScreen().setScreenState(TypeID.GETREADY_SCREEN_MP); // state 4 = start countdown
    }
    
    public void nextWaveTime(int timeUntilNextWave) {
    	((MultiplayerGameSession)session).setSpawnTimer(timeUntilNextWave * 30);
        Log.i("Output", "Next wave starts in " + timeUntilNextWave + " seconds!");
        session.addGameText("Next wave will begin in " + timeUntilNextWave + " seconds!");
    }
    
    public void debugLog(String s) {
    	if (s == null) {
    		Log.i("Output", "debugLog called with nil argument!");
    	} else {
    		session.addGameText("[kernel debug]: " + s);
    	}  
    }

    // path definition: x, y, dirx, diry, length
    public void pathDefinition(int x, int y, int dirx, int diry, int length) {
    	this.session.getMap().getPath().addCodedPath(new Point(x, y), new Point(dirx, diry), length);
    }

}

