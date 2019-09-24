using UnityEngine;
using System.Collections;
using PathologicalGames;

public class feverLightAnimation : MonoBehaviour {
	
	public GameObject light1;
	public GameObject light2;

	public void OnEnable(){
		light1.transform.localPosition = new Vector3(-1620f,0f,0f);
		light2.transform.localPosition = new Vector3(-540f,0f,0f);

		light1.GetComponent<Animation>().Play ("fever_lights1");
		StartCoroutine ("TurnOnAniLight2");

	}

	private IEnumerator TurnOnAniLight2(){

		float aniLength = light1.GetComponent<Animation>().GetClip ("fever_lights1").length;
		yield return new WaitForSeconds(aniLength/2f);
		light2.GetComponent<Animation>().Play ("fever_lights2");
	
	}
	
	

}
