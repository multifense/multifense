package com.team4.mptd;

import java.util.ArrayList;

import android.graphics.Canvas;
import android.graphics.Rect;
import android.view.SurfaceHolder;

public class DrawGraphics {
	private SurfaceHolder holder;

	public DrawGraphics(SurfaceHolder holder) {
		this.holder = holder;
	}

	public void updateGraphics(ArrayList<GameObject> listToDraw, Rect src,
			float offsetX, float offsetY) {
		// lock and get canvas
		Canvas canvas = holder.lockCanvas();
		if (canvas != null) {
			// Draw all objects
			for (GameObject objectToDraw : listToDraw) {
				objectToDraw.draw(canvas, src, offsetX, offsetY);
			}

			// unlock and post
			holder.unlockCanvasAndPost(canvas);
		}
	}
}
