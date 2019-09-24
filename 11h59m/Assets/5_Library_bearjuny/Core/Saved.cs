using UnityEngine;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;

public partial class Saved
{
	protected static	Dictionary<string, object>		mCache = new Dictionary<string, object>();

	public static string GetString(SaveKey key)
	{
		if(mCache.ContainsKey(key.ToString()))
		{
			return (string)mCache[key.ToString()];
		}
		else
		{
			string value = ObscuredPrefs.GetString(key.ToString(), (string)Default(key));
			mCache[key.ToString()] = value;
			return value;
		}
	}

	public static void SetString(SaveKey key, string value)
	{
		mCache[key.ToString()] = value;
		ObscuredPrefs.SetString(key.ToString(), value);
		ObscuredPrefs.Save();
	}

	public static int GetInt(SaveKey key)
	{
		if(mCache.ContainsKey(key.ToString()))
		{
			return (int)mCache[key.ToString()];
		}
		else
		{
			int value = ObscuredPrefs.GetInt(key.ToString(), (int)Default(key));
			mCache[key.ToString()] = value;
			return value;
		}
	}

	public static void SetInt(SaveKey key, int value)
	{
		mCache[key.ToString()] = value;
		ObscuredPrefs.SetInt(key.ToString(), value);
		ObscuredPrefs.Save();
	}

	public static bool GetBool(SaveKey key)
	{
		if(mCache.ContainsKey(key.ToString()))
		{
			return (bool)mCache[key.ToString()];
		}
		else
		{
			bool value = ObscuredPrefs.GetBool(key.ToString(), (bool)Default(key));
			mCache[key.ToString()] = value;
			return value;
		}
	}

	public static void SetBool(SaveKey key, bool value)
	{
		mCache[key.ToString()] = value;
		ObscuredPrefs.SetBool(key.ToString(), value);
		ObscuredPrefs.Save();
	}

	public static float GetFloat(SaveKey key)
	{
		if(mCache.ContainsKey(key.ToString()))
		{
			return (float)mCache[key.ToString()];
		}
		else
		{
			float value = ObscuredPrefs.GetFloat(key.ToString(), (float)Default(key));
			mCache[key.ToString()] = value;
			return value;
		}
	}

	public static void SetFloat(SaveKey key, float value)
	{
		mCache[key.ToString()] = value;
		ObscuredPrefs.SetFloat(key.ToString(), value);
		ObscuredPrefs.Save();
	}

	public static void ResetData()
	{
		ObscuredPrefs.DeleteAll();
	}

}
