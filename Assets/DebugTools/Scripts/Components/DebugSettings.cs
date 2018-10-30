using System;
using System.IO;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace DebugTools {

	[Serializable]
	public class DebugSettings : ScriptableObject {

		public static readonly char[] allReserveSymbols = {'@', '^'};
		public static readonly char[] allCommandSymbols = {'*', '#'};

		//General Settings
		public string profileName = "Default";
		public uint generalMargin = 10;
		public bool stripFromRelease = false;

		//Console Output Settings
		public uint consoleMaxLength = 20;
		public Color consoleColour = Color.green;

		//Console Input Settings
		public char commandChar = '*';
		public char reservedChar = '@';

		//Tracker Settings
		public Color trackerColour = Color.green;

		//Grapher Settings
		public Color graphMinColour = Color.red;
		public Color graphMaxColour = Color.green;
		public uint graphMargin = 10;
		public uint graphLineSize = 2;
		public uint graphUpdateFrequency = 25;

		//Logger Settings
		public bool useUniqueLogNames = false;
		public bool postLogFiles = false;

		//Key Bind Settings
		public KeyCode debugMenuKey = KeyCode.Minus;
		public KeyCode consoleKey = KeyCode.Comma;
		public KeyCode trackerKey = KeyCode.Period;
		public KeyCode grapherKey = KeyCode.Slash;

		public DebugSettings() {
		}

		public static DebugSettings FromXML(string _xml) {
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(DebugSettings));
			return (DebugSettings)xmlSerializer.Deserialize(new StringReader(_xml));
		}
			
		public string ToXML() {
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(DebugSettings));

			using (StringWriter textWriter = new StringWriter()) {
				xmlSerializer.Serialize(textWriter, this);
				return textWriter.ToString();
			}
		}

		public void ToScriptableObject() {
			DebugSettings ds = DebugSettings.FromXML(this.ToXML());

        	string filename = "Profile_" + ds.profileName;

			if (!AssetDatabase.IsValidFolder("Assets/DebugTools/Profiles"))
				AssetDatabase.CreateFolder("Assets/DebugTools", "Profiles");

        	AssetDatabase.CreateAsset(ds, "Assets/DebugTools/Profiles/" + filename + ".asset");
        	AssetDatabase.SaveAssets();
        	AssetDatabase.Refresh();
        	EditorGUIUtility.PingObject(ds);
		}
	}
}
