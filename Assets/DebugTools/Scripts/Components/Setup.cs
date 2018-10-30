//==========================================
// Company  : AaronWalwyn
// Package  : Unity DebugTools
// File     : Debug Tools
// Author   : Aaron Walwyn
//==========================================
using System;
using System.IO;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System.Xml.Serialization;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace DebugTools {
	public class Setup : MonoBehaviour {

		//==========================================
		// ENUMERATED TYPES
		//==========================================

		//==========================================
		// VARIABLES
		//==========================================

		public static Setup instance;
		public static DebugSettings settings;
		public Canvas debugCanvas;
		
		//private Texture2D debugGraphTexture;

		public CanvasGroup debugConsoleGroup;
		public CanvasGroup debugTrackerGroup;
		public CanvasGroup debugGrapherGroup;

		public bool initiliseOnStart = false;

		//private int graphIndex = 0;
		

		//==========================================
		// PROPERTIES
		//==========================================


		//==========================================
		// MAIN FUNCTIONS
		//==========================================

		/// <summary>
		/// Creates the tracker.
		/// </summary>
		private void Awake() {
			if (initiliseOnStart) {
				Initialise ();
			}
		}

		/// <summary>
		/// Creates the tracker.
		/// </summary>
		public void Initialise() {

			if (instance == null) {
				instance = this;
				DontDestroyOnLoad (this.gameObject);

				LoadConfig ();

				this.gameObject.AddComponent<DrawManager>().Initialise();

				CreateCanvas ();
				CreateConsole();
				CreateConsoleOutput ();
				CreateConsoleInput();
				CreateTracker ();
				CreateGrapher ();

			} else {
				Destroy (this.gameObject);
			}
		}

		/// <summary>
		/// Creates the tracker.
		/// </summary>
		public  static void Create() {
			if (instance == null) {
				new GameObject ("[ Debug ]").AddComponent<Setup> ().Initialise ();
			}
		}

		/// <summary>
		/// Creates the tracker.
		/// </summary>
		private void LoadConfig() {
			if (settings == null) {

				string xml = "";

				if (LoadSettingsFile (out xml) && xml != "") {
					settings = DebugSettings.FromXML (xml);
				} else {
					settings = new DebugSettings ();
				}
			}

			Assert.IsNotNull (settings);
		}

		/// <summary>
		/// Creates the tracker.
		/// </summary>
		private void CreateCanvas() {
			this.transform.position = Vector3.zero;

			debugCanvas = new GameObject("Canvas").AddComponent<Canvas>();
			debugCanvas.transform.SetParent (this.transform);
			debugCanvas.gameObject.layer = LayerMask.NameToLayer("UI");
			debugCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
			debugCanvas.sortingOrder = 32767;

			GraphicRaycaster gr = debugCanvas.gameObject.AddComponent<GraphicRaycaster>();

			if (UnityEngine.EventSystems.EventSystem.current == null) {
				GameObject g = new GameObject("Event");
				g.transform.SetParent(this.transform);
				UnityEngine.EventSystems.EventSystem.current = g.AddComponent<UnityEngine.EventSystems.EventSystem>();
				g.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
			}
		}

		/// <summary>
		/// Creates the tracker.
		/// </summary>
		private void CreateTracker() {
			GameObject g = new GameObject("Tracker");
			g.AddComponent<Tracker>().Initialise();
		}

		
		private void CreateConsole() {
			GameObject g = new GameObject("Console");
			RectTransform rt = g.AddComponent<RectTransform>();
			rt.SetParent(debugCanvas.gameObject.transform);
			rt.anchorMin = Vector2.zero;
			rt.anchorMax = Vector2.zero;
			rt.pivot = new Vector2(0.5f, 0);

			rt.anchoredPosition = new Vector2(Screen.width / 2f, settings.graphMargin);
			rt.sizeDelta = new Vector2(Screen.width - (2 * settings.graphMargin), (Screen.height / 2));

			Image i = g.AddComponent<Image>();
			i.color = Color.white * 0.5f;
			//i.sprite = Sprite.Create(new Texture2D(1, 1), new Rect)

			debugConsoleGroup = g.AddComponent<CanvasGroup> ();
			HideGroup (debugConsoleGroup);
		}

		/// <summary>
		/// Creates the console.
		/// </summary>
		private void CreateConsoleOutput() {
			
			Assert.IsNotNull (debugConsoleGroup);

			GameObject g = new GameObject("ConsoleOutput");
			g.AddComponent<ConsoleOutput>().Initialise();
		}

		private void CreateConsoleInput() {
			GameObject g = new GameObject("ConsoleInput");
			g.AddComponent<ConsoleInput>().Initialise();
		}

		/// <summary>
		/// Creates the grapher.
		/// </summary>
		private void CreateGrapher() {
			GameObject g = new GameObject("Grapher");
			g.AddComponent<Grapher>().Initialise();
		}

		///
		///
		///
		private void Update() {
			if (Input.GetKeyUp (settings.consoleKey) && debugConsoleGroup != null) {
				SwitchGroup (debugConsoleGroup);
			}

			if (Input.GetKeyUp (settings.trackerKey) && debugTrackerGroup != null) {
				SwitchGroup (debugTrackerGroup);
			}

			if (Input.GetKeyUp (settings.grapherKey) && debugGrapherGroup != null) {
				SwitchGroup (debugGrapherGroup);
			}
		}


		//==========================================
		// HELPER FUNCTIONS
		//==========================================

		public static bool LoadSettingsFile (out string xml) {
			xml = "";

			try {

				xml = File.ReadAllText(Application.streamingAssetsPath + "/DebugTools/settings.xml");

			} catch (Exception e) {
				//Debug.LogException (e);
				return false;
			}

			return (xml != "");
		}

		public static void SwitchGroup(CanvasGroup _cg) {
			if (_cg.alpha > 0f) {
				HideGroup(_cg);
			} else {
				ShowGroup(_cg);
			}
		}

		public static void ShowGroup(CanvasGroup _cg) {
			_cg.interactable = true;
			_cg.alpha = 1f;
			_cg.blocksRaycasts = true;
		}

		public static void HideGroup(CanvasGroup _cg) {
			_cg.interactable = false;
			_cg.alpha = 0f;
			_cg.blocksRaycasts = false;
		}


		//==========================================
		// NOT IMPLEMENTED
		//==========================================
	}
}
