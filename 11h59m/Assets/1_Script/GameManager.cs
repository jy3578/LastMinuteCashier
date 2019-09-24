using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;
using CodeStage.AntiCheat.ObscuredTypes;

public enum GameState{
	Reset,
	Ready,
	Play,
	Pause,
	End
}

public class GameManager : Singleton<GameManager> {

	//나중에 숨겨야 하는 변수.
	public int level; 
	public float timer;
	public int feverCustomerCount;
	private int levelCount;
	
	public int successReceipt;
	public int failedReceipt;
	public ObscuredInt score;
	public int combo;
	public ObscuredInt maxCombo;
	public bool onCombo;
	public float comboTimer;
	public int feverCount;
	public float feverTimer;
	public bool onFeverMode;
	public GameState GS;
	public ObscuredInt maxChange;

	private SpawnPool feverTouchPool;

	private float timerCount = 0f;
	private bool feverStarted;

	private bool isNewRecord;
	public bool continueOnce = false;

	//Time 등 UI부분 연결시켜야 할 부분.
	public GameObject pauseMenu;
	public FeverPanelEffect feverPanel;
	public BGFeverCtrl feverBG;

	public UILabel timerLb;
	public NewIntAnimation scoreLb;
	public ComboCtrl comboCtrl;
	public PlayMakerFSM bgOutside;

	
	private Receipt receipt;
	private Tray trayWithDecks;
	private Wallet wallet;   //wallet은 게임 시작시 애니메이션, 끝날때 애니메이션의 시작이 됨.(closing 등의 애니메이션이 wallet playmaker에서 전부 불러옴).
	private CustomerQueue customersQ;


	//입력받을 변수.
	public float limitTime;
	public float comboInterval; // 콤보 성공 시간 간격.
	public float feverDuration; //fever 모드 지속시간.
	private int feverInterval; // fever 도달까지 필요한 combo 개수.
	

	void Start(){

		receipt = Receipt.Instance;
		trayWithDecks = Tray.Instance;
		wallet = Wallet.Instance;
		customersQ = CustomerQueue.Instance;

		feverTouchPool = PoolManager.Pools["FeverTouches"];

		GS = GameState.Reset;
		
		level = 1;
		timer = 0f;
		levelCount = 0;

		feverCustomerCount = 0;
		successReceipt = 0;
		failedReceipt = 0;
		score = 0;
		combo = 0;
		maxCombo = 0;
		onCombo = true;
		comboTimer = comboInterval;
		maxChange = 0;

		feverCount = 0;
		feverTimer = 0;
		onFeverMode = false;
		feverStarted = false;
		isNewRecord = false;
		continueOnce = false;

		timerLb.text = "00";
		scoreLb.Play (0);
		GAManager.Instance.GAPlayGame();
	}

	void Update () {
		
		if(GS == GameState.Play){

			PlayUpdate();
		}

	}

	//Game State 전환.
	
	public void ResetPlay(){

				if (!continueOnce) { //first 이면, reset. continue 이면 게임정보 reset x.

						GAManager.Instance.GASendView("GameScene");
						level = 1;
						timer = 0f;

						feverCustomerCount = 0;
						successReceipt = 0;
						failedReceipt = 0;
						score = 0;
						combo = 0;
						maxCombo = 0;

						onCombo = true;
						comboTimer = comboInterval;


						feverCount = 0;
						feverTimer = 0;
						feverInterval = 4;
						onFeverMode = false;
						feverStarted = false;
						maxChange = 0;
						isNewRecord = false;
						continueOnce = false;

						scoreLb.Play (0, 0);

						timerLb.text = "00";

						comboCtrl.SetupNotVisible ();

						customersQ.InitializeQueue ();


						if (GS == GameState.End) {
								trayWithDecks.InitializeTray ();
						}
						GS = GameState.Reset;
						Ready ();


				} else { //연장.

						trayWithDecks.InitializeTray ();
						timer = 45f;
						timerLb.text = "45";
						receipt.GoReady ();


				}



	}
	
	public void Ready(){
		
		GS = GameState.Ready;

		//Receipt Ready Animation .
		receipt.GoReady();

	}


	public void ReadyAnimation(){

		//pos기 receipt 애니메이션.-> Queue 애니메이션 & receipt 애니메이션 & wallet 애니메이션.


		//receipt의 ready가 인출되어 throw 된 대까지.
		receipt.ChangeSpriteAfterReady(); 

		GS = GameState.Play;
		wallet.StartPlaying();

		if (!continueOnce) {
				
				customersQ.MoveForwardAll ();
				//	wallet.StartCoroutine("GetTouchOnWallet");

		}
	}


	private void PlayUpdate(){
		
		//play time control.
		timer += Time.deltaTime;
		if(timer >= limitTime){
			timer = 0f;
			timerLb.text = "00";
			//EndGame();
			AskExtraTime();
			return;
		}
		
		timerCount += Time.deltaTime;
		if(timerCount >= 1f){
			
			timerCount = 0f;
			timerLb.text = ((int)timer).ToString("D2");

			//shutter와 moon 애니메이션 추가.
	
		}
		
		if(onCombo){
			comboTimer -= Time.deltaTime;
			if(comboTimer <= 0f){
				comboTimer = 0f;
				ComboOff();
			}
		}
		
		if(onFeverMode){
			if(!feverStarted)	FeverModeOn ();
			feverTimer -= Time.deltaTime;


//			/*
			if(Input.touchCount>0){

				int i =0;
				while(i<Input.touchCount){

					Vector3 touchPoint3;
					if(Input.GetTouch (i).phase == TouchPhase.Began){
						touchPoint3 = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
						Vector2 touchPoint2 = new Vector2(touchPoint3.x,touchPoint3.y);
						if(feverPanel.GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPoint2)){
							if(feverPanel.gameObject.GetComponent<Collider2D>().tag == "FeverPanel"){
								Transform touchEffect = feverTouchPool.Spawn ("FeverTouch");
								touchEffect.position = new Vector3(touchPoint2.x,touchPoint2.y,0f);
								touchEffect.localScale = new Vector3(1f,1f,1f);

								customersQ.FeverAttack();
								
								if(customersQ.GetHP()<=0f) ConfirmPay ();		
							}
						}
					}
					i++;
				}
			}
	//		*/
			/*
			if(Input.GetMouseButtonDown(0)){
			
				Vector3 touchPoint3;

					touchPoint3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					Vector2 touchPoint2 = new Vector2(touchPoint3.x,touchPoint3.y);
					if(feverPanel.GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPoint2)){
						if(feverPanel.gameObject.GetComponent<Collider2D>().tag == "FeverPanel"){
							Transform touchEffect = feverTouchPool.Spawn ("FeverTouch");
							touchEffect.position = new Vector3(touchPoint2.x,touchPoint2.y,0f);
							touchEffect.localScale = new Vector3(1f,1f,1f);
													
							customersQ.FeverAttack(); 
			
							if(customersQ.GetHP()<=0f) ConfirmPay ();		
						}
					}

			}
			*/
			if(feverTimer <= 0f){ 
				feverTimer = 0f;
				FeverModeOff();
			}
		}

	}

	private void ComboOn(){
		
		onCombo = true;
		//combo 관련 on 상태의 애니메이션 추가. combo 글자 띄우기 & 사운드.
		
	}
	
	private void ComboOff(){
		
		onCombo = false;

		comboCtrl.ComboOff();
		if(combo>maxCombo) maxCombo = combo;
		
		combo = 0;
		comboTimer = comboInterval;
		feverInterval = 4;

	}
	
	private void FeverModeOn(){

		if(GS == GameState.Play){
			//fever 모드 시작시 나타나는 애니메이션. 캐릭터. ->customer 내의 EndPayment에서 관리.

			feverStarted = true;
//			wallet.FeverOnOff(true);

			//fever panel.
			feverPanel.gameObject.SetActive (true);

			//fever BG. - wall / lights / ball.
			feverBG.gameObject.SetActive(true);
		
			feverTimer = feverDuration;

			TutorialManager.Instance.ShowFeverTutorial();
		}
		//fever 관련 애니메이션.
	}
	
	private void FeverModeOff(){

	
		//fever Panel. Ends of Animation.
		feverPanel.EndFever();

		//fever Wall. Ends of Animation.
		feverBG.EndFever();

		customersQ.EndPayment(true,true,false);

//		wallet.FeverOnOff (false);

		onFeverMode = false;
		feverCount = 0;
		feverTimer = feverDuration;

		feverStarted = false;
		feverInterval++;


	}

	
	public void PauseGame(){
	
		if(GS == GameState.Pause){ //이미 pause 였다면, 다시 play 상태로.
			SoundManager.PlaySFX("button_click");
			GAManager.Instance.GAPauseBtnEvent("ResumeClick");
	
			pauseMenu.SetActive (false);
			GS = GameState.Play;

			bgOutside.SendEvent("Resume"); //Game이 pause 되면 멈추므로, 다시 restart 해준다.


		}else if(GS == GameState.Play){//후에 resume 에 의해서 end -> play 되지 않도록.
			SoundManager.PlaySFX("button_click");
			GS = GameState.Pause;
			GAManager.Instance.GAPauseBtnEvent("PauseClick");
			GAManager.Instance.GASendView("PauseScene");
			//pause 메뉴 버튼 활성화.
			pauseMenu.SetActive (true);
		}
	}


	//1) play 시간이 완료되어 끝난 경우. 2) pause -> go main. 3) pause -> restart.
	//공통: 문 내려오는 애니메이션.

	public void AskExtraTime(){

			GS = GameState.End;
			if (!continueOnce) { // continue 를 한번도 하지 않고 게임 종료.
						if(onFeverMode) FeverModeOff();
						ResultOnShutter.Instance.WhetherToShowResult (true);
						ResultOnShutter.Instance.WriteResultOnShutter (score, maxCombo, maxChange, feverCustomerCount);

						wallet.EndPlaying (0);
									
			} else {   // continue를 한번 하고 게임 종료.
			
						EndGame ();
			}
	}

	public void ExtraTimePlaying(){
				
				continueOnce = true;


	}


	public void EndGame()  
	{
		if(combo>maxCombo) maxCombo = combo;

		continueOnce = false;

		if(GS != GameState.Pause) // 1) case.
		{ 
					
				GS = GameState.End;
				
				if (onFeverMode) FeverModeOff ();

				ResultOnShutter.Instance.WhetherToShowResult (true);
				ResultOnShutter.Instance.WriteResultOnShutter (score, maxCombo, maxChange, feverCustomerCount);

				//	MainReceipt.Instance.ShowGameResult(isNewRecord,score);// customerQ에서 총 처리 손님수와 진상손님수를 reset하기 전에 call.
				//	CharInfo.Instance.ShowCharInfo ();

					wallet.EndPlaying (1);



		}else{//2) case.
		 
			GS = GameState.End;
			GAManager.Instance.GAPauseBtnEvent("GoMainClick");

			if(onFeverMode) FeverModeOff();
			pauseMenu.SetActive(false);

			ResultOnShutter.Instance.WhetherToShowResult(false);

					wallet.EndPlaying(2);

		}

		//end animation 시작으로 돌아가기.(wallet 닫히기 -> shutter 내려오기(receipt, BG, customer 제자리로) -> camera return).
		if(onCombo) comboCtrl.ComboOff();
	}

	public void EndGameFromPauseToRestart(){ //3) case.
		
		continueOnce = false;

		if(GS == GameState.Pause){
			GS = GameState.End;
			GAManager.Instance.GAPauseBtnEvent("RestartClick");

			if(onFeverMode) FeverModeOff();

			pauseMenu.SetActive (false);

			if(combo>maxCombo) maxCombo = combo;

			if(onCombo) comboCtrl.ComboOff();
				wallet.EndPlaying(3);
		//	wallet.StopCoroutine("GetTouchOnWallet");

			ResultOnShutter.Instance.WhetherToShowResult(false);
		

		}
	

	}




	//receipt - confirm 버튼 누를시.
	public void ConfirmPay(){
		if(GS == GameState.Play){
			if(!onFeverMode){
				int valueOnTray = trayWithDecks.GetValueOnTray();
				
				//방금 turn에서 success 했는지 check.
				bool isSuccess = CheckSuccessTurn (valueOnTray);
				bool isPerfect = true;

				wallet.ShowConfirm(isSuccess);
				if(isSuccess){

					if(!onCombo){
						ComboOn ();
						feverCount = 0;
						level=1;
						levelCount=0;
						isPerfect = false;
					}
					combo++;
					comboCtrl.ShowCombo();
					comboCtrl.ShowComment(isSuccess,isPerfect);
					comboTimer = comboInterval;
					feverCount++;

					levelCount++;
					if(level < 4){
						if(levelCount >= 1){
							levelCount = 0;
							level++;
						}
					}else if(level<15){
						if(levelCount >= 2){
							levelCount = 0;
							level++;
						}
					}else{
						if(levelCount >= 3){
							levelCount = 0;
							level++;
						}
					}
					CountScore (isSuccess);

					if(feverCount >= feverInterval && !onFeverMode){
						onFeverMode = true; // update에서 fevermode를 켜줌(game end에서 fevermodeOn과 동시에 시작되는 현상을 방지).
						customersQ.EndPayment (true,isPerfect,true);
					}else{
						customersQ.EndPayment (true,isPerfect,false);
					}
					SuccessTurn ();

				}else{
					if(onCombo){
						ComboOff ();
						feverCount = 0;
					}
					comboCtrl.ShowComment(false,false);

					level = 1;
					CountScore (isSuccess);
					customersQ.EndPayment(false,false,false);
					FailTurn();
					ComboOn();
				}

				//Initialize tray...
				trayWithDecks.InitializeTray();
				receipt.AfterEndPayment();

			}else{ // fever

				bool isPerfect = true;
				feverCustomerCount++;

				if(!onCombo){
					ComboOn ();
					feverCount = 0;
					isPerfect = false;
				}

				combo++;
				comboCtrl.ShowFeverCombo();
				comboCtrl.ShowComment(true,isPerfect);
				comboTimer = comboInterval;


				CountScore (true);
				customersQ.EndPayment (true,true,true);
				SuccessTurn();
			}
		}
	}

	private bool CheckSuccessTurn(int value){
		//일단 단순히 value만 비교. 추후 캐릭터 특성에 따른 variation추가 필요.
		if(value == receipt.GetChange()){
			if(maxChange<value) maxChange = value;
			return true;

		}else{

			return false;
		}
	}

	private void CountScore(bool isSuccess){

		if(isSuccess){  // 플레이어가 낸 돈이 change와 일치하고 && 그외 부가 조건을 만족시.
			if(!onFeverMode){
				score += receipt.price;
			}else{
				score += feverCustomerCount * 300;
			}
		}else{ // 플레이어가 낸 돈이 change와 일치하지 않고 && 캐릭터 조건을 만족하지 못했을 경우.

		}

	}
	
	private void SuccessTurn(){ // Turn success 했을때 일어날 이벤트 및 애니메이션.


		if(!onFeverMode){
			successReceipt += 1;
		}
		SoundManager.PlaySFX("receipt_success");
		scoreLb.Play (score);

	}

	private void FailTurn(){ //turn fail 했을때 일어날 이벤트 및 애니메이션.
		failedReceipt += 1;
		SoundManager.PlaySFX("receipt_fail");
	}
	





}
