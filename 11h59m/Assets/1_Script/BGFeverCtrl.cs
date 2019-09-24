using UnityEngine;
using System.Collections;

public class BGFeverCtrl : MonoBehaviour {
	

	public void OnEnable(){
		GetComponent<Animation>().Play ("fever_wall_enter");
	}

	public void EndFever(){
		GetComponent<Animation>().Play("fever_wall_exit");
		StartCoroutine ("CheckEndOfExitAni");
	}

	private IEnumerator CheckEndOfExitAni(){
		float animLength = GetComponent<Animation>().GetClip("fever_wall_exit").length;
		yield return new WaitForSeconds(animLength);
		gameObject.SetActive(false);
	}
}
