using UnityEngine;
using System.Collections;
using PathologicalGames;

public class BadIgnorant : CustomerBad {

	override public void BadEffect ()
	{

		SpawnPool pools = PoolManager.Pools[badEffectPoolName];
		badEffect = pools.Spawn("IgnorantIcons");
		badEffect.localScale = new Vector3(1f,1f,1f);
		badEffect.localPosition = new Vector3(800f,85f,1f);
		badEffect.GetComponent<Animation>().Play ("badIcon_flight");

	}

	override public void EndPayment(){


	}

	override public int GetBadType(){
		return 1;
	}
	

}
