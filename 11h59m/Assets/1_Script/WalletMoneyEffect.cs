using UnityEngine;
using System.Collections;
using PathologicalGames;

public class WalletMoneyEffect: MonoBehaviour {

	public int moneySPrice;
	public string effectPoolName;
	public UISprite coin;

	private SpawnPool pools;
	private bool onceChanged = false;

	void Start(){
		if(!onceChanged){
			if(SaveManager.GetLanguage() == "English"){
				coin.spriteName = "play_ui_moneyS_"+moneySPrice.ToString()+"_en";
			}
			onceChanged = true;
		}
	}

	void OnEnable(){

		GetComponent<Animation>().Play ("moneyS_"+moneySPrice.ToString());
		StartCoroutine ("TurnOff");
	}
	

	private IEnumerator TurnOff(){

		
		while(GetComponent<Animation>().isPlaying){
			
			yield return null;
		}
		DespawnMoneyS ();
		
	}

	private void DespawnMoneyS(){

		pools = PoolManager.Pools[effectPoolName];
		pools.Despawn(transform);

	}

}
