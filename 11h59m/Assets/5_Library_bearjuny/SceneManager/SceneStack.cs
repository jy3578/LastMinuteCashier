using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;

public class SceneStack : SingletonBehaviour<SceneStack>
{
	public enum Types {SYNC, ASYNC}
	public enum Methods {NORMAL, SINGLE_INSTANCE}

	private SceneLoader		mCurrent = null;
	public	SceneLoader		current {
		get {
			return mCurrent;
		}
	}
	
	public		Methods				method = Methods.NORMAL;
	public		bool				autoAccumulate = false;
	
	private		List<SceneLoader>	mList = new List<SceneLoader>();
	public		List<string>		additiveScenes = new List<string>();
	
	private		bool				mDoing = false;
	
	public		bool				doing {get{return mDoing;}}

	public		List<SceneLoader>	history {
		get {
			return mList;
		}
	}

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	public string GetPageName()
	{
		/*
		List<string> pages = new List<string>();

		if(mList != null)
		{
			foreach(var scene in mList)
			{
				pages.Add(scene.pageName);
			}
		}
		if(mCurrent != null)
		{
			pages.Add(mCurrent.pageName);
		}
		*/

		return mCurrent.pageName;
	}

	public void Add(string name)
	{
		SceneLoader loader = new SceneLoader(name);
		mCurrent = loader;
	}

	public bool Back(Types type=Types.SYNC, bool clearArgs=false)
	{
		SceneLoader backScene = Pop(1);
		if(clearArgs) backScene.args = null;
		if(backScene != null) return LoadScene(backScene, type);
		return false;
	}

	public bool BackTo(string name, Types type=Types.SYNC, bool clearArgs=false)
	{
		RemoveTilRecent(name);
		SceneLoader backScene = Pop();
		if(clearArgs) backScene.args = null;
		if(backScene != null) return LoadScene(backScene, type);
		return false;
	}

	public bool LoadLevel(string name, Types type=Types.SYNC)
	{
		return LoadLevel(new SceneLoader(name), type);
	}
	
	public bool LoadLevel(SceneLoader loader, Types type=Types.SYNC)
	{
		SceneLoader current = mCurrent;

		// Arguments Accumulation
		if(autoAccumulate && current != null) loader.SetBaseArgs(current.args);

		if(mCurrent != null)
		{
			Push(mCurrent);
			bool result = LoadScene(loader, type);
			if(!result)	mList.RemoveAt(mList.Count-1);

			return result;
		}
		else
		{
			return LoadScene(loader, type);
		}
	}

	private bool LoadScene(SceneLoader loader, Types type=Types.SYNC)
	{
		if(!mDoing && loader != null && !string.IsNullOrEmpty(loader.name))
		{
			mCurrent = loader;
			
			if(additiveScenes.Contains(loader.name))
			{
				ActiveAdditiveLevel(loader.name);
			}
			else
			{
				if(type == Types.SYNC) LoadLevelSync(loader.name);
				else if(type == Types.ASYNC) StartCoroutine(LoadLevelAsync(loader.name));
			}
			return true;
		}
		return false;
	}

	public void AdditiveLoad(string name)
	{
		if(!additiveScenes.Contains(name))
		{
			additiveScenes.Add(name);
			StartCoroutine(LoadLevelAdditive(name));
		}
	}

	void ActiveAdditiveLevel(string name)
	{
		mDoing = true;

		AdditiveScene[] scenes = GameObject.FindObjectsOfType<AdditiveScene>();

		foreach(var scene in scenes)
		{
			if(scene.sceneName != name && scene.show == true) scene.SetActive(false);
		}

		foreach(var scene in scenes)
		{
			if(scene.sceneName == name && scene.show == false) scene.SetActive(true);
		}

		mDoing = false;
	}

	void LoadLevelSync(string name)
	{
		mDoing = true;
		Application.LoadLevel(name);
	}
	
	IEnumerator LoadLevelAsync(string name)
	{
		mDoing = true;
		yield return Application.LoadLevelAsync(name);
		mDoing = false;
	}

	IEnumerator LoadLevelAdditive(string name)
	{
		mDoing = true;
		yield return Application.LoadLevelAdditiveAsync(name);
		mDoing = false;
	}
	
	void Push(SceneLoader loader)
	{
		if(loader != null && !string.IsNullOrEmpty(loader.name))
		{
			if(method == Methods.SINGLE_INSTANCE) RemoveAll(loader.name);
			mList.Add(loader);
		}
	}
	
	void RemoveAll(string name)
	{
		mList.RemoveAll((loader) => {
			return (loader == null || (!string.IsNullOrEmpty(name) && name == loader.name));
		});
	}
	
	void RemoveTilRecent(string name)
	{
		int index = mList.FindLastIndex((match) => {
			return (match.name == name);
		});

		if(index >= 0) mList.RemoveRange(index+1, mList.Count-index-1);
	}
	
	SceneLoader RemoveLast()
	{
		if(mList.Count > 0)
		{
			SceneLoader scene = mList[mList.Count-1];
			mList.RemoveAt(mList.Count-1);
			
			return scene;
		}
		return null;
	}
	
	SceneLoader Pop(int count=1)
	{
		SceneLoader lastScene = null;
		if(instance != null)
		{
			for(int i=0; i < count; i++)
			{
				SceneLoader tempLastScene = RemoveLast();
				if(tempLastScene == null) break;
				lastScene = tempLastScene;
			}
		}
		return lastScene;
	}
}