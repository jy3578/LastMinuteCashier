using UnityEngine;
using System.Collections;

public class Wallet : Singleton<Wallet> {

	public WalletSlot[] walletSlots;
	public PlayMakerFSM playMaker;
	public BoxCollider2D[] walletCol;

	public UISprite confirmImg;

	private bool isFever;

	
	void Update(){

		if(GameManager.Instance.onFeverMode && !isFever){
			for(int i=0;i<walletCol.Length;i++){
				walletCol[i].enabled = false;
			}
			isFever = true;

		}else if(!GameManager.Instance.onFeverMode && isFever){
			for(int i=0;i<walletCol.Length;i++){
				walletCol[i].enabled = true;
			}
			isFever = false;
		}
//		/*
			if(GameManager.Instance.GS == GameState.Play && !isFever){
				if(Input.touchCount>0){
					
					int i =0;
					while(i<Input.touchCount){
										
						Vector3 touchPoint3;
						if(Input.GetTouch (i).phase == TouchPhase.Began){
							touchPoint3 = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
							Vector2 touchPoint2 = new Vector2(touchPoint3.x,touchPoint3.y);
							if(walletCol[0] == Physics2D.OverlapPoint(touchPoint2)){
								walletSlots[0].TapSlot();
							}else if(walletCol[1] == Physics2D.OverlapPoint(touchPoint2)){
								walletSlots[1].TapSlot();
							}else if(walletCol[2] == Physics2D.OverlapPoint(touchPoint2)){
								walletSlots[2].TapSlot();
							}else if(walletCol[3] == Physics2D.OverlapPoint(touchPoint2)){
								walletSlots[3].TapSlot();
							}else if(walletCol[4] == Physics2D.OverlapPoint(touchPoint2)){
								walletSlots[4].TapSlot();
							}else if(walletCol[5] == Physics2D.OverlapPoint(touchPoint2)){
								walletSlots[5].TapSlot();
							}else if(walletCol[6] == Physics2D.OverlapPoint(touchPoint2)){
								walletSlots[6].TapSlot();
							}else if(walletCol[7] == Physics2D.OverlapPoint(touchPoint2)){
								walletSlots[7].TapSlot();
							}
						}
						i++;
					}

				}
			}
//   */
	/*

			if(Input.GetMouseButtonDown(0)){
					Vector3 touchPoint3;
					
						touchPoint3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
						Vector2 touchPoint2 = new Vector2(touchPoint3.x,touchPoint3.y);
						if(walletCol[0] == Physics2D.OverlapPoint(touchPoint2)){
							walletSlots[0].TapSlot();
						}else if(walletCol[1] == Physics2D.OverlapPoint(touchPoint2)){
							walletSlots[1].TapSlot();
						}else if(walletCol[2] == Physics2D.OverlapPoint(touchPoint2)){
							walletSlots[2].TapSlot();
						}else if(walletCol[3] == Physics2D.OverlapPoint(touchPoint2)){
							walletSlots[3].TapSlot();
						}else if(walletCol[4] == Physics2D.OverlapPoint(touchPoint2)){
							walletSlots[4].TapSlot();
						}else if(walletCol[5] == Physics2D.OverlapPoint(touchPoint2)){
							walletSlots[5].TapSlot();
						}else if(walletCol[6] == Physics2D.OverlapPoint(touchPoint2)){
							walletSlots[6].TapSlot();
						}else if(walletCol[7] == Physics2D.OverlapPoint(touchPoint2)){
							walletSlots[7].TapSlot();
						}

			}



		*/
	}

/*
	public void FeverOnOff(bool isFever){

		for(int i=0;i<walletSlots.Length;i++){
			walletSlots[i].FeverOnOff(isFever);
		}
	}
*/
	public void SetLocalMoney(string language){
	
		for(int i=0;i<walletSlots.Length;i++){
			walletSlots[i].LocalizeMoney(language);
		}

	}
	

	public void StartPlaying(){
		isFever = false;
		playMaker.SendEvent("StartPlaying");
		SoundManager.PlaySFX("wallet_open");
	}
	
	public void EndPlaying(int closingType){
	
		if (closingType == 0) {
				playMaker.SendEvent ("FirstClosing");
		} else if(closingType == 1) {

			playMaker.SendEvent("EndPlayingShowAndReturn");


		} else if (closingType == 2) {
			playMaker.SendEvent ("EndPlayingReturn");
		} else if (closingType == 3) {
			playMaker.SendEvent ("EndPlayingRestart");

		}
		SoundManager.PlaySFX("wallet_close");
	}

	//thief BadEffect 표시할때, slot 내의 coin 이미지와 boxcollider on/Off.
	public void ShowSlot(int slotNumber){
		walletSlots[slotNumber].ShowSlot();
	}

	public void HideSlot(int slotNumber){
		walletSlots[slotNumber].HideSlot();
	}

	public Vector3 GetLocalPositionOfSlot(int slotNumber){
		return walletSlots[slotNumber].gameObject.transform.localPosition;
	}

	public void ShowConfirm(bool success){
	
		if(success){
			confirmImg.spriteName = "play_effect_result_success";

		}else{
			confirmImg.spriteName = "play_effect_result_fail";
		}
		GetComponent<Animation>().Play ("confirm_wallet");
	}


}
