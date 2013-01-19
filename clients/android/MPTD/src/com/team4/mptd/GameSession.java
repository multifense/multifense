package com.team4.mptd;

import java.util.ArrayList;
import java.util.HashMap;

import android.app.Activity;
import android.content.Intent;
import android.content.res.Resources;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Rect;
import android.util.Log;
import android.view.Display;
import android.view.GestureDetector;
import android.view.MotionEvent;
import android.view.View;
import android.view.View.OnTouchListener;

public class GameSession {

	protected int mapID;

	private GamePanel gamePanel;
	private ArrayList<GameObject> monsters;
	private ArrayList<GameObject> towers;
	private ArrayList<GameObject> projectileList;
	private ArrayList<GameObject> trashList;
	private ArrayList<GameObject> oppMonsters;
	private ArrayList<GameObject> oppTowers;
	private ArrayList<GameObject> oppProjList;
	private ArrayList<GameObject> oppTrashList;
	private ArrayList<MobWave> mobWaves;
	private Map map;
	protected GameThread gameThread;
	protected Bitmap mapBitmap;
	protected Bitmap towerBmap;
	protected Bitmap mgcTowerBmap;
	protected Bitmap frTowerBmap;
	protected Bitmap iceTowerBmap;
	protected Bitmap hattyBmap;
	protected Bitmap rockyBmap;
	protected Bitmap monsterBmap;
	protected Bitmap bombMonsterBmap;
	protected Bitmap cannonProjectileBmap;
	protected Bitmap mgcProjectileBmap;
	protected Bitmap frProjectileBmap;
	protected Bitmap iceProjectileBmap;
	private GestureDetector gDetect;
	private float offsetX;
	private float offsetY;
	private int monsterWaveNumber = 0;
	private int screenWidth;
	private int screenHeight;
	protected Player player;
	private Economy eco;
	protected GameGui gameGUI;
	private Tower myCurrentTower;
	private HashMap<Integer, Tower> towerMap;
	protected HashMap<Integer, Monster> monsterMap;
	protected GameActivity activity;
	protected SoundManager soundManager;
	protected Kernel kernel;
	protected boolean multiplayer = false;
	private boolean isOppView = false; // initialize to false.
	private boolean isBeeingViewed = false;
	protected GameScreen gameScreen;

	public GameSession() {

	}

	public void init(GameActivity gameActivity, boolean multiplayer, int mapID) {
		// mode defines what kind of gamemode is selected and running, used to
		// determine whether or not it should try to update
		// multiplayer specific parts
		this.multiplayer = multiplayer;

		this.mapID = mapID;

		activity = gameActivity;

		// get device screen width and height
		Display display = activity.getWindowManager().getDefaultDisplay();
		screenWidth = display.getWidth();
		screenHeight = display.getHeight();

		// Create map
		map = new Map(screenWidth, screenHeight);

		// initialize shared kernel
		kernel = new Kernel(this, mapID);

		// initialize the local player arraylists
		mobWaves = new ArrayList<MobWave>();
		monsters = new ArrayList<GameObject>();
		towers = new ArrayList<GameObject>();
		projectileList = new ArrayList<GameObject>();
		trashList = new ArrayList<GameObject>();

		// initialize the opponent player arraylists
		oppMonsters = new ArrayList<GameObject>();
		oppTowers = new ArrayList<GameObject>();
		oppProjList = new ArrayList<GameObject>();
		oppTrashList = new ArrayList<GameObject>();

		// initialize gamescreen
		gameScreen = new GameScreen(0, 0, screenWidth, screenHeight, 1,
				activity.getResources(), this);
		// gameScreen.setScreenState(GameScreen.LOADING_SCREEN);
		// Initialize player
		player = new Player();

		// Initialize Economy
		eco = new Economy();

		// initialize map

		// initialize sound manager
		soundManager = new SoundManager();
		soundManager.initSounds(activity);

		// // +++++++++++++++++++++++++++++++++++++++++
		// // initialize bitmaps
		Resources res = activity.getResources();
		// switch (mapID) {
		// case 2: {
		// mapBitmap = Bitmap.createScaledBitmap(
		// BitmapFactory.decodeResource(res, R.drawable.awsomemap),
		// 1600, 960, false);
		// break;
		// }
		// default: {
		// mapBitmap = Bitmap.createScaledBitmap(BitmapFactory
		// .decodeResource(res, R.drawable.map1600x960new2), 1600,
		// 960, false);
		// break;
		// }
		//
		// }
		//
		// towerBmap = Bitmap.createScaledBitmap(
		// (BitmapFactory.decodeResource(res, R.drawable.cannontower)),
		// 50, 50, false);
		//
		// cannonProjectileBmap = Bitmap.createScaledBitmap(
		// (BitmapFactory.decodeResource(res, R.drawable.bullet)), 25, 25,
		// false);
		//
		// mgcTowerBmap = Bitmap.createScaledBitmap(
		// (BitmapFactory.decodeResource(res, R.drawable.magictower)), 50,
		// 50, false);
		//
		// mgcProjectileBmap = Bitmap.createScaledBitmap(
		// (BitmapFactory.decodeResource(res, R.drawable.magicproj)), 25,
		// 25, false);
		//
		// frTowerBmap = Bitmap.createScaledBitmap(
		// (BitmapFactory.decodeResource(res, R.drawable.firetower)), 50,
		// 50, false);
		//
		// frProjectileBmap = Bitmap.createScaledBitmap(
		// (BitmapFactory.decodeResource(res, R.drawable.fireprojsprite)),
		// 100, 25, false);
		//
		// iceTowerBmap = Bitmap.createScaledBitmap(
		// (BitmapFactory.decodeResource(res, R.drawable.igloo)), 50, 50,
		// false);
		//
		// iceProjectileBmap = Bitmap.createScaledBitmap(
		// (BitmapFactory.decodeResource(res, R.drawable.igloproj)), 25,
		// 25, false);
		//
		// bombMonsterBmap = Bitmap.createScaledBitmap(
		// (BitmapFactory.decodeResource(res, R.drawable.bombsprite)),
		// 200, 200, false);
		//
		// monsterBmap = Bitmap.createScaledBitmap(
		// (BitmapFactory.decodeResource(res, R.drawable.slimesprite)),
		// 200, 200, false);
		//
		// hattyBmap = Bitmap.createScaledBitmap(
		// (BitmapFactory.decodeResource(res, R.drawable.hatty)), 200,
		// 200, false);
		//
		// rockyBmap = Bitmap.createScaledBitmap(
		// (BitmapFactory.decodeResource(res, R.drawable.rocky)), 200,
		// 200, false);

		// ++++++++++++++++++++++++++++++++++++++++
		// ++++++++++++++++++++++++++++++++++++++++
		// ++++++++++++++++++++++++++++++++++++++++
		// ++++++++++++++++++++++++++++++++++++++++
		// INITIATE BITMAPS
		InitRes bm = InitRes.getSession();
		bm.init(this);

		// GestureDetect object to handle any touch events. Takes GameScroller
		// object as callback.
		gDetect = new GestureDetector(new GameListener(this));

		// ++++++++++++++++++++++
		// initializze HashMap for towers
		// ++++++++++++++++++++++
		towerMap = new HashMap<Integer, Tower>();

		// param list: (posx, posy, firerrate, range,
		// bitmap for tower, cost of tower,
		// frame width, frame height, number of frames, ID of the tower type,
		// the towers arc-projectile)
		// arc-projectile param list: (proj posx, proj posy, damage, proj speed,
		// target monster(not used in arc-type), proj bmap, frame width, height,
		// number of grames)

		// set cannon tower to key 0
		Tower tempTwr = new Tower(0, 0, 30, 200, towerBmap, 15, 50, 50, 1,
				TypeID.TOWER_CANNON, new TrackingProjectile(0, 0, 10, 15, null,
						cannonProjectileBmap, 25, 25, 1));
		towerMap.put(TypeID.TOWER_CANNON, tempTwr); // mapping for cannon tower

		// set mgcTower to key 1
		tempTwr = new Tower(0, 0, 75, 300, mgcTowerBmap, 25, 50, 50, 1,
				TypeID.TOWER_MAGIC, new TrackingProjectile(0, 0, 25, 30, null,
						mgcProjectileBmap, 25, 25, 1));

		towerMap.put(TypeID.TOWER_MAGIC, tempTwr); // mapping for mgc tower

		// set frTower to key 2
		tempTwr = new Tower(0, 0, 3, 150, frTowerBmap, 125, 50, 50, 1,
				TypeID.TOWER_FIRE, new TrackingProjectile(0, 0, 7, 15, null,
						frProjectileBmap, 25, 25, 4));

		towerMap.put(TypeID.TOWER_FIRE, tempTwr); // mapping for fire tower

		tempTwr = new Tower(0, 0, 90, 350, iceTowerBmap, 90, 50, 50, 1,
				TypeID.TOWER_ICE, new TrackingProjectile(0, 0, 100, 40, null,
						iceProjectileBmap, 25, 25, 1));

		towerMap.put(TypeID.TOWER_ICE, tempTwr); // mapping for fire tower

		// ++++++++++++++++++++++
		// initializze HashMap for Monsterv TODO:
		// ++++++++++++++++++++++
		monsterMap = new HashMap<Integer, Monster>();

		Monster tempMon;

		// Monster(double hpMod, double armorMod, double speedMod,Bitmap sprite,
		// GamePath path, int frWidth, int frHeight, int noFrames)
		/*
		 * MonsterDefinition(101, "bombsprite.png", 300, 3, 20, 4, 0);
		 * MonsterDefinition(102, "slimesprite.png", 75, 3, 5, 1, 0);
		 * MonsterDefinition(103, "slimesprite.png", 170, 4, 12, 2, 1);
		 * MonsterDefinition(104, "slimesprite.png", 2100, 3, 110, 18, 2);
		 * MonsterDefinition(105, "bombsprite.png", 3000, 3, 150, 25, 3);
		 */
		// Set monster BOMB
		tempMon = new Monster(3, 1, 3, bombMonsterBmap, map.getMonsterPath(),
				50, 50, 4);
		tempMon.setTypeID(TypeID.MONSTER_BOMB);
		tempMon.setCost(20);
		monsterMap.put(101, tempMon);

		// Set monster SLIME
		tempMon = new Monster(0.75, 1, 3, monsterBmap, map.getMonsterPath(),
				50, 50, 4);
		tempMon.setTypeID(TypeID.MONSTER_SLIME);
		tempMon.setCost(5);
		monsterMap.put(102, tempMon);

		// Set monster SLIME_FAST
		tempMon = new Monster(1.7, 1, 4, monsterBmap, map.getMonsterPath(), 50,
				50, 4);
		tempMon.setTypeID(TypeID.MONSTER_SLIME_FAST);
		tempMon.setCost(12);
		monsterMap.put(103, tempMon);

		// Set monster HATTY
		tempMon = new Monster(21, 1, 3, hattyBmap, map.getMonsterPath(), 50,
				50, 4);
		tempMon.setTypeID(TypeID.MONSTER_HATTY);
		tempMon.setCost(110);
		monsterMap.put(104, tempMon);

		// Set monster ROCKY
		tempMon = new Monster(30, 1, 3, rockyBmap, map.getMonsterPath(), 50,
				50, 4);
		tempMon.setTypeID(TypeID.MONSTER_ROCKY);
		tempMon.setCost(150);
		monsterMap.put(105, tempMon);

		// ++++++++++++++++++++++
		// END HASHMAP INITIALIZATION
		// ++++++++++++++++++++++

		// Initialize GUI

		gameGUI = new GameGui(0, 0, screenWidth, screenHeight, res, this,
				towerMap, monsterMap);

		// Start gamethread
		gameThread = new GameThread(this);

		if (!multiplayer) {
			// set initial countdown to start game
			gameScreen.setScreenState(TypeID.GETREADY_SCREEN);
		}
	}

	// setGamePanel displays appropriate pre/end-game screen.
	public void setGamePanel(GamePanel panel) {
		gamePanel = panel;
		gameThread.setSurfaceHolder(panel.getSurfaceHolder());
		// touch listener for GamePanel
		gamePanel.setOnTouchListener(new OnTouchListener() {
			public boolean onTouch(View v, MotionEvent e) {
				// propagate the event to the gesturedetect listener.
				gDetect.onTouchEvent(e);
				// check if this was an on end of scroll event
				if ((e.getAction() == MotionEvent.ACTION_UP)
						&& (gameGUI.isDragState())) {
					// scroll in progress has finished
					// call appropriate methods
					myCurrentTower = gameGUI.getMyCurrentTower();
					buildTower();
					// reset state of buildmenu
					gameGUI.setDragState(false);
				}
				return true;
			}
		});
	}

	// setOpponentView(boolean val) sets the mode of isOppView variable.
	public void setOpponentView(boolean val) {
		isOppView = val;
	}

	// return view mode (player/opponent)
	public boolean isOpponentView() {
		return isOppView;
	}
	
	//is set to true when a player is viewing once screen
	public void setIsViewed(boolean val) {
		isBeeingViewed = val;
	}
	
	//returns true if the players screen is beeing viewed
	public boolean isViewed() {
		return isBeeingViewed;
	}

	// ++++++++++++++++++++
	// START GAME LIFCYCLE
	// ++++++++++++++++++++
	// Following are lifecycle methods controling the gamethread
	// Start gamethread. called by gamepanel when ready to start drawing to
	// panel.

	public void startGame() {
		gameThread.setRunning(true);
		gameThread.setSurfaceHolder(gamePanel.getSurfaceHolder());
		gameThread.start();

		gameGUI.addTextToShow("Game Start!");
		soundManager.playBackgroundMusic();
	}

	public void pauseGame(int result) {
		// pause game and display result screen
		gameScreen.setScreenState(result);
	}

	// stop thread and call activity to finish.
	public void quitGame(int result) {
		// stop the thread
		gameThread.setRunning(false);
		// try {
		// gameThread.join();
		// } catch (InterruptedException e) {
		// Log.i("Output", "gamethread join failed");
		// }
		// stop the music
		soundManager.cleanup();

		// clear kernel
		kernel = null;

		// mark all bitmaps for cleanup
		// for(GameObject obj : getObjectsToDraw()) {
		// obj.getBitmap().recycle();
		// }

		// // create intent with data on how game finished
		Intent i = new Intent();
		i.putExtra("result", result);

		activity.setResult(Activity.RESULT_OK, i);
		activity.finish();
	}

	// ++++++++++++++++++++
	// END GAME LIFE CYCLE
	// ++++++++++++++++++++
	public void toggleOppView() {
		//togglesOpponent view on/off
		if (isOpponentView()) {
			isOppView = false;
		} else {
			isOppView = true;

		}
	}

	public void playSound(int soundId) {
		soundManager.playSound(soundId);
	}

	// returns the SoundManager object
	public SoundManager getSoundManager() {
		return soundManager;
	}

	public int getScreenWidth() {
		return screenWidth;
	}

	public int getScreenHeight() {
		return screenHeight;
	}

	public boolean getMultiplayer() {
		return multiplayer;
	}

	public GameScreen getGameScreen() {
		return gameScreen;
	}

	// return view that
	public GamePanel getGamePanel() {
		return gamePanel;
	}

	// return the running gamethread TODO: only used by test
	public GameThread getGameThread() {
		return gameThread;
	}

	// return the arraylist of monsters
	public ArrayList<GameObject> getMonsters() {
		return monsters;
	}

	// return the arraylist of towers
	public ArrayList<GameObject> getTowers() {
		return towers;
	}

	// Return the trash list
	public ArrayList<GameObject> getTrashList() {
		return trashList;
	}

	// Return the list of projectiles
	public ArrayList<GameObject> getProjectileList() {
		return projectileList;
	}

	// +++++++++++++Opponent getter methods++++++++++++++
	// ++++++++++++++++++++++++++++++++++++++++++++++++++
	// return the arraylist of monsters
	public ArrayList<GameObject> getOppMonsters() {
		return oppMonsters;
	}

	// return the arraylist of towers
	public ArrayList<GameObject> getOppTowers() {
		return oppTowers;
	}

	// Return the trash list
	public ArrayList<GameObject> getOppTrashList() {
		return oppTrashList;
	}

	// Return the list of projectiles
	public ArrayList<GameObject> getOppProjectileList() {
		return oppProjList;
	}

	// return map of the game session
	public Map getMap() {
		return map;
	}

	public Bitmap getMonsterBmap() {
		return monsterBmap;
	}

	// return the player object
	public Player getPlayer() {
		return player;
	}

	public Economy getEconomy() {
		return eco;
	}

	public float getOffsetX() {
		return offsetX;
	}

	public float getOffsetY() {
		return offsetY;
	}

	// returns the current src rect for a drawBitmap call on the map
	// This rect is a representation of the device screen.
	public Rect getCurrentRect() {
		return new Rect((int) offsetX, (int) offsetY, screenWidth
				+ (int) offsetX, screenHeight + (int) offsetY);
	}

	public GameGui getGameGUI() {
		return gameGUI;
	}

	public Kernel getKernel() {
		return kernel;
	}

	// ++++++++++++++++++++

	// return an arraylist<GameObjects> of all objects to draw.
	public ArrayList<GameObject> getObjectsToDraw() {
		ArrayList<GameObject> temp = new ArrayList<GameObject>();
		if (gameScreen.getWaiting()) {
			temp.add(gameScreen);
			return temp;
		}

		// add map to list.
		temp.add(map);

		// temporary arrays
		ArrayList<GameObject> tempMon;
		ArrayList<GameObject> tempTow;
		ArrayList<GameObject> tempProj;

		// check what mode is active
		if (isOppView) { // OV view active. only return opponent objects.
			tempMon = getOppMonsters();
			tempTow = getOppTowers();
			tempProj = getOppProjectileList();
		} else {
			tempMon = getMonsters();
			tempTow = getTowers();
			tempProj = getProjectileList();
		}

		// Add all objects to return list.
		// add monsters to list
		for (GameObject monster : tempMon) {
			assert (((Monster) monster).isAlive());
			temp.add(monster);
		}
		// add towers to list
		for (GameObject tower : tempTow) {
			temp.add(tower);
		}

		// add projectiles to list
		for (GameObject proj : tempProj) {
			temp.add(proj);
		}

		// add GUI element
		temp.add(gameGUI);
		return temp;
	}

	// ++++++++++++++++++++
	// Following methods contains all the code to keep track on the Rect to draw
	// to canvas
	// ++++++++++++++++++++

	// dragEvent either updates the offsets for correct scrolling, or in case of
	// dragState i buildMenu gui updates its state accordingly
	public void dragEvent(float x, float y, MotionEvent initialEvent) {
		// if in gameScreen do nothing
		if (gameScreen.getWaiting()) {
			return;
		}
		// check if buildmenu open
		if (gameGUI.showsBuildExpanded() && (!gameGUI.isDragState())) {
			// create rect representing the touch area
			Rect touchArea = new Rect((int) initialEvent.getX() - 5,
					(int) initialEvent.getY() - 5,
					(int) initialEvent.getX() + 5,
					(int) initialEvent.getY() + 5);
			// check if player touches valid selection and enter dragstate
			if (gameGUI.getMenuSelection(touchArea)) {
				gameGUI.setDragState(true);
				gameGUI.buildMenuHide();
			}
		}

		if (gameGUI.isDragState()) {
			// update buildmenugui for new position
			gameGUI.updateSelectionPosition(x, y);
		} else {
			// check if new offsets would cause the screen to be moved
			// beyond
			// the
			// map edges. If any one of the x or y is beyond the allowed
			// value,
			// set
			// it to the boundary of the map but allow the other offset to
			// change.
			// System.out.println("OfffsetX: " + offsetX + "new OffsetX: " +
			// x
			// + "OffsetY: " + offsetY + " new OffsetY: " + y);
			if (!((offsetX + x <= 0) || (offsetX + x + screenWidth > map
					.getBitmap().getWidth()))) {
				offsetX += x;
			}
			if (!((offsetY + y <= 0) || (offsetY + y + screenHeight > map
					.getBitmap().getHeight()))) {
				offsetY += y;
			}
		}
	}

	// onTap is called from the GameListener class (gesturedetector callback
	// handler) when it has been
	// confirmed the motion event was a single tap and not a scrolling motion.
	public void onTap(double x, double y) {
		// if in gameScreen check if win/lose screen and quit if so.
		if (gameScreen.getWaiting()) {
			if ((gameScreen.getScreenState() == TypeID.WIN_SCREEN)
					|| (gameScreen.getScreenState() == TypeID.LOSE_SCREEN)) {
				gameScreen.clearScreenState();
			} else {
				return;
			}
		}

		// touchArea represents an area touched by the player.
		Rect touchArea = new Rect((int) x - 10, (int) y - 10, (int) x + 10,
				(int) y + 10);
		// give touchArea to gameGUI to handle any event.
		gameGUI.guiTap(touchArea);
	}

	// clone current myCurrentTower and add to towers list if player can afford
	// it
	public void buildTower() {
		// All collisions using the same bitmap for tower. Meaning collision
		// only works if towers are all of the same size,

		Tower tempTower;

		// check if player affords the tower
		if (myCurrentTower.getCost() > player.getPlayerGold()) {
			return;
		}

		// update myCurrentTower to coordinates in the gamemap

		// Check if tower is already intersecting the selected area or the map
		// path.
		if (collisionDetect(myCurrentTower.getThisRect())
				|| map.intersect(myCurrentTower.getThisRect())) {
			return;
		}
		tempTower = myCurrentTower.clone();
		tempTower.setSession(this);
		synchronized (this) {
			// sets the current selection(myCurrentTower) to x, y then clones it
			// and adds the clone to towers list.
			towers.add(tempTower);
		}
		player.adjustPlayerGold(-myCurrentTower.getCost());

	}

	// check for tower collisions using their bitmaps
	public boolean collisionDetect(Rect checkRect) {

		for (GameObject tower : towers) {

			// Check if the two rectangles intersect. If they do, return false.
			if (Rect.intersects(tower.thisRect, checkRect)) {
				return true;
			}
		}
		return false;
	}

	// adds a monster to the monsters list
	public void addMonster(Monster mon) {
		// set gamesession for monster and add to monsters list.
		mon.setSession(this);
		synchronized (this) {
			monsters.add(mon);
		}
	}

	public Monster getMonsterFromType(int monID) {
		return monsterMap.get(monID).clone();
	}

	// ++++++++++++++++++++
	// Create mobwave for singleplayer
	// ++++++++++++++++++++
	public void setupMobWave() {

		mobWaves.add(new MobWave(this));
		mobWaves.add(new MobWave(this));
		mobWaves.add(new MobWave(this));
		mobWaves.add(new MobWave(this));
		mobWaves.add(new MobWave(this));
		mobWaves.add(new MobWave(this));
		mobWaves.add(new MobWave(this));
		mobWaves.add(new MobWave(this));

		mobWaves.get(0).addEntry(
				new MobWaveEntry(monsterMap.get(102), 0, 0, 1, 1));
		mobWaves.get(0).addEntry(
				new MobWaveEntry(monsterMap.get(102), 120, 100, 3, 2));

		mobWaves.get(1).addEntry(
				new MobWaveEntry(monsterMap.get(102), 0, 280, 2, 3));
		mobWaves.get(1).addEntry(
				new MobWaveEntry(monsterMap.get(101), 200, 250, 2, 1));
		mobWaves.get(1).addEntry(
				new MobWaveEntry(monsterMap.get(101), 500, 0, 1, 1));
		mobWaves.get(1).addEntry(
				new MobWaveEntry(monsterMap.get(102), 550, 0, 1, 2));

		mobWaves.get(2).addEntry(
				new MobWaveEntry(monsterMap.get(101), 0, 120, 2, 1));
		mobWaves.get(2).addEntry(
				new MobWaveEntry(monsterMap.get(102), 20, 80, 2, 3));

		mobWaves.get(3).addEntry(
				new MobWaveEntry(monsterMap.get(103), 0, 170, 2, 3));
		mobWaves.get(3).addEntry(
				new MobWaveEntry(monsterMap.get(102), 20, 140, 2, 4));
		mobWaves.get(3).addEntry(
				new MobWaveEntry(monsterMap.get(102), 300, 0, 1, 2));
		mobWaves.get(3).addEntry(
				new MobWaveEntry(monsterMap.get(102), 480, 0, 1, 3));
		mobWaves.get(3).addEntry(
				new MobWaveEntry(monsterMap.get(102), 500, 0, 1, 5));

		mobWaves.get(4).addEntry(
				new MobWaveEntry(monsterMap.get(101), 0, 190, 2, 6));
		mobWaves.get(4).addEntry(
				new MobWaveEntry(monsterMap.get(103), 60, 230, 2, 5));

		mobWaves.get(5).addEntry(
				new MobWaveEntry(monsterMap.get(104), 0, 0, 1, 1));

		mobWaves.get(6).addEntry(
				new MobWaveEntry(monsterMap.get(105), 0, 0, 1, 1));

		mobWaves.get(7).addEntry(
				new MobWaveEntry(monsterMap.get(105), 0, 0, 1, 10));

		mobWaves.get(0).createWave();
		mobWaves.get(1).createWave();
		mobWaves.get(2).createWave();
		mobWaves.get(3).createWave();
		mobWaves.get(4).createWave();
		mobWaves.get(5).createWave();
		mobWaves.get(6).createWave();
		mobWaves.get(7).createWave();
	}

	public void updateWave() {
		if ((getPlayer().getMobWaveCount() > mobWaves.size())
				&& (getMonsters().size() == 0)) { // is
			// game
			// all monster waves survived! return to menu
			pauseGame(TypeID.WIN_SCREEN);
		}

		if (monsterWaveNumber < mobWaves.size()) {
			mobWaves.get(monsterWaveNumber).update();
			if (mobWaves.get(monsterWaveNumber).isWaveOver()
					&& monsters.size() <= 0) {
				monsterWaveNumber++;
				// int waveIncome =
				// eco.getSPIncome(getPlayer().getMobWaveCount());
				int waveIncome = kernel.updateSPIncome(getPlayer()
						.getMobWaveCount());
				// Updates text displayed in statsgui
				addGameText("Got gold for surviving wave: " + waveIncome);
				getPlayer().adjustPlayerGold(waveIncome);
				// Updates text displayed in statsgui
				addGameText("New wave incoming!");
				getPlayer().incrementMobWaveCount(); // increment player mob
				// wave count
			}
		}
	}

	public void addGameText(String s) {
		if (gameGUI != null) {
			gameGUI.addTextToShow(s);
			int a = 1;
			if (a != 1)
				a = 1;
			if (a < 2)
				a = 1;
		}
	}

	// A method to notify GameSession that the player has killed a monster.
	// GameSession will mark the monster for removal and add gold to the player.
	public void monsterWasKilled(Monster monster) {
		if (!trashList.contains(monster)) {
			// Updates text displayed in statsgui
			// int monsterIncome = eco.getMonsterIncome(monster);
			int monsterIncome = kernel.getBountyForMonster(monster.getTypeID());
			addGameText("Got gold from monster: " + monsterIncome);

			player.adjustPlayerGold(monsterIncome);
			trashList.add(monster);
		}
	}

	// A notification that a monster has passed through the maze.
	// GameSession marks the monster for removal and removes life from the
	// player.
	public void monsterPassedThrough(Monster monster) {

		if (!trashList.contains(monster)) {
			player.adjustPlayerHP(-monster.getDmg());
			if (!player.isAlive() && !multiplayer) {
				// end game
				pauseGame(TypeID.LOSE_SCREEN);
			}
			trashList.add(monster);
		}
	}

	// Removes all monsters and projectiles marked for removal.
	public void emptyTrash() {
		for (GameObject trash : trashList) {
			synchronized (gameThread.getSurfaceHolder()) {
				if (monsters.contains(trash)) {
					monsters.remove(trash);
				} else {
					projectileList.remove(trash);
				}
			}
		}
		trashList.clear();
	}

	// +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	// TEST METHODS
	// +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
