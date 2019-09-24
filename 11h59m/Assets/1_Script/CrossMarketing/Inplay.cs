using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MyLibrary;

public class PromotionApp
{
	public string package = null;
	public string name = null;
	public string iconId = null;
	public string logoId = null;
	public string url = null;

	public PromotionApp(string package, string name, string iconId, string logoId, string url)
	{
		this.package = package;
		this.name = name;
		this.iconId = iconId;
		this.logoId = logoId;
		this.url = url;
	}
}

public class CachedInplay
{
	public string package = null;
	public string name = null;
	public string iconUrl = null;
	public string logoUrl = null;
	public Texture2D icon = null;
	public Texture2D logo = null;
	public string url = null;

	public CachedInplay(string package, string name, string iconId, string logoId, string url)
	{
		this.package = package;
		this.name = name;
		this.iconUrl = !string.IsNullOrEmpty(iconId) ? Inplay.IMAGE_URL + "?id=" + iconId : null;
		this.logoUrl = !string.IsNullOrEmpty(logoId) ? Inplay.IMAGE_URL + "?id=" + logoId : null;
		this.url = url;
	}

	public void Show()
	{
		// Nothing
	}

	public void Click()
	{
		Inplay.Click(this);
		Application.OpenURL( url );
	}
}

public class Inplay : MonoBehaviour
{
	public static event Action<bool> OnCompleteCache = null;

	private const string CONFIG_INPLAY_SHOW = @"inplay_show";
	private const string CONFIG_INPLAY_RATIO = @"inplay_ratio";

	//private const string PROMOTION_URL = @"http://127.0.0.1:8080/json/promotion";
	//public const string IMAGE_URL = @"http://127.0.0.1:8080/image";

	private const string PROMOTION_URL = @"http://harpseal-remote-app.appspot.com/json/promotion";
	public const string IMAGE_URL = @"http://harpseal-remote-app.appspot.com/image";

	private const float REQUEST_DELAY = 10f;
	private const string CACHE_KEY = "promotion_cache";

	private static List<string> _clickedPackages = new List<string>();
	private static List<PromotionApp> _promotionApps = null;
	//private static WebCache _webCache = null;
	//private static Cache _cache = new Cache

	private static ICache _cache;

	// 캐싱 되고 있는 상태 플래스
	private static bool _busy = false;

	// 캐싱 되고 있는 오프젝트
	private static CachedInplay _prepareInplay = null;

	// 캐싱이 완료된 오브젝트
	private static CachedInplay _cachedInplay = null;

	void Awake()
	{
		GameObject.DontDestroyOnLoad(gameObject);
	}

	void OnEnable()
	{
		_cache = new Cache(CACHE_KEY);

		// 프로모션 데이터 로드
		RequestPromotion();
	}

	void Update()
	{
		if( _prepareInplay != null && _busy == false)
		{
			_busy = true;

			StartCoroutine( DownloadImages() );
		}
	}

	IEnumerator DownloadImages()
	{
		CachedInplay inplay = _prepareInplay;

		if( inplay == null )
		{
			_busy = false;
			yield break;
		}

		if( !string.IsNullOrEmpty(inplay.iconUrl) )
		{
			Texture2D texture = _cache.GetTexture( inplay.iconUrl );
			if( texture != null )
			{
				inplay.icon = texture;
			}
			else
			{
				Web web = new Web(inplay.iconUrl);
				yield return StartCoroutine( web.Request(this) );
				if( web.Result.texture != null && web.Result.error == null )
				{
					inplay.icon = web.Result.texture;
					_cache.Put(inplay.iconUrl, web.Result.texture);
				}
				else
				{
					_busy = false;
					if( OnCompleteCache != null ) OnCompleteCache( false );

					yield break;
				}
			}
		}

		if( !string.IsNullOrEmpty(inplay.logoUrl) )
		{
			Texture2D texture = _cache.GetTexture( inplay.logoUrl );
			if( texture != null )
			{
				inplay.logo = texture;
			}
			else
			{
				Web web = new Web(inplay.logoUrl);
				yield return StartCoroutine( web.Request(this) );
				if( web.Result.texture != null && web.Result.error == null )
				{
					inplay.logo = web.Result.texture;
					_cache.Put(inplay.logoUrl, web.Result.texture);
				}
				else
				{
					_busy = false;
					if( OnCompleteCache != null ) OnCompleteCache( false );

					yield break;
				}
			}
		}

		if( _prepareInplay == inplay )
		{
			_cachedInplay = inplay;
			_prepareInplay = null;

			_busy = false;

			if( OnCompleteCache != null ) OnCompleteCache( true );
		}
	}

	void RequestPromotion()
	{
		string url = PROMOTION_URL + "?type=" + MyConst.StoreName +
			"&udid=" + SystemInfo.deviceUniqueIdentifier +
			"&package=" + MyConst.PackageName + "&version=" + MyConst.VersionName;

		Web web = new Web(url, int.MaxValue, REQUEST_DELAY);
		WebClient.Request(web, OnPromotionResponse);
	}

	void OnPromotionResponse(WebResult result)
	{
		try {
			if( result.error != null ) throw new System.Exception("network error");

			string text = result.text;
			List<object> jsonList = Json.Deserialize( text ) as List<object>;

			List<PromotionApp> apps = new List<PromotionApp>();
			foreach( object json in jsonList )
			{
				Dictionary<string, object> dic = json as Dictionary<string, object>;

				string package = dic.ContainsKey("package") ? dic["package"] as string : null;
				string name = dic.ContainsKey("name") ? dic["name"] as string : null;
				string icon = dic.ContainsKey("icon") ? dic["icon"] as string : null;
				string logo = dic.ContainsKey("logo") ? dic["logo"] as string : null;
				string url = dic.ContainsKey("url") ? dic["url"] as string : null;

				apps.Add( new PromotionApp(package, name, icon, logo, url) );
			}
			_promotionApps = apps;
		} catch {
			Invoke("RequestPromotion", REQUEST_DELAY);
		}
	}

	// 프로모션할 앱을 선택한다.
	private static PromotionApp SelectPromotionApp()
	{
		// 만약 프로모션 데이터를 받아오지 못하였거나 없다면 null을 리턴한다.
		if( _promotionApps == null || _promotionApps.Count <= 0 ) return null;

		// 이미 인스톨 되어있는 앱을 제외하기 위하여 Config에서 받아온다.
		var installedPackages = Config.InstalledApps;

		List<PromotionApp> filterdApps = new List<PromotionApp>( _promotionApps );
		if( installedPackages != null && installedPackages.Count > 0)
		{
			filterdApps.RemoveAll( (match) => {
				return installedPackages.Contains(match.package) || _clickedPackages.Contains(match.package);
			});
		}

		// 랜덤으로 선택한다.
		if( filterdApps.Count > 0 ) return filterdApps[ UnityEngine.Random.Range(0, filterdApps.Count) ];
		return null;
	}

	public static bool Cache()
	{
		if( !Config.GetBool(CONFIG_INPLAY_SHOW, false) ) return false;
		// 현재 캐싱이 시도되고 있거나 이미 완료된 오브젝트가 없다면
		// 캐싱을 시도한다.
		if( !_busy && _cachedInplay == null )
		{
			// 준비하고있는 Inplay 제거
			_prepareInplay = null;

			PromotionApp promotion = SelectPromotionApp();
			if( promotion != null )
			{
				_prepareInplay = new CachedInplay(promotion.package, promotion.name, promotion.iconId, promotion.logoId, promotion.url);
				return true;
			}
		}
		return false;
	}

	public static CachedInplay Get()
	{
		if( !Config.GetBool(CONFIG_INPLAY_SHOW, false) ) return null;
		if( UnityEngine.Random.Range(0f, 1f) >= Config.GetFloat(CONFIG_INPLAY_RATIO, 1f) ) return null;

		CachedInplay inplay = _cachedInplay;
		_cachedInplay = null;

		return inplay;
	}

	public void Show()
	{
		// Nothing
	}

	public static bool IsCached()
	{
		if( !Config.GetBool(CONFIG_INPLAY_SHOW, false) ) return false;

		return _cachedInplay != null;
	}

	public static void Click(CachedInplay inplay)
	{
		if( inplay != null && inplay.package != null &&
			!_clickedPackages.Contains( inplay.package) )
		{
			_clickedPackages.Add( inplay.package );
		}
	}
}