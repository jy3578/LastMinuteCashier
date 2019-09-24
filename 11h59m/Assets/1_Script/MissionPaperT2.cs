using UnityEngine;
using System.Collections;

public class MissionPaperT2 : MissionPaperNormal {
	/*
	public UISprite icon;
	public UILabel target;
	public UILabel mission;
	
	public UISprite missionBtn;
	public BoxCollider2D colBtn;
	
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
		
		unit.text = Localization.Get ("collection"+category.ToString()+"_unit");		
		
		if(SaveManager.GetLanguage() == "English"){
			target.text = (((float)targetNum)/1000f).ToString("F2");
			my.text =Localization.Get("collection_mynumber").ToString()+": "+Localization.Get ("collection"+category.ToString()+"_unit")+" "+(((float)myNum)/1000f).ToString("F2");
		}else{
			target.text = targetNum.ToString();
			my.text = Localization.Get("collection_mynumber").ToString()+": "+Localization.Get ("collection"+category.ToString()+"_unit")+" "+myNum.ToString();
		}
		
	}

}
