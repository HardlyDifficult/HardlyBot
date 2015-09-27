using System;

namespace Hardly {
	public class FileWriter {
		readonly string filename;

		public FileWriter(string filename) {
			if(!filename.IsEmpty() || !filename.IsTrimmed()) {
				this.filename = filename;
				File.Create(filename);
			} else {
				Debug.Fail();
				throw new ArgumentNullException();
			}
		}

		public void WriteLine(string message) {
			if(message != null) {
				lock (this) {
					File.WriteLine(filename, DateTime.Now.ToString() + " " + message + "\r\n");
				}
			} else {
				Debug.Fail();
			}
		}
	}
}
