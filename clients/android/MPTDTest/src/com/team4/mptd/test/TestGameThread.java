package com.team4.mptd.test;

import junit.framework.TestCase;

import android.app.Activity;
import android.test.ActivityInstrumentationTestCase2;

import com.team4.mptd.Economy;
import com.team4.mptd.GameActivity;
import com.team4.mptd.GameSession;
import com.team4.mptd.GameThread;


public class TestGameThread extends ActivityInstrumentationTestCase2<GameActivity> {
	private Activity mActivity;
	private GameSession mSession;
	
	public TestGameThread() {
		super("com.team4.mptd.GameActivity", GameActivity.class);
	}
	
	protected void setUp() throws Exception {
		super.setUp();
		setActivityInitialTouchMode(false);
		
		mActivity = getActivity();
		mSession = 	 ((GameActivity) mActivity).getGame();
	}
	
	public void testPreCondition() {
		assertNotNull(mSession);
	}
	
	public void testInitializedGameThread() {
		assertNotNull(mSession.getGameThread());
	}
	
	public void testSetIsRunning() {
		assertTrue(mSession.getGameThread().isRunning());
	}
	public void testThreadPause() {
		mSession.getGameThread().setRunning(false);
		assertTrue(mSession.getGameThread().isRunning() == false);
	}
	public void testThreadHolder() {
		assertNotNull(mSession.getGameThread().getSurfaceHolder());
	}
}
