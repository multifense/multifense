package com.team4.mptd;

import java.io.DataInputStream;
import java.io.IOException;
import java.net.Socket;

public class TCPListener extends Thread {
	private MultiplayerGameSession session;
	private Socket mySocket;
	private DataInputStream is;

	public TCPListener(MultiplayerGameSession session, Socket mySocket) {
		this.session = session;
		this.mySocket = mySocket;
		try {
			is = new DataInputStream(this.mySocket.getInputStream());
		} catch (IOException e) {
			System.out.println("error on listener" + e);
		}
	}

	@Override
	public void run() {
		String message;
		while (!mySocket.isClosed()) {
			// loop while socket is open.
			try {
				message = is.readLine();

				session.addMultiplayerData(message);

			} catch (IOException e) {
				System.out.println("tcp listener borking it up " + e);
			}
		}
	}
}
