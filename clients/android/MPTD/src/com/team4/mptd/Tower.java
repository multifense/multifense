package com.team4.mptd;

import java.util.ArrayList;

import android.graphics.Bitmap;
import android.graphics.Canvas;
import android.graphics.Rect;

public class Tower extends GameObject implements Cloneable {

	private int cost;
	private double attackCooldown;
	private double range = 500;
	private double attackUpdateCounter;
	private double pyth;
	private Monster targetMonster = null;
	private Monster nearestTarget;
	private TrackingProjectile proj;
	private TrackingProjectile arcProj;

	// param list: x pos, y pos, firerate, damage, range, projectile speed,
	// projectile bitmap, tower cost,tower frame widht, tower frame height,
	// tower noOfframes proj frame width, proj frame height, proj noOf frames
	public Tower(double x, double y, double fr, double range, Bitmap sprite,
			int cost, int frWidth, int frHeight, int noFrames, int typeID, TrackingProjectile arProj) { // constructor
		super(x, y, frWidth, frHeight, noFrames);

		arcProj = arProj;
		attackCooldown = fr;
		attackUpdateCounter = fr;
		this.range = range;
		this.cost = cost;
		setTypeID(typeID);
		setBitmap(sprite);
	}

	// protected Object clone() {
	// Object tmp = super.clone();
	// ((Tower) tmp.)updateRect();
	// return tmp;
	// }
	@Override
	public Tower clone() {
		Tower tmp = new Tower(left, top, attackCooldown, range, bmap, cost,
				frameWidth, frameHeight, noOfFrames, getTypeID(), arcProj);
		tmp.setBitmap(getBitmap());
		return tmp;
	}

	// // Default construcotr. deprecated:p
	// public Tower() {
	// super(400, 200, frameWidth, frameHeight, noOfFrames);
	// left = 400;
	// top = 200;
	// attackCooldown = 100; // attack rate not yet defined!!!!!!!
	// damage = 5;
	// range = 5000;
	// cost = 1;
	// attackUpdateCounter = attackCooldown;
	// }


	@Override
	public void draw(Canvas canvas, Rect src, float offsetX, float offsetY) {
		// draw this tower
		super.draw(canvas, src, offsetX, offsetY);
	}

	public void attack(ArrayList<GameObject> monsters) {
		if (attackUpdateCounter < attackCooldown) {
			attackUpdateCounter++;
		}

		if (targetMonster != null) {
			if (!targetMonster.isAlive()) {
				targetMonster = null;
			}
		} else {
			targetMonster = selectTarget(monsters);

		}
		// if(!targetMonster.isAlive() && targetMonster != null){
		// targetMonster = null;
		// }

		if (attackUpdateCounter >= attackCooldown) { // check if it is time to
			// fire
			if (targetMonster != null) { // check if there is a monster targeted
				// since last time we fired
				if (findDistance(targetMonster) <= range) { // check if that
					// monster is still in range
					// FIYAH TEH MISZILES!
					proj = arcProj.clone(left, top, targetMonster);
					if(session.getSoundManager() != null) {
						session.playSound( getTypeID() );
					}
					proj.setFrameDelay(1);
					attackUpdateCounter = 0;
				} else {
					targetMonster = null;
				}
			}
			if (targetMonster == null) {
				targetMonster = selectTarget(monsters); // if we have no
				// targeted monster,
				// find one
				if (targetMonster != null) { // check if there is a monster in
					// range
					proj = arcProj.clone(left, top, targetMonster);
					if(session.getSoundManager() != null) {
						session.playSound(getTypeID());
					}
					proj.setFrameDelay(1);
					// if
					// there
					// is,
					// FIRE!
					attackUpdateCounter = 0;
				}
			}
		}
	}

	// selects a monster in range
	public Monster selectTarget(ArrayList<GameObject> monsters) {
		pyth = 0;
		nearestTarget = null;

		for (GameObject tempMonster : monsters) {
			if (nearestTarget == null) {
				pyth = findDistance(tempMonster);
				if (range >= pyth) {
					nearestTarget = (Monster) tempMonster;
				}
			}
			//			else {
			//				if (findDistance((Monster) tempMonster) < findDistance((Monster) nearestTarget)) {
			//					nearestTarget = (Monster) tempMonster;
			//				}
			//			}
		}
		return nearestTarget;
	}

	public void resetProj() {
		proj = null;
	}

	public void resetTarget() {
		targetMonster = null;
	}

	public TrackingProjectile getProj() {
		return proj;
	}

	public Monster getTarget() {
		return targetMonster;
	}

	public double getRange() {
		return range;
	}

	public double getFireRate() { // returns the towers fire rate
		return attackCooldown;
	}

	// Returns the cost of this tower
	public int getCost() {
		return cost;
	}
}