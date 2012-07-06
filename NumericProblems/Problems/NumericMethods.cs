using System;
using System.Collections.Generic;

namespace NumericProblems.Problems
{
	public class NumericMethods
	{
		private readonly int[] _primes = new[]
		{
			2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41,
			43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101,
			103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157,
			163, 167, 173, 179, 181, 191, 193, 197, 199, 211, 223,
			227, 229, 233, 239, 241, 251, 257, 263, 269, 271, 277
		};

	    private int FindTrivialDiv(int num, int[] primes)
		{
			if (num == 2 || num == 3)
				return 1;
			if (num < 2)
				return 0;
			if (num % 2 == 0)
				return 2;

			for (var idx = 0; idx < primes.Length; idx++)
			{
				var div = primes[idx];
				if (div * div > num)
					break;
				if (num % div == 0)
					return div;
			}

			return 1;
		}

		/// <summary>
		/// 	Greatest Common Divisor
		/// </summary>
		public Int64 Gcd(Int64 x, Int64 y)
		{
			if (x == 0)
				return y;
			if (y == 0)
				return x;
			x = x % y;
			return Gcd(y, x);
		}

		public List<int> GetFactors(int num)
		{
			var divs = new List<int>();
			var res = 0;
			var rem = num;

			while (res != 1)
			{
				res = FindTrivialDiv(rem, _primes);
				rem /= res;
				divs.Add(res == 1 ? rem : res);
			}

			return divs;
		}
	}
}