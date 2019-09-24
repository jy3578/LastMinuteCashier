using System;
using UnityEngine;
using System.Collections;

public class WebClient : MonoBehaviour
{
	private		Web						mWeb = null;
	
	private		Action<WebResult>		OnCompleteRequest = null;
	
	IEnumerator Start()
	{
		if(mWeb != null)
		{
			yield return StartCoroutine(mWeb.Request(this));
			if(OnCompleteRequest != null) OnCompleteRequest(mWeb.Result);
		}
		GameObject.Destroy(gameObject);
	}
	
	public static void Request(Web web, Action<WebResult> OnComplete = null)
	{
		GameObject go = new GameObject("Web Request");
		WebClient client = go.AddComponent<WebClient>();
		client.SetData(web, OnComplete);
	}
	
	public static void Request(WebHost host, WebLocation location, IDictionary data, Action<WebResult> OnComplete = null, int retry = 0, float retryDelay = 0f)
	{
		Web web = new Web(host, location, data, retry, retryDelay);
		Request(web, OnComplete);
	}
	
	public static void Request(HostSetting hostSetting, WebLocation location, IDictionary data, Action<WebResult> OnComplete = null, int retry = 0, float retryDelay = 0f)
	{
		Request(hostSetting.host, location, data, OnComplete, retry, retryDelay);
	}
	
	public static void Request(string url, WebLocation location, IDictionary data, Action<WebResult> OnComplete = null, int retry = 0, float retryDelay = 0f)
	{
		Web web = new Web(url, location, data, retry, retryDelay);
		Request(web, OnComplete);
	}
	
	public void SetData(Web web, Action<WebResult> OnComplete = null)
	{
		this.mWeb = web;
		this.OnCompleteRequest = OnComplete;
	}
}
