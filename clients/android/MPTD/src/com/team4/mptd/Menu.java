package com.team4.mptd;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.pm.ActivityInfo;
import android.content.res.Configuration;
import android.media.MediaPlayer;
import android.os.Bundle;
import android.view.KeyEvent;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.Window;
import android.view.WindowManager;
import android.widget.Button;
import android.widget.EditText;
import android.widget.RelativeLayout;

public class Menu extends Activity {
	private EditText editText;
	private AlertDialog.Builder alert;
	private boolean showsMain = true;
	private MediaPlayer player;
	private RelativeLayout rel;
	private AlertDialog ad;

	/** Called when the activity is first created. */
	@Override
	public void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		this.getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN,
				WindowManager.LayoutParams.FLAG_FULLSCREEN);
		// setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE);
		requestWindowFeature(Window.FEATURE_NO_TITLE);

		// main view first displayed
		switchToMainView();
	}

	@Override
	public boolean onKeyDown(int keyCode, KeyEvent event) {
		if ((keyCode == KeyEvent.KEYCODE_BACK) && (!showsMain)) {
			// handle exiting or going back to main menu
			switchToMainView();
		} else {
			return super.onKeyDown(keyCode, event);
		}
		return true;
	}

	private void switchToMainView() {
		showsMain = true;
		setContentView(R.layout.main); // laddar layouten till mainen.
		// en knapp, med ID = StartGame
		Button startGameButton = (Button) findViewById(R.id.startknapp);
		rel = (RelativeLayout) findViewById(R.id.widget32);
		
		editText = new EditText(this);
		editText.setFocusable(true);
		// popupwindow to contain the edittext
		alert = new AlertDialog.Builder(this);

		alert.setTitle("Player name");
		alert.setMessage("Please enter your desired player name");

		// Set an EditText view to get user input
		alert.setView(editText);
		
		alert.setPositiveButton("Ok", new DialogInterface.OnClickListener() {
			public void onClick(DialogInterface dialog, int whichButton) {
				Bundle bundle = new Bundle();
				bundle.putString("mode", "multiplayer");
				bundle.putInt("map", 1);
				bundle.putString("nick", editText.getText().toString());
				Intent intent = new Intent(Menu.this, GameActivity.class);
				intent.putExtras(bundle);
				startActivityForResult(intent, 2);
			}
		});

		alert.setNegativeButton("Cancel",
				new DialogInterface.OnClickListener() {
					public void onClick(DialogInterface dialog, int whichButton) {
						// Canceled.
						
					}
				});

		// Lyssnaren till knappen, eventen som sker n�r man trycker p� den
		startGameButton.setOnClickListener(new OnClickListener() {

			public void onClick(View v) {
				// switch to view displaying the maps
				switchToMapView();
			}
		});

		Button multiplayerButton = (Button) findViewById(R.id.multiplayer);

		multiplayerButton.setOnClickListener(new OnClickListener() {

			public void onClick(View v) {
				// open editext for player name
				ad = alert.show();
				
			}
		});

	}

	private void switchToMapView() {
		showsMain = false;
		setContentView(R.layout.mapmenu);
		// this view displays all the maps in clickable buttons which starts the
		// game with that respective map
		Button mapButton1 = (Button) findViewById(R.id.button1);

		mapButton1.setOnClickListener(new OnClickListener() {

			public void onClick(View v) {
				Bundle bundle = new Bundle();
				bundle.putString("mode", "singleplayer");

				bundle.putInt("map", 1);
				// Intent intent = new Intent(Menu.this, GameActivity.class);
				Intent intent = new Intent(Menu.this, GameActivity.class);
				intent.putExtras(bundle);
				startActivity(intent);
			}
		});

		Button mapButton2 = (Button) findViewById(R.id.button2);

		mapButton2.setOnClickListener(new OnClickListener() {

			public void onClick(View v) {
				Bundle bundle = new Bundle();
				bundle.putString("mode", "singleplayer");
				bundle.putInt("map", 2);
				// Intent intent = new Intent(Menu.this, GameActivity.class);
				Intent intent = new Intent(Menu.this, GameActivity.class);
				intent.putExtras(bundle);
				startActivity(intent);
			}
		});

		Button backButton = (Button) findViewById(R.id.back);

		backButton.setOnClickListener(new OnClickListener() {

			public void onClick(View v) {
				switchToMainView();
			}
		});
	}

	@Override
	public void onActivityResult(int requestCode, int resultCode, Intent data) {
		// game finishes
		// TODO: display result from last game
		// if (resultCode == Activity.RESULT_OK) {
		// }
	}

	@Override
	public void onConfigurationChanged(Configuration newConfig) {
		super.onConfigurationChanged(newConfig);
		setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE);
	}

	@Override
	protected void onPause() {
		super.onPause();
		player.stop();
		player.release();
	}

	@Override
	protected void onResume() {
		super.onResume();
		player = MediaPlayer.create(this, R.raw.multowerdeplayer);
		player.setLooping(true);
		player.start();

	}
}