using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {


	private static T _instance;
	
	public static T Instance
	{

		get{
			if(_instance == null)
			{
				_instance = FindObjectOfType(typeof(T)) as T;
			
				if(_instance == null){

					GameObject singleton = new GameObject();
					_instance = singleton.AddComponent<T>();

					Debug.LogError ("No instance of" + typeof(T).ToString ());
				}

			}
			return _instance;

		}
	}




	public void OnApplicationQuit(){
		_instance = null;
	}
}
