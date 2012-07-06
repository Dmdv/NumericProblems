using System;
using System.Linq;

namespace NumericProblems
{
	public class Gray
	{
		public static int Grayencode(int g)
		{
			return g ^ (g >> 1);
		}

		public static int Graydecode(int gray)
		{
			int bin;
			for (bin = 0; gray > 0; gray >>= 1)
			{
				bin ^= gray;
			}
			return bin;
		}

		public void Test()
		{
			var array = Enumerable.Range(1, 100).Select(Grayencode).ToArray();
			foreach (var num in array)
			{
				Console.WriteLine(num);
			}
		}
	}
}