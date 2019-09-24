using UnityEngine;
using System.Collections;
using PathologicalGames;

public enum SlotState{

	HaveMoney,
	HaveNoMoney
	
}

public enum MoneyType{

	Bill,
	Coin
}

public class WalletSlot : MonoBehaviour {
	
	// 미리 입력 받을 변수.
	public int slotPrice;
	public MoneyType moneyType;

	public TraySlot pairOnTray;
	public BoxCollider2D col;// fever일때는, touch가 feverPanel의 boxcollider에 부딪쳤는지 판별하기 위해 반드시 반드시 on off 필요.

	private GameManager GM;
	private SpawnPool pools;

	//thief의 bad effect가 이미지 안보이게 하고, collider 안 눌리게.
	private UISprite moneyImg;
	private bool colEnabled;

	private string lang = "Korean";

	void Start(){

		GM = GameManager.Instance;
		pools = PoolManager.Pools["WalletMoney"];

		moneyImg = gameObject.GetComponent<UISprite>();

		colEnabled = true;


	}

	public void LocalizeMoney(string language){
		lang = language;
		pairOnTray.LocalizeMoney(language);
		if(lang == "English"){
			moneyImg.spriteName = "play_ui_coin_"+slotPrice.ToString()+"_en";
		}
	}

	public void TapSlot(){
		if(GM.GS == GameState.Play && colEnabled){

			if(GetComponent<Animation>().isPlaying) GetComponent<Animation>().Stop();
			GetComponent<Animation>().Play("tapWalletSlot");

			Transform moneyS = pools.Spawn("MoneyS"+slotPrice.ToString());

			if(moneyS != null){
				moneyS.transform.localPosition = transform.localPosition;
				moneyS.transform.localEulerAngles = Vector3.zero;
				moneyS.transform.localScale = new Vector3(1f,1f,1f);
			}
			SoundManager.PlaySFX(SoundManager.LoadFromGroup("Coin"));
			pairOnTray.MakeMoney();
		}
	}

	//for thief icon.  (thief가 show slot 할때와 fever모드가 끝나면서 collider를 enable할때 겹쳐서 제 기능을 하지 못하므로 분리).
	public void HideSlot(){
		moneyImg.enabled = false;
		colEnabled = false;
	}

	public void ShowSlot(){
		moneyImg.enabled = true;
		colEnabled = true;
	}
	/*
	public void FeverOnOff(bool isFever){

		col.enabled = (!isFever);
	}
	*/
}
