package com.team4.mptd;

import java.io.IOException;
import java.util.HashMap;

import android.content.Context;
import android.media.AudioManager;
import android.media.MediaPlayer;
import android.media.SoundPool;
import android.util.Log;

public class SoundManager {
	private SoundPool mSoundPool;
	private HashMap<Integer, Integer> mSoundPoolMap;
	private final int NUMBER_OF_STREAMS = 4;
	private int streamNumber = 0;
	private int[] streamIdArray = new int[NUMBER_OF_STREAMS];
	private AudioManager mAudioManager;
	private Context mContext;
	private MediaPlayer mediaPlayer;
	int streamID = 0;

	public void initSounds(Context context) {
		mContext = context;
		mSoundPool = new SoundPool(NUMBER_OF_STREAMS,
				AudioManager.STREAM_MUSIC, 0);
		mSoundPoolMap = new HashMap<Integer, Integer>();
		mAudioManager = (AudioManager) mContext
				.getSystemService(Context.AUDIO_SERVICE);
		mediaPlayer = MediaPlayer.create(context, R.raw.song);
		mediaPlayer.setLooping(true);
		
		//Add tower projectile sounds
		addSound(TypeID.TOWER_CANNON, R.raw.cannon);
		addSound(TypeID.TOWER_MAGIC, R.raw.magic);
		addSound(TypeID.TOWER_FIRE, R.raw.fire);
		addSound(TypeID.TOWER_ICE, R.raw.ice);
		
		//Add game screen sounds
		addSound(TypeID.LOSE_SCREEN, R.raw.multowerslowed);
		addSound(TypeID.WIN_SCREEN, R.raw.youwin);
	}

	public void addSound(int index, int SoundID) {
		mSoundPoolMap.put(index, mSoundPool.load(mContext, SoundID, 1));
	}

	public void playSound(int index) {
		float streamVolume = mAudioManager
				.getStreamVolume(AudioManager.STREAM_MUSIC);
		streamVolume = streamVolume
				/ mAudioManager.getStreamMaxVolume(AudioManager.STREAM_MUSIC);

		streamIdArray[streamNumber] = mSoundPool.play(mSoundPoolMap.get(index),
				streamVolume, streamVolume, 1, 0, 1f);
		streamNumber++;
		if (streamNumber >= 4) {
			streamNumber = 0;
		}
	}

	public void playLoopedSound(int index) {
		float streamVolume = mAudioManager
				.getStreamVolume(AudioManager.STREAM_MUSIC);
		streamVolume = streamVolume
				/ mAudioManager.getStreamMaxVolume(AudioManager.STREAM_MUSIC);
		mSoundPool.play(mSoundPoolMap.get(index), streamVolume, streamVolume,
				1, -1, 1f);
	}

	public void playBackgroundMusic() {
		mediaPlayer.start();
	}

	public void stopBackgroundMusic() {
		Log.i("Output", "StopBGM called.");
		mediaPlayer.pause();
		mediaPlayer.stop();
	}
	
	public boolean musicIsPlaying() {
		try {
			return mediaPlayer.isPlaying();
		} 
		catch(IllegalStateException e) {
			return false;
		}
	}
	
	private void stopSound() {
		for (int i = 0; i < 4; i++) {
			try {
				Log.i("Output", "Int i= " + i);
				mSoundPool.pause(streamIdArray[i]);
				mSoundPool.stop(streamIdArray[i]);
			} catch (Exception e) {

			}
		}
	}

	public void cleanup() {

		stopSound();
		try {
			mediaPlayer.stop();
			mediaPlayer.release();
		} catch (IllegalStateException e) {
			e.printStackTrace();
		}

		if (mSoundPool != null) {
			mSoundPool.release();
		}

		mSoundPool = null;
		mSoundPoolMap.clear();
		mAudioManager.unloadSoundEffects();
	}
}
