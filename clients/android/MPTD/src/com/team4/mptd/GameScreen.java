package com.team4.mptd;

import java.util.ArrayList;

import android.content.res.Resources;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Canvas;
import android.graphics.Paint;
import android.graphics.Rect;

public class GameScreen extends GameObject {

	private GameSession session;
	private int screenState;
	private Bitmap winScreen;
	private Bitmap spLoseScreen;
	private Bitmap connectScreen;
	private Bitmap getReadyScreen;
	private Bitmap getReadyScreenTop;
	private Bitmap loadingScreen;
	private boolean isWaiting = false;
	private Rect screenRect;
	private int countDownCounter;
	private int frameCounter = 0; // used to countdown counddowncounter once
									// every second

	public GameScreen(double x, double y, int frWidth, int frHeight,
			int noFrames, Resources res, GameSession session) {
		super(x, y, frWidth, frHeight, noFrames);
		winScreen = BitmapFactory.decodeResource(res, R.drawable.youwin);
		spLoseScreen = BitmapFactory.decodeResource(res, R.drawable.gameover2);
		connectScreen = BitmapFactory.decodeResource(res, R.drawable.searchingtop);
		getReadyScreen = BitmapFactory.decodeResource(res, R.drawable.getready);
		getReadyScreenTop = getReadyScreen = BitmapFactory.decodeResource(res, R.drawable.getreadytop);
		loadingScreen = BitmapFactory.decodeResource(res, R.drawable.loading);
		screenRect = new Rect(0, 0, frWidth, frHeight);
		this.session = session;
	}

	public int getScreenState() {
		return screenState;
	}

	public void clearScreenState() {
		isWaiting = false;
	}

	public void setScreenState(int state) {
		screenState = state;
		isWaiting = true;
		switch (state) {
		case 300: //Game won
			if(session.getSoundManager().musicIsPlaying()){
				session.getSoundManager().stopBackgroundMusic();
			}
			session.playSound(TypeID.WIN_SCREEN);
			
//			countDownCounter = 5;
			break;
		case 301: //Game lost
			if(session.getSoundManager().musicIsPlaying()){
				session.getSoundManager().stopBackgroundMusic();
			}
			session.playSound(TypeID.LOSE_SCREEN);
			
//			countDownCounter = 5;
			;
		case 302: // waiting for start game
			break;
		case 303: // time for countdown
		
			countDownCounter = 3;
			break;
		case 305:
			countDownCounter = 10;
		}
	}

	public boolean getWaiting() {
		return isWaiting;
	}

	@Override
	public void draw(Canvas canvas, Rect src, float offsetX, float offsetY) {
		Bitmap tmp;
		switch (screenState) {
		case 300: {
			tmp = winScreen;
			canvas.drawBitmap(tmp, null, screenRect, null);
			break;
		}
		case 301: {
			tmp = spLoseScreen;
			canvas.drawBitmap(tmp, null, screenRect, null);		
			break;
		}
		case 302: {
			tmp = connectScreen;
			canvas.drawBitmap(tmp, null, screenRect, null);
			Paint tP = new Paint();
			tP.setTextSize(20);
			tP.setARGB(255, 255, 255, 255);
			// iterate over all players connected
			ArrayList<Player> list = ((MultiplayerGameSession) session)
					.getOpponentList();	
			// first draw local player
			canvas.drawText("Player: " + session.getPlayer().getNick(), src.centerX() - 20, src.centerY() + tP.getTextSize(), tP);
			for (int i = 1; i < list.size() + 1; i++) {
				canvas.drawText("Player: " + list.get(i - 1).getNick(),
						src.centerX() - 20,
						src.centerY() + (int) tP.getTextSize() * (i + 1), tP);
			}
			break;
		}
		case 303: {
			tmp = getReadyScreen;
			canvas.drawBitmap(tmp, null, screenRect, null);
			break;
		}
		case 305:
			tmp = getReadyScreenTop;
			canvas.drawBitmap(tmp, null, screenRect, null);
			Paint tP = new Paint();
			tP.setTextSize(20);
			tP.setARGB(255, 255, 255, 255);
			// iterate over all players connected
			ArrayList<Player> list = ((MultiplayerGameSession) session)
					.getOpponentList();
			// first draw local player
			canvas.drawText("Player: " + session.getPlayer().getNick(), src.centerX() - 20, src.centerY() + tP.getTextSize(), tP);
			for (int i = 1; i < list.size() + 1; i++) {
				canvas.drawText("Player: " + list.get(i - 1).getNick(),
						src.centerX() - 20,
						src.centerY() + (int) tP.getTextSize() * (i + 1), tP);
			}
			break;
		default: {
			tmp = loadingScreen;
			canvas.drawBitmap(tmp, null, screenRect, null); // TODO: kl 00:00
															// fulhack f�r att
															// players i mp inte
															// skall m�las under
															// canvas -.-
		}
		}

		if (countDownCounter > 0) {
			// if coundDownCounter larger than 0 we should countDown and print
			// the value on the screen.
			Paint p = new Paint();
			p.setARGB(255, 255, 255, 255);
			p.setTextSize(90);
			// if state getready we show the counter, otherwise its not visible.
			if (screenState == TypeID.GETREADY_SCREEN  || screenState == TypeID.GETREADY_SCREEN_MP) {
				canvas.drawText("" + countDownCounter,
						src.centerX() - p.getTextSize() / 2,
						src.centerY() + 150, p);
			}
			if (frameCounter == 30) { // countDownCounter updates once every
										// second. TODO: this is hardcoded, if
										// we change game update / sec we gotta
										// change this
				countDownCounter--;
				frameCounter = 0;
			}
			if (countDownCounter == 0) {
				isWaiting = false;
			}
			frameCounter++;

		}
	}
}
