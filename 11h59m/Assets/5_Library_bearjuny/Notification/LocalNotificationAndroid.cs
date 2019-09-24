using System;
using UnityEngine;
using System.Collections;

public class LocalNotificationAndroid
{
#if UNITY_ANDROID
	private static AndroidJavaObject plugin;
	private static AndroidJavaClass pluginClass;

	static LocalNotificationAndroid()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		// find the plugin instance
		pluginClass = new AndroidJavaClass( "com.joonyoung.plugins.Notification" );
	}

	public static void ScheduleLocalNotification(int id, int notiId, string statusTitle, string title, string text, string icon, string sound, DateTime whenDate)
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
				
		var baseTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		long when = (long)(whenDate.ToUniversalTime() - baseTime).TotalMilliseconds;
		//bool plug = plugin.Call<bool>("ScheduleLocalNotification", id, notiId, statusTitle, title, text, icon, sound, when);
		//int groupId, int requestCode, String statusTitle, String title, String message, String smallIconName, String soundName, long when
		pluginClass.CallStatic("ScheduleNotification", id, notiId, statusTitle, title, text, icon, sound, when);
	}

	public static void ClearNotifications()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		pluginClass.CallStatic("ClearNotifications");
	}

	public static void CancelLocalNotification(int id)
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		pluginClass.CallStatic("CancelLocalNotification", id);
	}
#endif
}