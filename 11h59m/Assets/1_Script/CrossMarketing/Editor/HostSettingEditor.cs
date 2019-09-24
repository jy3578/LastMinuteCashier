using UnityEditor;
using UnityEngine;
using System.Collections;

public class HostSettingEditor : MonoBehaviour
{
	[MenuItem("Game/Setting/Create Host")]
	[MenuItem("Assets/Create/Settings/Host", false)]
	private static void CreateCustomData()
	{
		ScriptableObjectUtility.CreateAsset<HostSetting>();
	}
}