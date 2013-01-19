package com.team4.mptd;

import java.util.ArrayList;

public class GameUpdate {
	private GameSession session;
	private ArrayList<GameObject> monsters;
	private ArrayList<GameObject> towers;

	GameUpdate(GameSession s) {

		this.session = s;
		if (!s.getMultiplayer()) {
			session.setupMobWave();
		}
		monsters = session.getMonsters();
		towers = session.getTowers();
	}

	public void updateGame() {
		// update monster waves
		if(session.getPlayer().getPlayerHP() <= 0){
			session.getPlayer().adjustPlayerHP(-session.getPlayer().getPlayerGold());
		}
		if (!session.getMultiplayer()) {
			session.updateWave();
		}else {
			((MultiplayerGameSession)session).updateMpWave();
		}

		synchronized (session) {
			// Update monsters
			for (GameObject monsterObjectToUpdate : monsters) {
				((Monster) monsterObjectToUpdate).update();
			}
		}

		synchronized (session) {
			// Update towers
			for (GameObject towerToUpdate : towers) { 
				((Tower) towerToUpdate).attack(monsters);
				// Get new projectiles
				if (((Tower) towerToUpdate).getProj() != null) {
					session.getProjectileList().add(
							((Tower) towerToUpdate).getProj());
					((Tower) towerToUpdate).resetProj();
				}
			}
		}

		// Update projectiles
		for (GameObject projectile : session.getProjectileList()) {
			((TrackingProjectile) projectile).update();
			if (((TrackingProjectile) projectile).hasHit()) {
				session.getTrashList().add(projectile);
			}
		}

		// Remove all monsters and projectiles marked for removal.
		session.emptyTrash();

		if (session.getMultiplayer()) {
			((MultiplayerGameSession) session).updateMultiplayerData();
		}

	}
}