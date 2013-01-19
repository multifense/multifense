package com.team4.mptd.test;
import android.app.Activity;
import android.test.ActivityInstrumentationTestCase2;
import android.util.Log;

import com.team4.mptd.GameActivity;
import com.team4.mptd.GameSession;
import com.team4.mptd.MultiplayerGameSession;


public class TestMultiplayerGameSession extends ActivityInstrumentationTestCase2<GameActivity> {
	private Activity mActivity;
	private GameSession session;
	private char delimiter = 27;
	private String message = null;
	
	public TestMultiplayerGameSession () {
		 super("com.team4.mptd.GameActivity", GameActivity.class);
	}
	
    protected void setUp() throws Exception {
		super.setUp();
		setActivityInitialTouchMode(false);
    	
		mActivity = getActivity();
		session =  new GameSession((GameActivity) mActivity, "multiplayer");
	}
    
    public void testConnected() {
    	message = "Joakim" + delimiter + delimiter + "1337" + delimiter + delimiter + "ECHO";
    	Log.i("Output", message);
    	((MultiplayerGameSession)session).initConnection("134.229.154.15" , 1337);
    	((MultiplayerGameSession)session).sendData(message);
    	assertTrue(((MultiplayerGameSession)session).isConnected());
    }
}
