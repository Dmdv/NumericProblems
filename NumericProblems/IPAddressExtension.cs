using System;
using System.Collections.Generic;
using System.Net;

namespace NumericProblems
{
	/// <summary>
	/// IPAddress operations.
	/// </summary>
	public static class IPAddressHelper
	{
		public static List<IPAddress> FindRange(string fromAddress, string toAddress)
		{
			var from = IPAddress.Parse(fromAddress).GetAddressBytes();
			var to = IPAddress.Parse(toAddress).GetAddressBytes();
			Array.Reverse(from);
			Array.Reverse(to);

			var addr1 = BytesToInt32(from);
			var addr2 = BytesToInt32(to);

			if (addr1 > addr2)
			{
				const string Msg = "Lower bound is greater than upper bound: {0} > {1}";
				throw new ArgumentException(string.Format(Msg, fromAddress, toAddress));
			}

			var addresses = new List<IPAddress>();
			for (var i = addr1; i <= addr2; i++)
			{
				var bytes = BitConverter.GetBytes(i);
				addresses.Add(new IPAddress(new[] {bytes[3], bytes[2], bytes[1], bytes[0]}));
			}

			return addresses;
		}

		/// <summary>
		/// Little endian bytes to int32 conversion.
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static int BytesToInt32(byte[] bytes)
		{
			return bytes[0] | bytes[1] << 8 | bytes[2] << 16 | bytes[3] << 24;
		}
	}
}