using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MyLibrary;
//using Newtonsoft.Json;

/*
public class AppConfig
{
	[JsonProperty("installed_apps")]
	public List<string> installedApps {get;set;}
	[JsonProperty("config")]
	public string config {get;set;}
}
*/

public class Config : MonoBehaviour
{

	private const string LAUNCH_URL = "http://harpseal-remote-app.appspot.com/json/launch";
	//private const string LAUNCH_URL = "http://127.0.0.1:8080/json/launch";
	private const float REQUEST_DELAY = 10f;

	private static List<string> _installedApps = null;
	private static Dictionary<string, string> _config = null;

	private static bool _sendLaunch = false;
	private static bool _busy = false;

	public static List<string> InstalledApps {
		get {
			return _installedApps;
		}
	}

	void Awake()
	{
		GameObject.DontDestroyOnLoad(gameObject);
	}

	void OnEnable()
	{
		_sendLaunch = true;
	}

	void Update()
	{
		if( _sendLaunch && !_busy )
		{
			_busy = true;
			_sendLaunch = false;

			SendLaunch();
		}
	}

	void OnApplicationPause(bool pause)
	{
		if( pause ) _sendLaunch = true;
	}

	void SendLaunch()
	{
		string url = LAUNCH_URL + "?type=" + MyConst.StoreName +
			"&udid=" + SystemInfo.deviceUniqueIdentifier +
			"&package=" + MyConst.PackageName + "&version=" + MyConst.VersionName;

		Web web = new Web(url, int.MaxValue, REQUEST_DELAY);
		WebClient.Request(web, OnLaunchResponse);
	}

	void OnLaunchResponse(WebResult result)
	{
		try {
			if( result.error != null ) throw new System.Exception("network error");

			string text = result.text;
			Dictionary<string, object> dic = Json.Deserialize(text) as Dictionary<string, object>;

			_installedApps = dic["installed_apps"] as List<string>;
			BuildConfig( dic["config"] as string );

			//AppConfig appConfig = JsonConvert.DeserializeObject(text, typeof(AppConfig)) as AppConfig;
			//Build(appConfig);
		} catch {
			Invoke("SetRequest", REQUEST_DELAY);
		}
	}

	void SetRequest()
	{
		_sendLaunch = true;
		_busy = false;
	}

	/*
	void Build(AppConfig appConfig)
	{
		_installedApps = appConfig.installedApps;
		BuildConfig( appConfig.config );
	}
	*/

	void BuildConfig(string configText)
	{
		if( configText == null ) return;

		if( _config == null ) _config = new Dictionary<string, string>();
		else _config.Clear();

		string[] lines = configText.Split(new string[] {"\n"}, System.StringSplitOptions.RemoveEmptyEntries);
		if( lines != null && lines.Length > 0 )
		{
			foreach( string line in lines )
			{
				string[] keyVal = line.Split(new string[] {":"}, System.StringSplitOptions.RemoveEmptyEntries);
				if( keyVal != null && keyVal.Length == 2 )
				{
					_config.Add( keyVal[0].Trim(), keyVal[1].Trim() );
				}
			}
		}
	}

	public static int GetInt(string key, int defaultValue)
	{
		try {
			string[] values = Parse( key );
			if( values != null && values.Length > 0 )
			{
				return Random.Range( int.Parse(values[0].Trim()), int.Parse(values[1%values.Length].Trim()) );
			}
			return defaultValue;
		} catch {
			return defaultValue;
		}
	}

	public static string[] Parse(string key)
	{
		if( _config != null && _config.ContainsKey(key) )
		{
			string value = _config[key];
			return value.Split("~"[0]);
		}
		return null;
	}

	public static string GetString(string key, string defaultValue)
	{
		string value = defaultValue;
		if( _config != null && _config.ContainsKey(key) )
		{
			value = _config[key];
		}
		return value;
	}

	public static float GetFloat(string key, float defaultValue)
	{
		try {
			string[] values = Parse( key );
			if( values != null && values.Length > 0 )
			{
				return Random.Range( float.Parse(values[0]), float.Parse(values[1%values.Length]) );
			}
			return defaultValue;
		} catch {
			return defaultValue;
		}
	}

	public static bool GetBool(string key, bool defaultValue)
	{
		bool value = defaultValue;
		if( _config != null && _config.ContainsKey(key) )
		{
			bool.TryParse(_config[key], out value);
		}
		return value;
	}

	private static bool IsRange(string value)
	{
		return value.Contains("~");
	}

	private static string[] SpliteRange(string value)
	{
		return value.Split("~"[0]);
	}
}
