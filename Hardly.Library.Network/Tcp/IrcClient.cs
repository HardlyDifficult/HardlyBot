using System;
using System.Net.Sockets;
using System.IO;

namespace Hardly {
	public class IrcClient : TcpClient {
		object myLock = new object();
		Throttle throttle;
		StreamReader inputStream;
		StreamWriter outputStream;

		public IrcClient(string ip, uint port, string userName, string password)
			 : base(ip, (int)port) {
			throttle = new Throttle(TimeSpan.FromSeconds(2));
			inputStream = new StreamReader(GetStream());
			outputStream = new StreamWriter(GetStream());

			WriteLine("PASS " + password + Environment.NewLine
				 + "NICK " + userName + Environment.NewLine
				 + "USER " + userName + " 8 * :" + userName);
		}

		public void WriteLine(string message) {
			throttle.SleepTillReady(0);

			lock(myLock) {
				outputStream.WriteLine(message);
				outputStream.Flush();
			}
		}

		public string ReadNextLine_BLOCKING() {
			string message = inputStream.ReadLine();
            return message;
		}
	}
}