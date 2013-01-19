package com.team4.mptd;

import android.app.Activity;
import android.content.Context;
import android.content.pm.ActivityInfo;
import android.content.res.Configuration;
import android.os.Bundle;
import android.util.Log;
import android.view.KeyEvent;
import android.view.Window;
import android.view.WindowManager;

public class GameActivity extends Activity {
	private GameSession game;

	@Override
	public void onCreate(Bundle savedInstanceState) {
		// ---------------------------------
		// Initialize activity and window options

		super.onCreate(savedInstanceState);
		this.getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN,
				WindowManager.LayoutParams.FLAG_FULLSCREEN);
		requestWindowFeature(Window.FEATURE_NO_TITLE);
		// determine gamemode and start corresponding session
		Bundle b = this.getIntent().getExtras();
		String mode = b.getString("mode");

		
		Boolean mp;

		int map = b.getInt("map");

		if (mode.equalsIgnoreCase("singleplayer")) {

			game = new GameSession();
			mp = false;
			game.init(this, mp, map);
		} else {
			String nick = b.getString("nick");
			game = new MultiplayerGameSession(nick);
			mp = true;
			((MultiplayerGameSession)game).init(this, mp, map);
		}
		
		GamePanel gamePanel = new GamePanel(this, game);
		game.setGamePanel(gamePanel);
		

		Log.i("GameActivity", "started");
		
		setContentView(gamePanel);
		getWindow().addFlags(
				android.view.WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON);
		// ---------------------------------

	}
	
	// We override back button presses while in the game to make the game quit safely.
	@Override
	public boolean onKeyDown(int keyCode, KeyEvent event) {
		if (keyCode == KeyEvent.KEYCODE_BACK) {
			
			game.quitGame(TypeID.LOSE_SCREEN);
		} else {
			return super.onKeyDown(keyCode, event);
		}
		return true;
	}
	
	public void startGame() {
		game.startGame();
	}

	public GameSession getGame() {
		return game;
	}

	@Override
	protected void onPause() {
		super.onPause();
		game.getGameThread().setRunning(false);
		
	}

	@Override
	protected void onResume() {
		super.onResume();

	}

	@Override
	protected void onStop() {
		super.onStop();
		// quitGame();
	}
	
	@Override
	protected void onDestroy() {
		super.onDestroy();
		game = null;
	}

	@Override
	public void onConfigurationChanged(Configuration newConfig) {
		super.onConfigurationChanged(newConfig);
		setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE);
	}
}
