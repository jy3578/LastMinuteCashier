using UnityEngine;
using System.Collections;
using PathologicalGames;

public class CustomerBad : Customer {

	public GameObject msgBox;
	public string badEffectPoolName;


	protected Transform badEffect;


	public override void InitializeCustomer(){
		msgBox.SetActive(false);
		base.InitializeCustomer();

	}

	public override void InFrontOfDesk(){
	
		if(!onFever){ //fever 모드가 아니면 bad effect를 실행( 그리고 onBadEffect = true가 되어 나중에 선별하여 꺼주기).
			BadEffect();
			msgBox.SetActive(true);
			onBadEffect = true;
		}
	}

	public override void SortImageInQ(int orderInQ){

		base.SortImageInQ(orderInQ);
		msgBox.GetComponent<tk2dSprite>().SortingOrder = orderInQ;
		msgBox.transform.GetChild(0).GetComponent<tk2dSprite>().SortingOrder = orderInQ; // msgBox Icon.

	}

	public override void DespawnBadEffect ()
	{
		SpawnPool pools = PoolManager.Pools[badEffectPoolName];
		if(onBadEffect){
			if(badEffect != null){
				pools.Despawn(badEffect);
				onBadEffect = false;
			}
		}
	}
	
	public override void EndPayment(){
		base.EndPayment();
		msgBox.SetActive (false);

	}
}
