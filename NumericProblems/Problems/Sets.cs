using System.Collections.Generic;

namespace NumericProblems.Problems
{
	public static class Sets
	{
		private static readonly Dictionary<int, bool> _memo;
		private static readonly Dictionary<int, KeyValuePair<int, int>> _prev;

		static Sets()
		{
			_memo = new Dictionary<int, bool>();
			_prev = new Dictionary<int, KeyValuePair<int, int>>();
		}

		/// <summary>
		/// 	Generates subsets with indexes from original set
		/// </summary>
		public static int[][] GenerateSubsetIndexes<T>(T[] list)
		{
			var subsetsNum = 1 << list.Length; // number of subsets (2^n)
			var array = new int[subsetsNum][]; //array with subsets as elements

			for (var idx = 0; idx < array.Length; idx++)
			{
				array[idx] = new int[list.Length];
			}

			for (var i = 0; i < subsetsNum; i++) // filling "result"
			{
				for (var j = 0; j < list.Length; j++)
				{
					var t = 1 << j;
					if ((i & t) != 0)
					{
						array[i][j] = 1;
					}
				}
			}

			return array;
		}

		public static T[][] GenerateSubset<T>(T[] list)
		{
			var subsetsNum = 1 << list.Length; // number of subsets (2^n)
			var array = new T[subsetsNum][]; //array with subsets as elements

			for (var idx = 0; idx < array.Length; idx++)
			{
				array[idx] = new T[list.Length];
			}

			for (var i = 0; i < subsetsNum; i++) // filling "result"
			{
				for (var j = 0; j < list.Length; j++)
				{
					var t = 1 << j;
					if ((i & t) != 0)
					{
						array[i][j] = list[j];
					}
				}
			}

			return array;
		}

		public static List<T[]> CreateSubsets<T>(T[] originalArray)
		{
			var subsets = new List<T[]>();

			foreach (var item in originalArray)
			{
				var subsetCount = subsets.Count;
				subsets.Add(new[] {item});

				for (var j = 0; j < subsetCount; j++)
				{
					var newSubset = new T[subsets[j].Length + 1];
					subsets[j].CopyTo(newSubset, 0);
					newSubset[newSubset.Length - 1] = item;
					subsets.Add(newSubset);
				}
			}

			return subsets;
		}

		/// <summary>
		/// 	Dynamic programming approach to check if a set contains a subset sum equal to target number.
		/// </summary>
		/// <param name="inputArray"> Input array. </param>
		/// <param name="sum"> Target sum. </param>
		/// <returns> </returns>
		public static bool Find(int[] inputArray, int sum)
		{
			_memo.Clear();
			_prev.Clear();

			_memo[0] = true;
			_prev[0] = new KeyValuePair<int, int>(-1, 0);

			for (var i = 0; i < inputArray.Length; ++i)
			{
				var num = inputArray[i];
				for (var s = sum; s >= num; --s)
				{
					if (_memo.ContainsKey(s - num) && _memo[s - num])
					{
						_memo[s] = true;

						if (!_prev.ContainsKey(s))
						{
							_prev[s] = new KeyValuePair<int, int>(i, num);
						}
					}
				}
			}

			return _memo.ContainsKey(sum) && _memo[sum];
		}

		public static IEnumerable<int> GetLastResult(int sum)
		{
			while (_prev[sum].Key != -1)
			{
				yield return _prev[sum].Key;
				sum -= _prev[sum].Value;
			}
		}
	}
}