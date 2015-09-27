using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hardly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace Hardly.Tests {
	[TestClass()]
	public class StreamHelpersTests {
		[TestMethod()]
		public void ReadTest() {
			Stream stream = new MemoryStream();
			stream.Write(new byte[] { 0, 1, 2, 3 }, 0, 4);
			stream.Position = 0;
			byte[] result = stream.Read();
         Assert.AreEqual(result[3], 3);
		}
	}
}