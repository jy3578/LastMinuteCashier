using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;


public enum MissionType{
	Normal,
	Conditioned
}

public enum MissionState{
	BeforeStart  = 0,        //시작 전 - 0.
	Ongoing = 1,			//미션 중 - 1.
	StartAgain = 2,			//fail 후 - 2. 
	NotAcceptYet = 3,		//성공 후 - 3.
	Complete = 4
}
public enum BoxSize{
	L,
	M,
	S
}

public class CollectionStuffLabel : MonoBehaviour {

	public tk2dSprite stuffOnShelf;
	public tk2dSprite[] stuffOnDisplay;
	public int shelfNO; 
	public BoxSize boxSize;

	public MissionType MT;
	public MissionState MS;
	public int category;

	public UILabel targetNumber;
	public UILabel myNumber;
	public UISprite stamp;
	public UILabel missionLv;

	public UISprite infoBtnIcon;
	public Animation infoBtnAnim;

	public GameObject missionBG;
	public GameObject complete;

	protected SpawnPool paperPool;
	protected Transform missionPopup;
	protected Transform newEffect;

	protected bool popupOpen;  //open상태일때,(open & close가 클릭여러번했을때, 한번만 실행되게 하기 위해).

	void Start(){

		paperPool = PoolManager.Pools["Mission"];
		popupOpen = false;

	}
	virtual public void ShowStuffOnDisplay(){  //처음 시작할때!.
		int unlock = SaveManager.GetCollectionUnlockedInCategory(category);
		for(int i=0;i<stuffOnDisplay.Length;i++){
			if(i<unlock){
				stuffOnDisplay[i].GetComponent<Renderer>().enabled = true;
			}else{
				stuffOnDisplay[i].GetComponent<Renderer>().enabled = false;
			}
		}
	}

	virtual	public void ShowNumbersOnly(){ //unlock끝난후에 missionmanager에서 호출.

		MS = (MissionState) ((int)SaveManager.GetCollectionStatesInCategory(category));
		if(MS == MissionState.Complete){
			missionBG.SetActive(false);
			complete.SetActive(true);
			missionLv.text = "MAX";
		}else{
		
			ShowLocalizeNumbers();
		}
		SetInfoBtn();
	}

	public void ShowLocalizeNumbers(){

		int tgNum = MissionManager.Instance.GetMissionObjective(category);
		int myNum = SaveManager.GetMyNumbers(category);
		if(MS == MissionState.BeforeStart) myNum = 0;
		targetNumber.text = tgNum.ToString();
		missionLv.text = "Lv"+(SaveManager.GetCollectionUnlockedInCategory(category)+1).ToString();
		myNumber.text = myNum.ToString();

		if(category == 1 || category == 2 || category ==3){
			if(SaveManager.GetLanguage() =="English"){
				targetNumber.text = (((float)tgNum)/1000f).ToString("F2");
				myNumber.text = (((float)myNum)/1000f).ToString("F2");
			}
		}
	}

	virtual public void ShowStuffOnShelf(){ // 처음시작시, 게임끝난후. 호출.

		int state = SaveManager.GetCollectionStatesInCategory(category);
		MS = (MissionState) state;

		string stuffSpriteName = "";
		int unlocked = SaveManager.GetCollectionUnlockedInCategory(category);

		if(MS != MissionState.Complete){
			missionBG.SetActive (true);
			complete.SetActive (false);
			ShowLocalizeNumbers();
		}

		infoBtnAnim.Stop ();
		infoBtnAnim.gameObject.transform.localScale = new Vector3(1f,1f,1f);
	
		stuffOnShelf.GetComponent<Animation>().Stop ();
		stuffOnShelf.transform.localScale = new Vector3(1f,1f,1f); //애니메이션이 scale을 왔다갔다 하므로.

		SetInfoBtn();
		if(unlocked ==0){ // 초기 세팅-비어있는 공간으로 표시.
			if(state == 3){//성공상태.
				ShowSuccessStamp();
				stuffOnShelf.GetComponent<Renderer>().enabled = true;
				stuffOnShelf.SetSprite("collection_box"+boxSize.ToString());
				stuffOnShelf.GetComponent<Animation>().Play ("newBoxIdle");
			}else if(state == 2){
				ShowFailedStamp();
				stuffOnShelf.GetComponent<Renderer>().enabled = false;
			}else{
				ShowStampOff();
				stuffOnShelf.GetComponent<Renderer>().enabled  = false;
			}

		}else{
			stuffOnShelf.GetComponent<Renderer>().enabled = true;
			switch(state){
			case 0:
				ShowStampOff(); //start 이전.
				stuffSpriteName = "collection_icon"+(category+1).ToString("D2")+"_"+(unlocked).ToString();
				break;
			case 1: // ongoing 중.
				ShowStampOff();
				if(MT == MissionType.Conditioned){
				}
				stuffSpriteName = "collection_icon"+(category+1).ToString("D2")+"_"+(unlocked).ToString();
				break;
			case 2: // failed.
				ShowFailedStamp();
				stuffSpriteName = "collection_icon"+(category+1).ToString("D2")+"_"+(unlocked).ToString();
				break;
			case 3: // success.
				ShowSuccessStamp();
				stuffSpriteName = "collection_box"+boxSize.ToString();
				stuffOnShelf.GetComponent<Animation>().Play ("newBoxIdle");
				break;
			case 4: //complete.
				ShowStampOff ();
				missionBG.SetActive (false);
				complete.SetActive (true);
				stuffSpriteName = "collection_icon"+(category+1).ToString("D2")+"_"+(unlocked).ToString();
				
				break;
			}
		
			stuffOnShelf.SetSprite(stuffSpriteName);
		}
	
	}

	public void SetInfoBtn(){
		if(MT ==MissionType.Conditioned){
			if(MS ==MissionState.Ongoing){
				infoBtnAnim.Stop ();
				infoBtnIcon.spriteName = "collection_popUp_icon_challenge";
				infoBtnIcon.MakePixelPerfect();
				infoBtnAnim.Play("BtnIdle");
			}else{
				infoBtnAnim.Stop ();
				infoBtnIcon.spriteName = "collection_icon_info";
				infoBtnIcon.MakePixelPerfect();
			}
		}
	}

	virtual public void UnlockAnimation(){
	
		int unlocked = SaveManager.GetCollectionUnlockedInCategory(category);

		stuffOnShelf.GetComponent<Animation>().Stop ();
		stuffOnShelf.transform.localScale = new Vector3(1f,1f,1f);

		newEffect = paperPool.Spawn ("OpenBox"+boxSize.ToString(),stuffOnShelf.transform);
		newEffect.localPosition = new Vector3(0f,0f,0f);
		newEffect.localScale = new Vector3(1f,1f,1f);
		stuffOnShelf.GetComponent<Renderer>().enabled = false;

		newEffect.gameObject.GetComponent<tk2dSpriteAnimator>().Play ();
		StartCoroutine("WaitAndDespawnEffect",unlocked);
		stuffOnDisplay[unlocked].GetComponent<Renderer>().enabled = true;
		stuffOnDisplay[unlocked].GetComponent<Animation>().Play ("newStuff");

		MissionManager.Instance.UnlockAndChangeState(category,MT);

	}

	virtual protected IEnumerator WaitAndDespawnEffect(int unlock){
		yield return new WaitForSeconds(0.1f);
		stuffOnShelf.GetComponent<Renderer>().enabled = true;
		stuffOnShelf.SetSprite("collection_icon"+(category+1).ToString("D2")+"_"+(unlock+1).ToString());
		yield return new WaitForSeconds(0.9f);
		paperPool.Despawn(newEffect,CollectionCtrl.Instance.gameObject.transform);
	}

	public void ShowMissionPaper(){
	//클릭했을때, 나타나는 반응들.
		GAManager.Instance.GAMissionLabelBtnEvent(category);
		MS = (MissionState)((int) SaveManager.GetCollectionStatesInCategory(category));
		infoBtnAnim.Stop();


		if(MS == MissionState.NotAcceptYet){ //success case.
			UnlockAnimation();
			SoundManager.PlaySFX("boxOpen");
			return;
		}else if(MS != MissionState.Complete){ // 0, 1, 2 case.

			if(!popupOpen){ //팝업창 open.
				SoundManager.PlaySFX("mission_open");
				infoBtnAnim.Play ("BtnClick");
				if(MT == MissionType.Conditioned && MS == MissionState.Ongoing) infoBtnAnim.Play ("BtnIdle");

				popupOpen = true;
				int unlocked = SaveManager.GetCollectionUnlockedInCategory(category);
			
				string prefabName = "MissionPaper_T"+shelfNO.ToString();
				missionPopup = paperPool.Spawn(prefabName,transform);

				missionPopup.gameObject.GetComponent<MissionPaperPopUp>().stuffLabel = this;
				missionPopup.gameObject.GetComponent<MissionPaperPopUp>().SetDataInPaper(MS,category,unlocked);

				PlaceMissionPaper();
				CollectionCtrl.Instance.CloseAndShowSimultaneously(category);
			}

			return;
		}

	}

	protected void PlaceMissionPaper(){
		missionPopup.GetComponent<Animation>().Stop ();
		missionPopup.localScale = new Vector3(1.3f,1.3f,1f);
		missionPopup.localEulerAngles = Vector3.zero;
		missionPopup.localPosition = new Vector3(-100f,20f,0f);

		if (transform.localPosition.x > 350f) {
			missionPopup.localPosition = new Vector3 (-130f, 20f, 0f);
		} else {
			missionPopup.localPosition = new Vector3(-110f,20f,0f);
		}

		missionPopup.GetComponent<Animation>().Play ("mission_ShowUp");

	}

	public void CloseMissionPaperbyOther(){
		missionPopup.GetComponent<MissionPaperPopUp>().CloseMissionPaper();
		missionPopup = null;
		popupOpen = false;
	}

	public void CloseMissionPaperbyClick(){
		missionPopup = null;
		popupOpen = false;
	}


	protected void ShowSuccessStamp(){
	
		stamp.enabled = true;
		stamp.spriteName = "collection_ui_success";
	}

	protected void ShowFailedStamp(){
		stamp.enabled = true;
		stamp.spriteName = "collection_ui_fail";
	}

	protected void ShowStampOff(){
		stamp.enabled = false;
	}

	public void StartMissionByTryBtn(){
		infoBtnAnim.Stop();
		infoBtnIcon.spriteName =  "collection_popUp_icon_challenge";
		infoBtnIcon.MakePixelPerfect();
		infoBtnAnim.Play ("BtnIdle");

		ShowStampOff();
	}

}
