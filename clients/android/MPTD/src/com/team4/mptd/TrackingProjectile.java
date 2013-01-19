package com.team4.mptd;

import android.graphics.Bitmap;
import android.graphics.Rect;

public class TrackingProjectile extends GameObject {
	private int speed;
	private Monster target;
	private double dmg;
	private boolean reachedTarget = false;
	private double mX;
	private double mY;
	private double dis;

	// TODO: tracking projectiles fastnar om deras targert f�rsvinner
	// mid-flight.

	public TrackingProjectile(double x, double y, double dm, int sp,
			Monster monster, Bitmap bm, int frWidth, int frHeight, int noFrames) { // add
		super(x, y-15, frWidth, frHeight, noFrames);
		if (bm != null) {
			this.setBitmap(bm);
		}
		speed = sp;
		target = monster;
		dmg = dm;
	}
	//clones the projectile adding position and target
	public TrackingProjectile clone(double x, double y, Monster targetMon) {
		TrackingProjectile tmp = new TrackingProjectile(x, y, dmg, speed, targetMon,
				this.getBitmap(),frameWidth, frameHeight, noOfFrames);
		tmp.setBitmap(getBitmap());
		return tmp;
	}


	public void update() {
		if (!target.isAlive()) {
			reachedTarget = true;
			return;
		}
		if (target != null) {
			mX = target.getLeft();
			mY = target.getTop();
			dis = findDistance(target);

			left = left + (mX - left) / (dis / speed);
			top = top + (mY - top) / (dis / speed);
			
			// Check if intersect with monster
			if (Rect.intersects(thisRect, target.thisRect)) {
				target.takeDamage(dmg);
				reachedTarget = true;
			}
		}

		updateRect(); // important to update this to allow the projectile to be
						// drawn when its initial position is not in view
	}

	public boolean hasHit() {

		return reachedTarget;
	}
}
