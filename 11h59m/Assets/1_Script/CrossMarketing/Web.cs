using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Web
{
	public		WebResult			Result;
	
	private		string				mUrl = null;
	private		IDictionary			mData = null;
	private		int					mRetry = 0;
	private		float				mRetryDelay = 0f;

	private		Func<IDictionary, IEnumerator>	RequestEnumrator = null;

	public Web(WebHost host, WebLocation location, IDictionary data, int retry = 0, float retryDelay = 0f)
		: this(host.ToString(), location, data, retry, retryDelay)
	{
	}

	public Web(HostSetting hostSetting, WebLocation location, IDictionary data, int retry = 0, float retryDelay = 0f)
		: this(hostSetting.host, location, data, retry, retryDelay)
	{
	}

	public Web(string hostUrl, WebLocation location, IDictionary data = null, int retry = 0, float retryDelay = 0f)
		: this(hostUrl, location.location, data, retry, retryDelay)
	{
		if(location.method == WebLocation.Method.Post)
			RequestEnumrator = Post;
	}

	public Web(string hostUrl, string locationUrl, IDictionary data=null, int retry=0, float retryDelay = 0f)
	{
		mUrl = string.Format("{0}/{1}", hostUrl.Trim('/'), locationUrl.Trim('/'));
		mData = data;
		mRetry = retry;
		mRetryDelay = retryDelay;

		RequestEnumrator = Get;
	}

	public Web(string url, int retry = 0, float retryDelay = 0f)
	{
		mUrl = url;
		mData = null;
		mRetry = retry;
		mRetryDelay = retryDelay;

		RequestEnumrator = Get;
	}

	public IEnumerator Request(MonoBehaviour behaviour)
	{
		if( behaviour == null || RequestEnumrator == null ) yield break;

		int retryCount = 0;
		do {
			if(retryCount > 0) yield return new WaitForSeconds(mRetryDelay);

			yield return behaviour.StartCoroutine( RequestEnumrator(mData) );

		} while( Result.error != null && (mRetry > retryCount++ || mRetry == int.MaxValue) );
	}


	IEnumerator Get(IDictionary parameters)
	{
		string getUrl = MakeGetUrl(mUrl, parameters);

		using (WWW www = new WWW(getUrl) )
		{
			yield return www;
			SetResult(www);
		}
	}

	string MakeGetUrl(string url, IDictionary data)
	{
		string getUrl = url;
		
		if(data != null)
		{
			List<string> keyVals = new List<string>();
			foreach(string key in data.Keys)
			{
				object val = data[key];
				keyVals.Add(key + "=" + (val != null ? val.ToString() : ""));
			}
			
			if(keyVals.Count > 0) getUrl += "?" + string.Join("&", keyVals.ToArray());
		}
		
		return getUrl;
	}

	IEnumerator Post(IDictionary parameters)
	{
		WWWForm form = MakePostForm(parameters);
		using( WWW www = new WWW(mUrl, form) )
		{
			yield return www;
			SetResult(www);
		}
	}

	WWWForm MakePostForm(IDictionary data)
	{
		WWWForm form = new WWWForm();
		if(data != null)
		{
			foreach(string key in data.Keys)
			{
				object val = data[key];
				if(val != null)
				{
					if(val is int) form.AddField(key, (int)val);
					else form.AddField(key, val.ToString());
				}
			}
		}
		return form;
	}

	void SetResult(WWW www)
	{
		if(www != null)
		{
			Result.error = www.error;
			if( www.error == null )
			{
				Result.text = www.text;
				Result.texture = www.texture;
				Result.assetBundle = www.assetBundle;
			}
		}
	}
}

public struct WebResult
{
	public string error;
	public string text;
	public Texture2D texture;
	public AssetBundle assetBundle;

	public WebResult(string error, string text, Texture2D texture, AssetBundle assetBundle)
	{
		this.error = error;
		this.text = text;
		this.texture = texture;
		this.assetBundle = assetBundle;
	}
}

[System.Serializable]
public class WebHost
{
	public enum Protocol
	{
		Http,
		Https
	}

	[SerializeField]
	public		Protocol	protocol = Protocol.Http;

	[SerializeField]
	public		string		hostName = null;

	[SerializeField] [Range(0, 65535)]
	public		int			port = 80;

	public override string ToString()
	{
		if(hostName != null)
		{
			return string.Format("{0}://{1}:{2}",
			                     (protocol == Protocol.Http) ? "http" : "https",
			                     hostName.Trim('/'),
			                     port);
		}
		return null;
	}
}

[System.Serializable]
public class WebLocation
{
	public enum Method {
		Get,
		Post
	}

	[SerializeField]
	public		Method			method;

	[SerializeField]
	public		string			location;

	public WebLocation(Method method, string location)
	{
		this.method = method;
		this.location = location;
	}

	public WebLocation(string location) : this(Method.Get, location)
	{
	}
}
