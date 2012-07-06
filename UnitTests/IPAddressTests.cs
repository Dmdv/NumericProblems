using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumericProblems;

namespace UnitTests
{
	/// <summary>
	/// Summary description for IPAddressTests
	/// </summary>
	[TestClass]
	public class IPAddressTests
	{
		// ReSharper disable EmptyGeneralCatchClause

		[TestMethod]
		public void AssumeAreEqual()
		{
			var bytes = IPAddress.Parse("168.1.1.3").GetAddressBytes();
			Assert.AreEqual(IPAddressHelper.BytesToInt32(bytes), BitConverter.ToInt32(bytes, 0));
		}

		[TestMethod]
		public void AssumeIsGreater()
		{
			var bytes1 = IPAddress.Parse("1.1.1.1").GetAddressBytes();
			var bytes2 = IPAddress.Parse("1.1.1.2").GetAddressBytes();
			Assert.IsTrue(IPAddressHelper.BytesToInt32(bytes2) > IPAddressHelper.BytesToInt32(bytes1));
		}

		[TestMethod]
		public void MustThrowExcepton()
		{
			try
			{
				IPAddressHelper.FindRange("1.1.1.2", "1.1.1.1");
				Assert.Fail("FindRange must throw exception");
			}
			catch (Exception)
			{
			}
		}

		[TestMethod]
		public void AssumeCorrectConversion()
		{
			var bytes = new byte[] {21, 205, 91, 7};
			var bytesToInt32 = IPAddressHelper.BytesToInt32(bytes);
			var bytesNew = BitConverter.GetBytes(bytesToInt32);

			for (var i = 0; i < bytes.Length; i++)
			{
				Assert.AreEqual(bytes[i], bytesNew[i]);
			}
		}

		[TestMethod]
		public void AssumeCorrectAddressConversion()
		{
			var address = new IPAddress(new byte[] {21, 205, 91, 7});
			var bytes = address.GetAddressBytes();

			var bytesToInt32 = IPAddressHelper.BytesToInt32(bytes);
			var bytesNew = BitConverter.GetBytes(bytesToInt32);

			for (var i = 0; i < bytes.Length; i++)
			{
				Assert.AreEqual(bytes[i], bytesNew[i]);
			}
		}

		[TestMethod]
		public void AssumeTestOk()
		{
			var ipAddresses = IPAddressHelper.FindRange("2.2.2.1", "2.2.2.10");
			Assert.AreEqual(ipAddresses.Count, 10);
		}
	}
}