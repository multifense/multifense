package com.team4.mptd.test;

import java.util.ArrayList;

import android.app.Activity;
import android.content.Intent;
import android.graphics.BitmapFactory;
import android.os.Bundle;
import android.test.ActivityInstrumentationTestCase2;
import android.util.Log;

import com.team4.mptd.GameActivity;
import com.team4.mptd.GameObject;
import com.team4.mptd.GamePath;
import com.team4.mptd.GameSession;
import com.team4.mptd.MobWave;
import com.team4.mptd.MobWaveEntry;
import com.team4.mptd.Monster;
import com.team4.mptd.R;
import com.team4.mptd.WaitObject;


public class TestMobWave extends ActivityInstrumentationTestCase2<GameActivity> {
	private Activity mActivity;
	private MobWaveEntry mMobWaveEntry;
	private GameSession mSession;
	private MobWave mMobWave;
	private ArrayList<GameObject> list;
	public TestMobWave () {
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
	
		mMobWave = new MobWave(mSession);
	}
	public void testWaveOver(){
		assertTrue(!(mMobWave.isWaveOver()));
	}
	public void testWave(){
		mMobWave.addEntry(mMobWaveEntry);
		mMobWave.createWave();
		list = mMobWave.getWaveList();
		for(GameObject mon: list){
			if(mon.getTypeID() == 99) {
			Log.i("outPut","["+ mon.getTypeID() +"] : " + ((WaitObject)mon).getWaitTime());
			}else {
				Log.i("outPut","["+ mon.getTypeID() +"]");	
			} //should give : [99] : 0, then a series of 0, 99:20, 0, 99:200 repeating
			
		}
		Log.i("outPut", "array size = " + list.size());
	}
}


