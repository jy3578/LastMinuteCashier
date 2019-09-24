using UnityEngine;
using System.Collections;

public class MyConst
{
	private const string GOOGLEPLAY_TYPE = @"googleplay";
	private const string ITUNES_TYPE = @"itunes";
	private const string EDITOR_TYPE = @"editor";

	public static string StoreName {
		get {
			#if UNITY_EDITOR
			return EDITOR_TYPE;
			#elif UNITY_ANDROID
			return GOOGLEPLAY_TYPE;
			#elif UNITY_IPHONE
			return ITUNES_TYPE;
			#else
			return @"";
			#endif
		}
	}

	public static string PackageName {
		get {
			#if UNITY_EDITOR
			return @"com.papayacompany.lastminutecashier";
			#elif UNITY_ANDROID
			return "com.papayacompany.lastminutecashier";
			#elif UNITY_IPHONE
			return "com.papayacompany.lastminutecashier";
			#else
			return null;
			#endif
		}
	}

	public static string VersionName {
		get {
			#if UNITY_EDITOR
			return @"1.0.0";
			#elif UNITY_ANDROID
			return null;
			#elif UNITY_IPHONE
			return null;
			#else
			return null;
			#endif
		}
	}
}