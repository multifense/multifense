package com.team4.mptd.test;

import java.util.ArrayList;
import android.app.Activity;
import android.graphics.BitmapFactory;
import android.test.ActivityInstrumentationTestCase2;


import com.team4.mptd.GameActivity;
import com.team4.mptd.GameObject;
import com.team4.mptd.GamePath;
import com.team4.mptd.GameSession;
import com.team4.mptd.Monster;
import com.team4.mptd.R;
import com.team4.mptd.Tower;

	
	public class TestTower extends ActivityInstrumentationTestCase2<GameActivity> {
		private Activity mActivity;
		private GameSession mSession;
		
	    public TestTower(){
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
	    
	    public void testPosX() {
	        Tower hi = new Tower();
	        assertEquals( (double)400, hi.getLeft());
	    }
	    public void testPosY() {
	        Tower hi = new Tower();
	        assertEquals( (double)200, hi.getTop());
	    }
	    public void testDamage() {
	        Tower hi = new Tower();
	        assertEquals( (double)5, hi.getDamage());
	    }
	    public void testFireRate() {
	        Tower hi = new Tower();
	        assertEquals( (double)100, hi.getFireRate());
	    }
	    public void testGetCost() {
	    	Tower hi = new Tower();
	    	assertNotNull(hi.getCost());
	    }
	    

	    public void testAttack() {
	    	ArrayList<GameObject> target = new ArrayList<GameObject>();
	    	target.add(new Monster(1, 1, 1, BitmapFactory.decodeResource(mActivity.getResources(), R.drawable.bombmonster), new GamePath()));
	    	Tower tow = new Tower();
	    	
	    	tow.setBitmap(BitmapFactory.decodeResource(mActivity.getResources(), R.drawable.cannontower));
	    	tow.attack(target);
	    	assertTrue(tow.getTarget()!= null);
	    	
	    	assertNotNull(tow.getProj());
	    	tow.resetTarget();
	    	assertTrue(tow.getTarget() == null);
	    	
	    	

	    }

	    public void testClone() {
	    	Tower tow = new Tower();
	    	tow.setBitmap(BitmapFactory.decodeResource(mActivity.getResources(), R.drawable.cannontower));
	    	Tower tow2 = tow.clone();
	    	assertTrue(!tow.equals(tow2));
	    	
	    	
	    }
	}

