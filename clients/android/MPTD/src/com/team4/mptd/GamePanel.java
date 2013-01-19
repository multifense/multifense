package com.team4.mptd;

import android.content.Context;
import android.view.SurfaceHolder;
import android.view.SurfaceView;

public class GamePanel extends SurfaceView implements SurfaceHolder.Callback {
	private GameSession session;
	private Context context;
	private SurfaceHolder holder;
	private boolean isReady = false;

	public GamePanel(Context context, GameSession s) {
		super(context);
		this.context = context;
		session = s;
		holder = getHolder();
		holder.addCallback(this);
		setFocusable(true);
	}
	
	public boolean getReady() {
		return isReady;
	}

	public SurfaceHolder getSurfaceHolder() {

		return holder;
	}

	public void surfaceDestroyed(SurfaceHolder holder) {
//		session.getGameScreen().setScreenState(TypeID.LOSE_SCREEN);
//		session.getGameScreen().clearScreenState();
	}

	public void surfaceCreated(SurfaceHolder holder) {
		((GameActivity)context).startGame();
	}

	public void surfaceChanged(SurfaceHolder holder, int format, int width,
			int height) {

	}
}
