using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class NativeIOS : MonoBehaviour
{
#if UNITY_IPHONE
	[DllImport("__Internal")]
	private static extern void _ShowRateDialog( string titleText, string messageText, string yesText, string laterText, string noText );

	[DllImport("__Internal")]
	private static extern string _GetPackageName();

	[DllImport("__Internal")]
	private static extern string _GetAppVersion();

	[DllImport("__Internal")]
	private static extern string _GetBundleVersion();

	public static void ShowRateDialog(string titleText, string messageText, string yesText, string laterText, string noText)
	{
		if( Application.platform != RuntimePlatform.IPhonePlayer )
			return;
			
		_ShowRateDialog(titleText, messageText, yesText, laterText, noText);
	}

	public static string GetPackageName()
	{
		if( Application.platform != RuntimePlatform.IPhonePlayer )
			return null;

		return _GetPackageName();
	}

	public static string GetAppVersion()
	{
		if( Application.platform != RuntimePlatform.IPhonePlayer )
			return null;

		return _GetAppVersion();
	}

	public static string GetBundleVersion()
	{
		if( Application.platform != RuntimePlatform.IPhonePlayer )
			return null;

		return _GetBundleVersion();
	}
#endif
}
