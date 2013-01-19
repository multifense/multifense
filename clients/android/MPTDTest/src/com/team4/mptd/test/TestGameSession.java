package com.team4.mptd.test;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.test.ActivityInstrumentationTestCase2;

import com.team4.mptd.GameActivity;
import com.team4.mptd.GameSession;

public class TestGameSession extends ActivityInstrumentationTestCase2<GameActivity> {
	private Activity mActivity;
	private GameSession mSession;
	
	public TestGameSession () {
		super("com.team4.mptd.GameActivity", GameActivity.class);
	}
	
	protected void setUp() throws Exception {
		super.setUp();
		setActivityInitialTouchMode(false);
		Intent i = new Intent();
		i.setClassName("com.team4.mptd", "com.team4.mptd.GameActivity");
		Bundle b = new Bundle();
		b.putString("mode", "singleplayer");
		i.putExtras(b);
		setActivityIntent(i);
		mActivity = getActivity();
		mSession = 	 new GameSession((GameActivity) mActivity, "singleplayer");
	}
	
	public void testPreCondition() {
		assertNotNull(mSession);
	}
	public void testGetters() {
		assertNotNull(mSession.getMonsters());
		assertNotNull(mSession.getTowers());
		assertNotNull(mSession.getGameThread());
		assertNotNull(mSession.getProjectileList());
		assertNotNull(mSession.getTrashList());
		assertNotNull(mSession.getMap());
		assertNotNull(mSession.getPlayer());
		assertNotNull(mSession.getOffsetX());
		assertNotNull(mSession.getOffsetY());
		assertNotNull(mSession.getCurrentRect());
		assertNotNull(mSession.getObjectsToDraw());
		assertTrue(mSession.getGameMode().equalsIgnoreCase("singleplayer") ||mSession.getGameMode().equalsIgnoreCase("multiplayer"));
		
		
	}
}
