using UnityEngine;
using System.Collections;

public class InPlayAd : MonoBehaviour {

	public UITexture mIcon;
	public UITexture mLogo;

	private CachedInplay mInplay;

	public void SetInplayData(CachedInplay inplay){
		mInplay = inplay;
		transform.localPosition = Vector3.zero;
		transform.localScale = new Vector3(1f,1f,1f);
		mIcon.mainTexture = inplay.icon;
		mLogo.mainTexture = inplay.logo;
		inplay.Show ();
		GAManager.Instance.GAInplayEvent("display");
	}

	public void ClickInPlayAd(){
		mInplay.Click();

//		AdManager.Instance.SetInPlayAd (null);

		GAManager.Instance.GAInplayEvent("click");
	}
}
