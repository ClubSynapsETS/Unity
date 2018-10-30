using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace DebugTools {
    public class ConsoleOutput : MonoDrawer, ILogger {

		private Text consoleOutput;
		private Queue<string> consoleOutputQueue;

        public ILogHandler logHandler {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public bool logEnabled {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public LogType filterLogType {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public ConsoleOutput Initialise() {
			if (Console.output == null) {
				Console.output = this;

                drawerName = "ConsoleOutput";
                RegisterDrawer();

				Console.CreateLog();

				RectTransform rt = gameObject.AddComponent<RectTransform>();
				rt.SetParent(Setup.instance.debugConsoleGroup.gameObject.transform);
                rt.anchorMin = new Vector2(0.5f, 0);
                rt.anchorMax = new Vector2(0.5f, 0);
				rt.pivot = new Vector2(0.5f, 0);
				rt.sizeDelta = new Vector2(Screen.width - (4 * Setup.settings.graphMargin), (Screen.height / 2) - 100);
				rt.anchoredPosition = new Vector2(0, 60);
				ScrollRect sr = gameObject.AddComponent<ScrollRect>();

				GameObject g = new GameObject("Viewport");
				sr.viewport = rt = g.AddComponent<RectTransform>();
				
				rt.SetParent(sr.gameObject.transform);
                rt.anchorMin = new Vector2(0.5f, 0);
                rt.anchorMax = new Vector2(0.5f, 0);
                rt.sizeDelta = new Vector2(Screen.width - (4 * Setup.settings.graphMargin), (Screen.height / 2) - 100);
                rt.pivot = new Vector2(0.5f, 0);
                rt.anchoredPosition = new Vector2(0, 0);
				g.AddComponent<Mask>();
				g.AddComponent<Image>().color = Color.white * 0.5f;

				sr.horizontal = false;
				sr.vertical = true;
				
				g = new GameObject("Log");
				sr.content = rt = g.AddComponent<RectTransform>();

				rt.SetParent(sr.viewport.gameObject.transform);
                rt.anchorMin = new Vector2(0.5f, 0);
                rt.anchorMax = new Vector2(0.5f, 0);
                rt.pivot = new Vector2(0.5f, 0);
                rt.anchoredPosition = new Vector2(0, 0);
				rt.sizeDelta = new Vector2(Screen.width - (4 * Setup.settings.graphMargin), (Screen.height / 2) - 100);

				consoleOutput = g.AddComponent<Text>();
				consoleOutput.font = Font.CreateDynamicFontFromOSFont("Arial", 14);
				consoleOutput.color = Setup.settings.consoleColour;
				consoleOutput.alignment = TextAnchor.LowerLeft;

				consoleOutput.verticalOverflow = VerticalWrapMode.Overflow;
				consoleOutput.horizontalOverflow = HorizontalWrapMode.Wrap;

				ContentSizeFitter csf = g.AddComponent<ContentSizeFitter>();
				csf.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
				csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

				consoleOutputQueue = new Queue<string> ();

				return this;

			} else {
				Destroy (this);
				return null;
			}
		}

		private void Update() {
			UpdateLog();
		}

        private void UpdateLog() {
            while (consoleOutputQueue.Count > 0) {
                string log = consoleOutputQueue.Dequeue();

                consoleOutput.text += log + "\n";
            }
        }

        public bool IsLogTypeAllowed(LogType logType) { 
            switch (logType) {
                case LogType.Log:
                    return true;
                default:
                    return false;
            }
        }


		public void Log(object message, bool toConsole=false, bool toFile=false) {
			if (consoleOutputQueue.Count < Setup.settings.consoleMaxLength) {
				consoleOutputQueue.Enqueue (message.ToString ());

				if (toConsole) {
					Debug.Log (message);
				}

				if (toFile) {
					Console.LogToFile (message.ToString ());
				}

			} else {
				consoleOutputQueue.Dequeue ();
				Log (message, toConsole, toFile);
			}	
		}

        public void Log(object message) {

            if(message.GetType() == typeof(Exception)) {
                LogException(message as Exception);
                return;
            }

            if (consoleOutputQueue.Count < Setup.settings.consoleMaxLength) {
                consoleOutputQueue.Enqueue(message.ToString());
            } else {
                consoleOutputQueue.Dequeue();
                this.Log(message);
            }
        }

        public void Log(LogType logType, object message) {
            switch(logType) {
                case LogType.Assert:
                    Log("<color=purple>" + message.ToString() + "</color>");
                    break;
                case LogType.Exception:
                case LogType.Error:
                    Log("<color=red>" + message.ToString() + "</color>");
                    break;
                case LogType.Warning:
                    Log("<color=yellow>" + message.ToString() + "</color>");
                    break;
                case LogType.Log:
                default:
                    Log(message.ToString());
                    break;
            }
        }

        public void Log(LogType logType, object message, UnityEngine.Object context) {
            Log(logType, context);
            Log(logType, message);
        }

        public void Log(LogType logType, string tag, object message) {
            Log(logType, (object)("<bold>[" + tag + "]</bold> " + message.ToString()));
        }

        public void Log(LogType logType, string tag, object message, UnityEngine.Object context){
            Log(logType, "<bold>[" + tag + "]</bold> " + context.ToString());
            Log(logType, "<bold>[" + tag + "]</bold> " + message.ToString());
        }

        public void Log(string tag, object message) {
            Log((object)("<bold>[" + tag + "]</bold> " + message.ToString()));
        }

        public void Log(string tag, object message, UnityEngine.Object context) {
            Log("<bold>[" + tag + "]</bold> " + context.ToString());
            Log("<bold>[" + tag + "]</bold> " + message.ToString());
        }

        public void LogWarning(string tag, object message) { Log(LogType.Warning, tag, message); }
        public void LogWarning(string tag, object message, UnityEngine.Object context) { Log(LogType.Warning, message, context); }
        public void LogError(string tag, object message) { Log(LogType.Error, tag, message); }
        public void LogError(string tag, object message, UnityEngine.Object context) { Log(LogType.Error, tag, message, context); }

        public void LogFormat(LogType logType, string format, params object[] args){
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(format, args);
            Log(logType, sb.ToString());
        }

        public void LogException(Exception exception) { Log(LogType.Exception, exception); }

        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args){
            Log(logType, context);
            Log(logType, format, args);
        }

        public void LogException(Exception exception, UnityEngine.Object context) {
            Log(context);
            Log(exception);
        }

        public override void DrawVariable(string key, object message)
        {
            Log(message);
        }

        public override void DrawVariable(string key, string format, params object[] args)
        {
            Log(LogType.Log, format, args);
        }
    }

    public static class Console {

		public static ConsoleOutput output;
		private static StreamWriter logFile;
		private static string logPath = "";
		private static string logName = "";

        /// <summary>
        /// Creates the log.
        /// </summary>
        public static void CreateLog() {

            try {
                logName = (Setup.settings.useUniqueLogNames) ? DateTime.Now.ToFileTime().ToString() : "log.txt";
                logPath = Application.persistentDataPath;
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
				logFile = new StreamWriter(logPath + "\\" + logName);
#else
				logFile = new StreamWriter(logPath + "/" + logName);
#endif
            } catch (FileNotFoundException e) {
                Debug.Log(e.Message);
                return;
            }

		}

		public static void LogToFile(string s) {
			if (logFile != null) {
				try {
					logFile.WriteLine (s);
					logFile.Flush ();
				} catch (Exception e) {
					Debug.LogException (e);
				}
			}
		}

		public static void CleanUp() {
			logFile.Flush();
			logFile.Close();
			logFile.Dispose();
		}

        public static bool IsLogTypeAllowed(LogType logType) { return output.IsLogTypeAllowed(logType); }

        public static void Log(LogType logType, object message) { output.Log(logType, message); }

        public static void Log(LogType logType, object message, UnityEngine.Object context) { output.Log(logType, message, context); }

        public static void Log(LogType logType, string tag, object message) { output.Log(logType, tag, message); }

        public static void Log(LogType logType, string tag, object message, UnityEngine.Object context) { output.Log(logType, tag, message, context); }

        static void Log(object message) { if (output != null) output.Log(message); }

		public static void Log(object message, bool toConsole=false, bool toFile=false) { if(output != null) output.Log(message: message, toConsole: toConsole, toFile: toFile); }

        public static void Log(string tag, object message) { output.Log(tag, message); }

        public static void Log(string tag, object message, UnityEngine.Object context) { output.Log(tag, message, context); }

        public static void LogWarning(string tag, object message) { output.LogWarning(tag, message); }

        public static void LogWarning(string tag, object message, UnityEngine.Object context) { output.LogWarning(tag, message, context); }

        public static void LogError(string tag, object message) { output.LogError(tag, message); }

        public static void LogError(string tag, object message, UnityEngine.Object context) { output.LogError(tag, message, context); }

        public static void LogFormat(LogType logType, string format, params object[] args) { output.LogFormat(logType, format, args); }

        public static void LogException(Exception exception) { output.LogException(exception); }

        public static void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args) { output.LogFormat(logType, context, format, args); }

        public static void LogException(Exception exception, UnityEngine.Object context) { output.LogException(exception, context); }
    }
}
