package com.team4.mptd.test;

import android.app.Activity;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.os.Bundle;
import android.test.ActivityInstrumentationTestCase2;

import com.team4.mptd.GameActivity;
import com.team4.mptd.GamePath;
import com.team4.mptd.GameSession;
import com.team4.mptd.MobWaveEntry;
import com.team4.mptd.Monster;
import com.team4.mptd.R;


public class TestMobWaveEntry extends ActivityInstrumentationTestCase2<GameActivity> {
	private Activity mActivity;
	private MobWaveEntry mMobWaveEntry;
	private GameSession mSession;
	
	public TestMobWaveEntry () {
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
		mMobWaveEntry = new MobWaveEntry((new Monster(0.75, 1, 3,BitmapFactory.decodeResource(mActivity.getResources(), R.drawable.bombmonster), new GamePath())), 0, 200, 5, 2);
	}
	
	public void testGetNextSpawnAndSpendTime(){
		assertTrue(mMobWaveEntry.getNextSpawn() == 0);
		mMobWaveEntry.spendTime(20);
		assertTrue(mMobWaveEntry.getNextSpawn() == -20);
	}
	public void testIsFinished() {
		assertTrue(!(mMobWaveEntry.isFinished()));
	}
}


