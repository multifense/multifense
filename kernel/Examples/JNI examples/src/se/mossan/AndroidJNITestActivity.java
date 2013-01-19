package se.mossan;

import android.app.Activity;	
import android.os.Bundle;

public class AndroidJNITestActivity extends Activity {
	
//	public native void print();
	public native String print1();
    /** Called when the activity is first created. */
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
       setContentView(R.layout.main);
//       print();
       System.out.println(print1());
        
    }
    
    static {
    	try{
//		System.loadLibrary("HelloWorld");
		System.loadLibrary("Source1");
    	} catch (java.lang.ExceptionInInitializerError e) {
    		
    	}
	}
}