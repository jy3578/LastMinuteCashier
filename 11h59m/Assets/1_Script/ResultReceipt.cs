using UnityEngine;
using System.Collections;

public class ResultReceipt : MonoBehaviour {

	public NewIntAnimation scoreLb;
	public NewIntAnimation bestScoreLb;

	public UILabel[] gameResults;
	public UISprite newStuffIn;
	public Animation ani;

	public void ShowResult(int newTotalScore, int prevBest, int maxCombo,int customers, int exp, int achievements){

		scoreLb.Play (newTotalScore, newTotalScore);
		bestScoreLb.Play (prevBest, prevBest);

		if(SaveManager.GetLanguage() == "Korean"){
			gameObject.GetComponent<UISprite>().spriteName = "main_ui_result";
			newStuffIn.spriteName = "main_msg_newItem";
		
		}else if(SaveManager.GetLanguage() == "English"){
			gameObject.GetComponent<UISprite>().spriteName = "main_ui_result_en";
			newStuffIn.spriteName = "main_msg_newItem_en";
			
		}

		newStuffIn.MakePixelPerfect();
		newStuffIn.gameObject.transform.localScale = new Vector3(1.6875f,1.6875f,1f);
		
		transform.localPosition = new Vector3(0f,178.25f,0f);
		
		gameResults[0].text = maxCombo.ToString();
		gameResults[1].text = customers.ToString();
		gameResults [2].text = exp.ToString ();

		if(achievements == 0){
			newStuffIn.enabled = false;
			gameResults[3].text = "";
		}else{
			newStuffIn.enabled = true;
			gameResults[3].text = achievements.ToString();
		}


		ani.Play("resultReceipt_Idle");

	}



}
