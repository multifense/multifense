package com.team4.mptd;

public class WaitObject extends GameObject{
 private int waitTime;  // int vairabel..
	public WaitObject(int time){
		super(0, 0, 0, 0, 0);
		this.setTypeID(99);	//the WaitObject type ID = 99
		waitTime = time;	// set int vaitabel...
	}
	
	public int getWaitTime(){ // get int variabel....
		return waitTime;
	}
}
