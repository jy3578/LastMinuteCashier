using UnityEngine;
using System.Collections;

public class NativeAndroid : MonoBehaviour
{
#if UNITY_ANDROID
	private static AndroidJavaObject plugin;

	static NativeAndroid()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		// find the plugin instance
		using( var pluginClass = new AndroidJavaClass( "com.bearjuny.plugins.NativePlugin" ) )
			plugin = pluginClass.CallStatic<AndroidJavaObject>( "instance" );
	}

	public static void ShowRateDialog(string titleText, string messageText, string yesText, string laterText, string noText)
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
			
		plugin.Call("ShowRateDialog", titleText, messageText, yesText, laterText, noText);
	}

	public static string GetPackageName()
	{
		if( Application.platform != RuntimePlatform.Android )
			return null;

		return plugin.Call<string>("GetPackageName");
	}

	public static string GetVersionName()
	{
		if( Application.platform != RuntimePlatform.Android )
			return null;

		return plugin.Call<string>("GetVersionName");
	}

	public static int GetVersionCode()
	{
		if( Application.platform != RuntimePlatform.Android )
			return -1;

		return plugin.Call<int>("GetVersionCode");
	}

#endif
}
