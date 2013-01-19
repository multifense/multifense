package com.team4.mptd;

import android.content.res.Resources;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;

public class InitRes {
	private static InitRes session;

	public static InitRes getSession() {
		if (session == null) {
			session = new InitRes();
		}

		return session;
	}
	
	public InitRes() {
		
	}
	
	public void init(GameSession session) {
		// +++++++++++++++++++++++++++++++++++++++++
		// initialize bitmaps
		Resources res = session.activity.getResources();
		switch (session.mapID) {
		case 2: {
			session.getMap().setBitmap(Bitmap.createScaledBitmap(
					BitmapFactory.decodeResource(res, R.drawable.awsomemap),
					1600, 960, false));
			break;
		}
		default: {
			session.getMap().setBitmap(Bitmap.createScaledBitmap(BitmapFactory
					.decodeResource(res, R.drawable.map1600x960new2), 1600,
					960, false));
			break;
		}

		}

		session.towerBmap = Bitmap.createScaledBitmap(
				(BitmapFactory.decodeResource(res, R.drawable.cannontower)),
				50, 50, false);

		session.cannonProjectileBmap = Bitmap.createScaledBitmap(
				(BitmapFactory.decodeResource(res, R.drawable.bullet)), 25, 25,
				false);

		session.mgcTowerBmap = Bitmap.createScaledBitmap(
				(BitmapFactory.decodeResource(res, R.drawable.magictower)), 50,
				50, false);

		session.mgcProjectileBmap = Bitmap.createScaledBitmap(
				(BitmapFactory.decodeResource(res, R.drawable.magicproj)), 25,
				25, false);

		session.frTowerBmap = Bitmap.createScaledBitmap(
				(BitmapFactory.decodeResource(res, R.drawable.firetower)), 50,
				50, false);

		session.frProjectileBmap = Bitmap.createScaledBitmap(
				(BitmapFactory.decodeResource(res, R.drawable.fireprojsprite)),
				100, 25, false);

		session.iceTowerBmap = Bitmap.createScaledBitmap(
				(BitmapFactory.decodeResource(res, R.drawable.igloo)), 50, 50,
				false);

		session.iceProjectileBmap = Bitmap.createScaledBitmap(
				(BitmapFactory.decodeResource(res, R.drawable.igloproj)), 25,
				25, false);

		session.bombMonsterBmap = Bitmap.createScaledBitmap(
				(BitmapFactory.decodeResource(res, R.drawable.bombsprite)),
				200, 200, false);

		session.monsterBmap = Bitmap.createScaledBitmap(
				(BitmapFactory.decodeResource(res, R.drawable.slimesprite)),
				200, 200, false);

		session.hattyBmap = Bitmap.createScaledBitmap(
				(BitmapFactory.decodeResource(res, R.drawable.hatty)), 200,
				200, false);

		session.rockyBmap = Bitmap.createScaledBitmap(
				(BitmapFactory.decodeResource(res, R.drawable.rocky)), 200,
				200, false);

		
	}
}
