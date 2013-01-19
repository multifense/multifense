package com.team4.mptd;

import android.view.GestureDetector.SimpleOnGestureListener;
import android.view.MotionEvent;

public class GameListener extends SimpleOnGestureListener {

	private GameSession session;

	public GameListener(GameSession session) {
		super();
		this.session = session;
	}

	// onDown has to be implemented (and return true) to allow onScroll to fire.
	@Override
	public boolean onDown(MotionEvent e) {
		return true;
	}

	// on single click try to build tower on that location.
	@Override
	public boolean onSingleTapConfirmed(MotionEvent e) {
		// TODO: here we will check if build menu selected. and what tower is
		// chosen.
		session.onTap(e.getX(), e.getY());
		return true;
	}

	// e1 is the down event fired, e2 is the move event fired. distanceX and
	// distanceY is the incremental movement of a scroll
	@Override
	public boolean onScroll(MotionEvent e1, MotionEvent e2, float distanceX,
			float distanceY) {

		session.dragEvent(distanceX, distanceY, e1);

		// event handled. moving on.
		return true;
	}

	public boolean onUp(MotionEvent e) {

		return true;
	}
}
