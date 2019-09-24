using UnityEngine;
using System.Collections;

public class AnimatedCI : MonoBehaviour {

	public Animation p1;
	public Animation a1;
	public Animation p2;
	public Animation a2;
	public Animation y3;
	public Animation a3;

	public string sceneName;
	public float duration;

	void Start(){
		StartCoroutine("PreloadGame");
		StartCoroutine("WaitAndPlay");

	}

	private IEnumerator PreloadGame(){
		AsyncOperation status =  Application.LoadLevelAsync(sceneName);
		status.allowSceneActivation = false;
		
		yield return new WaitForSeconds( duration );
		
		status.allowSceneActivation = true;
	}

	private IEnumerator WaitAndPlay(){
		p1.Play ("CI_ani_P");
		a3.Play ("CI_ani_A");

		yield return new WaitForSeconds(0.2f);

		a1.Play ("CI_ani_A");
		y3.Play ("CI_ani_Y");

		yield return new WaitForSeconds(0.3f);

		p2.Play ("CI_ani_P");
		a2.Play ("CI_ani_A");
	}

}
