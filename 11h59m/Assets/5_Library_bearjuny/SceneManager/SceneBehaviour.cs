using UnityEngine;
using System.Collections;

public class SceneBehaviour : MonoBehaviour
{
	public		bool		useKeyEvent = false;

	void OnEnable()
	{
		if(useKeyEvent) DeviceEvent.OnKeyEscape += BackFromKey;
	}

	void OnDisable()
	{
		DeviceEvent.OnKeyEscape -= BackFromKey;
	}

	private void BackFromKey()
	{
		Back(true);
	}

	public void BackFromButton()
	{
		Back(false);
	}

	private void Back(bool isKey)
	{
		if(isKey) return;
		if(SceneStack.instance.Back()) return;
		Application.Quit();
	}


}
