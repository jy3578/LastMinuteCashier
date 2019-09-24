using UnityEngine;
using System.Collections;

public class ResultOnShutter : Singleton<ResultOnShutter> {

	public GameObject continuePopup;

	public PlayMakerFSM closingFSM;

	public NewIntAnimation scoreLb;
	public NewIntAnimation bestScoreLb;
	public UILabel[] resultInfos; // max score, max combo, number of customers, exp.
	
	public NewIntAnimation newScoreLb; // scoreLb on the brown BG(new Record BG).
	public AnimatedColor newRecord;

	public GameObject getStuffMsg;
	public UILabel getStuffLb;


	public AnimatedColor bonus;
	public UILabel bonusRatioLb;
	public AnimatedColor whCover;

	public AnimatedColor startMsg;
	// for localization.
	public UISprite receiptSprite;
	public UISprite stuffMsg;
	public UILabel currencyMark;

	private int mNewScore;
	private int mBonusScore;
	private int mTypeOfSuccess;

	private int achievements;


	//animated color - bonus / whCover / 



	public void WhetherToShowResult(bool doesShow){
		// if doesShow == true -> show result panel. if not hide result panel.
		if (doesShow) {
						if (!gameObject.activeSelf) {
								gameObject.SetActive (true);
								continuePopup.SetActive (false);
						}
		} else {
			MainReceipt.Instance.StartBtnIdle();
			gameObject.SetActive (false);
		}
	}


	public void ShowContinuePopup(){
			continuePopup.SetActive (true);		//		 closing 애메이션 나온 후 등장.
			if(!gameObject.activeSelf) gameObject.SetActive(true);
			
	}
				

	public void WriteResultOnShutter(int newScore, int maxCombo, int maxChange, int feverSuccess){

		//enter all the result information. at first.

		mNewScore = newScore;

		int bonusRatio = SaveManager.GetPlayerLevel() - 1;
		mBonusScore = (int) ( (float) newScore * ( 0.01f * (float)bonusRatio )  );
	
		int bestScore = SaveManager.GetMaxScore ();
		int customers = CustomerQueue.Instance.GetTotalSuccessCustomer ();

		mTypeOfSuccess = WhichTypeOfNewRecord();		// 0: new record x, 1: new Record, 2: new Record after adding bonusScore.
		
		int totalPlayCount = SaveManager.GetTotalPlay();
		totalPlayCount++;
		SaveManager.SetTotalPlay(totalPlayCount);

		int rewardExp = 2;
		if (mTypeOfSuccess != 0)	rewardExp += 2;


		scoreLb.Play (newScore, newScore);
		newScoreLb.Play(newScore, newScore);
		bestScoreLb.Play (bestScore, bestScore);

		resultInfos [0].text = maxCombo.ToString();
		resultInfos [1].text = customers.ToString();
		resultInfos [2].text = rewardExp.ToString ();
		bonusRatioLb.text = bonusRatio.ToString ();

		//set up the animation.

		Color visible = new Color (1f, 1f, 1f, 1f);
		Color invisible = new Color (1f, 1f, 1f, 0f);

		whCover.color = invisible;
		bonus.color = invisible;
		newRecord.color = invisible;
		startMsg.color = invisible;

		if (bonusRatio > 0) {
			bonus.color = visible; // if bonus ratio =0, then it hides.
			bonus.gameObject.transform.localScale = new Vector3(1f,0f,0f);
		}
		   
		if (mTypeOfSuccess == 0) {  // not achieve best score.
			newRecord.color = invisible;
		} else if (mTypeOfSuccess == 1) {   //new best score (totally best score).
			newRecord.color = visible;
		} else {  // ==2 new best score after adding the bonus score.
			newRecord.color = invisible;
		}

		//update info.
		UpdateGameResult (rewardExp , newScore+mBonusScore, maxCombo , maxChange , feverSuccess );

		if (achievements == 0) {
			getStuffMsg.SetActive (false);
		} else {
			getStuffMsg.SetActive(true);
			getStuffLb.text = achievements.ToString();
		}

		MainReceipt.Instance.ShowGameResult (mNewScore + mBonusScore, bestScore, maxCombo, customers, rewardExp, achievements);
		SceneManager.Instance.NewOnCollectionAtEndOfGame(achievements);
		//start animation.
		GAManager.Instance.GAEndGameScore(mNewScore);
		GAManager.Instance.GAEndGameCombo(maxCombo);
	}



	public void ShowResultAni(){  // to be called after shutter would be closed.
	
		
		if (mTypeOfSuccess == 0) {
			GetComponent<Animation>().Play ("ShowResult_type0");

		} else if (mTypeOfSuccess == 1) {
			GetComponent<Animation>().Play ("ShowResult_type0");

		} else {
			GetComponent<Animation>().Play ("ShowResult_type2");

		}

	}

	public void IncreaseScoreToTotal(){ //call from animation.

		if (mTypeOfSuccess == 0) {
			scoreLb.Play (mNewScore + mBonusScore);
		} else {
			newScoreLb.Play (mNewScore + mBonusScore);
		} 
	}

	public void ClickToReturn(){
		closingFSM.SendEvent ("ReturnNow");

	}

	private int WhichTypeOfNewRecord(){
		int prevBest = SaveManager.GetMaxScore();

		if(prevBest < mNewScore){
			return 1;
		}else if(prevBest<(mNewScore+mBonusScore) && prevBest >= mNewScore){
			return 2;
		}else{
			return 0;
		}

	}

	private void UpdateGameResult( int rewardExp,int newTotalScore, int maxCombo, int maxChange, int feverCustomerCount){
		if(mTypeOfSuccess != 0)	SocialManager.Instance.ReportScoreAfterPlaying(newTotalScore);
		
		SaveManager.SetPlayerExp(rewardExp);
		SaveManager.SavePlayerExp();

		achievements = 0;
		
		achievements += MissionManager.Instance.UpdateCategory1 (newTotalScore);
		achievements += MissionManager.Instance.UpdateCategory2 (newTotalScore);
		achievements += MissionManager.Instance.UpdateCategory3 (maxChange);
		
		int [,] numbS = new int[6,4];
		numbS = CustomerQueue.Instance.GetNumberOfSpecific();
		
		achievements += MissionManager.Instance.UpdateCategory4 (numbS[2,0],numbS[2,1],numbS[2,2],numbS[2,3]); // ignorant.
		achievements += MissionManager.Instance.UpdateCategory5 (numbS[3,0],numbS[3,1],numbS[3,2],numbS[3,3]); // homeless.
		achievements += MissionManager.Instance.UpdateCategory6 (numbS[4,0],numbS[4,1],numbS[4,2],numbS[4,3]); // thief.
		achievements += MissionManager.Instance.UpdateCategory7 (numbS[5,0],numbS[5,1],numbS[5,2],numbS[5,3]); //skeptic.
		
		achievements += MissionManager.Instance.UpdateCategory8 (numbS[1,0]); //bald.
		achievements += MissionManager.Instance.UpdateCategory9 (numbS[0,0],numbS[0,1],numbS[0,2],numbS[0,3]); //girl.
		achievements += MissionManager.Instance.UpdateCategory10 (CustomerQueue.Instance.GetNumberOfFailed());
		
		achievements += MissionManager.Instance.UpdateCategory11(CustomerQueue.Instance.GetNumberOfReceipt()); //total receipt.
		achievements += MissionManager.Instance.UpdateCategory12 (maxCombo);
		achievements += MissionManager.Instance.UpdateCategory13 (feverCustomerCount);

		MissionManager.Instance.SaveAllData();
	}

	public void SetLocalizeReceipt(){
		if (SaveManager.GetLanguage () == "Korean") {
			receiptSprite.spriteName = "main_ui_result";
			stuffMsg.spriteName = "main_msg_newItem";
			currencyMark.text = @"\";
		} else if (SaveManager.GetLanguage () == "English") {
			receiptSprite.spriteName = "main_ui_result_en";
			stuffMsg.spriteName = "main_msg_newItem_en";
		currencyMark.text = "$";
		}
		stuffMsg.MakePixelPerfect();
		stuffMsg.gameObject.transform.localScale = new Vector3(1.6875f,1.6875f,1f);

	
	}




}
