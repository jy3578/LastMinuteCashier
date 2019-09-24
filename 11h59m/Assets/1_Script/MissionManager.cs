using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class MyArrayInt{
	public int[] numbers;

}

public class MissionManager : Singleton<MissionManager> {
	
	public MyArrayInt[] numbersToCompleteMission;//미션을 달성하기 위한 조건들.

	public MyArrayInt[] conditionForCategory4;	//4x4 matrix. 0 : good / 1: successive good / 2: perfect / 3: successive perfect.
	public MyArrayInt[] conditionForCategory5;	//각 미션에서 good을 카운트하는것이면 1,0,0,0 형태로 저장. 연속 perfect 달성이면 0,0,1,0 으로 저장.
	public MyArrayInt[] conditionForCategory6;
	public MyArrayInt[] conditionForCategory7;

	public float[] conditionForCategory8; //대머리아저씨 미션에서 각 미션의 제한시간.
	public MyArrayInt[] conditionForCategory9; //소녀.
	public int[] conditionForCategory10;          //(불만족고객미션달성)위한 제한된 플레이 횟수.

	private int[] unlocked;
	private int[] missionStates;
	private int[] myNumbers;   //현재 진행중인 나의 점수.

	private float baldMissionCheckTime; // 앱 실행 시작 시점에(start) lastPlayedTime과 앱 실행시 datetime의 차이를 합하여 계산된 미션 진행시간.

	void Start(){
		myNumbers = new int[14];

		unlocked = new int[numbersToCompleteMission.Length];
		missionStates = new int[numbersToCompleteMission.Length];

		SetBaldMissionTimeAtStart();

	}

	private void SetBaldMissionTimeAtStart(){ //앱 실행 처음시점에 시간제 미션의 시간을 (현재 시점 - 마지막 실행시점)의 시간을 더해서 체크함.
		missionStates[8] = SaveManager.GetCollectionStatesInCategory(8);
		if(missionStates[8] == 1){

			baldMissionCheckTime = 0f;
			string lastTime = SaveManager.GetBaldLastPlayedTime();
			if(lastTime != ""){
				DateTime lt = Convert.ToDateTime(lastTime);
				TimeSpan timeSpan = DateTime.Now - lt;

				float baldTime = SaveManager.GetBaldPlayingTime()+ (float) timeSpan.TotalSeconds;
				SaveManager.SetBaldPlayingTime(baldTime);
				UpdateCategory8 (0);
			}
		}
	}
	
	private int UpdateCategoryForAccumulated(int whichcategory, int newValue){

		int newTotalValue = myNumbers[whichcategory] + newValue;
		int newUnlocked = 0;
		unlocked[whichcategory] = SaveManager.GetCollectionUnlockedInCategory(whichcategory);
		missionStates[whichcategory] = SaveManager.GetCollectionStatesInCategory(whichcategory);

		if(unlocked[whichcategory]<numbersToCompleteMission[whichcategory].numbers.Length){  //아직 lock되어있는것이 있고.
			if(newTotalValue >= numbersToCompleteMission[whichcategory].numbers[unlocked[whichcategory]]){  //numbers에 저장되어 있는 미션 스코어보다 크면!.
				//unlock 조건을 만족시켰으므로, states를 NotAcceptYet 상태로 바꿈(아직 unlock은 아님, 수령후 unlock됨).

				if(missionStates[whichcategory] == 1){ //ongoing 상태이면.
					GAManager.Instance.GAMissionSuccessEvent(whichcategory.ToString()+"_"+unlocked[whichcategory].ToString());
					missionStates[whichcategory] = 3;
					SaveManager.SetCollectionStates(whichcategory,3);

					bool nextNeedsOpen = true;
					if(unlocked[whichcategory] == (numbersToCompleteMission[whichcategory].numbers.Length -1)) nextNeedsOpen = false;
					SocialManager.Instance.ReportAchievementAfterPlaying(whichcategory,unlocked[whichcategory]+1,nextNeedsOpen);
		
					newUnlocked = 1;
				}
			}
		}

		return newUnlocked;
	}

	private int UpdateCategoryForMax(int whichcategory, int newValue){
	
		unlocked[whichcategory] = SaveManager.GetCollectionUnlockedInCategory(whichcategory);
		missionStates[whichcategory] = SaveManager.GetCollectionStatesInCategory(whichcategory);

		if(missionStates[whichcategory] !=3 && missionStates[whichcategory] !=4){
			if(myNumbers[whichcategory]<newValue) myNumbers[whichcategory] = newValue;

			if(unlocked[whichcategory]<numbersToCompleteMission[whichcategory].numbers.Length){
				if(myNumbers[whichcategory]>=numbersToCompleteMission[whichcategory].numbers[unlocked[whichcategory]]){
					GAManager.Instance.GAMissionSuccessEvent(whichcategory.ToString()+"_"+unlocked[whichcategory].ToString());
					missionStates[whichcategory] =3;
					SaveManager.SetCollectionStates (whichcategory,3);

					bool nextNeedsOpen = true;
					if(unlocked[whichcategory] == (numbersToCompleteMission[whichcategory].numbers.Length -1)) nextNeedsOpen = false;
					SocialManager.Instance.ReportAchievementAfterPlaying(whichcategory,unlocked[whichcategory]+1,nextNeedsOpen);

				//	CollectionCtrl.Instance.ShowMissionSuccess(whichcategory);
					return 1;
				}
			}
		}
		return 0;
	}

	//category - 0.
	public int UpdateCategory0(){
		//missionType이 normal이면, 1,2,3번 mission state만 존재.
		//missionType이 condition이면, 콜렉션화면에서 수락이 있어야 숫자가 올라감.
		int categoryNum = 0;

		missionStates[categoryNum] = SaveManager.GetCollectionStatesInCategory(categoryNum);
		unlocked[categoryNum] = SaveManager.GetCollectionUnlockedInCategory(categoryNum);
		myNumbers[categoryNum] = SaveManager.GetTotalUnlocked();	//3번상황에서도 unlocked된 개수가 update 되어야 하므로 총 unlocked된 개수는 앞서 계산한다.

		if(missionStates[categoryNum] ==0){
			SaveManager.SetCollectionStates(categoryNum,1);
			missionStates[categoryNum] = 1;
		}

		if(missionStates[categoryNum] == 1){
			if(myNumbers[categoryNum] >= numbersToCompleteMission[categoryNum].numbers[unlocked[categoryNum]]){
				SaveManager.SetCollectionStates(categoryNum,3); //not accept yet.
				
				bool nextNeedsOpen = true;
				if(unlocked[categoryNum] == (numbersToCompleteMission[categoryNum].numbers.Length -1)) nextNeedsOpen = false;
				SocialManager.Instance.ReportAchievementAfterPlaying(categoryNum,unlocked[categoryNum]+1,nextNeedsOpen);

				CollectionCtrl.Instance.UpdateInfoOn(0);
				return 1;
			}
			CollectionCtrl.Instance.UpdateInfoOn(0);
			return 0;
		}
		CollectionCtrl.Instance.UpdateInfoOn(0);
		return 0;
	}

	//category - 1 - total score(누적점수).
	public int UpdateCategory1(int newScore){
		myNumbers[1] = SaveManager.GetTotalScore();
	

		if(SaveManager.GetCollectionStatesInCategory(1) ==0) SaveManager.SetCollectionStates(1,1);

		int newTotalScore = myNumbers[1] + newScore; // myNumbers에는 예전 점수가 저장되어있음.
		SaveManager.SetTotalScore(newTotalScore);

		return UpdateCategoryForAccumulated(1,newScore);
	}
	
	//category - 2. - max score(최고점수).
	public int UpdateCategory2(int newScore){
		myNumbers[2] = SaveManager.GetMaxScore();
		if(SaveManager.GetCollectionStatesInCategory(2) ==0) SaveManager.SetCollectionStates(2,1);

		if(newScore>myNumbers[2]){
			SaveManager.SetMaxScore(newScore);
		}
		return UpdateCategoryForMax(2,newScore);
	}

	//category - 3. - max change(최고 거스름돈).
	public int UpdateCategory3(int newChange){
		myNumbers[3] = SaveManager.GetMaxChange();
		if(SaveManager.GetCollectionStatesInCategory(3) ==0) SaveManager.SetCollectionStates(3,1);

		if (newChange > myNumbers [3]) {
			SaveManager.SetMaxChange (newChange);
		
		}
		return UpdateCategoryForMax (3, newChange);

	}

	//category - 4 - 초딩관련.
	public int UpdateCategory4(int good, int successiveGood, int perfect, int successivePerfect){

		int categoryNum = 4;
		missionStates[categoryNum] = SaveManager.GetCollectionStatesInCategory(categoryNum);
		unlocked[categoryNum] = SaveManager.GetCollectionUnlockedInCategory(categoryNum);
		myNumbers[categoryNum] = SaveManager.GetIgnorant();

		int newValue;

		if(missionStates[categoryNum]==1){//ongoing 상태.
			int missionType = -1;
			for(int i=0;i<conditionForCategory4.Length;i++){
				if(conditionForCategory4[unlocked[categoryNum]].numbers[i]==1){
					missionType = i;
					break;
				}
			}

			switch(missionType){
			case 0: // good.
				newValue = myNumbers[categoryNum] + good;
				SaveManager.SetIgnorant(newValue);
				return UpdateCategoryForAccumulated(categoryNum,good);
			case 1: // successive good.
				if(successiveGood == -1){  
					SaveManager.SetCollectionStates(categoryNum,2); //fail.
					GAManager.Instance.GAMissionFailedEvent(categoryNum.ToString()+"_"+unlocked[categoryNum].ToString());
				//	CollectionCtrl.Instance.ShowMissionFailed(categoryNum);
					return 0;
				}else{
					newValue = myNumbers[categoryNum] + successiveGood;
					SaveManager.SetIgnorant(newValue);
					return UpdateCategoryForAccumulated(categoryNum,successiveGood);
				}
			case 2: // perfect.
				newValue = myNumbers[categoryNum] + perfect;
				SaveManager.SetIgnorant(newValue);
				return UpdateCategoryForAccumulated(categoryNum,perfect);

			case 3: //successive perfect.
				if(successivePerfect == -1){
					SaveManager.SetCollectionStates(categoryNum,2);
					GAManager.Instance.GAMissionFailedEvent(categoryNum.ToString()+"_"+unlocked[categoryNum].ToString());
				//	CollectionCtrl.Instance.ShowMissionFailed(categoryNum);
					return 0;
				}else{
					newValue = myNumbers[categoryNum] + successivePerfect;
					SaveManager.SetIgnorant(newValue);
					return UpdateCategoryForAccumulated(categoryNum,successivePerfect);
				}
			}
			return 0;

		}else{
			return 0;
		}
	}
	//category - 5.
	public int UpdateCategory5(int good, int successiveGood, int perfect, int successivePerfect){
		int categoryNum = 5;
		missionStates[categoryNum] = SaveManager.GetCollectionStatesInCategory(categoryNum);
		unlocked[categoryNum] = SaveManager.GetCollectionUnlockedInCategory(categoryNum);
		myNumbers[categoryNum] = SaveManager.GetHomeless();
		
		int newValue;
		
		if(missionStates[categoryNum]==1){//ongoing 상태.
			int missionType = -1;
			for(int i=0;i<conditionForCategory5.Length;i++){
				if(conditionForCategory5[unlocked[categoryNum]].numbers[i]==1){
					missionType = i;
					break;
				}
			}
			
			switch(missionType){
			case 0: // good.
				newValue = myNumbers[categoryNum] + good;
				SaveManager.SetHomeless(newValue);
				return UpdateCategoryForAccumulated(categoryNum,good);
			case 1: // successive good.
				if(successiveGood == -1){  
					SaveManager.SetCollectionStates(categoryNum,2); //fail.
					GAManager.Instance.GAMissionFailedEvent(categoryNum.ToString()+"_"+unlocked[categoryNum].ToString());
				//	CollectionCtrl.Instance.ShowMissionFailed(categoryNum);
					return 0;
				}else{
					newValue = myNumbers[categoryNum] + successiveGood;
					SaveManager.SetHomeless(newValue);
					return UpdateCategoryForAccumulated(categoryNum,successiveGood);
				}
			case 2: // perfect.
				newValue = myNumbers[categoryNum] + perfect;
				SaveManager.SetHomeless(newValue);
				return UpdateCategoryForAccumulated(categoryNum,perfect);
				
			case 3: //successive perfect.
				if(successivePerfect == -1){
					SaveManager.SetCollectionStates(categoryNum,2);
					GAManager.Instance.GAMissionFailedEvent(categoryNum.ToString()+"_"+unlocked[categoryNum].ToString());
				//	CollectionCtrl.Instance.ShowMissionFailed(categoryNum);
					return 0;
				}else{
					newValue = myNumbers[categoryNum] + successivePerfect;
					SaveManager.SetHomeless(newValue);
					return UpdateCategoryForAccumulated(categoryNum,successivePerfect);
				}
			}
			return 0;
			
		}else{
			return 0;
		}
	}
	//category - 6.
	public int UpdateCategory6(int good, int successiveGood, int perfect, int successivePerfect){
		int categoryNum =6;
		missionStates[categoryNum] = SaveManager.GetCollectionStatesInCategory(categoryNum);
		unlocked[categoryNum] = SaveManager.GetCollectionUnlockedInCategory(categoryNum);
		myNumbers[categoryNum] = SaveManager.GetThief();
		
		int newValue;
		
		if(missionStates[categoryNum]==1){//ongoing 상태.
			int missionType = -1;
			for(int i=0;i<conditionForCategory6.Length;i++){
				if(conditionForCategory6[unlocked[categoryNum]].numbers[i]==1){
					missionType = i;
					break;
				}
			}
			
			switch(missionType){
			case 0: // good.
				newValue = myNumbers[categoryNum] + good;
				SaveManager.SetThief(newValue);
				return UpdateCategoryForAccumulated(categoryNum,good);
			case 1: // successive good.
				if(successiveGood == -1){  
					SaveManager.SetCollectionStates(categoryNum,2); //fail.
					GAManager.Instance.GAMissionFailedEvent(categoryNum.ToString()+"_"+unlocked[categoryNum].ToString());
				//	CollectionCtrl.Instance.ShowMissionFailed(categoryNum);
					return 0;
				}else{
					newValue = myNumbers[categoryNum] + successiveGood;
					SaveManager.SetThief(newValue);
					return UpdateCategoryForAccumulated(categoryNum,successiveGood);
				}
			case 2: // perfect.
				newValue = myNumbers[categoryNum] + perfect;
				SaveManager.SetThief(newValue);
				return UpdateCategoryForAccumulated(categoryNum,perfect);
				
			case 3: //successive perfect.
				if(successivePerfect == -1){
					SaveManager.SetCollectionStates(categoryNum,2);
					GAManager.Instance.GAMissionFailedEvent(categoryNum.ToString()+"_"+unlocked[categoryNum].ToString());
				//	CollectionCtrl.Instance.ShowMissionFailed(categoryNum);
					return 0;
				}else{
					newValue = myNumbers[categoryNum] + successivePerfect;
					SaveManager.SetThief(newValue);
					return UpdateCategoryForAccumulated(categoryNum,successivePerfect);
				}
			}
			return 0;
		}else{
			return 0;
		}
	}

	//category - 7.
	public int UpdateCategory7(int good, int successiveGood, int perfect, int successivePerfect){
		int categoryNum =7;
		missionStates[categoryNum] = SaveManager.GetCollectionStatesInCategory(categoryNum);
		unlocked[categoryNum] = SaveManager.GetCollectionUnlockedInCategory(categoryNum);
		myNumbers[categoryNum] = SaveManager.GetSkeptic();
		
		int newValue;
		
		if(missionStates[categoryNum]==1){//ongoing 상태.
			int missionType = -1;
			for(int i=0;i<conditionForCategory7.Length;i++){
				if(conditionForCategory7[unlocked[categoryNum]].numbers[i]==1){
					missionType = i;
					break;
				}
			}
			switch(missionType){
			case 0: // good.
				newValue = myNumbers[categoryNum] + good;
				SaveManager.SetSkeptic(newValue);
				return UpdateCategoryForAccumulated(categoryNum,good);
			case 1: // successive good.
				if(successiveGood == -1){  
					SaveManager.SetCollectionStates(categoryNum,2); //fail.
					GAManager.Instance.GAMissionFailedEvent(categoryNum.ToString()+"_"+unlocked[categoryNum].ToString());
				//	CollectionCtrl.Instance.ShowMissionFailed(categoryNum);
					return 0;
				}else{
					newValue = myNumbers[categoryNum] + successiveGood;
					SaveManager.SetSkeptic(newValue);
					return UpdateCategoryForAccumulated(categoryNum,successiveGood);
				}
			case 2: // perfect.
				newValue = myNumbers[categoryNum] + perfect;
				SaveManager.SetSkeptic(newValue);
				return UpdateCategoryForAccumulated(categoryNum,perfect);
				
			case 3: //successive perfect.
				if(successivePerfect == -1){
					SaveManager.SetCollectionStates(categoryNum,2);
					GAManager.Instance.GAMissionFailedEvent(categoryNum.ToString()+"_"+unlocked[categoryNum].ToString());
			//		CollectionCtrl.Instance.ShowMissionFailed(categoryNum);
					return 0;
				}else{
					newValue = myNumbers[categoryNum] + successivePerfect;
					SaveManager.SetSkeptic(newValue);
					return UpdateCategoryForAccumulated(categoryNum,successivePerfect);
				}
			}
			return 0;
			
		}else{
			return 0;
		}
	}

	//category - 8 - 대머리아저씨 시간관련미션.
	public int UpdateCategory8(int good){
		int categoryNum = 8;
		missionStates[categoryNum] = SaveManager.GetCollectionStatesInCategory(categoryNum);
		unlocked[categoryNum] = SaveManager.GetCollectionUnlockedInCategory(categoryNum);
		myNumbers[categoryNum] = SaveManager.GetBald();

		int newTotalValue = myNumbers[categoryNum] + good;

		if(missionStates[categoryNum] == 1){
			float accumulatedPlayTime = SaveManager.GetBaldPlayingTime();
			float timeSpanFromLastCheck = Time.time - baldMissionCheckTime;
			float newPlayTime = timeSpanFromLastCheck + accumulatedPlayTime; // 현재까지 진행한 미션타임.


			if(timeSpanFromLastCheck <0){ //시간이 바뀌거나 거꾸로 가게되면 미션 실패로.
				SaveManager.SetCollectionStates(categoryNum,2);
				return 0;
			}

			SaveManager.SetBald(newTotalValue);
		
			if(newPlayTime<conditionForCategory8[unlocked[categoryNum]]){  //아직 미션 타임 내.
				int tempCheck = UpdateCategoryForAccumulated(categoryNum,good);
				if(tempCheck==0){ // 아직 미션 중이나, 성공이 아닐때는, 시간관련 변수들 업데이트 필요.
					baldMissionCheckTime = Time.time;
					SaveManager.SetBaldPlayingTime(newPlayTime);
				}
				return tempCheck;
			}else{ //타임이 오버되면.

				SaveManager.SetCollectionStates(categoryNum,2);
				GAManager.Instance.GAMissionFailedEvent(categoryNum.ToString()+"_"+unlocked[categoryNum].ToString());
		//		CollectionCtrl.Instance.ShowMissionFailed(categoryNum);
				return 0;
			}

		}else{
			return 0;
		}
	}

	//category - 9 - 소녀.
	public int UpdateCategory9(int good, int successiveGood, int perfect, int successivePerfect){
		int categoryNum = 9;
		missionStates[categoryNum] = SaveManager.GetCollectionStatesInCategory(categoryNum);
		unlocked[categoryNum] = SaveManager.GetCollectionUnlockedInCategory(categoryNum);
		myNumbers[categoryNum] = SaveManager.GetGirl();
		
		int newValue;
		
		if(missionStates[categoryNum]==1){//ongoing 상태.
			int missionType = -1;
			for(int i=0;i<conditionForCategory9.Length;i++){
				if(conditionForCategory9[unlocked[categoryNum]].numbers[i]==1){
					missionType = i;
					break;
				}
			}
			switch(missionType){
			case 0: // good.
				newValue = myNumbers[categoryNum] + good;
				SaveManager.SetGirl(newValue);
				return UpdateCategoryForAccumulated(categoryNum,good);
			case 1: // successive good.
				if(successiveGood == -1){  
					SaveManager.SetCollectionStates(categoryNum,2); //fail.
					GAManager.Instance.GAMissionFailedEvent(categoryNum.ToString()+"_"+unlocked[categoryNum].ToString());
			//		CollectionCtrl.Instance.ShowMissionFailed(categoryNum);
					return 0;
				}else{
					newValue = myNumbers[categoryNum] + successiveGood;
					SaveManager.SetGirl(newValue);
					return UpdateCategoryForAccumulated(categoryNum,successiveGood);
				}
			case 2: // perfect.
				newValue = myNumbers[categoryNum] + perfect;
				SaveManager.SetGirl(newValue);
				return UpdateCategoryForAccumulated(categoryNum,perfect);
				
			case 3: //successive perfect.
				if(successivePerfect == -1){
					SaveManager.SetCollectionStates(categoryNum,2);
					GAManager.Instance.GAMissionFailedEvent(categoryNum.ToString()+"_"+unlocked[categoryNum].ToString());
			//		CollectionCtrl.Instance.ShowMissionFailed(categoryNum);
					return 0;
				}else{
					newValue = myNumbers[categoryNum] + successivePerfect;
					SaveManager.SetGirl(newValue);
					return UpdateCategoryForAccumulated(categoryNum,successivePerfect);
				}
			}
			return 0;
			
		}else{
			return 0;
		}
	}


	//category - 10 - 불만족고객수.
	public int UpdateCategory10(int dissatisfied){
		//dissatisfied - 1회 플레이에서 불만족 고객수.
		int playCount = SaveManager.GetDisPlayCount()+1;
		unlocked[10] = SaveManager.GetCollectionUnlockedInCategory(10);
		missionStates[10] = SaveManager.GetCollectionStatesInCategory(10);
		myNumbers[10] = SaveManager.GetTotalDissatisfied();

		if(missionStates[10]==1){ //미션이 진행중일때.(missionstates는 1일때 ongoing상태).
			int newTotalDis = myNumbers[10]+dissatisfied;
			if(newTotalDis > numbersToCompleteMission[10].numbers[unlocked[10]]){ // 불만족 고객이 특정수 이상 되었음-미션실패.
				//fail 상태이므로 out!
				SaveManager.SetDisPlayCount(0);
				SaveManager.SetCollectionStates(10,2); // 2번은 fail을 의미.
				GAManager.Instance.GAMissionFailedEvent("10_"+unlocked[10].ToString());
		//		CollectionCtrl.Instance.ShowMissionFailed(10);
				return 0;
			}else{
				if(playCount >= conditionForCategory10[unlocked[10]]){ // 미션성공!.

					GAManager.Instance.GAMissionSuccessEvent("10_"+unlocked[10].ToString());
					SaveManager.SetDisPlayCount(0);
					SaveManager.SetCollectionStates(10,3);

					bool nextNeedsOpen = true;
					if(unlocked[10] == (numbersToCompleteMission[10].numbers.Length -1)) nextNeedsOpen = false;
					SocialManager.Instance.ReportAchievementAfterPlaying(10,unlocked[10]+1,nextNeedsOpen);

			//		CollectionCtrl.Instance.ShowMissionSuccess(10);
					return 1;
				
				}else{ // 계속진행.
					SaveManager.SetDisPlayCount(playCount);
					SaveManager.SetTotalDissatisfied(newTotalDis);
					return 0;
				}
			}
		}
		return 0;
	}

	//category - 11. - total Receipt.
	public int UpdateCategory11(int newReceipt){
	
		myNumbers[11] = SaveManager.GetTotalReceipt();

		if(SaveManager.GetCollectionStatesInCategory(11) ==0) SaveManager.SetCollectionStates(11,1);

		int newTotalReceipt = myNumbers[11] + newReceipt; // myNumbers에는 예전 점수가 저장되어있음.
		SaveManager.SetTotalReceipt(newTotalReceipt);
		return UpdateCategoryForAccumulated(11,newReceipt);

	}

	//category - 12. - max Combo.
	public int UpdateCategory12(int newCombo){

		myNumbers[12] = SaveManager.GetMaxCombo();
		if(SaveManager.GetCollectionStatesInCategory(12) ==0) SaveManager.SetCollectionStates(12,1);

		if(newCombo>myNumbers[12]){
			SaveManager.SetMaxCombo(newCombo);
		}
		return UpdateCategoryForMax(12,newCombo);
	}

	//category - 13. - max FeverSuccess.
	public int UpdateCategory13(int newFeverSuccess){
		myNumbers[13] = SaveManager.GetMaxFeverSuccess();
		if(SaveManager.GetCollectionStatesInCategory(13) ==0) SaveManager.SetCollectionStates(13,1);

		if(newFeverSuccess>myNumbers[13]){
			SaveManager.SetMaxFeverSuccess(newFeverSuccess);
		}
		return UpdateCategoryForMax(13,newFeverSuccess);
	}

	public void ResetAndStartCategory(int categoryNum){
		SaveManager.SetCollectionStates(categoryNum,1); //ongoing 상태로.

		switch(categoryNum){
		case 4:
			SaveManager.SetIgnorant(0);
			break;
		case 5:
			SaveManager.SetHomeless(0);
			break;
		case 6:
			SaveManager.SetThief(0);
			break;
		case 7:
			SaveManager.SetSkeptic(0);
			break;
		case 8:
			SaveManager.SetBald(0);
			SaveManager.SetBaldPlayingTime(0f);
			SaveManager.SetBaldLastPlayedTime(DateTime.Now.ToString());
			baldMissionCheckTime = Time.time;
			break;
		case 9:
			SaveManager.SetGirl(0);
			break;
		case 10:
			SaveManager.SetTotalDissatisfied(0);
			SaveManager.SetDisPlayCount(0);
			break;
		}
	}

	public int GetMissionObjective(int whichCategory){
		unlocked[whichCategory] = SaveManager.GetCollectionUnlockedInCategory(whichCategory);
		if(unlocked[whichCategory] < numbersToCompleteMission[whichCategory].numbers.Length){
			return numbersToCompleteMission[whichCategory].numbers[unlocked[whichCategory]];
		}else{
			return -1;
		}
	}
	//success stamp 클릭시.
	public void UnlockAndChangeState(int whichcategory,MissionType MT){
		unlocked[whichcategory] = SaveManager.GetCollectionUnlockedInCategory(whichcategory);

		if(unlocked[whichcategory] >= numbersToCompleteMission[whichcategory].numbers.Length){//complete 상태 - 더이상 진행할 미션없음.
			SaveManager.SetCollectionStates(whichcategory,4);
			return;
		}else if(unlocked[whichcategory] == (numbersToCompleteMission[whichcategory].numbers.Length-1)){ //마지막 미션.
			GAManager.Instance.GAGetStuffEvent(whichcategory.ToString()+"_"+unlocked[whichcategory].ToString());
			SaveManager.SetCollectionUnlocked(whichcategory);
			SaveManager.SetCollectionStates(whichcategory,4);
		}else{
			GAManager.Instance.GAGetStuffEvent(whichcategory.ToString()+"_"+unlocked[whichcategory].ToString());
			SaveManager.SetCollectionUnlocked(whichcategory);
			if(MT == MissionType.Normal){
				SaveManager.SetCollectionStates(whichcategory,1);
			}else if(MT == MissionType.Conditioned){
				SaveManager.SetCollectionStates(whichcategory,0);
			}
		}

		switch(whichcategory){
		case 4:
			SaveManager.SetIgnorant(0);
			break;
		case 5:
			SaveManager.SetHomeless(0);
			break;
		case 6:
			SaveManager.SetThief(0);
			break;
		case 7:
			SaveManager.SetSkeptic(0);
			break;
		case 8:
			SaveManager.SetBald(0);
			break;
		case 9:
			SaveManager.SetGirl(0);
			break;
		case 10:
			SaveManager.SetTotalDissatisfied(0);
			break;
		}

		CollectionCtrl.Instance.UpdateNumbers(whichcategory);

		UpdateCategory0(); //먼저 선행이 되고, 난후, updateinfoon이 시간차를 두고 되어야 함.
		//CollectionCtrl.Instance.UpdateInfoOn(0); //다른 물품들이 unlock되면 그 즉시, update 한다. -> update 문 안으로 보내버림.
	}
	

	void OnApplicationFocus(bool focusState){
		if(!focusState){
			SaveAllData();

		}else{

		}
	}

	public void SaveAllData(){
		
		SaveManager.SaveTotalScore();
		SaveManager.SaveMaxScore();
		SaveManager.SaveMaxChange();
		SaveManager.SaveBald();
		
		SaveManager.SaveIgnorant();
		SaveManager.SaveHomeless();
		SaveManager.SaveThief();
		SaveManager.SaveSkeptic();
		SaveManager.SaveBald();
		SaveManager.SaveGirl();
		SaveManager.SaveTotalDissatisfied();
		SaveManager.SaveTotalReceipt();
		SaveManager.SaveMaxCombo();
		SaveManager.SaveMaxFeverSuccess();
		
		SaveManager.SaveDisPlayCount();
		
		missionStates[8] = SaveManager.GetCollectionStatesInCategory(8);
		if(missionStates[8] == 1){
			float baldTime = SaveManager.GetBaldPlayingTime() + (Time.time - baldMissionCheckTime);
			baldMissionCheckTime = Time.time;
			SaveManager.SetBaldPlayingTime(baldTime);
			SaveManager.SaveBaldPlayingTime();
			
			SaveManager.SetBaldLastPlayedTime(Convert.ToString(DateTime.Now));
			SaveManager.SaveBaldLastPlayedTime();
		}
		
		SaveManager.SaveCollectionInfo();
		GameManager.Instance.PauseGame(); // game중이면 게임 pause하기.

		/*
		if(GameManager.Instance.GS == GameState.Pause){
			GAManager.Instance.GAExitEvent("GameScene");
		}else{
			GAManager.Instance.GAExitEvent("MainScene");
		}
		*/
	}

}
