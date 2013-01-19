package com.team4.mptd;

import java.util.ArrayList;
import java.util.HashMap;

import android.content.res.Resources;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Canvas;
import android.graphics.Matrix;
import android.graphics.Paint;
import android.graphics.Rect;
import android.util.DisplayMetrics;

public class GameGui extends GameObject {
	private Rect smallBuildRect;
	private Rect smallSendRect;
	private Rect largeBuildRect;
	private Rect largeSendRect;
	private Rect currentBuildRect;
	private Rect currentSendRect;
	private Rect toggleOpViewRect;
	private Rect beeingViewedRect;
	private Bitmap buildMenuBmap;
	private Bitmap sendMenuBmap;
	private Bitmap menuScrollBmap;
	private Bitmap menuInvScrollBmap;
	private Matrix inversMatrix = new Matrix();
	private boolean showBuildExpanded = false;
	private boolean showSendExpanded = false;
	private boolean isBuildWaiting = false;
	private boolean isSendWaiting = false;
	private boolean multiplayer = false;
	private boolean dragState = false;
	private Tower myCurrentTower;
	private ArrayList<Bitmap> selectionTowerList;
	private ArrayList<Bitmap> selectionMonsterList;
	private ArrayList<Rect> selectionTowerRectList;
	private ArrayList<Rect> selectionMonsterRectList;
	private final HashMap<Integer, Tower> towerMap; // we have to make sure we don't change this
	private final HashMap<Integer, Monster> monsterMap; // or this.
	private GameSession session;
	//start statusgui variable declarations
	private Player player;
	private Rect statusRect;
	private Rect textRect;
	private Rect textRectLarge;
	private Rect currentTextRect;
	private boolean showPlayers = false;
	private boolean textToShow = false;
	private boolean showExpanded = false;
	private ArrayList<String> text;
	private Bitmap goldIcon;
	private Bitmap hpIcon;
	// end statusgui variable declarations

	public GameGui(int left, int top, int screenWidth, int screenHeight,
			Resources res, GameSession session,
			final HashMap<Integer, Tower> towerMap,
			final HashMap<Integer, Monster> monsterMap) {
		super(left, top, 0, 0, 0);
		this.towerMap = towerMap;
		this.monsterMap = monsterMap;
		this.session = session;
		smallBuildRect = new Rect(screenWidth / 20, screenHeight
				- (screenHeight / 5), (screenWidth / 20) + 75, screenHeight
				- (screenHeight / 5) + 75);
		smallSendRect = new Rect(screenWidth - ((screenWidth / 20) + 75),
				screenHeight - (screenHeight / 5), screenWidth
				- (screenWidth / 20), screenHeight - (screenHeight / 5)
				+ 75);
		largeBuildRect = new Rect(screenWidth / 20, screenHeight
				- (screenHeight / 3), (screenWidth / 2), screenHeight
				- (screenHeight / 50));
		largeSendRect = new Rect(screenWidth - (screenWidth / 2), screenHeight
				- (screenHeight / 3), screenWidth - (screenWidth / 20),
				screenHeight - (screenHeight / 50));
		toggleOpViewRect = new Rect(screenWidth - ((screenWidth / 20) + 75),
				screenHeight - (screenHeight / 5) - 80, screenWidth
				- (screenWidth / 20), screenHeight - (screenHeight / 5)
				- 5);
		beeingViewedRect = new Rect((screenWidth / 20),
				(screenHeight / 8), (screenWidth / 20)+ 25, (screenHeight / 8)
				+25);
		
		currentSendRect = smallSendRect;
		currentBuildRect = smallBuildRect;

		// +++++++++++++++++++++++++++++++++++++++++++++++++
		// Rect list below is for drawing and checking intersecs in the menu
		// list
		// +++++++++++++++++++++++++++++++++++++++++++++++++
		selectionTowerList = new ArrayList<Bitmap>();
		selectionMonsterList = new ArrayList<Bitmap>();
		selectionTowerRectList = new ArrayList<Rect>();
		selectionMonsterRectList = new ArrayList<Rect>();
		selectionTowerList.add(Bitmap.createScaledBitmap(
				BitmapFactory.decodeResource(res, R.drawable.cannontower), 50,
				50, false));

		selectionTowerList.add(Bitmap.createScaledBitmap(
				BitmapFactory.decodeResource(res, R.drawable.magictower), 50,
				50, false));

		selectionTowerList.add(Bitmap.createScaledBitmap(
				BitmapFactory.decodeResource(res, R.drawable.firetower), 50,
				50, false));

		selectionTowerList.add(Bitmap.createScaledBitmap(
				BitmapFactory.decodeResource(res, R.drawable.igloo), 50, 50,
				false));

		selectionMonsterList.add(Bitmap.createScaledBitmap(
				BitmapFactory.decodeResource(res, R.drawable.bombned1), 50, 50,
				false));
		selectionMonsterList.add(Bitmap.createScaledBitmap(
				BitmapFactory.decodeResource(res, R.drawable.slimened1), 50,
				50, false));
		selectionMonsterList.add(Bitmap.createScaledBitmap(
				BitmapFactory.decodeResource(res, R.drawable.slimened1), 50,
				50, false));
		selectionMonsterList.add(Bitmap.createScaledBitmap(
				BitmapFactory.decodeResource(res, R.drawable.hattyicon), 50,
				50, false));
		selectionMonsterList.add(Bitmap.createScaledBitmap(
				BitmapFactory.decodeResource(res, R.drawable.rockned1), 50,
				50, false));

		int buildWidth = selectionTowerList.get(0).getWidth();
		int buildHeight = selectionTowerList.get(0).getHeight();

		int buildLeft = largeBuildRect.left + 40;
		int buildTop = largeBuildRect.centerY() - buildWidth / 2;
		int buildRight = buildLeft + buildWidth;
		int buildBottom = buildTop + buildHeight;

		int sendWidth = selectionMonsterList.get(0).getWidth();
		int sendHeight = selectionMonsterList.get(0).getHeight();

		int sendLeft = largeSendRect.left + 20;
		int sendTop = largeSendRect.centerY() - sendWidth / 2;
		int sendRight = sendLeft + sendWidth;
		int sendBottom = sendTop + sendHeight;

		inversMatrix.preScale(-1, 1);

		buildMenuBmap = Bitmap.createScaledBitmap(
				(BitmapFactory.decodeResource(res, R.drawable.build)), 50, 50,
				false);

		sendMenuBmap = Bitmap.createScaledBitmap(
				(BitmapFactory.decodeResource(res, R.drawable.monstericon)),
				50, 50, false);

		menuScrollBmap = (BitmapFactory.decodeResource(res, R.drawable.scroll));

		menuInvScrollBmap = Bitmap.createBitmap(menuScrollBmap, 0, 0,
				menuScrollBmap.getWidth(), menuScrollBmap.getHeight(), inversMatrix,
				false);
		menuInvScrollBmap.setDensity(DisplayMetrics.DENSITY_DEFAULT);

		// construct rect list for tower selection.
		for (int k = 0; k < selectionTowerList.size(); k++) {
			if (k != 0) {

				buildLeft = buildRight + 10;
				buildRight = buildLeft + buildWidth;
			}
			selectionTowerRectList.add(new Rect(buildLeft, buildTop,
					buildRight, buildBottom));
		}

		// construct rect list for monster selection.
		for (int k = 0; k < selectionMonsterList.size(); k++) {
			if (k != 0) {

				sendLeft = sendRight + 10;
				sendRight = sendLeft + sendWidth;
			}
			selectionMonsterRectList.add(new Rect(sendLeft, sendTop, sendRight,
					sendBottom));
		}

		// begin statusgui initialization
		// create UI box rect
		statusRect = new Rect(screenWidth / 20, screenHeight / 50, screenWidth
				- (screenWidth / 3), (screenHeight / 10));
		textRect = new Rect(statusRect.right + statusRect.left, statusRect.top,
				screenWidth - statusRect.left, statusRect.bottom);
		textRectLarge = new Rect(statusRect.right + statusRect.left,
				statusRect.top, screenWidth - statusRect.left,
				(statusRect.bottom * 10) - 10);
		// sendMonsterButton = new Rect (screenWidth/20, rect.bottom + 10,
		// (screenWidth - (screenWidth / 20)) / 10, rect.bottom + 50);
		currentTextRect = textRect;
		text = new ArrayList<String>();
		this.goldIcon = Bitmap.createScaledBitmap((BitmapFactory.decodeResource(res,
				R.drawable.economyicon)), 10, 10, false);
		this.session = session;
		this.hpIcon = Bitmap.createScaledBitmap((BitmapFactory.decodeResource(res,
				R.drawable.healthicon)), 10, 10, false);
		player = session.getPlayer();
		// end statusgui initialization
	}

	// guiTap receives a rect which represents an area touched on the gui by the user.
	// guiTap handles whatever happens and returns void
	public void guiTap(Rect touchArea) {
		// Check if any of the GUI elements clicked.
		if (Rect.intersects(touchArea, getTextBounds())) {
			// statusGUI pressed
			if (showsExpanded()) {
				// menu shows expanded view
				menuHide();
			} else {
				// menu should display expanded view
				menuPress();
			}

		} else if (Rect.intersects(touchArea, getStatusBounds())
				&& multiplayer) {
			toggleStatusBar();
		} else if (Rect.intersects(touchArea, getBuildBounds()) && !session.isOpponentView()) {
			// Build menu pressed
			if (!showsBuildExpanded()) {
				// menu should display expanded view
				buildMenuPress();
			} else
				// hide menu
				if (!(Rect.intersects(touchArea, getSendBounds()))) {
					buildMenuHide();
				}
			// clear selection
			myCurrentTower = null;
			// multiplayer = true; this line is/was used to test mp options in an sp game.
		} else if (Rect.intersects(touchArea, getOpViewBounds()) && multiplayer) {
			//opponent view pressed
			session.toggleOppView();
			//toggleOpponent view
		}
		if ((Rect.intersects(touchArea, getSendBounds()))
				&& multiplayer) {
			// if monster button is pressed
			// and if menu is in display expanded view
			if (showsSendExpanded()) {
				// select monster to send.
				Monster m = getSendMenuSelection(touchArea);
				// fireandforget methodcall to multiplayergamesession method trysendmonster.
				if (m != null) {
					((MultiplayerGameSession)session).trySendMonster(m);
				}

			}
			// TODO: why is this method call here??
			sendMenuPress();
		} else if (multiplayer) {
			if (!(Rect.intersects(touchArea, getBuildBounds()))) {
				sendMenuHide();
			}
		}
		// multiplayer = false; test line as above. we just mark mp as false again once we are out of the menu.

	}

	public void setDragState(boolean state) {
		dragState = state;
	}

	public boolean isDragState() {
		return dragState;
	}

	public void multiplayer() {
		multiplayer = true;
	}

	public boolean isBuildWaiting() {
		return isBuildWaiting;
	}

	public boolean isSendWaiting() {
		return isSendWaiting;
	}

	public void clearBuildWaiting() {
		isBuildWaiting = false;
	}

	public void clearSendWaiting() {
		isSendWaiting = false;
	}

	public Tower getMyCurrentTower() {
		return myCurrentTower;
	}

	// return rect/bounds for buildmenu
	public Rect getBuildBounds() {
		return currentBuildRect;
	}

	// rect/bounds for send menu
	public Rect getSendBounds() {
		return currentSendRect;
	}

	//bool for checking if we are expanded
	public boolean showsBuildExpanded() {
		return showBuildExpanded;
	}

	// bool for checking if we are expanded
	public boolean showsSendExpanded() {
		return showSendExpanded;
	}

	// switches the current rect to draw for build menu into the button rect. effectively hiding the build menu.
	public void buildMenuHide() {
		showBuildExpanded = false;
		currentBuildRect = smallBuildRect;
		clearBuildWaiting();
	}
	// same as buildmenuhide for the sendmonster menu
	public void sendMenuHide() {
		showSendExpanded = false;
		currentSendRect = smallSendRect;
		clearSendWaiting();
	}

	// build button pressed. switch currentbuildrect to large version.
	public void buildMenuPress() {
		showBuildExpanded = true;
		currentBuildRect = largeBuildRect;
		isBuildWaiting = true;
	}

	// same as for buildmenupress...
	public void sendMenuPress() {
		showSendExpanded = true;
		currentSendRect = largeSendRect;
		isSendWaiting = true;
	}

	// start statusgui methods

	public void addTextToShow(String s) {
		text.add(s);
		textToShow = true;
	}

	public boolean isTextToShow() {
		return textToShow;
	}

	public boolean showsExpanded() {
		return showExpanded;
	}

	public Rect getTextBounds() {
		return currentTextRect;
	}

	public void setMultiplayer(boolean b) {
		multiplayer = b;
	}

	public void menuPress() {
		showExpanded = true;
		currentTextRect = textRectLarge;
	}

	public void menuHide() {
		showExpanded = false;
		currentTextRect = textRect;
	}

	public Rect getStatusBounds() {
		return statusRect;
	}
	public Rect getOpViewBounds() {
		return toggleOpViewRect;
	}

	public void toggleStatusBar() {
		if (showPlayers) {
			showPlayers = false;
		} else {
			showPlayers = true;
		}
	}

	// end statusgui methods

	// called when in updateState. updates position of myCurrentTower
	public void updateSelectionPosition(float x, float y) {
		myCurrentTower.left -= x;
		myCurrentTower.top -= y;
		myCurrentTower.updateRect();
	}

	public boolean getMenuSelection(Rect touched) {
		// Checks all tower selections for an intersect with touched. sets mycurrenttower
		// to that selection and enter dragmode.
		if (touched != null && !session.isOpponentView()) {
			for (int i = 0; i < selectionTowerRectList.size(); i++) {
				if (Rect.intersects(touched, selectionTowerRectList.get(i))) {
					// user selected a tower in list.set initial position as
					// same as selection. enter dragstate
					myCurrentTower = towerMap.get(i);
					myCurrentTower.setLeft(selectionTowerRectList.get(i).left
							+ session.getOffsetX());
					myCurrentTower.setTop(selectionTowerRectList.get(i).top
							+ session.getOffsetY() - 50);
					myCurrentTower.updateRect();
					dragState = true;
					return true;
				}
			}
		}
		return false;

	}

	public Monster getSendMenuSelection(Rect touched) {
		// check with intersect which icon is touched and return typeindex of
		// selection
		if (touched == null) {
			return null;
		} else {

			for (int i = 0; i < monsterMap.size(); i++) {
				if (Rect.intersects(touched, selectionMonsterRectList.get(i))) {
					return (Monster) monsterMap.get(i + 101);
				}
			}
		}
		return null;
	}

	@Override
	public void draw(Canvas canvas, Rect src, float offsetX, float offsetY) {
		// Ignore all offsets, GUI is static to canvas
		Paint p = new Paint();
		p.setARGB(127, 205, 201, 201);
		Paint tP = new Paint();
		tP.setARGB(255, 0, 0, 0);

		// start code from statusgui
		tP.setTextSize(14);

	
		if (!showPlayers) {
			canvas.drawRect(statusRect, p);
			// Draw health
			canvas.drawBitmap(
					hpIcon,
					statusRect.left + (int) tP.getTextSize() / 2,
					statusRect.top + (int) tP.getTextSize()
					- goldIcon.getHeight(), null);
			canvas.drawText(" : " + (int) player.getPlayerHP(),
					statusRect.left + (int) tP.getTextSize() / 2,
					statusRect.top + (int) tP.getTextSize(), tP);

			// Draw gold
			canvas.drawBitmap(
					goldIcon,
					statusRect.left + (int) tP.getTextSize() / 2,
					statusRect.top + 2 * (int) tP.getTextSize()
					- goldIcon.getHeight() + 5, null);
			canvas.drawText(" : " + player.getPlayerGold(), statusRect.left
					+ (int) tP.getTextSize() / 2 + goldIcon.getWidth(),
					statusRect.top + 2 * (int) tP.getTextSize() + 5, tP);

			// Draw mobwave count
			canvas.drawText(
					"Mob wave: " + player.getMobWaveCount(),
					statusRect.left + 10 * (int) tP.getTextSize(),
					statusRect.top +  (int) tP.getTextSize(), tP);

			// Draw defeated monsters
			canvas.drawText(
					"Defeated monsters: " + player.getDefeatedMonsters(),
					statusRect.left + 10 * (int) tP.getTextSize(),
					statusRect.top + 2 * (int) tP.getTextSize() + 5, tP);
			

			// Draw next mobwave counter (only mp)
			if (session.getMultiplayer()) {
				canvas.drawText("Next wave in: " + ((MultiplayerGameSession)session).getMPMobWave().getTimeToWave(),
						statusRect.left + 25 * (int) tP.getTextSize() + 5,
						statusRect.top + (int) tP.getTextSize(), tP);
			}

			// Draw income per mobwave
			int temp;
			if(session.getMultiplayer()) {
				temp = session.getKernel().mpIncome();
			} else {
				temp = session.getKernel().spIncome();
			}
			canvas.drawText(
					"Income: " + temp,
					statusRect.left + 25 * (int) tP.getTextSize(),
					statusRect.top + 2 * (int) tP.getTextSize() + 5, tP);

		} else {
			// iterate over all players connected
			ArrayList<Player> list = ((MultiplayerGameSession) session)
			.getOpponentList();
			for (int i = 0; i < list.size(); i++) {
				// draw name and health
				canvas.drawText("Player: " + list.get(i).getNick()
						+ "        Health: " + list.get(i).getPlayerHP(),
						statusRect.left + (int) tP.getTextSize() / 2,
						statusRect.top + 10 + (int) tP.getTextSize() * i, tP);
			}
		}

		// If text to show, draw it in new box to the right of status.
		if (textToShow) {
			if (!(showExpanded)) {
				tP.setTextSize(10);
				canvas.drawRect(textRect, p);
				for (int i = text.size() - 1; i > (text.size() - 4) && (i > -1); i--) {
					canvas.drawText(
							text.get(i),
							textRect.left + (int) tP.getTextSize() / 2,
							statusRect.top + (int) tP.getTextSize()
							* (text.size() - i), tP);
				}
			} else {
				tP.setTextSize(10);
				canvas.drawRect(textRectLarge, p);
				for (int i = text.size() - 1; i > (text.size() - 46)
				&& (i > -1); i--) {
					canvas.drawText(
							text.get(i),
							textRectLarge.left + (int) tP.getTextSize() / 2,
							textRectLarge.top + (int) tP.getTextSize()
							* (text.size() - i), tP);
				}
			}

		}

		// end code from statusgui		
		
		
		//if the players screen is beeing viewed by another player, display viewed icon
		if(session.isViewed()) {
			canvas.drawBitmap(goldIcon, null, beeingViewedRect, tP);
		}
			


		tP.setTextSize(12);

		if(multiplayer){
			canvas.drawBitmap(sendMenuBmap, null, toggleOpViewRect, p);
		}
		if(!session.isOpponentView()){
			if (!showBuildExpanded) {
				// Draw build options
				canvas.drawBitmap(buildMenuBmap, null, smallBuildRect, p);

			} else { // show expanded menu

				canvas.drawBitmap(menuScrollBmap, null, largeBuildRect, p);
				for (int i = 0; i < selectionTowerRectList.size(); i++) {
					canvas.drawBitmap(selectionTowerList.get(i), null,
							selectionTowerRectList.get(i), null);
					// check player money to see if to draw in black or red.
					if (session.getPlayer().getPlayerGold() < towerMap.get(i)
							.getCost()) {
						tP.setARGB(255, 255, 0, 0);
					}
					canvas.drawText("" + towerMap.get(i).getCost(),
							selectionTowerRectList.get(i).centerX() - 5,
							selectionTowerRectList.get(i).top - 5, tP);
					tP.setARGB(255, 0, 0, 0);
					// canvas.drawRect(selectionRectList.get(i), p); // for
					// debugging purposes. draws the rect for each menu items
				}
			}
		} else {
			
			tP.setARGB(50, 0, 0, 0);
			tP.setTextSize(60);
			canvas.drawText(" Opponents Screen ",
					largeBuildRect.left + (int) tP.getTextSize() / 2,
					largeBuildRect.top + (int) tP.getTextSize(), tP);
			tP.setARGB(255, 0, 0, 0);
			
		}

		// draw drag selection if in dragstate
		if (dragState) {
			// draw the path blocking rects
			// session.getMap().drawRects(canvas, src, offsetX, offsetY);

			// call on usual draw method for tower but offset is 0 since its
			// current position is based on the src and not the gamemap
			myCurrentTower.draw(canvas, src, offsetX, offsetY);
			if (session.collisionDetect(myCurrentTower.getThisRect())
					|| session.getMap().intersect(myCurrentTower.getThisRect())) {
				p.setARGB(75, 255, 0, 0);
				canvas.drawCircle(myCurrentTower.getThisRect().centerX()
						- offsetX, myCurrentTower.getThisRect().centerY()
						- offsetY, (float) myCurrentTower.getRange(), p);
			} else {
				p.setARGB(75, 0, 255, 0);
				canvas.drawCircle(myCurrentTower.getThisRect().centerX()
						- offsetX, myCurrentTower.getThisRect().centerY()
						- offsetY, (float) myCurrentTower.getRange(), p);
			}
		}


		if (!showSendExpanded && multiplayer) {
			// Draw build options
			canvas.drawBitmap(sendMenuBmap, null, smallSendRect, p);
		} else if (multiplayer) { // show expanded menu
			canvas.drawBitmap(menuInvScrollBmap, null, largeSendRect, p);
			for (int i = 0; i < selectionMonsterRectList.size(); i++) {
				canvas.drawBitmap(selectionMonsterList.get(i), null,
						selectionMonsterRectList.get(i), null);
				if (session.getPlayer().getPlayerGold() < monsterMap.get(i + 101)
						.getCost()) {
					tP.setARGB(255, 255, 0, 0);
				}
				canvas.drawText("" + monsterMap.get(i + 101).getCost(),
						selectionMonsterRectList.get(i).centerX() - 5,
						selectionMonsterRectList.get(i).top - 5, tP);
				tP.setARGB(255, 0, 0, 0);
			}
		}
	}
}
