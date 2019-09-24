using System;
using UnityEngine;
using System.Collections;

public class DeviceEvent : PrivateSingletonBehaviour<DeviceEvent>
{
	public static Action OnKeyEscape;

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	void Update()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			if(Input.GetKeyDown(KeyCode.Escape) && OnKeyEscape != null) OnKeyEscape();
		}
	}
}