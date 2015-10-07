using System;
using System.IO;
using System.Net;
using System.Net.Cache;

namespace Hardly {
	public class WebClient {
		public static string GetHTML(string url) {
			if(url != null && url.Trim().Length > 0) {

				Log.info("WebClient " + url);

				try {
					WebRequest request = HttpWebRequest.Create(url);
					request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
					using(WebResponse response = request.GetResponse()) {
						if(response != null) {
							StreamReader reader = new StreamReader(response.GetResponseStream());
							string html = reader.ReadToEnd();
							reader.Close();
							if(html.Length > 0) {
								return html;
							}
						}
					}
				} catch(Exception e) {
					Log.error("Web client error", e);
				}
			} else {
				Debug.Fail();
			}

			return null;
		}
	}
}