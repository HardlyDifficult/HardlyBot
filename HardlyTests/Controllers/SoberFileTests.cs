using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hardly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly.Tests {
	[TestClass()]
	public class SoberFileWriterTests {
		[TestMethod()]
		public void FileTest() {
			string filename = "testfile_donotuse.txt";
			File.Delete(filename);
			Assert.IsFalse(File.Exists(filename));
         FileWriter writer = new FileWriter(filename);
			string message = "This is a test.";
			writer.WriteLine(message);
			writer.WriteLine(message);

			string fileContents = File.ReadAllLines(filename);

			Assert.IsTrue(fileContents.EndsWith(message));
		}
	}
}