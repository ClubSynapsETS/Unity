using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using DebugTools;
using System.IO;

public class DebugTest : MonoBehaviour {

	string[] test_strings = new string[] {
		"This is a test string",
		"This is where your custom output can go",
		"red lorry yellow lorry red lorry yellow lorry",
		"The quick brown fox jumped over the lazy dog",
		"She sells sea shels on the sea shore",
		"This is an example output to a drawer",
		"Oak is strong and also gives shade.",
		"Cats and dogs each hate the other.",
		"The pipe began to rust while new.",
		"Developed by Aaron Walwyn",
		"www.aaronwalwyn.com",
		"FREE FOR A LIMITED TIME",
		"Gotta get me some of that szechuan sauce",
		"chaos is a ladder",
		"The quick brown fox jumped over the lazy dog",
		"The best laid plans of mice and men often go awry",
		"I am legion",
		"Please consider leaving a rating on the ",
		"DebugTools is actively in development",
		"Check out DefendTheRocket on Android",
		"This is a test of a drawer",
		"Got a suggestion, leave it as a comment on the asset store",
		"check out our forum page for details on the latest updates",
		"Feedback? email me@aaronwalwyn.com",
		"It's actually quite hard to think of lots of random strings"
	};

	GraphCanvas fpsGraph, deltaGraph, lineGraph;
	//Grapher.Graph fpsGraph, deltaGraph;

	void Awake() {
		Setup.Create ();
	}

	// Use this for initialization
	void Start () {
		Console.Log ("Product Name:\t\t" + Application.productName);
		Console.Log ("Company Name:\t\t" + Application.companyName);
		Console.Log ("Product Version:\t" + Application.version);
		Console.Log ("Unity3D Version:\t" + Application.unityVersion);
		Console.Log ("Log Time:\t\t" + System.DateTime.Now.ToFileTime() + "\n");
        Console.Log("");
        Console.Log(LogType.Warning, "This is an example of a warning!");
        Console.Log(LogType.Exception, new IOException("Oh No! There was an exception"));

		DrawManager.DrawVariable("ConsoleOutput", "This is a test of the draw manager");

		DrawManager.DrawVariable("Wall", "This is a test of the draw manager");

		fpsGraph = Grapher.CreateGraph("Realtime Graph", GraphCanvasType.LINE_PLOT, true, true, 0f, 100f);
		deltaGraph = Grapher.CreateGraph("Plot Graph", GraphCanvasType.SCATTER_PLOT, false, false, 100f, 100f);
		lineGraph = Grapher.CreateGraph("Line Graph", GraphCanvasType.LINE_PLOT, false, false, 100f, 100f);

		for(int i = 0; i< 150; i++)
			deltaGraph.SetPlotPoint(new Vector2(Random.value * 50, Random.value * 100));

        for (int i = 0; i < 9; i++)
            lineGraph.SetPlotPoint(new Vector2((i + 1) * 10, Random.Range(0, 100)));


		InvokeRepeating("PostRandom", 2.5f, 2.5f);
    }
	
	// Update is called once per frame
	void Update () {
		Tracker.Track ("FPS", (1f / Time.deltaTime));
		Tracker.Track ("Time", Time.time);

		fpsGraph.SetPlotPoint(new Vector2(0, 1f / Time.deltaTime));
		//deltaGraph.latestValue = Time.deltaTime;
	}

	void PostRandom() {

		DrawManager.DrawVariable("ConsoleOutput", test_strings[Random.Range(0, test_strings.Length)]);

		DrawManager.DrawVariable("Wall", test_strings[Random.Range(0, test_strings.Length)]);

	}
}
