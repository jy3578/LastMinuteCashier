using UnityEngine;
using System.Collections;
using PathologicalGames;

public class BadThief : CustomerBad {
	
	private int slotNumber; 

	override public void BadEffect ()
	{

		Receipt receipt = Receipt.Instance;

		if(receipt.change >= 100 && receipt.change<1000){
			slotNumber = Random.Range (5,8);
		}else if(receipt.change >= 1000 && receipt.change < 5000){
			slotNumber = Random.Range (4,7);
		}else if(receipt.change >= 5000 && receipt.change < 10000){
			slotNumber = Random.Range (3,6);
		}else if(receipt.change >= 10000 && receipt.change <50000){
			slotNumber = Random.Range(2,5);
		}else{
			slotNumber = Random.Range (1,4);
		}

		Wallet.Instance.HideSlot (slotNumber-1);
	
		SpawnPool pools = PoolManager.Pools[badEffectPoolName];

		badEffect = pools.Spawn("ThiefIcon");
		badEffect.localPosition = Wallet.Instance.GetLocalPositionOfSlot(slotNumber-1);
		badEffect.localScale = new Vector3(1f,1f,1f);

	}

	override public void EndPayment(){
		if(onBadEffect){
			Wallet.Instance.ShowSlot(slotNumber-1);

		}
	}

	override public int GetBadType(){
		return 3;
	}

}
