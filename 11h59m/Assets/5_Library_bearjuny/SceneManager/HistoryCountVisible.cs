using UnityEngine;
using System.Collections;

public class HistoryCountVisible : SceneStackVisible
{
	public int threshHold = 0;

	protected override bool IsVisible ()
	{
		return SceneStack.instance.history.Count >= threshHold;
	}
}
