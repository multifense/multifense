package com.team4.mptd;

import android.view.SurfaceHolder;

public class GameThread extends Thread {
	// variable declares if thread is running
	private boolean isRunning = false;
	private DrawGraphics graphics;
	private SurfaceHolder holder;
	private GameUpdate gameUpdate;

	// timer variable to calculate fps
	private long timer = 0;
	private long sleepTime = 0;
	private GameSession session;

	public GameThread(GameSession session) {

		this.session = session;

		gameUpdate = new GameUpdate(session);

	}

	// getSurfaceHolder is used for synchronization calls with the thread.
	public SurfaceHolder getSurfaceHolder() {
		return holder;
	}

	public void setSurfaceHolder(SurfaceHolder holder) {
		this.holder = holder;
		graphics = new DrawGraphics(holder);
	}

	// getter for isRunning
	public boolean isRunning() {
		return isRunning;
	}

	// set method for isRunning
	public void setRunning(boolean val) {
		isRunning = val;
	}

	public long checkFPS() {
		if (timer != 0) {
			return 1000 / timer;
		} else
			return 1;
	}

	// pause loop
	public void paused() {
		timer = System.currentTimeMillis();
		while (session.getGameScreen().getWaiting() && isRunning) {
			if (session.getMultiplayer()) {
				((MultiplayerGameSession) session).updateMultiplayerData();
			}
			graphics.updateGraphics(session.getObjectsToDraw(),
					session.getCurrentRect(), session.getOffsetX(),
					session.getOffsetY());
			// time to next cycle is 33 ms minus time taken
			timer += 33;
			sleepTime = timer - System.currentTimeMillis();
			// do sleep
			if (sleepTime > 0)
				sleep();
			// timer diff
		}

		switch (session.getGameScreen().getScreenState()) {
		case TypeID.LOSE_SCREEN:
			if (session.getMultiplayer())
				((MultiplayerGameSession) session)
						.quitGame(TypeID.LOSE_SCREEN);
			else
				session.quitGame(TypeID.LOSE_SCREEN);
			isRunning = false;
			break;
		case TypeID.WIN_SCREEN:
			if (session.getMultiplayer())
				((MultiplayerGameSession) session)
						.quitGame(TypeID.WIN_SCREEN);
			else
				session.quitGame(TypeID.WIN_SCREEN);
			isRunning = false;
			break;
		default:
			// if ((session.getGameScreen().getScreenState() !=
			// TypeID.WIN_SCREEN)
			// || (session.getGameScreen().getScreenState() !=
			// TypeID.LOSE_SCREEN)) {
			 isRunning = true;
			 run();
			break;
		}
		// }
	}

	// actual gameloop implemented in the run() method
	@Override
	public void run() {
		// first do pregame loop

		// game loop
		timer = System.currentTimeMillis();
		while (isRunning) {
			// check if pause and show gamescreen
			

			if (session.getMultiplayer()) {
				((MultiplayerGameSession) session).updateMultiplayerData();
			}

			// Do game update
			gameUpdate.updateGame();

			// Draw all objects
			graphics.updateGraphics(session.getObjectsToDraw(),
					session.getCurrentRect(), session.getOffsetX(),
					session.getOffsetY());

			// time to next cycle is 33 ms minus time taken
			timer += 33;
			sleepTime = timer - System.currentTimeMillis();
			
			if (session.getGameScreen().getWaiting()) {
				// isRunning = false;
				paused();
			}
			
			// do sleep
			if (sleepTime > 0)
				sleep();
			// timer diff
			// timer = System.currentTimeMillis() - timer;
		}
	}

	// enforce 30FPS
	private void sleep() {
		try {
			Thread.sleep(sleepTime, 0);
		} catch (InterruptedException e) {

		}
	}
}
