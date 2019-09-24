using UnityEngine;
using System.Collections;
using System;

public class GAManager : Singleton<GAManager> {
	public Analytics gaObj;

	public void StartAndStopSession(bool isStart){		
		gaObj.StartSession(isStart);
	}

	public void GASendView(string view){
	// view  : StartMainScene / GameScene / CollectionScene / ResultMainScene / PauseScene /
		gaObj.SendView(view);
	}

	public void GAMainBtnEvent(string label){
	//category, action, label, value.
		string category = "UI";
		string action = "BtnClick";
		gaObj.SendEvent(category, action, label, 1);
	}

	public void GAPlayGame()
	{
		string category = "Game";
		string action = "Start";
		string label = "start";
		gaObj.SendEvent(category, action, label, 1);	
	}
	
	public void GAEndGameScore(int value)
	{
		string category = "Game";
		string action = "End";
		string label = "score";
		gaObj.SendEvent(category, action, label, value);
	}

	public void GAEndGameCombo(int value)
	{
		string category = "Game";
		string action = "End";
		string label = "combo";
		gaObj.SendEvent(category, action, label, value);
	}

	public void GARateEvent(string label){
		string category = "Event";
		string action = "Rate";
		gaObj.SendEvent(category, action, label, 1);
	}

	public void GALevelUpEvent(int level){
		string category = "Event";
		string action = "LevelUp";
		gaObj.SendEvent(category, action, level.ToString(), 1);
	}

	public void GAPauseBtnEvent(string whichBtn){
		string category = "UI";
		string action = "BtnClick";
		gaObj.SendEvent(category, action, whichBtn, 1);
	}

	public void GAMissionSuccessEvent(string label){
		string category = "Mission";
		string action = "MissionSuccess";
		gaObj.SendEvent(category,action,label,1);
	}

	public void GAMissionFailedEvent(string label){
		string category = "Mission";
		string action = "MissionFailed";
		gaObj.SendEvent(category,action,label,1);
	}

	public void GAGetStuffEvent(string label){
		string category = "Mission";
		string action = "GetStuff";
		gaObj.SendEvent(category,action,label,1);
	}

	public void GAMissionLabelBtnEvent(int whichOne){
		string category = "UI";
		string action = "MissionShowClick";
		string label = whichOne.ToString();
		gaObj.SendEvent (category,action,label,1);
	}

	public void GAMissionTryBtnEvent(int whichOne){
		string category = "UI";
		string action = "MissionTryClick";
		string label = whichOne.ToString();
		gaObj.SendEvent (category,action,label,1);
	}

	public void GAMissionRetryBtnEvent(int whichOne){
		string category = "UI";
		string action = "MissionRetryClick";
		string label = whichOne.ToString();
		gaObj.SendEvent (category,action,label,1);
	}

	public void GAGPLogInEvent(bool isSuccess){
		string category = "GooglePlay";
		string action = "LogIn";
		string label = "";
		if(isSuccess){
			label = "Success";
		}else{
			label = "Fail";
		}
		gaObj.SendEvent(category,action,label,1);
	}

	public void GAGPAchievementEvent(string label){
		string category = "GooglePlay";
		string action = "GetAchievement";
		gaObj.SendEvent(category,action,label,1);
	}

	public void GAGPUpdateRanking(bool isSuccess){
		string category = "GooglePlay";
		string action = "UpdateScore";
		string label = "";
		if(isSuccess){
			label = "Success";
		}else{
			label = "Fail";
		}
		gaObj.SendEvent(category,action,label,1);
	}

	public void GAPurchaseRemoveAds(bool isPurchaseSuccess, bool isCancelled){
		string category = "Purchase";
		string action = "RemoveAds";
		string label = "";
		if(isPurchaseSuccess){
			if(!isCancelled){
				label = "Success";
			}else{
				label = "Restored";
			}

		}else{ // fail or cancel.
			if(isCancelled){
				label = "Cancelled";
			}else{
				label = "Failed";
			}
		}
		gaObj.SendEvent(category,action,label,1);
	}

	public void GAChartBoostEvent(string label){
		string category = "Ads";
		string action = "ChartBoost";
		gaObj.SendEvent(category, action, label, 1); 
		//display, click, close.
	}

	public void GAAdMobEvent(string label){
		string category = "Ads";
		string action = "Admob";
		gaObj.SendEvent(category,action,label,1);
	}

	public void GAInplayEvent(string label){
		string category = "Ads";
		string action = "Inplay";
		gaObj.SendEvent(category,action,label,1);
	}

	public void GAUnityadsEvent(string label){
		string category = "Ads";
		string action = "Unity";
		gaObj.SendEvent(category,action,label,1);
	}

	public void GANewdayEvent()
	{
		gaObj.SendCustomMetric();
	}
}
