//<<<<<<< .mine
//package com.team4.mptd.test;
//
//import android.graphics.Bitmap;
//import android.graphics.BitmapFactory;
//
//import com.team4.mptd.*;
//import com.team4.mptd.R;
//
//import junit.framework.TestCase;
//import android.graphics.Bitmap;
//import android.graphics.BitmapFactory;
//
//import junit.framework.TestCase;
//import com.team4.mptd.Monster;
//
//public class TestMonster extends TestCase {
//
//	GamePath path = new GamePath();
//	GameThreadTestClass t = new GameThreadTestClass();
//	GameSession session = new GameSession(t);
//
//	public TestMonster(String name) {
//		super(name);
//	}
//
//	// monsterparam list: the int type of the monster, the sprite of the
//	// monster, the
//	// path of the monster, the GameSession instance
//	public void testPosX() {
//		Monster hi = new Monster(new MonsterType(1, 1, 1), null, path, session);
//		assertEquals(0.0, hi.getLeft());
//	}
//
//	public void testPosY() {
//		Monster hi = new Monster(new MonsterType(1, 1, 1), null, path, session);
//		assertEquals(200.0, hi.getTop());
//	}
//
//	public void testHealth() {
//		Monster hi = new Monster(new MonsterType(1, 1, 1), null, path, session);
//		assertEquals(100.0, hi.getHealth());
//	}
//
//	public void testMove() {
//		double x = 0;
//		double y = 200;
//		double dX = 1;
//		double dY = 0;
//		double sp = 5;
//		Monster hi = new Monster(new MonsterType(1, 1, 1), null, path, session);
//		hi.update();
//		assertEquals(x + (sp * dX), hi.getLeft());
//		assertEquals(y + (sp * dY), hi.getTop());
//	}
//
//	public void testReduceHealth() {
//		double hp = 100;
//		double dmg = 2;
//		Monster hi = new Monster(new MonsterType(1, 1, 1), null, path, session);
//		hi.takeDamage(dmg);
//		assertEquals(hp - dmg, hi.getHealth());
//	}
//
//	public void testDead() { // set monster to initial hp = 10, test reducing hp
//								// by 10. assert monster isAlive==false
//		double hp = 10;
//		double dmg = 10;
//		Monster hi = new Monster(new MonsterType(1, 1, 1), null, path, session);
//		hi.takeDamage(dmg);
//		assertEquals(false, hi.isAlive());
//	}
//
//	public static void main(String[] args) {
//		junit.textui.TestRunner.run(TestMonster.class);
//	}
//}
//=======
package com.team4.mptd.test;

import android.app.Activity;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;

import com.team4.mptd.*;
import com.team4.mptd.R;

import junit.framework.TestCase;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.test.ActivityInstrumentationTestCase2;

import junit.framework.TestCase;
import com.team4.mptd.Monster;


public class TestMonster extends ActivityInstrumentationTestCase2<GameActivity> {
	private Activity mActivity;
	private GameSession mSession;
	
	public TestMonster() {
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
	
	// test the spawn method in gamesession.
	public void testMonsterCreated() {
		mSession.addMonster(new Monster(1, 1, 1, BitmapFactory.decodeResource(mActivity.getResources(), R.drawable.bombmonster), new GamePath()));
		assertEquals(1, mSession.getMonsters().size());
	}
	

	// monsterparam list: the int type of the monster, the sprite of the
	// monster, the
	// path of the monster, the GameSession instance

	public void testCustomMonster() {
		Monster m = new Monster(1, 1, 1, BitmapFactory.decodeResource(mActivity.getResources(), R.drawable.bombmonster), new GamePath());
		assertNotNull(m);
		assertNotNull(m.getDmg());
		assertNotNull(m.getHealth());
		assertNotNull(m.getSpeed());
		assertNotNull(m.getTypeID());
		assertNotNull(m.getBitmap());
		assertEquals(true, m.isAlive());
		assertEquals(m.getMaxHealth(), m.getHealth());
		

	}


	// monster test for movement
	public void testMovement() {
		Monster m = new Monster(1, 1, 1, BitmapFactory.decodeResource(mActivity.getResources(), R.drawable.bombmonster), new GamePath());
		double x = m.getLeft();
		double y = m.getTop();
		m.update();
		// Test movement moves in hardcoded path in X direction. see GamePath for info on path.
		assertTrue(x != m.getLeft());
		assertTrue(y == m.getTop());

	}
	
	// monster test for reducing health
	public void testHealth() {

		Monster m = new Monster(1, 1, 1, BitmapFactory.decodeResource(mActivity.getResources(), R.drawable.bombmonster), new GamePath());
		m.takeDamage(10);
		assertTrue(m.getMaxHealth() != m.getHealth());
		m.takeDamage(10000);
		assertTrue(m.getHealth() <= 0);
		assertTrue(!m.isAlive());
		assertTrue(m.isKilledByPlayer());
	}

}
//>>>>>>> .r339
