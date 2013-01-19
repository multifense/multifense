package com.team4.mptd;

import android.graphics.Bitmap;
import android.graphics.Canvas;
import android.graphics.Paint;
import android.graphics.Rect;

public class Monster extends GameObject {

	private double currentHealth;
	private double maxHealth = 100;
	private double directionX;
	private double directionY;
	private double speed = 1;
	private double currDistance = 0;
	private double monsterDmg;
	private boolean isAlive;
	private boolean killedByPlayer = false;
	private GamePath newPath;
	private GamePathPoint lastPoint;
	private Rect healthRect;
	private double hpMod;
	private double armorMod;
	private double speedMod;
	private int cost;
	private int ownerID;

	// param list: the int type of the monster, the sprite of the monster, the
	// path of the monster, the GameSession instance, frame width, frame height,
	// number of frames
	public Monster(double hpMod, double armorMod, double speedMod,
			Bitmap sprite, GamePath path, int frWidth, int frHeight,
			int noFrames) {
		super(path.pathStart.p.x - frWidth / 2, path.pathStart.p.y - frHeight
				/ 2, frWidth, frHeight, noFrames);

		this.hpMod = hpMod;
		this.armorMod = armorMod;
		this.speedMod = speedMod;
		
		// COST OF MONSTER HARD CODED
		cost = 2;

		maxHealth = maxHealth * hpMod;
		currentHealth = maxHealth;
		speed = speed * speedMod;
		isAlive = true;
		newPath = path;
		lastPoint = newPath.pathStart;
		currDistance = lastPoint.length;
		directionX = lastPoint.dir.x;
		directionY = lastPoint.dir.y;
		if (sprite != null) {
			setBitmap(sprite);
		}
		monsterDmg = 1;

	}
	
	private void updateStateID() {
		if(directionX == 0) {
			if(directionY == 1) { //direction is down
				stateID = 1;
			} else { // direction is up
				stateID = 0;
			}
		} else {
			if(directionX == 1) { // direction is right
				stateID = 2;
			} else { // direction is left
				stateID = 3;
			}
		}
	}

	@Override
	public void draw(Canvas canvas, Rect src, float offsetX, float offsetY) {
		super.draw(canvas, src, offsetX, offsetY);

		// Draw health bar
		healthRect = new Rect(thisRect.left - (int) offsetX, thisRect.top - 20
				- (int) offsetY, thisRect.left
				+ (int) ((currentHealth / maxHealth) * frameWidth)
				- (int) offsetX, thisRect.top - 10 - (int) offsetY);
		Paint p = new Paint();
		p.setARGB(255, 127, 255, 0);
		canvas.drawRect(healthRect, p);
	}
	
	public int getCost() {
		return cost;
	}

	public double getDmg() {
		return monsterDmg;
	}

	public boolean isAlive() {
		return isAlive;
	}

	public void setSpeed(double sp) { // sets the speed of the monster
		speed = sp;
	}

	public double getHealth() { // returns the current health of the monster
		return currentHealth;
	}

	public double getMaxHealth() {
		return maxHealth;
	}

	public double getSpeed() {
		return speed;
	}
	public void setOwner(int owner){
		ownerID = owner;
	}
	public int getOwner(){
		return ownerID;
	}

	public void takeDamage(double dmg) { // applies damage taken to monster
											// health
		currentHealth = currentHealth - dmg;
		if (currentHealth <= 0) {
			session.monsterWasKilled(this);
			session.getPlayer().defeatedMonster();
			isAlive = false;
			killedByPlayer = true;
		}
	}

	public boolean isKilledByPlayer() {
		return killedByPlayer;
	}

	// Updates the status of the monster
	public void update() {
		// update the position of the monster
		move();
	}

	// changes the monsters position
	public void move() {
		// Move monster position distance governed by its speed.
		moveDistance(speed);
		
		// If monster just passed the next waypoint
		if (currDistance <= 0) {

			lastPoint = lastPoint.next;
			if (lastPoint.next == null) {
				// monster passed last point, time to despawn.
				isAlive = false;
				session.monsterPassedThrough(this);
				return;
			}
			// set new left, top to first waypoint coordinates and direction.
			left = lastPoint.p.x - frameWidth / 2; // compensate for left and
													// top reference benig
													// corner of monster frame
			top = lastPoint.p.y - frameHeight / 2;
			directionX = lastPoint.dir.x;
			directionY = lastPoint.dir.y;

			// time to move the remaining distance in the new direction
			// we always do a remainder move since we rarely will have the
			// condition currDistance == 0.
			moveDistance(-currDistance);
			// Set currDistance to next length
			currDistance = lastPoint.length + currDistance;
		}

		// update the position of the monster
		updateRect();
		// update state direction of the monster, used by animation to select correct row.
		updateStateID();
	}

	// unconditionally adjusts the monsters position
	public void moveDistance(double distance) {
		double distX = distance * directionX;
		double distY = distance * directionY;
		left = left + distX;
		top = top + distY;
		// Adjusts the currDistance counter.
		currDistance = currDistance - distance;
	}
	
	public void setCost(int cost) {
		this.cost = cost;
	}

	@Override
	public Monster clone() {
		Monster mon = new Monster(hpMod, armorMod, speedMod, bmap, newPath,
				frameWidth, frameHeight, noOfFrames);
		mon.setTypeID(this.getTypeID());
		
		return mon;
	}
}
