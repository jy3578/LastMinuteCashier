using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LocalNotificationIOS
{
#if UNITY_IPHONE
	public static bool ScheduleLocalNotification(string id, string notiId, string statusTitle, string title, string text, string icon, string sound, DateTime whenDate)
	{
		CancelLocalNotification(notiId);

		UnityEngine.iOS.LocalNotification noti = new UnityEngine.iOS.LocalNotification();

		noti.alertAction = title;
		noti.alertBody = text;
		noti.fireDate = whenDate;
		noti.soundName = sound;

		noti.userInfo.Add("id", notiId);
		UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(noti);

		return true;
	}
	
	public static void ClearNotifications()
	{
		UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications();
	}
	
	public static void CancelLocalNotification(string id)
	{
		if(UnityEngine.iOS.NotificationServices.localNotifications != null)
		{
			List<UnityEngine.iOS.LocalNotification> removeList = new List<UnityEngine.iOS.LocalNotification>();
			foreach(var noti in UnityEngine.iOS.NotificationServices.scheduledLocalNotifications)
			{
				IDictionary userInfo = noti.userInfo;
				if(userInfo.Contains("id") && id.Equals(userInfo["id"].ToString()))
				{
					removeList.Add(noti);
				}
			}

			foreach(UnityEngine.iOS.LocalNotification removeNoti in removeList)
			{
				UnityEngine.iOS.NotificationServices.CancelLocalNotification(removeNoti);
			}
		}
	}
#endif
}
