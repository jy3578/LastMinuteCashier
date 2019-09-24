using UnityEngine;
using System.Collections;

public class MissionPaperNormal : MissionPaperPopUp {

	/*
	public UISprite icon;
	public UILabel target;
	public UILabel my;
	public UILabel title;
	public UILabel unit;

	public CollectionStuffLabel stuffLabel;

	protected bool OnClose;
	protected int category;
	protected MissionState MS;

*/
	
	public override void SetDataInPaper (MissionState newMS, int categoryNum, int unlocked)
	{
		category = categoryNum;
		MS = newMS;
		
		icon.spriteName = "collection_icon" + (category+1).ToString("d2");
		title.text = Localization.Get ("collection"+category.ToString()+"_desc");
		int targetNum = MissionManager.Instance.GetMissionObjective(category);
		int myNum  = SaveManager.GetMyNumbers(category);

		target.text = targetNum.ToString();
		my.text = Localization.Get("collection_mynumber").ToString()+": "+myNum.ToString()+" "+Localization.Get ("collection"+category.ToString()+"_unit");
		unit.text = Localization.Get ("collection"+category.ToString()+"_unit");

	}
	
	public override void ClickTryBtn ()
	{
	
	}

}
