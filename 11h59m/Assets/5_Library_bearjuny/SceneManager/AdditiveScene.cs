using UnityEngine;
using System.Collections;

public class AdditiveScene : MonoBehaviour
{
	public		string			sceneName = null;
	public		GameObject[]	objects = null;

	public		bool			show = false;

	void Awake()
	{
		if(SceneStack.instance != null && SceneStack.instance.current != null)
		{
			name += string.Format("({0})", sceneName);
			if(SceneStack.instance.current.name == sceneName) SetActive(true);
			//Debug.Log (SceneStack.instance.current.name);
		}
	}

	public void SetActive(bool active)
	{
		if(objects != null)
		{
			foreach(var go in objects)
			{
				if(go.activeSelf != active)
				{
					if(!active) SendActiveMessage(go, false);
					go.SetActive(active);
					if(active) SendActiveMessage(go, true);
				}
			}
		}
		show = active;
	}

	void SendActiveMessage(GameObject go, bool active)
	{
		if(active) go.BroadcastMessage("OnActive", SendMessageOptions.DontRequireReceiver);
		else go.BroadcastMessage("OnInactive", SendMessageOptions.DontRequireReceiver);
	}
}