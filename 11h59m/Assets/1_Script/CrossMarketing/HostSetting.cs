using UnityEngine;
using System.Collections;

[System.Serializable]
public class HostSetting : ScriptableObject
{
	[SerializeField]
	public		WebHost			host = null;
}