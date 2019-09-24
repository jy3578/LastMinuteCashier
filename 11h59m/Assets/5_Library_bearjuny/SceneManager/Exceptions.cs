using System;
using UnityEngine;
using System.Collections;

public class SceneArgsIsNullException : Exception
{
	public string key = null;
	
	public SceneArgsIsNullException(string key) : base(string.Format("SceneArgsIsNullException : {0}", key))
	{
		this.key = key;
	}
}

public class NotExistSpawnName : Exception
{
}