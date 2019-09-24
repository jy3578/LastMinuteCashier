using UnityEngine;
using System.Collections;
using CodeStage.AntiCheat.ObscuredTypes;


public class CharInfo : Singleton<CharInfo> {

	public UISprite charInfo;
	public UISprite strap;

	public Animation anim;
	public UILabel charLevelBonus;
	public UILabel charExp;
	public UISprite charGauge;
	public UISprite[] charStar;
	public UISpriteAnimation charStarAni;

	public GameObject lvUpPrize;

	private int level;
	private Color[] bonusColor; // bonus 글씨 컬러.

	void Start(){

		SetBonusColor();
		level = SaveManager.GetPlayerLevel();
		SetCharInfoCard();
		SetCharInfoContents();
		CharInfoStartAni();

		lvUpPrize.SetActive (false);
	}

	private void SetBonusColor(){
		bonusColor = new Color[4];
		bonusColor[0] = new Color(118f/255f,164f/255f,6f/255f);
		bonusColor[1] = new Color(53f/255f,84f/255f,169f/255f);
		bonusColor[2] = new Color(1f,97f/255f,118f/255f);
		bonusColor[3] = new Color(130f/255f,88f/255f,5f/255f);
	}

	public void CharInfoStartAni(){
		anim.Play ("charInfo_Start");
	}

	public void MoveToCollection(){
		anim.Play("charInfo_MoveOut");
	}

	public void MoveOutCollection(){
		anim.Play("charInfo_MoveIn");
	}

	public void ShowCharInfo(){ // 게임 끝나고.
		StartCoroutine(WaitAndShow());
	}

	private IEnumerator WaitAndShow(){
		yield return null;

		int nextLevel = SaveManager.GetPlayerLevel();
		bool isLvUp = false;
		
		if(level!=nextLevel){ 				//levelup!.
			
			isLvUp = true;
			level = nextLevel;
			GAManager.Instance.GALevelUpEvent(nextLevel);
			//levelup animation.
		}
		if(isLvUp){
			if(level%6 ==1){ 				// 새로운 사원증으로 교체.
				StartCoroutine(ShowLevelUpCard());
			}else{ 							// 새로운 별 획득.
				ShowLevelUpStar();
			}
		}else{								//not level up.
			SetCharInfoContents();
		}
	}

	private void SetCharInfoCard(){ // 사원증 세팅.

		charInfo.spriteName = "main_ui_charInfo"+((level-1)/6 +1).ToString();
		strap.spriteName = "main_ui_charInfo_line"+((level-1)/6+1).ToString();
		charLevelBonus.color = bonusColor[level/6];
	}

	private void SetCharInfoContents(){ //별 표시, 보너스 표시.

		float levelExp = SaveManager.GetPlayerLevelExp();

		charLevelBonus.text = (level -1).ToString();
		charExp.text = string.Format("{0:N0}",(levelExp*100));
		charGauge.width = (int) (levelExp*100f);
		
		//star 표시.
		int numOfStar = (level%6)-1;
		if(level%6 ==0) numOfStar = 5;
		
		for(int i=0;i<numOfStar;i++){
			charStar[i].enabled = true;
		}
		for(int i = numOfStar;i< charStar.Length;i++){
			charStar[i].enabled = false;
		}

	}

	private IEnumerator ShowLevelUpCard(){

		yield return new WaitForSeconds (0.7f);
		//새로운 사원증으로 교체 & animation.
		GetComponent<Animation>().Play ("charInfo_MoveOut");
		yield return new WaitForSeconds(GetComponent<Animation>().GetClip("charInfo_MoveOut").length + 0.7f);
		SetCharInfoCard();
		SetCharInfoContents();

		GetComponent<Animation>().Play ("charInfo_MoveIn");

	}

	private void ShowLevelUpStar(){
		//새로운 별 animation.
		SetCharInfoContents();

		GameObject charStarAnigo = charStarAni.gameObject;
		int openStar = (level-1)%6;   //level 표 참고 - 해당 레벨과 별 획득수가 6 주기로 반복됨.
		
		openStar -= 1;  //열리는 star가 몇번째인지 알고 난후, index로 변환(마이너스 1).

		//star animation.
		charStarAnigo.SetActive(true);
		charStarAnigo.transform.localPosition = charStar[openStar].transform.localPosition;
		charStarAni.Play();

		//textbox animation.
		lvUpPrize.SetActive (true);
		lvUpPrize.transform.localScale = new Vector3 (0f, 0f, 1f);


		StartCoroutine (WaitAndStarAniOff());

	}

	private IEnumerator WaitAndStarAniOff(){

		yield return new WaitForSeconds (0.7f);
		GetComponent<Animation>().Play ("charInfo_LvUpStar");
		yield return new WaitForSeconds(GetComponent<Animation>().GetClip("charInfo_LvUpStar").length);
		charStarAni.gameObject.SetActive(false);
		lvUpPrize.SetActive (false);
	}

	public void CharInfoClicked(){
		GetComponent<Animation>().Stop ();
		GetComponent<Animation>().Play ("charInfo_Click");
	}

	/*
	private IEnumerator ShowGaugeAni(){
		float levelExp = SaveManager.GetPlayerLevelExp();
		float frac = (float) charGauge.width;
		while(true){
			if(levelExp*100f-2f<= charGauge.width){
				charGauge.width = (int) levelExp*100;
				break;
			}
			frac =  Mathf.Lerp (frac,levelExp,0.3f);
			charGauge.width = (int) frac * 100;
			yield return null;
		}
	}
*/

}