using UnityEngine;
using System.Collections;
using PathologicalGames;

public class FeverTouchEffect : MonoBehaviour {

	public UISpriteAnimation anim;
	
//	private bool mIsPlaying = false;


	void OnEnable(){
	
//		mIsPlaying = false;
		anim.Play ();
		StartCoroutine ("TurnOff");


	}
	/*
	void OnDisable(){

		if(mIsPlaying){
			DespawnFeverEffect();
		}
	}
*/
	private IEnumerator TurnOff(){

//		mIsPlaying = true;

		while(anim.isPlaying){

			yield return null;
		}
//		mIsPlaying = false;
		DespawnFeverEffect ();

	}

	private void DespawnFeverEffect(){

		PoolManager.Pools["FeverTouches"].Despawn(transform);

	}
}
