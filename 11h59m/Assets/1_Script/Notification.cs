using System;
using UnityEngine;
using System.Collections;

public class Notification : MonoBehaviour
{
	//Local
	public static void ScheduleLocalNotification(int id, int notiId, string statusTitle, string title, string text, string icon, string sound, DateTime whenDate)
	{
		//if(whenDate.Hour < 8 || whenDate.Hour > 22 || whenDate <= DateTime.Now) return;
		#if UNITY_ANDROID && !UNITY_EDITOR
		LocalNotificationAndroid.ScheduleLocalNotification(id, notiId, statusTitle, title, text, icon, sound, whenDate);

		#elif UNITY_IPHONE && !UNITY_EDITOR
		LocalNotificationIOS.ScheduleLocalNotification(id.ToString(), notiId.ToString(), statusTitle, title, text, icon, sound, whenDate);
		#endif
	}
	
	//Local
	public static void CancelNotification(int id)
	{
		#if UNITY_ANDROID && !UNITY_EDITOR
		LocalNotificationAndroid.CancelLocalNotification(id);

		#elif UNITY_IPHONE && !UNITY_EDITOR
		LocalNotificationIOS.CancelLocalNotification(id.ToString());
		#endif
	}

	public static void ClearNotification()
	{
		#if UNITY_ANDROID && !UNITY_EDITOR
		LocalNotificationAndroid.ClearNotifications();

		#elif UNITY_IPHONE && !UNITY_EDITOR
		LocalNotificationIOS.ClearNotifications();
		#endif
	}
}
