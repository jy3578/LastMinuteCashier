using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

public partial class MyTools
{
	public static string Md5(string text)
	{
		StringBuilder MD5Str = new StringBuilder();
		byte[] byteArr = Encoding.ASCII.GetBytes( text );
		byte[] resultArr = (new MD5CryptoServiceProvider()).ComputeHash(byteArr);

		for (int cnti = 0; cnti < resultArr.Length; cnti++)
		{
			MD5Str.Append(resultArr[cnti].ToString("X2"));
		}
		return MD5Str.ToString();
	}
}
