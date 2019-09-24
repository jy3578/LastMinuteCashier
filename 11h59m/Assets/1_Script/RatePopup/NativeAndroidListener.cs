using System;
using UnityEngine;
using System.Collections;

public class NativeAndroidListener : MonoBehaviour
{
	public static event Action<RateResult> RateDialogResultDelegate = null;

	static NativeAndroidListener()
	{
		new GameObject(typeof(NativeAndroidListener).Name, typeof(NativeAndroidListener));
	}

	public void OnRateDialogResult( string resultStr )
	{
		if( RateDialogResultDelegate != null )
			RateDialogResultDelegate( (RateResult)Enum.Parse(typeof(RateResult), resultStr) );
	}
}