package com.team4.mptd;

import android.graphics.Point;

public class GamePath {

	private GamePathPoint pathEnd; // end point
//	public Point spawnPoint; // there be dragons here
	public GamePathPoint pathStart; // starting point


	public GamePath(int mapID) {
		initializePath(mapID);
	}
	
	public GamePath() {
		
	}
	
	public void addCodedPath(Point p, Point dir, double length) {
		GamePathPoint point = new GamePathPoint(p, dir, length);
		if(pathStart == null) {
			pathStart = pathEnd = point;
		} else {
			pathEnd = pathEnd.next = point;
		}
	}


	private void initializePath(int mapID) {
		switch(mapID){
			case 2:
				this.addPathPoint(200,0);
				this.addPathPoint(200,400);
				this.addPathPoint(100,400);
				this.addPathPoint(100,600);
				this.addPathPoint(1400,600);
				this.addPathPoint(1400,150);
				this.addPathPoint(550,150);
				this.addPathPoint(550,800);
				this.addPathPoint(1150,800);
				this.addPathPoint(1150,960);
				break;
				
			default:
					this.addPathPoint(0, 200);
					this.addPathPoint(130, 200);
					this.addPathPoint(130, 750);
					this.addPathPoint(460, 750);
					this.addPathPoint(460, 500);
					this.addPathPoint(700, 500);
					this.addPathPoint(700, 100);
					this.addPathPoint(910, 100);
					this.addPathPoint(910, 730);
					this.addPathPoint(1300, 730);
					this.addPathPoint(1300, 300);
					this.addPathPoint(1600, 300);
					break;
		}
	}

	/*
	 * method : addPoint arguments : x and y coordinates
	 * 
	 * Creates a new GamePathPoint object based on the given coords, and adds it
	 * to the system. This method uses an internal, private method
	 * appendPathPoint which takes a GamePathPoint object. (see private section
	 * below)
	 */
	public void addPathPoint(int x, int y) {
		Point p = new Point();
		p.x = x;
		p.y = y;
		appendPathPoint(new GamePathPoint(p));
	}

	/*
	 * method : loadPathFromFile arguments : filename = filename from which to
	 * read the data returns : boolean describing success loading the path info
	 * from the file in question
	 * 
	 * Load binary values for map pathing from a file on disk. See bottom of
	 * this document for further information.
	 */
	public boolean loadPathFromFile(String filename) {
		return false;
	}

	/*
	 * method : appendPathPoint arguments : pathPoint = the pathPoint to add
	 * 
	 * If the point is the first point (i.e. if pathStart == null), pathStart
	 * and pathEnd are both set to the new object and spawnPoint is set to the
	 * given point (p). Otherwise pathEnd's connectWithPathPoint method is
	 * called, with the newly created object as argument. In all cases, pathEnd
	 * is set to the object.
	 */
	private void appendPathPoint(GamePathPoint pathPoint) {
		if (pathStart == null) {
			pathStart = pathPoint;
//			spawnPoint = pathPoint.p;
		} else {
			pathEnd.connectWithPathPoint(pathPoint);
		}
		pathEnd = pathPoint;
	}

	public GamePathPoint getEndPoint() {
		return pathEnd;
	}

	// ///////////////TEST METHODS////////////////////////

	public double getDirectionVectorX() {
		return pathStart.dir.x;
	}

	public double getDirectionVectorY() {
		return pathStart.dir.y;
	}

	public double getLength() {
		return pathStart.length;
	}

};

//// representation of a map, which includes a GamePath
//class GameMap {
//	public GamePath path; // path associated with this map
//};
