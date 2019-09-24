using UnityEngine;
using System.Collections;

public class MissionPaperConditioned : MissionPaperPopUp {

	
	/*
	
	public UISprite icon;
	public UILabel target;
	public UILabel title;
	
	public UISprite tryBtn;
	public BoxCollider2D tryBtnBox;
	
	protected bool OnClose;
	protected int category;
	protected MissionState MS;

*/
	public UISprite tryBtn;
	public UISprite tryBtnStatus;
	public UILabel tryBtnLabel;
	

	public override void SetDataInPaper (MissionState newMS, int categoryNum, int unlocked)
	{
		category = categoryNum;
		MS = newMS;
		icon.spriteName = "collection_icon" + (category+1).ToString("d2");
		title.text = Localization.Get ("collection"+category.ToString()+"_desc");
		my.text = Localization.Get ("collection"+category.ToString()+"_cond"+(unlocked+1).ToString());
		target.text = MissionManager.Instance.GetMissionObjective(category).ToString();
		unit.text = Localization.Get ("collection"+category.ToString()+"_unit");

		SetTryBtn(MS);

	}

	protected void SetTryBtn(MissionState newMS){
		if(newMS == MissionState.Ongoing){ //ongoing 상태.
			tryBtn.spriteName = "collection_popUp_btnGrey";
			tryBtnStatus.spriteName = "collection_popUp_icon_challenge";
			tryBtnStatus.MakePixelPerfect();
			tryBtnLabel.text = Localization.Get ("collection_ongoing");
			tryBtn.gameObject.GetComponent<CircleCollider2D>().enabled = false;
			tryBtn.gameObject.GetComponent<UIButton>().normalSprite = "collection_popUp_btnGrey";
			
		}else if(newMS == MissionState.BeforeStart){
			tryBtn.spriteName = "collection_popUp_btnGreen";
			tryBtnStatus.spriteName = "collection_popUp_icon_check";
			tryBtnStatus.MakePixelPerfect();
			tryBtnLabel.text = Localization.Get ("collection_try");
			tryBtn.gameObject.GetComponent<CircleCollider2D>().enabled = true;
			tryBtn.gameObject.GetComponent<UIButton>().normalSprite = "collection_popUp_btnGreen";
			
		}else if(newMS == MissionState.StartAgain){
			tryBtn.spriteName = "collection_popUp_btnRed";
			tryBtnStatus.spriteName = "collection_popUp_icon_check";
			tryBtnStatus.MakePixelPerfect();
			tryBtnLabel.text = Localization.Get ("collection_retry");
			tryBtn.gameObject.GetComponent<CircleCollider2D>().enabled = true;
			tryBtn.gameObject.GetComponent<UIButton>().normalSprite = "collection_popUp_btnRed";
		}
	}

	public override void ClickTryBtn ()
	{

		stuffLabel.StartMissionByTryBtn();
		MissionManager.Instance.ResetAndStartCategory(category);
		SetTryBtn(MissionState.Ongoing);

		if(MS==MissionState.BeforeStart){ // 도전.
			GAManager.Instance.GAMissionTryBtnEvent(category);

		}else{ // 재도전.
			GAManager.Instance.GAMissionRetryBtnEvent(category);
		}

		
	}

}
