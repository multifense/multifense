package com.team4.mptd;

import java.io.IOException;
import java.io.PrintStream;
import java.net.Socket;
import java.net.SocketTimeoutException;
import java.util.ArrayList;
import java.util.HashMap;

import android.util.Log;

public class MultiplayerGameSession extends GameSession {
	private Socket mySocket = null;
	private PrintStream os;
	private String hostName;
	private ArrayList<String> buffer;
	private String playerName;
	private int hostPort;
	private TCPListener tcpListener;
	private boolean mpDataToRead = false;
	private MPMobWave mpWave;
	private HashMap<Integer, Player> opponents;

	public MultiplayerGameSession(String nick) {
		playerName = nick;
	}

	public void init(GameActivity activity, Boolean multiplayer, int mapID) {
		super.init(activity, multiplayer, mapID);

		// initiate opponen map
		opponents = new HashMap<Integer, Player>();

		this.getGameGUI().setMultiplayer(true);
		// set wait state
		gameScreen.setScreenState(TypeID.CONNECT_SCREEN);

		mpWave = new MPMobWave(this);
		multiplayer = true;
		this.getGameGUI().multiplayer();
		buffer = new ArrayList<String>();
//		 initConnection("83.179.38.37", 1337);
		initConnection("130.229.154.15", 1337);

	}

	public void initConnection(String host, int port) {
		hostName = host;
		hostPort = port;
		Log.i("Output", "Trying to connect to " + host + " on port " + port);
		try {
			mySocket = new Socket(hostName, hostPort);
			os = new PrintStream(mySocket.getOutputStream());
			// Start tcp listener thread
			tcpListener = new TCPListener(this, mySocket);
			tcpListener.start();
			Log.i("Output", "Connected..");
			// display searching screen and all players as they connect
			gameScreen.setScreenState(TypeID.CONNECT_SCREEN);
			// register name with kernel & self
			Log.i("Output", Thread.currentThread().toString());
			getPlayer().setNick(playerName);
			kernel.setNickname(playerName);
			Log.i("Output", "Player name registered");
			kernel.findGame(0); // 2 = 2 players. TODO:fix later. 0 = first game room found
		} catch (SocketTimeoutException ex) {
			// timeout. return to menu.
			quitGame(TypeID.LOSE_SCREEN);
		} catch (IOException e) {
			System.out.println("error in initializer " + e);
		}
	}

	// //PLAYER RELATED CODE
	public Player getOpponentById(int playerID) {
		return opponents.get(new Integer(playerID));
	}

	public ArrayList<Player> getOpponentList() {
		return new ArrayList<Player>(opponents.values());
	}

	public void addOpponent(Player p) {
		opponents.put(new Integer(p.getId()), p);
	}
	
	// this method takes a monster object and evaluates wheter or not local player can send it.
	public void trySendMonster(Monster m) {
		if (m.getCost() > player.getPlayerGold()) {
			return;
		}
		player.adjustPlayerGold(-m.getCost());
		kernel.didRecruitMonster(m.getTypeID(),
				(int) m.getMaxHealth());
		// eco.setMonsterIncomeIncrease(m);
		kernel.updateMPIncomeForBuyingMonster(m.getTypeID());
	}

	public void removeOpponentByPlayerID(int playerID) {
		opponents.remove(new Integer(playerID));
	}

	// public Collection<Player> getOpponentList() {
	// return opponents.values();
	// }
	// /// END PLAYER RELATED CODE

	@Override
	public void startGame() {
		gameThread.setRunning(true);
		gameThread.start();
		gameGUI.addTextToShow("Game Start!");
		soundManager.playBackgroundMusic();
	}

	// when quitting game we have to disconnect from the server
	@Override
	public void quitGame(int result) {
		try {
			if (mySocket != null && mySocket.isConnected()) {
				mySocket.close();
			}
//			tcpListener.join();
		} catch (IOException e) {
			Log.i("Output", "exception on quit " + e);
		} 
//		catch (InterruptedException e) {
//			Log.i("Output", "fail on waiting tcplistener");
//		}
		super.quitGame(result);
	}

	public synchronized void addMultiplayerData(String message) {
		synchronized (this) {
			Log.i("Output", "[Net data] - " + message);
			buffer.add(message + "\r\n");
			mpDataToRead = true;
		}
	}

	public synchronized void updateMultiplayerData() {
		synchronized (this) {
			if (mpDataToRead) {
				for (String s : buffer) {
					kernel.didRead(s);
				}
				buffer.clear();
				mpDataToRead = false;
			}
		}
	}

	public void setSpawnTimer(int time) {
		mpWave.setSpawnTimer(time);
	}

	public void updateMpWave() {
		mpWave.update();
	}

	
	public void spawnMonster(int monsterID, int hp, int owner) {

		mpWave.addMonsterToNextWave(monsterID, hp, owner);
	}
	
	public void monsterPassedThrough(Monster monster) {
			kernel.didRecruitMonster(monster.getTypeID(),
					(int) monster.getHealth());
			kernel.didTakeDamage(1, monster.getOwner());
			Player culprit = opponents.get(new Integer(monster.getOwner()));
			culprit.adjustPlayerHP(culprit.getPlayerHP() + 1.0);
			// [_me.osv updatePlayer:culprit];
			
			super.monsterPassedThrough(monster);
	}

	public void sendData(String s) {
		if (mySocket != null)
			if (mySocket.isConnected()) {
				Log.i("Output", "trying to send " + s);

				os.print(s);

			}
	}
	
	public MPMobWave getMPMobWave() {
		return mpWave;
	}

	public boolean isConnected() {
		if (mySocket != null) {
			return mySocket.isConnected();
		}
		return false;
	}

	public boolean dataToRead() {
		return mpDataToRead;
	}

}
