package com.team4.mptd.test;

import android.app.Activity;
import android.graphics.BitmapFactory;
import android.test.ActivityInstrumentationTestCase2;

import com.team4.mptd.Economy;
import com.team4.mptd.GameActivity;
import com.team4.mptd.GamePath;
import com.team4.mptd.GameSession;
import com.team4.mptd.Monster;
import com.team4.mptd.Player;
import com.team4.mptd.R;
import com.team4.mptd.Tower;

public class TestEconomy extends ActivityInstrumentationTestCase2<GameActivity> {
	private Activity mActivity;
	private GameSession mSession;
	private Player player;
	private Economy eco;
	public TestEconomy () {
		super("com.team4.mptd.GameActivity", GameActivity.class);
	}
	
	protected void setUp() throws Exception {
		super.setUp();
		setActivityInitialTouchMode(false);
		
		mActivity = getActivity();
		mSession = 	 ((GameActivity) mActivity).getGame();
		player = mSession.getPlayer();
		eco = new Economy();
	}
	
	public void testPreCondition() {
		
		assertNotNull(mSession);
	}
	public void testPlayerGold(){
		assertEquals(100, player.getPlayerGold());
		
	}
	  
	public void testAdjustPlayerGold(){
		player.adjustPlayerGold(10);
		assertEquals(110, player.getPlayerGold());
	}
	
	public void testEconomyClass() {
		assertTrue(eco.getSPIncome(1) == 10);
		assertTrue(eco.getSPIncome(2) == 11);
		assertNotNull(eco.getMonsterIncome(new Monster(1, 1, 1, BitmapFactory.decodeResource(mActivity.getResources(), R.drawable.bombmonster), new GamePath())));
	}
}
