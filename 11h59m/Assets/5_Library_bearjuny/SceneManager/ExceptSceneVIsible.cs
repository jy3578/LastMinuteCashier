using UnityEngine;
using System.Collections.Generic;

public class ExceptSceneVIsible : SceneStackVisible
{
	public		List<string>		invisibleScenes = null;

	protected override bool IsVisible ()
	{
		if(SceneStack.instance.current != null)
		{
			if(invisibleScenes == null ||
			   !invisibleScenes.Contains(SceneStack.instance.current.name))
				return true;
		}
		return false;
	}

}
