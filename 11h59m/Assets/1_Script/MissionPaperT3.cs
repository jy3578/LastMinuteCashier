using UnityEngine;
using System.Collections;

public class MissionPaperT3 : MissionPaperConditioned {

	public override void SetDataInPaper (MissionState newMS, int categoryNum, int unlocked)
	{
		base.SetDataInPaper (newMS, categoryNum, unlocked);

		if(category != 7 && MS == MissionState.Ongoing && unlocked == 0){ // 4, 5, 6, 카테고리에서 1번째 미션 컨디션을 진행중일경우, 표시 안함.
			my.text = "";
		}
	}

}
