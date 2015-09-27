using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hardly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Tests {
	[TestClass()]
	public class LogTests {
		static string testMessage = "Test";
		static string expectedMessage = testMessage + testMessage + "\r\n\tException: " + testMessage;
#if DEBUG
		static string expectedDebugMessage = expectedMessage;
#else
		static string expectedDebugMessage = "";
#endif
		static string expectedErrorMessage = testMessage + testMessage + "\r\n\tException: " + testMessage + "Unexpected Exception\r\n\tException: " + testMessage;
		static string expectedAllMessage = expectedDebugMessage + expectedErrorMessage + expectedMessage + expectedMessage;
      static Exception testException = new Exception(testMessage);

		string listenTestMessages = "";

		public void listenTest(string message) {
			listenTestMessages += message;
		}

		[TestMethod()]
		public void LogObserverThreadSafeTest() {
			ConfirmNotListening();

			Log.RegisterListener(listenTest, Log.MessageType.ALL);

			int loopCount = 3;
			Thread[] threads = new Thread[5];
			string message = "123.";

			for(int i = 0; i < threads.Length; i++) {
				threads[i] = new Thread(() => {
					int p = 0;
					p++;

					for(int j = 0; j < loopCount; j++) {
						Log.info(message);
					}
				});
			}

			threads.Run();
			string repeatedMessage = "";

			for(int i = 0; i < loopCount * threads.Length; i++) {
				repeatedMessage += message;
			}

			Assert.AreEqual(repeatedMessage, listenTestMessages);

			listenTestMessages = "";
         Log.DeregisterListener(listenTest);
			ConfirmNotListening();
		}

		[TestMethod()]
		public void LogObserverTest() {
			ConfirmNotListening();

			Log.RegisterListener(listenTest, Log.MessageType.INFO);
			WriteStuff();
			Assert.AreEqual(expectedMessage, listenTestMessages);
			listenTestMessages = "";
			Log.DeregisterListener(listenTest);

			ConfirmNotListening();

			Log.RegisterListener(listenTest, Log.MessageType.DEBUG);
			WriteStuff();
			Assert.AreEqual(expectedDebugMessage, listenTestMessages);
			listenTestMessages = "";
			Log.DeregisterListener(listenTest);

			ConfirmNotListening();

			Log.RegisterListener(listenTest, Log.MessageType.INFO);
			Log.RegisterListener(listenTest, Log.MessageType.DEBUG);
			WriteStuff();
			Assert.AreEqual(expectedMessage + expectedMessage, listenTestMessages);
			listenTestMessages = "";
			Log.DeregisterListener(listenTest);

			ConfirmNotListening();

			Log.RegisterListener(listenTest, Log.MessageType.ERROR);
			WriteStuff();
			Assert.AreEqual(expectedErrorMessage, listenTestMessages);
			listenTestMessages = "";
			Log.DeregisterListener(listenTest);

			ConfirmNotListening();

			Log.RegisterListener(listenTest, Log.MessageType.FATAL);
			WriteStuff();
			Assert.AreEqual(expectedMessage, listenTestMessages);
			listenTestMessages = "";
			Log.DeregisterListener(listenTest);

			ConfirmNotListening();

			Log.RegisterListener(listenTest, Log.MessageType.ALL);
			WriteStuff();
			Assert.AreEqual(expectedAllMessage, listenTestMessages);
			listenTestMessages = "";
			Log.DeregisterListener(listenTest);

			ConfirmNotListening();
		}

		private void ConfirmNotListening() {
			WriteStuff();
			Assert.AreEqual("", listenTestMessages);
		}

		private static void WriteStuff() {
			Log.debug(testMessage);
			Log.debug(testMessage, testException);
			Log.error(testMessage);
			Log.error(testMessage, testException);
			Log.exception(testException);
			Log.fatal(testMessage);
			Log.fatal(testMessage, testException);
			Log.info(testMessage);
			Log.info(testMessage, testException);
		}
	}
}