using System;
using UnityEngine;
using System.Collections;

public class NotificationManager : MonoBehaviour {
	DateTime pivotTime = DateTime.Now;

	void Start () {
		Notification.ScheduleLocalNotification(
				id: 0,
				notiId: 0,
				statusTitle: Localization.Get("pushtitle00"),
				title: Localization.Get("pushtitle00"),
				text: Localization.Get("pushlabel00"),
				icon: "app_icon",
				sound: null,
				whenDate: pivotTime.AddHours(1));
				
		Notification.ScheduleLocalNotification(
				id: 1,
				notiId: 1,
				statusTitle: Localization.Get("pushtitle01"),
				title: Localization.Get("pushtitle01"),
				text: Localization.Get("pushlabel01"),
				icon: "app_icon",
				sound: null,
				whenDate: GetScheduleTime(1, 11, 50));

		Notification.ScheduleLocalNotification(
				id: 2,
				notiId: 2,
				statusTitle: Localization.Get("pushtitle02"),
				title: Localization.Get("pushtitle02"),
				text: Localization.Get("pushlabel02"),
				icon: "app_icon",
				sound: null,
				whenDate: GetScheduleTime(1, 19, 50));
				
		Notification.ScheduleLocalNotification(
				id: 3,
				notiId: 3,
				statusTitle: Localization.Get("pushtitle03"),
				title: Localization.Get("pushtitle03"),
				text: Localization.Get("pushlabel03"),
				icon: "app_icon",
				sound: null,
				whenDate: GetScheduleTime(2, 19, 50));
				
		Notification.ScheduleLocalNotification(
				id: 4,
				notiId: 4,
				statusTitle: Localization.Get("pushtitle04"),
				title: Localization.Get("pushtitle04"),
				text: Localization.Get("pushlabel04"),
				icon: "app_icon",
				sound: null,
				whenDate: GetScheduleTime(3, 19, 50));
				
		Notification.ScheduleLocalNotification(
				id: 5,
				notiId: 5,
				statusTitle: Localization.Get("pushtitle05"),
				title: Localization.Get("pushtitle05"),
				text: Localization.Get("pushlabel05"),
				icon: "app_icon",
				sound: null,
				whenDate: GetScheduleTime(4, 19, 50));
	}
	
	// pivot hour : 00 ~ 23
	DateTime GetScheduleTime(int diffDay, int pivotHour, int pivotMinute)
	{
		DateTime tempTime = pivotTime.AddDays(diffDay);
			
		int diffMinute = pivotMinute - tempTime.Minute;
		tempTime = tempTime.AddMinutes(diffMinute);

		int diffHour = pivotHour - tempTime.Hour;
		tempTime = tempTime.AddHours(diffHour);

		return tempTime;
	}
}
