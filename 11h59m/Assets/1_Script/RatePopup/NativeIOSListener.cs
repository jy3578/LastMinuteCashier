using System;
using UnityEngine;
using System.Collections;

public class NativeIOSListener : MonoBehaviour
{
	public static event Action<RateResult> RateDialogResultDelegate = null;

	static NativeIOSListener()
	{
		new GameObject(typeof(NativeIOSListener).Name, typeof(NativeIOSListener));
	}

	public void OnRateDialogResult( string resultStr )
	{
		if( RateDialogResultDelegate != null )
			RateDialogResultDelegate( (RateResult)Enum.Parse(typeof(RateResult), resultStr) );
	}
}