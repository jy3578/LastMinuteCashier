using UnityEngine;
using System.Collections;

public class MainReceipt : Singleton<MainReceipt> {

	public GameObject ResultReceipt;
	public GameObject StartReceipt;

	public UISprite btwPapers;

	public Transform topPaper;
	public Transform bottomPaper;

	private string prefixName;

	//for showing result receipt.
	public NewIntAnimation scoreLb;
	public NewIntAnimation bestScoreLb;

	public UILabel[] gameResults;
	public UISprite newStuffIn;


	void Start(){
	
		prefixName = "main_ui_startbtn2_";
		StartBtnIdle();
	}


	//scene loading 했을때 & collection scene에서 돌아왔을때. start btn ani.
	public void StartBtnIdle(){
		ResultReceipt.SetActive(false);
		StartReceipt.SetActive (true);
		GetComponent<Animation>().Play ("startBtn_Start");
		prefixName = "main_ui_startbtn2_";
	}

	public void StartBtnClicked(){

		GetComponent<Animation>().Stop();
		topPaper.localPosition = bottomPaper.localPosition;

		SceneManager.Instance.ClickToGameStart();
	}

	//animation에서 불러올 method.
	public void StartBtnAni0(){

		btwPapers.spriteName = prefixName +"0";
		btwPapers.width = 34;
	}

	public void StartBtnAni1(){
		btwPapers.spriteName = prefixName +"0";
		btwPapers.width = 42;
	}

	public void StartBtnAni2(){
		btwPapers.spriteName = prefixName +"0";
		btwPapers.width = 51;
	}

	//Main -> Collection animation.
	public void MoveToCollection(){


		GetComponent<Animation>().Stop();
		if(StartReceipt.activeSelf){
			GetComponent<Animation>().Play("startBtn_MoveOut");
			StartCoroutine("EndOfMovingToCollection");
		}else{
			GetComponent<Animation>().Play ("resultReceipt_MoveOut");
			StartCoroutine ("EndOfMovingToCollection");
		}
	}

	private IEnumerator EndOfMovingToCollection(){


		yield return new WaitForSeconds(GetComponent<Animation>().GetClip("resultReceipt_MoveOut").length);

		if(!StartReceipt.activeSelf){
			StartReceipt.SetActive(true);
			ResultReceipt.SetActive (false);
		}
		StartReceipt.transform.localPosition = new Vector3(600f,0f,0f);
	}
	//Collection -> Main
	public void MoveOutCollection(){

		GetComponent<Animation>().Stop();
		GetComponent<Animation>().Play("startBtn_MoveIn");
		StartCoroutine("EndOfMovingToMain");


	}

	private IEnumerator EndOfMovingToMain(){
		yield return new WaitForSeconds(GetComponent<Animation>().GetClip ("startBtn_MoveIn").length);
		StartBtnIdle();
	}

	//Game -> main.
	public void ShowGameResult(int newTotalScore, int prevBest, int maxCombo,int customers, int exp, int achievements){


		ResultReceipt.SetActive (true);
		StartReceipt.SetActive (false);



		scoreLb.Play (newTotalScore, newTotalScore);
		bestScoreLb.Play (prevBest, prevBest);
		
		if(SaveManager.GetLanguage() == "Korean"){
			ResultReceipt.GetComponent<UISprite>().spriteName = "main_ui_result";
			newStuffIn.spriteName = "main_msg_newItem";
			
		}else if(SaveManager.GetLanguage() == "English"){
			ResultReceipt.GetComponent<UISprite>().spriteName = "main_ui_result_en";
			newStuffIn.spriteName = "main_msg_newItem_en";
			
		}
		
		newStuffIn.MakePixelPerfect();
		newStuffIn.gameObject.transform.localScale = new Vector3(1.6875f,1.6875f,1f);
		
		ResultReceipt.transform.localPosition = new Vector3(0f,178.25f,0f);
		
		gameResults[0].text = maxCombo.ToString();
		gameResults[1].text = customers.ToString();
		gameResults [2].text = exp.ToString ();
		

		if(achievements == 0){
			newStuffIn.enabled = false;
			gameResults[3].text = "";
		}else{
			newStuffIn.enabled = true;
			gameResults[3].text = achievements.ToString();
		}

		
		GetComponent<Animation>().Play("resultReceipt_Idle");

	
	}




}
