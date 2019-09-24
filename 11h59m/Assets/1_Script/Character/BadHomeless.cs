using UnityEngine;
using System.Collections;
using PathologicalGames;

public class BadHomeless : CustomerBad {


	public override void BadEffect(){

		SpawnPool pools = PoolManager.Pools[badEffectPoolName];

		badEffect = pools.Spawn("HomelessIcon");
		badEffect.localEulerAngles = new Vector3(0f,0f,0f);
		badEffect.localScale = new Vector3(1f,1f,1f);
		badEffect.localPosition = new Vector3(17.5f,-125f,0f);
		badEffect.gameObject.GetComponent<UISprite>().spriteName = "play_char_poor_shit"+(Random.Range(1,6)).ToString();
	}


	override public void EndPayment(){


	}
	
	override public int GetBadType(){
		return 2;
	}
}