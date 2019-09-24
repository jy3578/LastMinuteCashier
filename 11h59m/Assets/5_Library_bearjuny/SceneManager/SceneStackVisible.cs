using UnityEngine;
using System.Collections;

public abstract class SceneStackVisible : MonoBehaviour
{
	public		Transform		target = null;

	private		bool			mLastVisible = false;
	private		bool			mStart = false;

	void Enable()
	{
		mStart = true;
	}

	void Update()
	{
		UpdateVisible();
	}

	void UpdateVisible()
	{
		bool visible = IsVisible();
		if(mStart || mLastVisible != visible)
		{
			SetVisible(visible);
			mLastVisible = visible;

			mStart = false;
		}
	}

	void SetVisible(bool visible)
	{
		target.gameObject.SetActive(visible);
	}

	protected abstract bool IsVisible();
}