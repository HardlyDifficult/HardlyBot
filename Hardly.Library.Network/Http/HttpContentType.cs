using System;

namespace Hardly {
	public class HttpContentType {
		public static readonly HttpContentType UnknownContentType = new HttpContentType("application/octet-stream", new string[] { });
		public static readonly HttpContentType HtmlContentType = new HttpContentType("text/html", new[] { ".htm", ".html" });
		public static readonly HttpContentType CssContentType = new HttpContentType("text/css", new[] { ".css" });
		public static readonly HttpContentType TextContentType = new HttpContentType("text/plain", new[] { ".txt" });
		public static readonly HttpContentType JsContentType = new HttpContentType("text/js", new[] { ".js" });
		public static readonly HttpContentType JpegContentType = new HttpContentType("image/jpeg", new[] { ".jpeg", ".jpg" });
		public static readonly HttpContentType PngContentType = new HttpContentType("image/png", new[] { ".png" });
		public static readonly HttpContentType OggContentType = new HttpContentType("audio/ogg", new[] { ".ogg" });

		static readonly HttpContentType[] AllTypes = new[] { HtmlContentType, CssContentType, TextContentType, JsContentType, JpegContentType, PngContentType, OggContentType };

		public static HttpContentType FromFileName(string path) {
			Debug.Assert(path != null && path.Trim().Length > 0 && path.Trim().Equals(path));

			foreach(HttpContentType type in AllTypes) {
				foreach(string extension in type.fileExtensions) {
					if(path.EndsWith(extension, StringComparison.CurrentCultureIgnoreCase)) {
						return type;
					}
				}
			}

			Log.debug("Unknown content type for file: " + path);
			return UnknownContentType;
		}

		string type;
		string[] fileExtensions;

		public HttpContentType(string type, string[] fileExtensions = null) {
			this.type = type;
			this.fileExtensions = fileExtensions;
		}

		public override string ToString() {
			return type;
		}
	}
}
