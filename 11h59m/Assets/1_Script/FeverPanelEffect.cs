using UnityEngine;
using System.Collections;

public class FeverPanelEffect : MonoBehaviour {

	private bool onEnter;


	public void OnEnable(){

		GetComponent<Animation>().Play ("fever_panel_enter");
		onEnter = true;
		StartCoroutine ("CheckEndOfEnterAni");
	}

	private IEnumerator CheckEndOfEnterAni(){
		float animLength = GetComponent<Animation>().GetClip("fever_panel_enter").length;
		yield return new WaitForSeconds(animLength);
		onEnter  = false;
		GetComponent<Animation>().Play("fever_panel");

	}

	public void EndFever(){

		if(onEnter) StopCoroutine ("CheckEndOfEnterAni");
	
		onEnter = false;
		GetComponent<Animation>().Play ("fever_panel_exit");
		StartCoroutine("CheckEndOfExitAni");

	}
	private IEnumerator CheckEndOfExitAni(){

		float animLength = GetComponent<Animation>().GetClip("fever_panel_exit").length;
		yield return new WaitForSeconds(animLength);
		gameObject.SetActive(false);
	}
}
