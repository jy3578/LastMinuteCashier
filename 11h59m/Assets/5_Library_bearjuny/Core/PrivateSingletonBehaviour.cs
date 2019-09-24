using UnityEngine;
using System.Collections;

public class PrivateSingletonBehaviour<T> : MonoBehaviour where T : PrivateSingletonBehaviour<T>
{
	private static T mInstance = null;
	
	protected static T instance {
		get {
			if(mInstance == null)
			{
				T[] objs = GameObject.FindObjectsOfType<T>();
				if(objs.Length > 0)
				{
					if(objs.Length == 1)
					{
						mInstance = objs[0];
					}
					else
					{
						Debug.LogError("You have more than one " + typeof(T).Name + " in the scene. You only need 1, it's a singleton!");
						
						mInstance = objs[0];
						for(int i=1; i < objs.Length; i++) GameObject.Destroy(objs[i].gameObject);
					}
				}
				else
				{
					GameObject go = new GameObject(typeof(T).Name, typeof(T));
					mInstance = go.GetComponent<T>();
				}
			}
			return mInstance;
		}
	}
}