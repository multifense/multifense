package com.team4.mptd.test;

import junit.framework.TestCase;

import com.team4.mptd.Player;


public class TestPlayer extends TestCase{
	public TestPlayer(String name) {
		super(name);
		
	}
	public static void main(String[] args) {
		junit.textui.TestRunner.run(TestPlayer.class);
	}
	
	public void testSetAndGet() {
		Player p = new Player();
		// test setting and getting methods (sometimes called adjust)
		assertEquals((double) 100, p.getPlayerHP());
		assertEquals((int)500, p.getPlayerGold());
		assertEquals((int)0, p.getMobWaveCount());
		// lower hp by 25 then assert hp == 75
		p.adjustPlayerHP((double) -25);
		assertEquals((double) 75, p.getPlayerHP());
		// lower gold by 125 then assert gold == 375
		p.adjustPlayerGold((int) - 125);
		assertEquals((int) 375, p.getPlayerGold());
		// test increment monsters killed and assert increased
		p.defeatedMonster();
		assertEquals(1, p.getDefeatedMonsters());
		// test increment mob wave count and assert increased
		p.incrementMobWaveCount();
		assertEquals(1, p.getMobWaveCount());
	}
}
