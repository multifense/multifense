package com.team4.mptd;

import android.graphics.Bitmap;
import android.graphics.Canvas;
import android.graphics.Rect;

public class GameObject {
	protected Bitmap bmap;
	protected double left;
	protected double top;
	protected Rect thisRect; // the rect specifying the location on the map
	protected Rect srcRect; // The rect specifying which frame from the
	// spritesheet we draw

	protected GameSession session;
	// sprite animation variables
	protected int frameWidth;
	protected int frameHeight;
	protected int noOfFrames;
	protected int currentFrame;
	protected int frameUpdateCounter; // check if time to change frame.
	protected int frameDelayValue; // value between updates
	protected int stateID; // used to determine what row of spritesheet to draw from.

	private int typeID;

	// TypeID is use to identify what type of object it is,.
	public void setTypeID(int iD) {
		typeID = iD; // Monster: , Tower: , Projectile: , WaitObject: 99
	}

	public int getTypeID() {
		return typeID;
	}
	
	public void setFrameDelay(int delay) {
		frameDelayValue = delay;
	}

	public Rect getThisRect() {
		return thisRect;
	}

	public void setTop(double top) {
		this.top = top;
	}

	public void setLeft(double left) {
		this.left = left;
	}

	public GameObject(double x, double y, int frWidth, int frHeight,
			int noFrames) { // constructor, x and y is upper
							// left corner of object and its
							// bitmap.
		left = x;
		top = y;
		frameWidth = frWidth;
		frameHeight = frHeight;
		noOfFrames = noFrames;
		currentFrame = 0;
		frameDelayValue = 2; // 15 fps
		frameUpdateCounter = frameDelayValue;
		stateID = 0;
	}

	// updates the rect on the map.
	protected void updateRect() {
		thisRect.set((int) left, (int) top, (int) left + frameWidth, (int) top
				+ frameHeight);
	}

	public double getLeft() {
		return left;
	}

	public double getTop() {
		return top;
	}

	public void setSession(GameSession s) {
		session = s;
	}

	public GameSession getSession() {
		return session;
	}

	// ++++++++++
	// This is the draw method. It takes the destination canvas and source
	// rectangle and checks if this object is visible
	// ++++++++++
	public void draw(Canvas canvas, Rect src, float offsetX, float offsetY) {
		// Check if visible

		// check if time to change frame
		if (frameUpdateCounter == 0) {
			currentFrame++;
			if (currentFrame > noOfFrames - 1) { // time to reset current frame
				currentFrame = 0;
			}
			// just update to next frame
			srcRect.left = currentFrame * frameWidth;
			srcRect.right = srcRect.left + frameWidth;
			srcRect.top = stateID * frameHeight;
			srcRect.bottom = srcRect.top + frameHeight;
			
			// reset update counter
			frameUpdateCounter = frameDelayValue;
		} else {
			// decrease counter
			frameUpdateCounter--;
		}
		if (isVisible(src)) {
			// do the drawing of the monster.
			canvas.drawBitmap(bmap, srcRect, new Rect(thisRect.left
					- (int) offsetX, thisRect.top - (int) offsetY,
					thisRect.right - (int) offsetX, thisRect.bottom
							- (int) offsetY), null);

		}
	}

	// isVisible takes the rect representing the screen and checks if this rect
	// is visible by doing an intersect check.
	protected boolean isVisible(Rect src) {

//		if (src == null)
//			System.out.println("Src rect null!");
//		if (thisRect == null)
//			System.out.println("thisRect null!!");
		return (src.left < thisRect.right && src.right > thisRect.left)
				&& (src.top < thisRect.bottom && src.bottom > thisRect.top);
	}

	public Bitmap getBitmap() { // returns the bitmap of the object
		return bmap;
	}

	public void setBitmap(Bitmap sprite) {
		// overloading parent method to initiate rect based on frame instead of
		// bitmap
		bmap = sprite;
		thisRect = new Rect((int) left, (int) top, (int) left + frameWidth,
				(int) top + frameHeight);
		srcRect = new Rect(0, 0, frameWidth, frameHeight);
	}

	// Return the center X coordinate of the rectangle
	public double getCenterX() {
		return thisRect.centerX();
	}

	// return the center Y coordinate of the rectangle
	public double getCenterY() {
		return thisRect.centerY();
	}

	// findDistance checks distance between the center position of this object
	// and the center position of target object
	public double findDistance(GameObject targ) {
		double pyth = 0;

		double targX = targ.getCenterX() - this.getCenterX();
		double targY = targ.getCenterY() - this.getCenterY();
		pyth = Math.sqrt(Math.pow(targX, 2) + Math.pow(targY, 2));

		return pyth;

	}
}
