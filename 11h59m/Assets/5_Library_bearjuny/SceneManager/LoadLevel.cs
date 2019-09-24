using UnityEngine;
using System.Collections;

public class LoadLevel : MonoBehaviour
{
	public enum Methods {LOAD, BACK_TO}

	public		string				levelName = null;
	public		SceneStack.Types	type = SceneStack.Types.SYNC;
	public		Methods				method = Methods.LOAD;

	public void Load()
	{

		if(method == Methods.LOAD) SceneStack.instance.LoadLevel(levelName, type);
		else if(method == Methods.BACK_TO) SceneStack.instance.BackTo(levelName, type);
	}
}
