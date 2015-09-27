using System;
using System.IO;

namespace Hardly {
	public static class StreamHelpers {
		public static byte[] Read(this Stream stream) {
			if(stream != null && stream.CanRead) {
				try {
					byte[] buffer = new byte[32768];
					using(MemoryStream ms = new MemoryStream()) {
						while(true) {
							int read = stream.Read(buffer, 0, buffer.Length);
							if(read <= 0)
								return ms.ToArray();
							ms.Write(buffer, 0, read);
						}
					}
				} catch(Exception e) {
					Log.error("Stream helper failed", e);
				}
			} else {
				Debug.Fail();
			}

			return null;
		}
	}
}
