using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour{

	public UILabel tutMsg;

	private string mPrefix;
	private int mOrder;
	private int mMaxLine;
	private int mType; 

	public void ShowTutorial(string prefix,int type ,int maxLine){
		// type =>>> 1 : start, 2 : game , 3 : fever, 4 : mission.


		mPrefix = prefix;
		mOrder = 1;
		mType = type;
		mMaxLine = maxLine;
		tutMsg.text = Localization.Get (mPrefix + "Tutorial" + mOrder.ToString ());
		GetComponent<Animation>().Play ("tutorial_show");

		if (mType == 2 || mType == 3) {
			GameManager.Instance.GS = GameState.Pause;    // need to be fixed.	
		}
	}

	public void ShowNextMsg(){
		mOrder++;

		if (mOrder <= mMaxLine) {
			GetComponent<Animation>().Play ("tutorial_next");
			tutMsg.text = Localization.Get (mPrefix + "Tutorial" + mOrder.ToString ());

		} else {
			GetComponent<Animation>().Play("tutorial_close");
			SaveManager.SetTutorial(mType,true);
			StartCoroutine(CloseAni());

			if (mType == 2 || mType == 3) {
				GameManager.Instance.GS = GameState.Play;
			}
		}

	}
	
	public void ClickTut(){

		ShowNextMsg ();
	
	}

	
	private IEnumerator CloseAni(){


		yield return new WaitForSeconds(GetComponent<Animation>().GetClip("tutorial_close").length);
		Destroy(gameObject);
	}

}
