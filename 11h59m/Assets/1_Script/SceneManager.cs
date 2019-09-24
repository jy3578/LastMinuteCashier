using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneManager : Singleton<SceneManager> {
	

	public Transform mainSpawnPoint;

	public RatePopup ratePopup;

	public GameObject toastMsg;

	public PlayMakerFSM playMaker;

	public Transform positOfCollection;
	public GameObject exitBtnFromCollection;
	public BoxCollider2D subBtnToCollection;
	
	public UISprite soundIcon;
	public UISprite newMarkOnBtnCol;
	public UILabel numOfUnlocked;
	public Animation collectionBtnAnim;

	//bgm.
	public AudioClip mainBgm;
	public AudioClip playBgm;

	//camera.
	public Camera uiCamera;
	public tk2dCamera tkCamera;
	public Camera bgCamera;

	private bool isInCollection;
	private bool isInMain; // true = can click the ESC button on Android.
	private bool onToastMsg;

	//At start of Scene : charInfo ani, signPanel ani, startBtn ani. need to play.
	void Start(){
		GUI.enabled = false;
		SetLocalLanguage();
		ResetPlayerPref();

		if(SaveManager.GetSound()){ SoundManager.Play (playBgm,true);}

		isInMain = true;
		onToastMsg = false;

		NewOnCollectionAtStart();

		SetSound();
		GAManager.Instance.GASendView("StartMainScene");
		isInCollection = false;
		TutorialManager.Instance.ShowStartTutorial ();
	}

	private void SetLocalLanguage(){
		SystemLanguage language = Application.systemLanguage;
		if(language == SystemLanguage.Korean){
			Localization.language = "Korean";
			SaveManager.SetLanguage("Korean");
			Wallet.Instance.SetLocalMoney("Korean");
			Tray.Instance.SetLocalMoney("Korean");
			ResultOnShutter.Instance.SetLocalizeReceipt();
			Receipt.Instance.ReceiptLocalize();
		}else{
			Localization.language = "English";
			SaveManager.SetLanguage("English");
			Wallet.Instance.SetLocalMoney("English");
			Tray.Instance.SetLocalMoney("English");
			ResultOnShutter.Instance.SetLocalizeReceipt();
			Receipt.Instance.ReceiptLocalize();
		}
		
	}
#if UNITY_ANDROID

	void Update(){
				if (isInMain && !onToastMsg) {
						if (Input.GetKeyDown (KeyCode.Escape)) {
								GameObject toast = (GameObject)Instantiate (toastMsg, Vector3.zero, Quaternion.identity);

								if (!isInCollection) {
										toast.transform.parent = mainSpawnPoint;
										toast.transform.localPosition = Vector3.zero;
								} else {
										toast.transform.parent = CollectionCtrl.Instance.gameObject.transform;
										toast.transform.localPosition = new Vector3 (0f, -750f, 0f);
								}


								toast.transform.localScale = new Vector3 (1f, 1f, 1f);
								onToastMsg = true;
						}
				} else if (isInMain && onToastMsg) {
						if (Input.GetKeyDown (KeyCode.Escape)) {
								Application.Quit ();
						}
				}
	}

	public void OffToastMsg (){
		onToastMsg = false;
	}
#endif


	//Main to Game.
	public void ClickToGameStart(){
		AdManager.Instance.LoadInterstitials();
		GAManager.Instance.GAMainBtnEvent("StartBtnClick");
		SoundManager.PlaySFX ("button_click");


		if(SaveManager.GetSound()){ SoundManager.Play (playBgm,true);}

		//camera moving. ( scene manager 에 fsm 호출.(자동으로 game manager 플레이 함).
		playMaker.SendEvent("GameStart");

		TutorialManager.Instance.ShowGameTutorial ();

		isInMain = false;
		ResetPlayerPref();
	}




	//Game to main
	public void ReturnToMain(){
		//from Closing FSM, the sendevent is already sent.
		//playMaker.SendEvent ("EndPlaying");
		GAManager.Instance.GASendView("ResultMainScene");

		if(SaveManager.GetSound()){ SoundManager.Play(playBgm,true); }
		StartCoroutine("WaitAndUpdateCollectionScene");

		bool isShow = AdManager.Instance.ShowInterstitials();


		if (!isShow) { //when the interstitial not showing, then the rate popup show up!.

			ShowRatePopup();
		}

		isInMain = true;
	}
	
	//Main -> Collection & Collection -> Main
	public void MoveToCollection(){
		//버튼 누르고.SendEvent.
		if(!isInCollection){
			SoundManager.PlaySFX("button_click");
			GAManager.Instance.GAMainBtnEvent("CollectionInClick");
			GAManager.Instance.GASendView("CollectionScene");

			collectionBtnAnim.Play ("BtnClick");

			subBtnToCollection.enabled = false;

			//시간관련 미션도 업데이트.
			MissionManager.Instance.UpdateCategory8 (0);
			CollectionCtrl.Instance.UpdateInfoOn (8);


			StartCoroutine("ZoomInCollection");

		}
	}

	public void MoveOutCollection(){
		//버튼 누르고.SendEvent.
		if(isInCollection){
			SoundManager.PlaySFX("button_click");
			GAManager.Instance.GAMainBtnEvent("CollectionOutClick");
			GAManager.Instance.GASendView("ResultMainScene");

			MissionManager.Instance.SaveAllData();

			subBtnToCollection.enabled = true;
			NewOnCollectionAtStart();
			CollectionCtrl.Instance.CloseOnly();

			TutorialManager.Instance.DestroyMissionTutorial();

			AdManager.Instance.ShowInterstitials ();
			StartCoroutine("ZoomOutCollection");
		}
	}
	
	private IEnumerator ZoomInCollection(){

		//화면 밖으로 빠지는 애니메이션.
		CharInfo.Instance.MoveToCollection();
		BGSignPanel.Instance.MoveToCollection();
		MainReceipt.Instance.MoveToCollection();

		yield return new WaitForSeconds(0.2f); //자연스러운 애니메이션을 위한 딜레이.

		float frac = 1f;
		float fracXposition = 0f; //카메라의 x좌표.
		float fracYposition = 0f; //카메라의 y좌표.
		float targetXposition = positOfCollection.localPosition.x;
		float targetYposition = positOfCollection.localPosition.y;
		exitBtnFromCollection.SetActive (true);

		while(true){
			
			if(frac<0.251f){
				frac =0.25f;
				ChangeOrthoSize(frac);
				transform.localPosition = new Vector3(targetXposition,targetYposition,0f);
				exitBtnFromCollection.SetActive(true);
				break;
			}
			frac = Mathf.Lerp (frac,0.25f,0.15f);
			fracXposition = Mathf.Lerp (fracXposition,targetXposition,0.15f);
			fracYposition = Mathf.Lerp(fracYposition,targetYposition, 0.15f);

			transform.localPosition = new Vector3(fracXposition,fracYposition,0f);

			ChangeOrthoSize(frac);
			yield return null;
		}
		isInCollection = true;
		TutorialManager.Instance.ShowMissionTutorial();
	}

	private IEnumerator ZoomOutCollection(){

		float frac = 0.25f;
		float fracXposition = positOfCollection.localPosition.x;
		float fracYposition = positOfCollection.localPosition.y;

		bool flagForAnims = false;

		while(true){
			
			if(frac>0.999f){
				frac =1f;
				ChangeOrthoSize(frac);
				transform.localPosition = Vector3.zero;

				break;
			}

			if(frac>0.9f && !flagForAnims){
				flagForAnims = true;
				CharInfo.Instance.MoveOutCollection();
				BGSignPanel.Instance.MoveOutCollection();
				MainReceipt.Instance.MoveOutCollection();
			}

			if(frac>0.3f) exitBtnFromCollection.SetActive (false);

			frac = Mathf.Lerp (frac,1f,0.15f);
			fracXposition = Mathf.Lerp(fracXposition,0f,0.15f);
			fracYposition = Mathf.Lerp(fracYposition,0f,0.15f);

			transform.localPosition = new Vector3(fracXposition, fracYposition,0f);
			ChangeOrthoSize(frac);
			yield return null;
		}
		isInCollection = false;

	}

	private void ChangeOrthoSize(float size){
		uiCamera.orthographicSize = size;
		tkCamera.ZoomFactor =1/size;
		bgCamera.orthographicSize = size;
	}

	public void AudioMute(){
		SoundManager.PlaySFX("button_click");
		if(SaveManager.GetSound()){
			GAManager.Instance.GAMainBtnEvent("SoundOFFClick");
			SoundManager.Mute(true);
			SaveManager.SetSound(false);
			soundIcon.spriteName = "main_ui_soundOFF";
		}else{
			GAManager.Instance.GAMainBtnEvent("SoundONClick");
			SoundManager.Mute(false);
			SaveManager.SetSound(true);
			soundIcon.spriteName = "main_ui_soundON";
		}
	}
	public void SetSound(){
		if(SaveManager.GetSound ()){
			SoundManager.Mute (false);
			soundIcon.spriteName ="main_ui_soundON";
		}else{
			SoundManager.Mute (true);
			soundIcon.spriteName ="main_ui_soundOFF";
		}
	}

	public void NewOnCollectionAtEndOfGame(int newOn){
		newMarkOnBtnCol.GetComponent<Animation>().Stop ();
		if(newOn!=0){
			newMarkOnBtnCol.enabled = true;
			newMarkOnBtnCol.GetComponent<Animation>().Play ("NewBtnIdle");
		}
	}
	private void NewOnCollectionAtStart(){ // start 할때 & collection 화면에서 다시 main으로 돌아올때.
		numOfUnlocked.text = ((int) SaveManager.GetTotalUnlocked()).ToString();
		
		for(int i=0;i<14;i++){
			if(SaveManager.GetCollectionStatesInCategory(i)==3){
				newMarkOnBtnCol.enabled = true;
				newMarkOnBtnCol.GetComponent<Animation>().Play("NewBtnIdle");
				return;
			}
		}
		newMarkOnBtnCol.enabled = false;
	}


	private IEnumerator WaitAndUpdateCollectionScene(){
		yield return new WaitForSeconds(0.7f); //임의로 1초를 줌.
		CollectionCtrl.Instance.UpdateInfoAll();
	}

	private void ShowRatePopup(){
	
		ratePopup.TryPopup ();

	}

	void OnApplicationFocus(bool focusState) {
		ResetPlayerPref();
	}
	
	private void ResetPlayerPref() {
		string thisDate = System.DateTime.UtcNow.ToString("yyMMdd");
		string firstDate = PlayerPrefs.GetString("first date", "");
		if(string.IsNullOrEmpty(firstDate))
		{
			firstDate = thisDate;
			PlayerPrefs.SetString("first date", firstDate);
			PlayerPrefs.Save();
		}
		
		string lastDate = PlayerPrefs.GetString("last date", "");
		if(string.IsNullOrEmpty(lastDate))
		{
			lastDate = thisDate;
			PlayerPrefs.SetString("last date", lastDate);
			PlayerPrefs.Save();
		}

		int totalattend = PlayerPrefs.GetInt("total attend", 1);
		if(thisDate != lastDate)
		{
			PlayerPrefs.SetInt("total attend", totalattend + 1);
			PlayerPrefs.SetString("last date", thisDate);
			PlayerPrefs.Save();

			GAManager.Instance.GANewdayEvent();
		}
	}

}
