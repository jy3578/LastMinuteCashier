using UnityEngine;
using System.Collections;
using PathologicalGames;

public class FeverAttackEffect : MonoBehaviour {

	public string poolName;
	public tk2dSpriteAnimator anim;

	void OnEnable(){
		anim.Play ();
		StartCoroutine ("TurnOff");
	}

	private IEnumerator TurnOff(){
		yield return new WaitForSeconds(0.5f);

		DespawnFeverAttackEffect();

	}

	private void DespawnFeverAttackEffect(){
		PoolManager.Pools[poolName].Despawn(transform);
	}
}
