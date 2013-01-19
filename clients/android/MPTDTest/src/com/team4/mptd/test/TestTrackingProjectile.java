package com.team4.mptd.test;

import junit.framework.TestCase;


import android.app.Activity;
import android.graphics.BitmapFactory;
import android.test.ActivityInstrumentationTestCase2;

import com.team4.mptd.GameActivity;

import com.team4.mptd.GamePath;
import com.team4.mptd.GameSession;
import com.team4.mptd.Monster;
import com.team4.mptd.R;
import com.team4.mptd.TrackingProjectile;


public class TestTrackingProjectile extends ActivityInstrumentationTestCase2<GameActivity> {
	private Activity mActivity;
	private GameSession mSession;
	
	public TestTrackingProjectile() {
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
	
	public void testProjectileExists() {

		Monster m = new Monster(1, 1, 1, BitmapFactory.decodeResource(mActivity.getResources(), R.drawable.bombmonster), new GamePath());
		TrackingProjectile s = new TrackingProjectile(50,50,40,10,m, BitmapFactory.decodeResource(mActivity.getResources(), R.drawable.bullet));
	

		assertTrue(s.hasHit() == false);
		for(int i = 0; i < 100; i++) {

			s.update();
			if(s.hasHit()){
				break;
			}
		}
		assertTrue(s.hasHit());

	}

}	
