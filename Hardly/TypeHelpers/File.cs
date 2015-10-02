using System;
using System.IO;

namespace Hardly {
	public static class File {
		public static string ReadAllLines(string filename) {
			if(filename != null) {
				try {
					if(Exists(filename)) {
                        using(FileStream fileStream = new FileStream(
                            filename,
                            FileMode.Open,
                            FileAccess.Read,
                            FileShare.ReadWrite)) {
                            using(StreamReader streamReader = new StreamReader(fileStream)) {
                                return streamReader.ReadToEnd();
                            }
                        }
                    }
				} catch(Exception e) {
					Log.error("File, failed to read", e);
				}
			} else {
				Debug.Fail();
			}

			return null;
		}

		public static void WriteLine(string filename, string message) {
			if(filename != null) {
				try {
					if(message != null) {
						System.IO.File.AppendAllText(filename, message);
					}
				} catch(Exception e) {
					Log.error("File, failed to write", e);
				}

				Debug.Assert(Exists(filename));
			} else {
				Debug.Fail();
			}
		}

		public static void Delete(string filename) {
			if(filename != null) {
				try {
					if(Exists(filename)) {
						System.IO.File.Delete(filename);
					}
				} catch(Exception e) {
					Log.error("File, failed to delete", e);
				}

				Debug.Assert(!Exists(filename));
			} else {
				Debug.Fail();
			}
		}

		public static void Create(string filename) {
			if(filename != null) {
				try {
					if(!Exists(filename)) {
						System.IO.File.Create(filename)?.Close();
					}
				} catch(Exception e) {
					Log.error("File, failed to create", e);
				}

				Debug.Assert(Exists(filename));
			} else {
				Debug.Fail();
			}
		}

		public static bool Exists(string filename) {
			if(filename != null) {
				try {
					if(filename != null) {
						return System.IO.File.Exists(filename);
					}
				} catch(Exception e) {
					Log.error("File, failed to check if exists", e);
				}
			} else {
				Debug.Fail();
			}

			return false;
		}

		public static byte[] ReadAllBytes(string filename) {
			if(filename != null) {
				try {
					if(Exists(filename)) {
						return System.IO.File.ReadAllBytes(filename);
					}
				} catch(Exception e) {
					Log.error("File, failed to read", e);
				}
			} else {
				Debug.Fail();
			}

			return null;
		}
	}
}
