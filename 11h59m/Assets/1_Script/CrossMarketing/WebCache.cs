using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WebCache
{
	private		ICache			mCache = null;

	public WebCache( ICache cache )
	{
		mCache = cache;
	}

	public void LoadTexture(MonoBehaviour behaviour, string url, int retryCount, float retryDelay, Action<Texture2D> OnComplete)
	{
		Texture2D texture = mCache.GetTexture( url );

		if( texture != null )
		{
			OnComplete( texture );
		}
		else
		{
			behaviour.StartCoroutine( LoadTextureFromWeb(behaviour, url, retryCount, retryDelay, OnComplete) );
		}
	}

	IEnumerator LoadTextureFromWeb(MonoBehaviour behaviour, string url, int retryCount, float retryDelay, Action<Texture2D> OnComplete)
	{
		Web web = new Web(url, retryCount, retryDelay);

		yield return behaviour.StartCoroutine( web.Request(behaviour) );

		if( web.Result.error == null && web.Result.texture != null )
		{
			mCache.Put(url, web.Result.texture);
			OnComplete( web.Result.texture );
		}
		else
		{
			OnComplete( null );
		}
	}
	
	public void LoadText(MonoBehaviour behaviour, string url, int retryCount, float retryDelay, Action<string> OnComplete)
	{
		string text = mCache.GetText( url );

		if( text != null )
		{
			OnComplete( text );
		}
		else
		{
			behaviour.StartCoroutine( LoadTextFromWeb(behaviour, url, retryCount, retryDelay, OnComplete) );
		}
	}

	IEnumerator LoadTextFromWeb(MonoBehaviour behaviour, string url, int retryCount, float retryDelay, Action<string> OnComplete)
	{
		Web web = new Web(url, retryCount, retryDelay);

		yield return behaviour.StartCoroutine( web.Request(behaviour) );

		if( web.Result.error == null && web.Result.text != null )
		{
			mCache.Put(url, web.Result.text );
			OnComplete( web.Result.text );
		}
		else
		{
			OnComplete( null );
		}
	}
}
