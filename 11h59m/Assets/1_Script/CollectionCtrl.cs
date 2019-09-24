using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;


public class CollectionCtrl : Singleton<CollectionCtrl> {
	
	public CollectionStuffLabel[] missionLabels;
	
	private List<int> openedPaper; //열려있는 미션 페이퍼 카테고리 넘버 저장.
	
	void Start(){
	
		openedPaper = new List<int>();
		StartCoroutine("ShowMissionStuffAtStartWithDelay");
	}

	private IEnumerator ShowMissionStuffAtStartWithDelay(){
		yield return new WaitForSeconds(0.5f);
		ShowMissionStuff ();
	}

	private void ShowMissionStuff(){
		for(int i=0;i<missionLabels.Length;i++){
			missionLabels[i].ShowStuffOnDisplay();
			missionLabels[i].ShowStuffOnShelf();
		}
	}

	//팝업창을 스스로 닫았을때.
	public void CloseMissionPaperbyClick(){ // 자기자신을 클릭하여 닫기만 하였을때.
		if(openedPaper.Count!=0){
			for(int i=0;i<openedPaper.Count;i++){
				missionLabels[openedPaper[i]].CloseMissionPaperbyClick();
			}
		}
		openedPaper.Clear ();
	}
	//외부에서 호출하여 팝업창 닫을때(콜렉션화면을 나갈때).
	public void CloseOnly(){
		if(openedPaper.Count != 0){
			for(int i = 0; i<openedPaper.Count ; i++){
				missionLabels[openedPaper[i]].CloseMissionPaperbyOther(); //열려있던 paper를 닫는다.
			}
			openedPaper.Clear ();
		}
	}
	//팝업창을 클릭하여 열고, 이전에 열렸던 팝업창 닫을때.
	public void CloseAndShowSimultaneously(int category){
		CloseOnly();
		openedPaper.Add(category); //새로 열린것 category 넘버를 리스트에 기억해둠.
	}
	

	private IEnumerator WaitAndShowMissionPaper(int category){
		yield return new WaitForSeconds(0.5f); //임시로- 원래는 애니메이션 끝나는 시간으로.
		//effect효과 despawn 시키기.

		missionLabels[category].ShowMissionPaper();
	}

	//collection화면에서 표시되는 것.
	public void UpdateNumbers(int whichCategory){
		missionLabels[whichCategory].ShowNumbersOnly();
	}

	public void UpdateNumbersAll(){
		for(int i=0;i<missionLabels.Length;i++){
			missionLabels[i].ShowNumbersOnly();
		}
	}

	public void UpdateInfoOn(int whichcategory){
		missionLabels[whichcategory].ShowStuffOnShelf();
	}

	public void UpdateInfoAll(){
		for(int i=0;i<missionLabels.Length;i++){

			missionLabels[i].ShowStuffOnShelf();
		}
	}
}
