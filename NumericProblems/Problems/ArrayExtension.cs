namespace NumericProblems.Problems
{
	public static class ArrayExtension
	{
		public static unsafe bool IsEqual(this int[] x, int[] y)
		{
			if (x == null || y == null)
				return false;
			if (x.Length != y.Length)
				return false;

			var len = x.Length;

			fixed (int* p1 = x, p2 = y)
			{
				var left = p1;
				var right = p2;

				while (len > 0)
				{
					if (*left != *right)
						return false;
					left++;
					right++;
					len--;
				}
			}

			return true;
		}

		public static unsafe bool Find(this int[] array, int dupl, int position = -1)
		{
			if (position == -1)
				position = array.Length;

			var idx = 0;

			fixed (int* pointer = array)
			{
				var current = pointer;

				while (idx++ < position)
				{
					if (*current == dupl)
						return true;
					current++;
				}
			}

			return false;
		}

		/// <summary>
		/// Returns sum of an array.
		/// </summary>
		/// <param name="array"></param>
		/// <returns></returns>
		public static unsafe int Sum(this int[] array)
		{
			var sum = 0;

			fixed (int* ptr = array)
			{
				var cur = ptr;

				var idx = 0;
				while (idx++ < array.Length)
				{
					sum += *cur;
					cur++;
				}
			}

			return sum;
		}

	    /// <summary>
		/// Return sum and summed indexes.
		/// </summary>
		/// <returns></returns>
		public static unsafe int Sum(this int[] array, out int[] indexes)
		{
			var sum = 0;
			indexes = new int[array.Length];

			fixed (int* ptr = array)
			{
				var cur = ptr;
				var idx = 0;

				while (idx < array.Length)
				{
					var value = *cur;

					if (value != 0)
						indexes[idx] = 1;

					sum += value;
					cur++;
					idx++;
				}
			}

			return sum;
		}
	}
}