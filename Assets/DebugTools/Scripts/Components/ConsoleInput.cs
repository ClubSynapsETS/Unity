using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace DebugTools {
	public class ConsoleInput : MonoBehaviour {

		private string[] reservedTokens = new string[] {
			"GAMEOBJECT", "FIND",
			"LOAD", "LEVEL"
		};

		InputField consoleInput;

		public ConsoleInput Initialise() {

			RectTransform rt = gameObject.AddComponent<RectTransform>();

			rt.SetParent(Setup.instance.debugConsoleGroup.gameObject.transform);
            rt.anchorMin = new Vector2(0.5f, 0);
            rt.anchorMax = new Vector2(0.5f, 0);
            rt.pivot = new Vector2(0.5f, 0);
            rt.anchoredPosition = new Vector2(0, Setup.settings.generalMargin);
            rt.sizeDelta = new Vector2(Screen.width - (4 * Setup.settings.graphMargin), 30);

			consoleInput = gameObject.AddComponent<InputField>();
			
			rt = new GameObject("Text").AddComponent<RectTransform>();
			rt.SetParent(gameObject.transform);
			Image i = gameObject.AddComponent<Image>();
			i.color = Color.white * 0.5f;
			consoleInput.targetGraphic = i;
			consoleInput.textComponent = rt.gameObject.AddComponent<Text>();
			consoleInput.textComponent.supportRichText = false;
			consoleInput.textComponent.font = Font.CreateDynamicFontFromOSFont("Arial", 14);
			consoleInput.textComponent.color = Color.white;
			consoleInput.textComponent.alignment = TextAnchor.MiddleLeft;

			rt.anchorMin = rt.anchorMax = rt.pivot = (Vector2.one  * 0.5f);
			rt.anchoredPosition = Vector2.zero;
			rt.sizeDelta = new Vector2(Screen.width - (4 * Setup.settings.graphMargin), 30);

			rt = new GameObject("Placeholder").AddComponent<RectTransform>();
			rt.SetParent(gameObject.transform);
			consoleInput.placeholder = rt.gameObject.AddComponent<Text>();
			((Text)consoleInput.placeholder).text = "Enter command";
			((Text)consoleInput.placeholder).font = Font.CreateDynamicFontFromOSFont("Arial", 14);
			((Text)consoleInput.placeholder).color = Color.white;
			((Text)consoleInput.placeholder).alignment = TextAnchor.MiddleLeft;

			rt.anchorMin = rt.anchorMax = rt.pivot = (Vector2.one  * 0.5f);
			rt.anchoredPosition = Vector2.zero;
			rt.sizeDelta = new Vector2(Screen.width - (4 * Setup.settings.graphMargin), 30);

			consoleInput.gameObject.AddComponent<ConsoleInput>();

			consoleInput.Rebuild(CanvasUpdate.MaxUpdateValue);

			//input = GetComponent<InputField>();
			consoleInput.onEndEdit.AddListener(ProcessInput);

			return this;
		}

		void ProcessInput(string str) {

			if (string.IsNullOrEmpty(str)) return;

			consoleInput.text = "";

			if (str.Trim().ToCharArray()[0] == '#') {
				ProcessCommand(str);
			} else {
				Console.Log(LogType.Log, " >  " + str);
			}
		}

		private void ProcessCommand(string command) {

			List<string[]> allCommands = new List<string[]>();

			List<string> tokens = new List<string>();
			tokens.AddRange(command.Split(new char[]{' '}));

			if (tokens.Count < 2 || tokens[0] != "#") return;
			else tokens.RemoveAt(0);	

			for(int i=tokens.Count-1; i>=0; i--) {
				if (Array.Exists(reservedTokens, element => element.ToUpper() == tokens[i].ToUpper())) {
					tokens[i] = tokens[i].ToUpper();
					allCommands.Add(tokens.GetRange(i, tokens.Count-i).ToArray());
					tokens.RemoveRange(i, tokens.Count-i);
				}
			}
		

			foreach (string[] s in allCommands) {
				string sss = "";
				foreach (string ss in s) {
					sss += ss + " ";
				}
				Debug.Log(sss);
			}
		}
	}
}
