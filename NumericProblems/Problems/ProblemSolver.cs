using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericProblems.Problems
{
	public sealed class ProblemSolver
	{
		public void Run()
		{
			const int N = 100;
		    var listset = new List<int> {3, 19, 45, 21, 10, 4, 7};

            var sw = new Stopwatch();
            sw.Start();

		    var task1 = Task<List<int[]>>.Factory.StartNew(() => FindMultipliers(N));
            var task2 = Task<Dictionary<int, List<int[]>>>.Factory.StartNew(() => FindSubsetSums(listset.ToArray()));

		    var multipliers = task1.Result;
		    if (multipliers.Count == 0)
			{
				Console.WriteLine("It's a prime");
			    Console.ReadKey();
                return;
			}

            var task3 = Task<List<int[]>>.Factory.StartNew(() => FindSolutionInSums(listset, multipliers, task2.Result));
            var task4 = Task<Dictionary<int, List<int>>>.Factory.StartNew(() => FindSolutionInDiff(listset, multipliers));

            var solution = task3.Result;
            var answer = task4.Result;

		    // Multipliers in the form of sums.
            if (solution.Count > 0)
            {
                var message =
                    solution
                    .Select(mults => mults.Aggregate("(", (x, y) => x + " + " + y, str => str + ")"))
                    .Aggregate(string.Empty, (x, y) => x + " * " + y, str => str);
                Console.WriteLine(message);
                Console.ReadLine();
                return;
            }

            // Multipliers in the form of differences.
		    var multVal = new List<string>();
		    foreach (var pair in answer)
		    {
		        var value = string.Format("({0}", pair.Key.ToString(CultureInfo.InvariantCulture));
		        value = pair.Value.Aggregate(new StringBuilder(value), (current, num) =>
                    current.AppendFormat(" - {0}", num), sb => sb.Append(")").ToString());
                multVal.Add(value);
		    }

		    for (var i = 0; i < multVal.Count; i++)
		    {
		        if (i != 0)
		            Console.Write(" * ");
		        Console.Write(multVal[i]);
    	    }

		    Console.WriteLine();

            sw.Stop();
            Console.WriteLine();
		    Console.WriteLine("Elapsed = {0}", sw.Elapsed);
            Console.WriteLine();

            Console.WriteLine("Press Enter to exit...");
		    Console.ReadLine();
		}

	    private Dictionary<int, List<int>> FindSolutionInDiff(List<int> listset, IEnumerable<int[]> multipliers)
	    {
	        var answer = new Dictionary<int, List<int>>();

	        foreach (int[] equation in multipliers)
	        {
	            var equationGood = true;
	            var hashSet = new HashSet<int>(listset);
	            var result = new Dictionary<int, List<int>>();

	            foreach (int multiplier in equation)
	            {
	                var found = false;

	                // simple multiplier from set
	                if (hashSet.Contains(multiplier))
	                {
	                    result[multiplier] = new List<int>();
	                    hashSet.RemoveWhere(x => x == multiplier);
	                    continue;
	                }

	                hashSet.Add(multiplier);
	                var sets = FindSubsetSums(hashSet.ToArray());

	                foreach (var keyValue in sets)
	                {
	                    // for each possible sum
	                    foreach (int[] sum in keyValue.Value)
	                    {
	                        if (multiplier != keyValue.Key &&
	                            sum.Find(multiplier) &&
	                            hashSet.Contains(keyValue.Key) &&
	                            sum.All(hashSet.Contains))
	                        {
	                            var list = sum.ToList();
	                            list.Add(keyValue.Key);
	                            hashSet.ExceptWith(list);
	                            list.Remove(multiplier);
	                            list.Remove(keyValue.Key);
	                            result[keyValue.Key] = list;
	                            found = true;
	                            break;
	                        }
	                    }

	                    // to next multiplier
	                    if (found) break;
	                }

	                equationGood &= found;

	                // to next equation
	                if (!equationGood)
	                    break;
	            }

	            if (!equationGood) continue;

	            answer = result;
	            break;
	        }
	        return answer;
	    }

	    private static List<int[]> FindSolutionInSums(List<int> listset,
            IEnumerable<int[]> multipliers, Dictionary<int, List<int[]>> subsetSums)
	    {
	        var solution = new List<int[]>();

	        foreach (int[] equation in multipliers)
	        {
	            solution = FindSum(equation, listset.ToArray(), subsetSums);
	            if (solution.Count != 0)
	                break;
	        }
	        return solution;
	    }

	    private static List<int[]> FindSum(int[] multipliers, int[] set, Dictionary<int, List<int[]>> subsetSums)
        {
            var list = new List<int[]>();
            var hashSet = new HashSet<int>(set);

            foreach (var multiplier in multipliers)
            {
                if (hashSet.Contains(multiplier))
                {
                    hashSet.RemoveWhere(x => x == multiplier);
                    list.Add(new[] {multiplier});
                    continue;
                }

                var equat = ComposeMiltiplier(hashSet.ToArray(), subsetSums, multiplier);
                if (equat.Length == 0)
                {
                    list.Clear();
                    return list;
                }

                list.Add(equat);
            }

            return list;
        }

	    private static int[] ComposeMiltiplier(int[] set, IDictionary<int, List<int[]>> subsetSums, int multiplier)
	    {
	        if (subsetSums.ContainsKey(multiplier))
	        {
	            foreach (int[] numbers in subsetSums[multiplier])
	            {
                    // return any available sum for multiplier.
                    if (numbers.All(x => set.Find(x)))
                        return numbers;
	            }
	        }

	        return new int[] {};
	    }

	    private Dictionary<int, List<int[]>> FindSubsetSums(int[] set)
		{
			var subsets = Sets.GenerateSubset(set);
            var hash = new Dictionary<int, List<int[]>>();

			// throw away empty, 1-value or original sets (already checked).
			foreach (var subset in subsets)
			{
				int[] indexes;
				var sum = subset.Sum(out indexes);
				var bits = indexes.Sum();
				if (bits ==0 || bits == subset.Length)
					continue;

				// create a bucket with sum at zero position.
				var bucket = new int[bits];

				// current index in the bucket.
				var kx = 0;

				// fill bucket in 0..N
				for (var idx = 0; idx < indexes.Length; idx++)
				{
					if (indexes[idx] == 1)
						bucket[kx++] = subset[idx];
				}

                if (hash.ContainsKey(sum))
                {
                    hash[sum].Add(bucket);
                }
                else
                {
                    hash[sum] = new List<int[]> {bucket};
                }
			}

            return hash;
		}

		/// <summary>
		/// Find all multipliers of the number.
		/// </summary>
		private List<int[]> FindMultipliers(int number)
		{
			var solver = new NumericMethods();

			var factors = solver.GetFactors(number).ToArray();
			var subsetIndexes = Sets.GenerateSubsetIndexes(factors);

		    // Possible multipliers for N.
			var multipliers = new List<int[]>();

			// Iterate over all subsets.
			for (var idx = 0; idx < subsetIndexes.Length; idx++)
			{
				// remove indexes with just 1 index in array.
				var bits = subsetIndexes[idx].Sum();
				if (bits <= 1) continue;
				if (bits == factors.Length) // if original factorization.
				{
  					// var exit = false;

                    // TODO: This is a dirty hack, but here I have missed 2 * 2 * 5 * 5 combinations.
                    var bucket = new[]{4, 25};
                    multipliers.Add(bucket);
                    continue;

                    // Uncomment below and comment above.

                    //bucket = new int[factors.Length];
                    //for (var i = 0; i < factors.Length; i++)
                    //{
                    //    var factor = factors[i];
                    //    if (bucket.Find(factor, i))
                    //    {
                    //        exit = true;
                    //        break;
                    //    }
                    //    bucket[i] = factor;
                    //}

                    //if (!exit)
                    //    multipliers.Add(bucket);

                    //continue;
				}

				// find array dimension.
				var dim = factors.Length - bits + 1;

				// create multipliers bucket.
				var currentMult = new int[dim];

				// fill 0 multiplier in the last added pair
				currentMult[0] = GetProduct(idx, subsetIndexes, factors);

				// index in current multiplier bucket.
				var kx = 1;

				var success = true;

				// fill other multipliers and check for duplicates.
				for (var jx = 0; jx < factors.Length; jx++)
				{
					if (subsetIndexes[idx][jx] != 1)
					{
						var factor = factors[jx];
						if (currentMult.Find(factor, kx))
						{
							success = false;
							break;
						}
						currentMult[kx++] = factor;
					}
				}

				if (!success)
					continue;

				// Sort bucket.
				Array.Sort(currentMult);

				// if it doesn't exist, add new one.
				if (!multipliers.Any(currentMult.IsEqual))
				{
					multipliers.Add(currentMult);
				}
			}

			return multipliers;
		}

		private static unsafe int GetProduct(int idx, int[][] subsetIndexes, int[] factors)
		{
			// target product.
			var product = 1;

			fixed (int* indexSubset = subsetIndexes[idx], factor = factors)
			{
				var jx = 0;

				// find multiplier.
				while (jx < factors.Length)
				{
					if (indexSubset[jx] == 1)
					{
						product *= factor[jx];
					}

					jx++;
				}
			}

			return product;
		}
	}
}