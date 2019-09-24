using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

#if UNITY_ANDROID
using GooglePlayGames;
#endif


[System.Serializable]
public class MyStrings{
	public string[] mystrings;
}

public class SocialManager: Singleton<SocialManager> {
	
	public string leaderBoardIDonG;
	public string leaderBoardIDoniOS;

	public MyStrings[] achieveIDonG;
	public MyStrings[] achieveIDoniOS;

#if UNITY_ANDROID
	void Start(){
		PlayGamesPlatform.Activate();
		PlayGamesPlatform.DebugLogEnabled = true;
		LogInGameCenter();
	}

	public void LogInGameCenter(){
		Social.localUser.Authenticate((bool success) =>{
			Social.ReportScore (SaveManager.GetMaxScore(),leaderBoardIDonG,(bool isSuccess) =>{
			});
			GAManager.Instance.GAGPLogInEvent(success);
		});
	}

	public void ReportScoreAfterPlaying(int score){
		if(!Social.localUser.authenticated){
			Social.localUser.Authenticate((bool success) =>{
				GAManager.Instance.GAGPLogInEvent(success);
				if(success){
					Social.ReportScore(score, leaderBoardIDonG,(bool successR) =>{
						GAManager.Instance.GAGPUpdateRanking(successR);
					});
				}
			});
		}else{
			Social.ReportScore(score, leaderBoardIDonG,(bool successR) =>{ 
				GAManager.Instance.GAGPUpdateRanking(successR);
			});
		}
	}

	public void ReportAchievementAfterPlaying(int category, int unlocked,bool revealNext){//몇번째 카테고리, 몇번째 아이템이 열렸는지, 그다음 미션을 reveal 하는지(마지막 미션 unlock시 false).
		if(!Social.localUser.authenticated){
			Social.localUser.Authenticate((bool success) =>{
				GAManager.Instance.GAGPLogInEvent(success);
				if(success){
					Social.ReportProgress (achieveIDonG[category].mystrings[unlocked-1],100.0f,(bool achieveSuccess) =>{
						if(achieveSuccess){
							GAManager.Instance.GAGPAchievementEvent(category.ToString()+"_"+(unlocked-1).ToString());
						}
					});
					if(revealNext) Social.ReportProgress(achieveIDonG[category].mystrings[unlocked],0.0f,(bool revealSuccess)=>{	});
				}
			});

		}else{
			Social.ReportProgress (achieveIDonG[category].mystrings[unlocked-1],100.0f,(bool achieveSuccess) =>{
				if(achieveSuccess){
					GAManager.Instance.GAGPAchievementEvent(category.ToString()+"_"+(unlocked-1).ToString());
				}
			});
			if(revealNext) Social.ReportProgress(achieveIDonG[category].mystrings[unlocked],0.0f,(bool revealSuccess)=>{	});
		}
	}

	public void ShowLeaderBoard(){
		GAManager.Instance.GAMainBtnEvent("RankingClick");
		SoundManager.PlaySFX("button_click");
		if(Social.localUser.authenticated){
			((PlayGamesPlatform) Social.Active).ShowLeaderboardUI(leaderBoardIDonG);
		}else{
			Social.localUser.Authenticate((bool success) =>{
				GAManager.Instance.GAGPLogInEvent(success);
				if(success){
					((PlayGamesPlatform) Social.Active).ShowLeaderboardUI(leaderBoardIDonG);
				}
			});
		}
	}


	#endif

#if UNITY_IPHONE
	void Start(){
		LogInGameCenter();
	}

	
	public void LogInGameCenter(){
		Debug.Log ("Log in Game");
		Social.localUser.Authenticate((bool success) =>{

			Social.ReportScore(SaveManager.GetMaxScore(), leaderBoardIDoniOS,(bool successR) =>{ 
			});
			GAManager.Instance.GAGPLogInEvent(success);
		});
	}
	
	public void ReportScoreAfterPlaying(int score){
		if(!Social.localUser.authenticated){
			Social.localUser.Authenticate((bool success) =>{
				GAManager.Instance.GAGPLogInEvent(success);
				if(success){
					Social.ReportScore(score, leaderBoardIDoniOS,(bool successR) =>{ 
						GAManager.Instance.GAGPUpdateRanking(successR);
					});
				}
			});
		}else{
			Social.ReportScore(score, leaderBoardIDoniOS,(bool successR) =>{ 
				GAManager.Instance.GAGPUpdateRanking(successR);
			});
		}
	}

	public void ReportAchievementAfterPlaying(int category, int unlocked,bool revealNext){//몇번째 카테고리, 몇번째 아이템이 열렸는지, 그다음 미션을 reveal 하는지(마지막 미션 unlock시 false).
		if(!Social.localUser.authenticated){
			Social.localUser.Authenticate((bool success) =>{
				GAManager.Instance.GAGPLogInEvent(success);
				if(success){
					Social.ReportProgress (achieveIDoniOS[category].mystrings[unlocked-1],100.0f,(bool achieveSuccess) =>{
						if(achieveSuccess){
							GAManager.Instance.GAGPAchievementEvent(category.ToString()+"_"+(unlocked-1).ToString());
						}
					});
					if(revealNext) Social.ReportProgress(achieveIDoniOS[category].mystrings[unlocked],0.0f,(bool revealSuccess)=>{	});
				}
			});
			
		}else{
			Social.ReportProgress (achieveIDoniOS[category].mystrings[unlocked-1],100.0f,(bool achieveSuccess) =>{
				if(achieveSuccess){
					GAManager.Instance.GAGPAchievementEvent(category.ToString()+"_"+(unlocked-1).ToString());
				}
			});
			if(revealNext) Social.ReportProgress(achieveIDoniOS[category].mystrings[unlocked],0.0f,(bool revealSuccess)=>{	});
		}
	}

	public void ShowLeaderBoard(){
		GAManager.Instance.GAMainBtnEvent("RankingClick");
		if(Social.localUser.authenticated){
			Social.ShowLeaderboardUI();
		}else{
			Social.localUser.Authenticate((bool success) =>{
				GAManager.Instance.GAGPLogInEvent(success);
				if(success){
					Social.ShowLeaderboardUI();
				}
			});
		}
	}

	#endif
}
