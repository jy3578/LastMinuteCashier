using System;
using UnityEngine;
using System.Collections;
using CodeStage.AntiCheat.ObscuredTypes;

public class SaveManager : MonoBehaviour {
	private static String mLanguage;

	private static ObscuredBool mRemoveAds;
	private static ObscuredBool mStartTut;
	private static ObscuredBool mGameTut;
	private static ObscuredBool mMissionTut;
	private static ObscuredBool mFeverTut;

	private static ObscuredInt mPlayerExp;
	private static ObscuredInt mMaxScore;
	private static ObscuredInt mTotalScore;
	private static ObscuredInt mTotalPlay;
	private static ObscuredInt mMaxCombo;
	private static ObscuredInt mTotalReceipt;
	private static ObscuredInt mMaxChange;
	private static ObscuredInt mMaxFeverSuccess;

	private static ObscuredInt mTotalDissatisfied;
	private static ObscuredInt mDisPlayCount;

	private static ObscuredInt[] mColUnlocked = new ObscuredInt[14]; // collection.
	private static ObscuredInt[] mColStates = new ObscuredInt[14];

	private static ObscuredInt mIgnorant;
	private static ObscuredInt mHomeless;
	private static ObscuredInt mThief;
	private static ObscuredInt mSkeptic;
	private static ObscuredInt mGirl;

	private static ObscuredInt mBald;
	private static ObscuredString mBaldLastPlayedTime;
	private static ObscuredFloat mBaldPlayingTime;

	private static ObscuredBool mSound;

	void Awake(){
	//	/*
		mRemoveAds = Saved.GetBool(SaveKey.RemoveAds);
		mStartTut = Saved.GetBool(SaveKey.StartTutorial);
		mGameTut = Saved.GetBool(SaveKey.GameTutorial);
		mMissionTut = Saved.GetBool(SaveKey.MissionTutorial);
		mFeverTut = Saved.GetBool (SaveKey.FeverTutorial);


		mPlayerExp = Saved.GetInt(SaveKey.PlayerExp);
		mMaxScore = Saved.GetInt(SaveKey.MaxScore);
		mTotalScore = Saved.GetInt(SaveKey.TotalScore);
		mTotalPlay = Saved.GetInt(SaveKey.TotalPlay);

		mMaxCombo = Saved.GetInt(SaveKey.MaxCombo);
		mTotalReceipt = Saved.GetInt(SaveKey.TotalReceipt);
		mMaxChange = Saved.GetInt(SaveKey.MaxChange);
		mMaxFeverSuccess = Saved.GetInt(SaveKey.MaxFeverSuccess);
		mSound = Saved.GetBool (SaveKey.Sound);
		mDisPlayCount = Saved.GetInt(SaveKey.DisPlayCount);
		mTotalDissatisfied = Saved.GetInt (SaveKey.TotalDissatisfied);
		mGirl = Saved.GetInt(SaveKey.GirlMission);

		mBald = Saved.GetInt(SaveKey.BaldMission);
		mBaldLastPlayedTime = Saved.GetString(SaveKey.BaldLastPlayedTime);
		mBaldPlayingTime = Saved.GetFloat(SaveKey.BaldPlayingTime);

		GetCollectionInfo();

		mIgnorant = Saved.GetInt (SaveKey.IgnorantMission);
		mHomeless = Saved.GetInt (SaveKey.HomelessMission);
		mThief = Saved.GetInt (SaveKey.ThiefMission);
		mSkeptic = Saved.GetInt (SaveKey.SkepticMission);
//	*/
	//	ObscuredPrefs.DeleteAll();
	

	
	}

	public static string GetLanguage(){
		return mLanguage;
	}
	public static void SetLanguage(string language){
		mLanguage = language;
	}
	public static ObscuredBool GetRemoveAdsPurchased(){
		return mRemoveAds;
	}
	public static void SetRemoveAdsPurchased(ObscuredBool purchased){
		mRemoveAds = purchased;
		Saved.SetBool(SaveKey.RemoveAds,mRemoveAds);
	}

	public static ObscuredBool GetStartTutorial(){
		return mStartTut;
	}

	public static ObscuredBool GetGameTutorial(){
		return mGameTut;
	}

	public static ObscuredBool GetMissionTutorial(){
		return mMissionTut;
	}

	public static ObscuredBool GetFeverTutorial(){
		return mFeverTut;
	}

	public static void SetMissionTutorial(ObscuredBool isDone){
		mMissionTut = isDone;
		Saved.SetBool(SaveKey.MissionTutorial,mGameTut);
	}
	

	public static void SetTutorial(int type,ObscuredBool isDone){
		switch (type) {
		case 1:
			mStartTut = isDone;
			Saved.SetBool(SaveKey.StartTutorial,mStartTut);
			break;
		case 2:
			mGameTut = isDone;
			Saved.SetBool(SaveKey.GameTutorial,mGameTut);
			break;
		case 3:
			mFeverTut = isDone;
			Saved.SetBool(SaveKey.FeverTutorial,mFeverTut);
			break;
		case 4: 
			mMissionTut = isDone;
			Saved.SetBool(SaveKey.MissionTutorial,mMissionTut);
			break;
		}
	}



	public static ObscuredInt GetTotalUnlocked(){
		ObscuredInt total = 0;
		for(int i=0;i<mColUnlocked.Length;i++){
			total += mColUnlocked[i];
		}
		return total;
	}

	private static void GetCollectionInfo(){
		String unlocked = Saved.GetString(SaveKey.CollectionUnlocked);
		for(int i=0;i<unlocked.Length;i++){
			mColUnlocked[i] = int.Parse(unlocked[i].ToString());
		}

		String states = Saved.GetString(SaveKey.CollectionStates);
		for(int i=0;i<states.Length;i++){
			mColStates[i] = int.Parse(states[i].ToString()); //0이면 미션완료하였으나 "입고"버튼 누르기전. 1이면 미션완료후 "성공버튼"클릭완료 & 다음 미션 중.
		}
	}
	public static ObscuredInt GetCollectionUnlockedInCategory(int whichCategory){
		return mColUnlocked[whichCategory];
	}
	public static ObscuredInt GetCollectionStatesInCategory(int whichCategory){
		return mColStates[whichCategory];
	}


	public static void SetCollectionUnlocked(int whichCategory){
		mColUnlocked[whichCategory]++;
	//	mColStates[whichCategory] = 0; //입고버튼 누르기전.
	}
	public static void SetCollectionStates(int whichCategory,int states){
		mColStates[whichCategory] = states;
	}

	public static void SaveCollectionInfo(){
		ObscuredString unlocked="";
		for(int i=0;i<mColUnlocked.Length;i++){
			unlocked = unlocked + mColUnlocked[i].ToString();
		}
		Saved.SetString(SaveKey.CollectionUnlocked,unlocked);

		ObscuredString states="";
		for(int i=0;i<mColStates.Length;i++){
			states = states + mColStates[i].ToString();
		}
		Saved.SetString(SaveKey.CollectionStates,states);
	}

	public static ObscuredInt GetIgnorant(){
		return mIgnorant;
	}

	public static void SetIgnorant(int value){
		mIgnorant = value;
	}
	public static void SaveIgnorant(){
		Saved.SetInt(SaveKey.IgnorantMission,mIgnorant);
	}

	public static ObscuredInt GetHomeless(){
		return mHomeless;
	}
	public static void SetHomeless(int value){
		mHomeless = value;
	}
	public static void SaveHomeless(){
		Saved.SetInt(SaveKey.HomelessMission,mHomeless);
	}

	public static ObscuredInt GetThief(){
		return mThief;
	}
	public static void SetThief(int value){
		mThief = value;
	}
	public static void SaveThief(){
		Saved.SetInt(SaveKey.ThiefMission,mThief);
	}

	public static ObscuredInt GetSkeptic(){
		return mSkeptic;
	}
	public static void SetSkeptic(int value){
		mSkeptic = value;
	}
	public static void SaveSkeptic(){
		Saved.SetInt(SaveKey.SkepticMission,mSkeptic);
	}

	public static ObscuredInt GetBald(){
		return mBald;
	}
	public static void SetBald(int value){
		mBald = value;
	}
	public static void SaveBald(){
		Saved.SetInt(SaveKey.BaldMission,mBald);
	}

	public static ObscuredInt GetGirl(){
		return mGirl;
	}
	public static void SetGirl(int value){
		mGirl = value;
	}
	public static void SaveGirl(){
		Saved.SetInt(SaveKey.GirlMission,mGirl);
	}
	
	public static ObscuredInt GetPlayerLevel(){

		if(mPlayerExp < 10) return 1;
		for(int i=2 ; i<24 ; i++){
			if(mPlayerExp < Math.Pow ((double) i, 2) + 9* i && mPlayerExp >= Math.Pow((double) i-1,2) + 9*(i-1)){
				return i;
			}
		}
		return 24;
	}

	public static float GetPlayerLevelExp(){//해당 level에서의 exp.
		int level = GetPlayerLevel();
		float levelExp = (mPlayerExp - Mathf.Pow (level - 1f, 2f) - 9f * (level - 1f)) / (level * 2f + 8f);
		if (levelExp >= 1f) return 1.00f;

		return levelExp;
	}

	public static ObscuredInt GetPlayerExp(){ //total exp.
		return mPlayerExp;
	}

	public static void SetPlayerExp(ObscuredInt exp){
		mPlayerExp += exp;
	}

	public static void SavePlayerExp(){
		Saved.SetInt(SaveKey.PlayerExp, mPlayerExp);
	}
	

	public static ObscuredInt GetMaxScore(){
		return mMaxScore;
	}
	public static void SetMaxScore(ObscuredInt maxScore){
		mMaxScore = maxScore;
	}
	public static void SaveMaxScore(){
		Saved.SetInt(SaveKey.MaxScore, mMaxScore);
	}

	public static ObscuredInt GetTotalScore(){
		return mTotalScore;
	}

	public static ObscuredInt GetTotalPlay(){
		return mTotalPlay;
	}

	public static void SetTotalScore(int Score){
		mTotalScore = Score;
	}
	
	public static void SetTotalPlay(int count){
		mTotalPlay = count;
	}

	public static void SaveTotalScore(){
		Saved.SetInt(SaveKey.TotalScore, mTotalScore);
	}

	public static void SaveTotalPlay(){
		Saved.SetInt(SaveKey.TotalPlay, mTotalPlay);
	}

	public static ObscuredInt GetMaxCombo(){
		return mMaxCombo;
	}
	public static void SetMaxCombo(ObscuredInt maxCombo){
		if(mMaxCombo < maxCombo) { mMaxCombo = maxCombo; }
	}
	public static void SaveMaxCombo(){
		Saved.SetInt(SaveKey.MaxCombo, mMaxCombo);
	}
	

	public static ObscuredInt GetTotalReceipt(){
		return mTotalReceipt;
	}
	
	public static void SetTotalReceipt(ObscuredInt numOfreceipt){
		mTotalReceipt = numOfreceipt;
	}
	
	public static void SaveTotalReceipt(){
		Saved.SetInt(SaveKey.TotalReceipt,mTotalReceipt);
	}

	public static ObscuredInt GetMaxChange(){
		return mMaxChange;
	}
	public static void SetMaxChange(ObscuredInt maxChange){
		if(mMaxChange < maxChange) { mMaxChange = maxChange; }
	}
	public static void SaveMaxChange(){
		Saved.SetInt(SaveKey.MaxChange, mMaxChange);
	}

	public static ObscuredInt GetMaxFeverSuccess(){
		return mMaxFeverSuccess;
	}
	public static void SetMaxFeverSuccess(ObscuredInt maxFeverSuccess){
		if(mMaxFeverSuccess < maxFeverSuccess){	mMaxFeverSuccess = maxFeverSuccess; }
	}
	public static void SaveMaxFeverSuccess(){
		Saved.SetInt(SaveKey.MaxFeverSuccess, mMaxFeverSuccess);
	}

	public static bool GetSound(){
		return mSound;
	}
	public static void SetSound(bool soundOn){
		mSound = soundOn;
		SaveSound();
	}
	public static void SaveSound(){
		Saved.SetBool(SaveKey.Sound,mSound);
	}

	public static ObscuredInt GetDisPlayCount(){
		return mDisPlayCount;
	}
	
	public static void SetDisPlayCount(ObscuredInt playCount){
		mDisPlayCount = playCount;
	}
	
	public static void SaveDisPlayCount(){
		Saved.SetInt(SaveKey.DisPlayCount,mDisPlayCount);
	}


	public static ObscuredInt GetTotalDissatisfied(){
		return mTotalDissatisfied;
	}
	
	public static void SetTotalDissatisfied(ObscuredInt dissatisfied){
		mTotalDissatisfied = dissatisfied;
	}
	
	public static void SaveTotalDissatisfied(){
		Saved.SetInt(SaveKey.TotalDissatisfied,mTotalDissatisfied);
	}

	public static ObscuredString GetBaldLastPlayedTime(){
		return mBaldLastPlayedTime;
	}
	public static void SetBaldLastPlayedTime(string value){
		mBaldLastPlayedTime = value;
	}
	public static void SaveBaldLastPlayedTime(){
		Saved.SetString(SaveKey.BaldLastPlayedTime,mBaldLastPlayedTime);
	}

	public static ObscuredFloat GetBaldPlayingTime(){
		return mBaldPlayingTime;
	}
	
	public static void SetBaldPlayingTime(ObscuredFloat playCount){
		mBaldPlayingTime = playCount;
	}
	
	public static void SaveBaldPlayingTime(){
		Saved.SetFloat(SaveKey.BaldPlayingTime,mBaldPlayingTime);
	}

	public static ObscuredInt GetMyNumbers(int whichCategory){
		switch(whichCategory){
		case 0: return GetTotalUnlocked();
		case 1: return mTotalScore;
		case 2: return mMaxScore;
		case 3: return mMaxChange;
		case 4: return mIgnorant;
		case 5: return mHomeless;
		case 6: return mThief;
		case 7: return mSkeptic;
		case 8: return mBald;
		case 9: return mGirl;
		case 10: return mTotalDissatisfied;
		case 11: return mTotalReceipt;
		case 12: return mMaxCombo;
		case 13: return mMaxFeverSuccess;
		}

		return -1;
	}
}
