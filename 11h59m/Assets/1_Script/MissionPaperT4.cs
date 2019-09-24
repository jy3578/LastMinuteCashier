using UnityEngine;
using System.Collections;

public class MissionPaperT4 : MissionPaperConditioned {

	public override void SetDataInPaper (MissionState newMS, int categoryNum, int unlocked)
	{
		category = categoryNum;
		MS = newMS;
		icon.spriteName = "collection_icon" + (category+1).ToString("d2");
		title.text = Localization.Get ("collection"+category.ToString()+"_desc");
		
		target.text = MissionManager.Instance.GetMissionObjective(category).ToString();
		unit.text = Localization.Get ("collection"+category.ToString()+"_unit");
		
		SetTryBtn(MS);
		
		if(category == 8 && MS == MissionState.Ongoing){ //bald 미션일때 남은시간 표시.
			StartCoroutine("CountRemainingTime",unlocked);
		}else if(category ==10 && MS == MissionState.Ongoing){
			ShowRemainingPlay(unlocked);
		}else{
			my.text = Localization.Get ("collection"+category.ToString()+"_cond"+(unlocked+1).ToString());
		}
	}
	
	protected IEnumerator CountRemainingTime(int unlocked){
		
		float remain  = MissionManager.Instance.conditionForCategory8[unlocked] -  SaveManager.GetBaldPlayingTime();
		
		while(true){
			remain--;
			
			int remainingM =(int) (remain/60f);
			int remainingS = ((int)(remain - 60*remainingM));
			
			my.text = Localization.Get ("collection"+category.ToString()+"_condi_remaining") + ": "+remainingM.ToString()+"m " + remainingS.ToString()+"s";
			yield return new WaitForSeconds(1f);
			
		}
	}

	protected void ShowRemainingPlay(int unlocked){

		int remain = MissionManager.Instance.conditionForCategory10[unlocked]-SaveManager.GetDisPlayCount();
		my.text = Localization.Get ("collection"+category.ToString()+"_condi_remaining") +": "+remain.ToString()+Localization.Get ("collection"+category.ToString()+"_condi_remaining_unit");

	}
}
