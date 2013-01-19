package com.team4.mptd.test;
import android.app.Activity;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.test.ActivityInstrumentationTestCase2;

import com.team4.mptd.*;
import com.team4.mptd.R;


import junit.framework.TestCase;

public class TestMonsterPath extends ActivityInstrumentationTestCase2<GameActivity> {
	private Activity mActivity;
	private GameSession mSession;
	private GamePath path;
	private Bitmap b;
		
	public TestMonsterPath () {
		super("com.team4.mptd.GameActivity", GameActivity.class);
	}
	
	protected void setUp() throws Exception {
		super.setUp();
		setActivityInitialTouchMode(false);
		
		mActivity = getActivity();
		mSession = 	 ((GameActivity) mActivity).getGame();
		path = mSession.getMap().getMonsterPath();
		
	}
	
	public void testPreCondition() {
		
		assertNotNull(mSession);
	}
	  public void testDirectionVectorX() {
	        GamePath path = new GamePath();
	        assertEquals(1.0, path.getDirectionVectorX());

	  }	  
	  public void testDirectionVectorY() {
	        GamePath path = new GamePath();
	        assertEquals(0.0, path.getDirectionVectorY());
	  }
	  
	  public void testLength() {
	        GamePath path = new GamePath();
	        assertEquals(130.0, path.getLength());

	  }
	
	 public static void main(String[] args) {
	        junit.textui.TestRunner.run(
	            TestMonsterPath.class);
	    }
}
