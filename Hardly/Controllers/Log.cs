using System;
using System.Collections.Generic;

namespace Hardly {
	public static class Log {
		public enum MessageType {
			ALL, DEBUG, INFO, ERROR, FATAL
		}

		#region Observer Pattern
#if DEBUG
		static LinkedList<Action<string>> debugListeners = new LinkedList<
			 Action<string>>(new Action<string>[]{
					 new FileWriter("log_debug.txt").WriteLine, Console.Out.WriteLine });
#endif
		static LinkedList<Action<string>> infoListeners = new LinkedList<
			 Action<string>>(new Action<string>[]{
					 new FileWriter("log_info.txt").WriteLine 
#if DEBUG
			 , Console.Out.WriteLine
#endif
			 });
		static LinkedList<Action<string>> errorListeners = new LinkedList<
			 Action<string>>(new Action<string>[]{
					 new FileWriter("log_error.txt").WriteLine
#if DEBUG
			 , Console.Out.WriteLine
#endif
			 });
		static LinkedList<Action<string>> fatalListeners = new LinkedList<
			 Action<string>>(new Action<string>[]{
					 new FileWriter("log_fatal.txt").WriteLine
#if DEBUG
			 , Console.Out.WriteLine
#endif
			 });

		public static void RegisterListener(Action<string> listener, MessageType type) {
			if(listener != null) {
				foreach(LinkedList<Action<string>> listeners in getListeners(type)) {
					lock (listeners) {
						if(!listeners.Contains(listener)) {
							listeners.AddLast(listener);
						}
					}
				}
			} else {
				Debug.Fail();
			}
		}

		public static void DeregisterListener(Action<string> listener) {
			if(listener != null) {

#if DEBUG
				lock (debugListeners) {
					debugListeners.Remove(listener);
				}
#endif
				lock (infoListeners) {
					infoListeners.Remove(listener);
				}
				lock (errorListeners) {
					errorListeners.Remove(listener);
				}
				lock (fatalListeners) {
					fatalListeners.Remove(listener);
				}
			} else {
				Debug.Fail();
			}
		}

		private static void ObserveMessage(MessageType type, string message, Exception e) {
			LinkedList<Action<string>>[] listenersList = getListeners(type);

			if(listenersList != null) {
				string completeMessage =
					 (message == null ? "" : message)
					 + (message.IsDefaultValue() || e.IsDefaultValue() ? "" : "\r\n\t")
					 + (e.IsDefaultValue() ? "" : "Exception: " + e.Message 
						+ (e.StackTrace.IsDefaultValue() ? "" : "\r\n\t" + e.StackTrace));
				if(completeMessage != null) {
					foreach(LinkedList<Action<string>> listeners in listenersList) {
						lock (listeners) {
							foreach(Action<string> listener in listeners) {
								try {
									listener(completeMessage);
								} catch(Exception) {
									Debug.Fail();
								}
							}
						}
					}
				} else {
					Debug.Fail();
				}
			}
		}
		#endregion

		#region Log actions
		[System.Diagnostics.Conditional("DEBUG")]
		public static void debug(string message, Exception e = null) {
			ObserveMessage(MessageType.DEBUG, message, e);
		}

		public static void info(string message, Exception e = null) {
			ObserveMessage(MessageType.INFO, message, e);
		}

		public static void error(string message, Exception e = null) {
			ObserveMessage(MessageType.ERROR, message, e);
		}

		public static void exception(Exception e) {
			error("Unexpected Exception", e);
		}

		public static void fatal(string message, Exception e = null) {
			ObserveMessage(MessageType.FATAL, message, e);
		}
		#endregion

		static LinkedList<Action<string>>[] getListeners(MessageType type) {
			LinkedList<Action<string>>[] listeners = null;

			switch(type) {
			case MessageType.ALL:
				listeners = new LinkedList<Action<string>>[] {
#if DEBUG
					debugListeners,
#endif
					infoListeners, errorListeners, fatalListeners
				};
				break;
#if DEBUG
			case MessageType.DEBUG:
				listeners = new LinkedList<Action<string>>[] { debugListeners };
            break;
#endif
			case MessageType.INFO:
				listeners = new LinkedList<Action<string>>[] { infoListeners };
				break;
			case MessageType.ERROR:
				listeners = new LinkedList<Action<string>>[] { errorListeners };
				break;
			case MessageType.FATAL:
				listeners = new LinkedList<Action<string>>[] { fatalListeners };
				break;
			}

			return listeners;
		}
	}
}
