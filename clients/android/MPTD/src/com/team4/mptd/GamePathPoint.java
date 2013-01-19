package com.team4.mptd;

import android.graphics.Point;

//class Point {
//	public double x;
//	public double y;
//	
//	public Point() {
//		
//	}
//	
//	public Point (double x, double y) {
//		this.x = x;
//		this.y = y;
//	}
//}

// a single point on a path, with a length, unless it is the end point
public class GamePathPoint {

	public GamePathPoint next; // pointer to the next path point or nil if final point
	public Point dir = new Point(); // direction vector (normalized to length 1)
	public double length; // the distance between this point and the next point
	public Point p = new Point(); // point position (double x,y)

	/*
	 * constructor : GamePathPoint arguments : p = point with x,y coordinates
	 * (doubles, not ints)
	 * 
	 * Regular constructor with a single point on the map. Requires generation
	 * via connectWithPathPoint below to function, unless it is the final point.
	 */
	public GamePathPoint(Point p) {
		this.p = p;
		next = null;
	}

	/*
	 * constructor : GamePathPoint arguments : p = position dir = direction
	 * vector, normalized length = distance to move
	 * 
	 * Special constructor used when dir and length have been generated and are
	 * loaded from a serialized state. Requires setNext() method to function.
	 */
	public GamePathPoint(Point p, Point dir, double length) {
		this.p = p;
		this.dir = dir;
		this.length = length;
	}

	/*
	 * method : connectWithPathPoint arguments : pathPoint = GamePathPoint
	 * object that should follow this point
	 * 
	 * This method sets the next pointer to the path point, creates a point
	 * (vector) which points at the next path point from self (next.p minus p),
	 * then calculates the length of this point, saving this in length, and the
	 * normalized vector of this point, storing it as dir.
	 */
	public void connectWithPathPoint(GamePathPoint pathPoint) {
		next = pathPoint;
		
		dir.x = pathPoint.p.x - this.p.x;
		dir.y = pathPoint.p.y - this.p.y;

		length = Math.sqrt(dir.x * dir.x + dir.y * dir.y);

		dir.x = (int) (dir.x / length);
		dir.y = (int) (dir.y / length);
	}

	/*
	 * method : setNext arguments : pathPoint = GamePathPoint object that should
	 * follow this point
	 * 
	 * Simply sets next to pathPoint. This is used when secondary constructor
	 * above is used.
	 */
	public void setNext(GamePathPoint pathPoint) {
		next = pathPoint;
	}
};
