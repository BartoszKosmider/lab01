using System;
using System.IO;
using System.Numerics;
using System.Text;

namespace bbs
{
	class Program
	{
		static void Main(string[] args)
		{
			while (true)
			{
				Console.WriteLine($"1 - generate key, 2 - crypt string, 3 - decrypt string");
				var input = Console.ReadLine();
				switch (input)
				{
					case "1":
					{
						GenerateKey();
						break;
					}
					case "2":
					{
						EncryptFile();
						break;
					}
					case "3":
					{
						DecryptFile();
						break;
					}
					default:
					{
						Console.WriteLine("incorrect input");
						break;

					}
				}
			}
		}
		private static void EncryptFile()
		{
			var key = File.ReadAllText(keyFileName);
			Console.WriteLine("type value to encrypt");
			var valueToCrypt = Console.ReadLine();
			Console.WriteLine(valueToCrypt);
			byte[] bytes = Encoding.ASCII.GetBytes(valueToCrypt);
			string converted = null;
			for (int i = 0; i < bytes.Length; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					converted += (bytes[i] & 0x80) > 0 ? "1" : "0";
					bytes[i] <<= 1;
				}
			}
			if (!string.IsNullOrEmpty(converted))
			{
				EncryptAndSave(converted, key);
			}
		}

		private static void DecryptFile()
		{ 
			var key = File.ReadAllText(keyFileName); 
			var toDecrypt = File.ReadAllText(cryptedValueFileName);
			var xorValue = "";
			var result = "";

			if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(toDecrypt))
				return;

			for (int i = 0; i < toDecrypt.Length; i++)
			{
				if (toDecrypt[i] == '0' && key[i] == '0' || (toDecrypt[i] == '1' && key[i] == '1'))
					xorValue += "0";
				else
					xorValue += "1";
			}
			//Console.WriteLine($"xor: {xorValue}, length: {xorValue.Length} ");

			for (int i = 0; i < xorValue.Length; i += 8)
			{
				var oneSign = xorValue.Substring(i, 8);
				var intValue = Convert.ToInt32(oneSign, 2);
				result += (char)intValue;
			}
			Console.WriteLine($"decrypted value: {result}");
			File.Delete(decryptedValueFileName);
			File.AppendAllText(decryptedValueFileName, result);
		}

		private static void EncryptAndSave(string toEncrypt, string key)
		{
			File.Delete(cryptedValueFileName);
			var result = "";
			for (int i = 0; i < toEncrypt.Length; i++)
			{
				if (toEncrypt[i] == '0' && key[i] == '0' || (toEncrypt[i] == '1' && key[i] == '1'))
					result += "0";
				else
					result += "1";
			}
			File.AppendAllText(cryptedValueFileName, result);
			//Console.WriteLine($"result {result}");
		}
		
		private static void GenerateKey()
		{
			File.Delete(keyFileName);
			//BigInteger p = 30000000091;
			//BigInteger q = 40000000003;
			BigInteger p = 100002979;
			BigInteger q = 100003243;
			//BigInteger p = 100000004483;
			//BigInteger q = 100000004987;
			var M = p * q;
			BigInteger N = 20000;
			//BigInteger seed = 4882516701;
			BigInteger seed = new Random().Next(0, Int32.MaxValue);
			Console.WriteLine($"input values: \n p: {p}, q: {q}, M: {M}, N: {N}, seed: {seed}");
			var count0 = 0;
			var count1 = 0;

			for (int i = 0; i < N; i++)
			{
				seed = seed * seed % M;
				var bit = seed % 2;
				if (bit == 0)
					count0++;
				else
					count1++;

				File.AppendAllText(keyFileName, bit.ToString());
			}
			Console.WriteLine($"\nnumber of 0: {count0} \nnumber of 1: {count1}");
		}
		private static readonly string keyFileName = "./../output.txt";
		private static readonly string cryptedValueFileName = "./../encrypted.txt";
		private static readonly string decryptedValueFileName = "./../decrypted.txt";

	}
}
