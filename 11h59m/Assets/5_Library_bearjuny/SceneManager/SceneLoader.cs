using UnityEngine;
using System.Collections.Generic;

public class SceneLoader
{
	public const string PageKey = "PageName";

	public string name = null;
	public Dictionary<string, object> args = null;

	public string pageName {
		get {
			if(args != null && args.ContainsKey(PageKey)) return args[PageKey] as string;
			return name;
		}
	}

	public SceneLoader(string name, Dictionary<string, object> args = null)
	{
		this.name = name;
		this.args = args;
	}
	
	public object this[string key]
	{
		get {
			if(args != null) return args[key];
			else throw new SceneArgsIsNullException(key);
		}
	}

	public void SetBaseArgs(Dictionary<string, object> baseArgs)
	{
		if(baseArgs != null)
		{
			if(this.args != null)
			{
				foreach(var keyVal in baseArgs)
				{
					if(keyVal.Key != PageKey && // Don't accumulate page name.
					   !this.args.ContainsKey(keyVal.Key)) this.args.Add(keyVal.Key, keyVal.Value);
				}
			}
			else
			{
				this.args = baseArgs;
			}
		}
	}
}