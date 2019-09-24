using System;
using UnityEngine;
using System.Collections;

public enum RateResult {
	Yes = 0,
	Later = 1,
	No = 2
}

public class RatePopup : MonoBehaviour
{
	private const string PREF_PREFIX = "RatePopup_";

	private 		int m_count = 0;

	[SerializeField]
	private			int			m_IntervalHours = 0;
	[SerializeField]
	private 		int 		m_IntervalCount = 0;



	public void TryPopup()
	{
		m_count++;

		if( IsNever() ) return;

		DateTime prevPop = GetStoredDatetime();
		DateTime now = DateTime.Now;

		TimeSpan spanTime = now.Subtract( prevPop );
		if( (int) spanTime.TotalHours >= m_IntervalHours && m_count >= m_IntervalCount )
		{
			RegisterLinstener();
			Show();
		}

		GAManager.Instance.GARateEvent("display");
	}

	void Show()
	{
		var local = RateLocalization.Get();
		if( local == null ) return;

#if UNITY_ANDROID
		NativeAndroid.ShowRateDialog(
			local.title,
			local.message,
			local.yes,
			local.later,
			local.no
		);
#elif UNITY_IPHONE
		NativeIOS.ShowRateDialog(
			local.title,
			local.message,
			local.yes,
			local.later,
			local.no
		);
#endif
	}

	bool IsNever()
	{
		return PlayerPrefs.GetInt(PREF_PREFIX + "never", 0) > 0;
	}

	void SetNever()
	{
		PlayerPrefs.SetInt(PREF_PREFIX + "never", 1);
		PlayerPrefs.Save();
	}

	DateTime GetStoredDatetime()
	{
		string tickStr = PlayerPrefs.GetString(PREF_PREFIX + "tick", null);
	
		long tick = 0;
		if(tickStr != ""){
			long.TryParse(tickStr, out tick); 
		}
		else
		{
		
		//	StoreDateTime(now);

		}
		return new DateTime( tick );
	}

	void StoreDateTime(DateTime datetime)
	{
		PlayerPrefs.SetString(PREF_PREFIX + "tick", datetime.Ticks.ToString());
		PlayerPrefs.Save();
	}

	void OnResult( RateResult result )
	{
		UnregisterListener();

		switch( result )
		{
		case RateResult.Yes:
			GAManager.Instance.GARateEvent("yes");
#if UNITY_ANDROID
			Application.OpenURL( "https://play.google.com/store/apps/details?id=com.papayacompany.lastminutecashier" ); //url address.
#elif UNITY_IPHONE
			Application.OpenURL("https://itunes.apple.com/us/app/pixel-store/id924428782");
#endif
			SetNever();
			break;
		case RateResult.Later:
			GAManager.Instance.GARateEvent("later");
			StoreDateTime( DateTime.Now );
			break;
		case RateResult.No:
			GAManager.Instance.GARateEvent("no");
			SetNever();
			break;
		}
	}

	void RegisterLinstener()
	{
#if UNITY_ANDROID
		NativeAndroidListener.RateDialogResultDelegate += OnResult;
#elif UNITY_IPHONE
		NativeIOSListener.RateDialogResultDelegate += OnResult;
#endif
	}

	void UnregisterListener()
	{
#if UNITY_ANDROID
		NativeAndroidListener.RateDialogResultDelegate -= OnResult;
#elif UNITY_IPHONE
		NativeIOSListener.RateDialogResultDelegate -= OnResult;
#endif
	}
}