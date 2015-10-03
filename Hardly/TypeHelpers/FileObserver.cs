using System;
using System.Collections.Concurrent;
using System.Text;
using System.IO;

namespace Hardly {
    public class FileObserver {
        readonly string filename;
        List<Action<string>> observers = new List<Action<string>>();
        string lastLine = null;

        public FileObserver(string filename, bool keepFileOpen) {
            this.filename = filename;

            if(keepFileOpen) {
                new Thread(WatchFile_Blocking).Start();
            } else {
                // This pattern does not work when another process locks the file being observed.
                FileSystemWatcher watcher = new FileSystemWatcher(filename.GetBefore("\\", false), filename.GetAfter("\\", false));
                watcher.Changed += Watcher_Changed;
                watcher.EnableRaisingEvents = true;
            }
        }

        private void WatchFile_Blocking() {
            FileStream fileStream = new FileStream((string)filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader streamReader = new StreamReader(fileStream);
            while(true) {
                string message;
                if((message = streamReader.ReadLine()) != null) {
                    Observe(message);
                } else {
                    if(fileStream.Position > fileStream.Length) {
                        fileStream.Position = 0;
                    }
                    Thread.SleepInSeconds(1);
                }
            }
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e) {
            foreach(var line in NewLines()) {
                Observe(line);
            }
        }

        private void Observe(string line) {
            foreach(var observer in observers) {
                observer(line);
            }
        }

        private List<string> NewLines() {
            string[] lines = File.ReadAllLines(filename).Tokenize("\r\n"); // TODO - this is taking too long to parse
            List<string> newLines = new List<string>();

            bool found = lastLine == null;
            foreach(string line in lines) {
                if(found) {
                    newLines.Add(line);
                } else {
                    if(line.Equals(lastLine)) {
                        found = true;
                    }
                }
            }

            lastLine = newLines.Count > 0 ? newLines.Last : lastLine; // TODO - this is being set to empty string... won't work.

            return newLines;
        }

        public void RegisterObserver(Action<string> observer) {
            observers.Add(observer);
        }
    }
}
