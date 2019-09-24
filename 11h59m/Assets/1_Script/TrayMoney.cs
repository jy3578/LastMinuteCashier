using UnityEngine;
using System.Collections;

public class TrayMoney : MonoBehaviour {
	
	public int moneyPrice;
	public tk2dSprite moneyImg;

	private bool onceChanged = false;

	void Start(){
		if(!onceChanged){
			if(SaveManager.GetLanguage() == "English"){
				moneyImg.SetSprite("play_ui_moneyS_"+moneyPrice.ToString()+"_en");
			}
			onceChanged = true;
		}
	}

	public void SortingImgInDeck(int numberOfMoney){
		moneyImg.SortingOrder =  13 - numberOfMoney + 1;
	}

}
