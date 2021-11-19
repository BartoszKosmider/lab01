using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace bbs
{
	public class BbsTests
	{
		[SetUp]
		public void onStart()
		{
			inputValue = File.ReadAllText(fileName);
			Console.WriteLine(inputValue.Length);

			if (inputValue.Length >= length)
				inputValue = inputValue[..length];
			else
				throw new Exception("too short input string");
		}

		[Test]
		public void testFips140_2()
		{
			var count1 = inputValue.Count(c => c == '1');

			Assert.IsTrue(count1 is > 9725 and < 10275);
			Assert.AreEqual(length, inputValue.Length);
			Console.WriteLine($"1: {count1}, 0: {length - count1}" );
		}

		[Test]
		public void testSerii()
		{
			int[] streak0 = { 0, 0, 0, 0, 0, 0 };
			int[] streak1 = { 0, 0, 0, 0, 0, 0 };
			var count = 1;

			for (int i = 0; i < length-1; i++)
			{
				if (inputValue[i] == inputValue[i + 1] && count < 6)
					count++;
				else if(inputValue[i] == '0')
				{
					streak0[count - 1]++;
					count = 1;
				}
				else if (inputValue[i] == '1')
				{
					streak1[count - 1]++;
					count = 1;
				}
			}
			Console.WriteLine($"{streak0[0]},{streak0[1]},{streak0[2]},{streak0[3]},{streak0[4]},{streak0[5]} ");
			Console.WriteLine($"{streak1[0]},{streak1[1]},{streak1[2]},{streak1[3]},{streak1[4]},{streak1[5]} ");
			Assert.IsTrue(streak0[0] is > 2315 and < 2685);
			Assert.IsTrue(streak1[0] is > 2315 and < 2685);
			Assert.IsTrue(streak0[1] is > 1114 and < 1386);
			Assert.IsTrue(streak1[1] is > 1114 and < 1386);
			Assert.IsTrue(streak0[2] is > 527 and < 723);
			Assert.IsTrue(streak1[2] is > 527 and < 723);
			Assert.IsTrue(streak0[3] is > 240 and < 384);
			Assert.IsTrue(streak1[3] is > 240 and < 384);
			Assert.IsTrue(streak0[4] is > 103 and < 209);
			Assert.IsTrue(streak1[4] is > 103 and < 209);
			Assert.IsTrue(streak0[5] is > 103 and < 209);
			Assert.IsTrue(streak1[5] is > 103 and < 209);
		}

		[Test]
		public void testDlugiejSerii()
		{
			int count = 1;
			int max = 0;
			for (int i = 0; i < length - 1; i++)
			{
				if (inputValue[i] == inputValue[i + 1])
				{
					count++;
					max = count > max ? count : max;
				}
				else
				{
					count = 1;
				}
			}
			Console.WriteLine($"Longest streak: {max}");
			Assert.IsTrue(max < 26);
		}

		[Test]
		public void popopopokertest()
		{
			int[] countConverted = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
			for (int i = 0; i < length; i += 4)
			{
				var segment = inputValue.Substring(i, 4);
				//Console.WriteLine($"{segment}, {i}");
				var convertedValue = Convert.ToInt32(segment, 2);
				countConverted[convertedValue]++;
			}
			Assert.AreEqual(5000, countConverted.Sum());

			for (int i = 0; i < 16; i++)
			{
				//Console.WriteLine($"value: {i}, count: {countConverted[i]}");
				countConverted[i] *= countConverted[i];
			}

			double result = 16.0 / 5000.0 * countConverted.Sum() - 5000;
			Console.WriteLine($"result: {result}");
			Assert.IsTrue(result is > 2.16 and < 46.17);
		}

		private readonly int length = 20000;
		private string inputValue;
		private readonly string fileName = "./../../../binTest/output.txt";
	}
}
