package com.team4.mptd;

import java.util.ArrayList;

import android.graphics.Canvas;
import android.graphics.Paint;
import android.graphics.Rect;
import android.util.Log;
import android.graphics.Point;

public class Map extends GameObject {
	private GamePath path;
	private Rect screenRect;
	private ArrayList<Rect> pathRects;

	// Constructor takes a monster path and resources variable (provided by the
	// calling context)
	public Map(int screenWidth, int screenHeight) {
		// set pos to (0,0)
		super(0, 0, 0, 0, 0);
		path = new GamePath();
		screenRect = new Rect(0, 0, screenWidth, screenHeight);
	}
	
	public void preparePath() {
		initiatePathRect();
	}

	private void initiatePathRect() {
		pathRects = new ArrayList<Rect>();

		// for all gamepath points construct rects stretching between all rects.
		// path width 100.
		int width = 50; // 100/2 either way.
		GamePathPoint pointStart = path.pathStart;
		while (pointStart != path.getEndPoint()) {
			Point p = pointStart.p;
			// creates a new rect with left, top == x - width, y - width, right,
			// bottom == next.x + width, next.y + width
			Rect tempRect = new Rect((int) (p.x - width), (int) (p.y - width),
					(int) (pointStart.next.p.x + width),
					(int) (pointStart.next.p.y + width));
		
			tempRect.sort();
	
			pathRects.add(tempRect);
			pointStart = pointStart.next;
		}
	}

	public boolean intersect(Rect checkRect) {
		
		for (Rect r : pathRects) {
			if( Rect.intersects(r, checkRect)) {
				return true;
			}
		}
		return false;
	}

	public GamePath getPath() {
		return path;
	}
	
	@Override
	// ++++++++++
	// Draws part of the map specified by r onto the canvas. Takes source
	// rectangle to draw
	// ++++++++++
	public void draw(Canvas c, Rect r, float offsetX, float offsetY) {
		// possible performance increase if we safe a Rect with screen info on
		// construction of the object.
		c.drawBitmap(bmap, r, screenRect, null);

	}
	
//	public void drawRects(Canvas c, Rect r, float offsetX, float offsetY) {
//		/// WARNING: code below not working as intended. uncomment at your own risk!
////		// Draw the rects for path
//		Paint p = new Paint();
//		p.setARGB(100, 200, 0, 0);
//		for (Rect rec : pathRects) {
//			// check visibility TODO: this is a modified version of the
//			// isvisible method in gameobject. modidfy that one and use it here
//			// instead.
//			Rect temp = new Rect((int) (rec.left - offsetX), (int)(rec.top - offsetY), (int)(rec.right - offsetX), (int)(rec.bottom - offsetY));
//			c.drawRect(temp, p);
//		}
//	}

	public GamePath getMonsterPath() {
		return path;
	}
	
	public ArrayList<Rect> getPathRect() {
		return pathRects;
	}

}
