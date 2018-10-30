using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

using SharpOSC;
using muse_osc_server;
using DebugTools;
// using OSC_receiver;

public class OSCManager : MonoBehaviour {

	public int port = 8000;
	public string IpAddress = "127.0.0.1";

	public GraphVisualizer graph;
	public OSC_receiver go;

	private UDPListener listener;
	private UDPSender sender;
	private Dictionary<string, string> dictAddress; 

	// Use this for initialization
	void Start () {

		// Initialise le dictionnaire des addresse d'intérêts
		initDictionaryAddress();

		// Callback function for received OSC messages. 
		// Prints EEG and Relative Alpha data only.
		HandleOscPacket callback = delegate(OscPacket packet)
		{
			var messageReceived = (OscMessage)packet;
			var addr = messageReceived.Address;
			

			if(dictAddress.ContainsKey(addr)) {
				int nbArgs = dictAddress[addr].Length;
				displayMessage(messageReceived, nbArgs);
			}
		};

		// Create an OSC server.
		listener = new UDPListener(port, callback);
		Debug.Log("OSC ready");

		// SendOSCTest();
		sender = new UDPSender(IpAddress, port);
	
	}

	void Update() {
		float value = 0.2f*(Mathf.Sin(2*Mathf.PI*Time.time));
		sender.Send(new OscMessage("/muse/eeg", value));

	}

	void initDictionaryAddress() {
		dictAddress = new Dictionary<string, string>();
		dictAddress.Add("/muse/eeg","ffff");
		// dictAddress.Add("/muse/elements/delta_relative","ffff");
		// dictAddress.Add("/muse/elements/theta_relative","ffff");
		// dictAddress.Add("/muse/elements/alpha_relative", "ffff");
		// dictAddress.Add("/muse/elements/beta_relative","ffff");
		// dictAddress.Add("/muse/elements/gamma_relative","ffff");

		// dictAddress.Add("/muse/elements/blink","i");
		// dictAddress.Add("/muse/elements/jaw_clench","i");
		
	} 

	void displayMessage(OscMessage message, int nbArgs) {
		StringBuilder stringMsg = new StringBuilder();
		stringMsg.Append(message.Address + ": |");

		foreach(var arg in message.Arguments) {
					stringMsg.Append(arg);
					stringMsg.Append(" | ");
				}
		
		// graph.setNewValue(value);
		// for (int i=0; i<nbArgs; i++) {
		// 	go.setValue((float)message.Arguments[0]);
		// }
		float value = (float)message.Arguments[0];
		go.setValue(value);
		// Debug.Log(stringMsg);
		
	}

	void SendOSCTest() {
		var sender = new UDPSender(IpAddress, port);
		sender.Send(new OscMessage("/muse/eeg", 2.3f, 1.5f, 3.7f, 0.2f));
		sender.Send(new OscMessage("/muse/elements/alpha_relative", 23f, 42.01f, 65f, 2.4f));
	}

	void End() {
		listener.Close();
	}

}

