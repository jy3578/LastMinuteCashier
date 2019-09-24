using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AdditiveSceneLoader : MonoBehaviour
{
	public		string 					loadScene = null;
	public		List<AdditiveScene>		scenes = null;

	void Awake()
	{
		if(scenes != null)
		{
			Build();
		}
	}

	void Start()
	{
		if(!string.IsNullOrEmpty(loadScene))
		{
			// Add current scene in SceneStack
			SceneStack.instance.Add(loadScene);	
		}

		if(scenes != null)
		{
			StartCoroutine(LoadScenes(scenes.ToArray()));
		}
	}

	IEnumerator LoadScenes(AdditiveScene[] scenes)
	{
		foreach(var scene in scenes)
		{
			SceneStack.instance.AdditiveLoad(scene.name);
			yield return null;
		}
	}

	void Build()
	{
		scenes.Sort((a, b) => {
			return b.priority.CompareTo(a.priority);
		});
	}

	[System.Serializable]
	public class AdditiveScene
	{
		public string name = null;
		public int priority = 0;
	}
}
