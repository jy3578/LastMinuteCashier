using UnityEngine;
using System.Collections;

public class ComboCtrl : MonoBehaviour {

	public UILabel comboLb;
	public UILabel comboLbStroke;
	public UISprite combo;
	public UISprite comboStroke;

	public UISprite comment;

	private Color[] colorSet;
	private Color colorNow;

	private bool itsShowing;

	void Start(){

		colorSet = new Color[11];
		colorSet[0] = new Color(1f,131f/255f,9f/255f,1f);
		colorSet[1] = new Color(180f/255f,15f/255f,185f/255f,1f);
		colorSet[2] = new Color(1f,145f/255f,40f/255f,1f);
		colorSet[3] = new Color(155f/255f,240f/255f,1f,1f);
		colorSet[4] = new Color(235f/255f,50f/255f,1f,1f);
		colorSet[5] = new Color(1f,125f/255f,50f/255f,1f);
		colorSet[6] = new Color(155f/255f,50f/255f,1f,1f);
		colorSet[7] = new Color(1f,50f/255f,130f/255f,1f);
		colorSet[8] = new Color(50f/255f,100f/255f,1f,1f);
		colorSet[9] = new Color(0f,230f/255f,35f/255f,1f);
		colorSet[10] = new Color(1f,50f/255f,130f/255f,1f);

		SetupNotVisible();
	}

	public void SetupNotVisible(){
		itsShowing = false;
		comboLb.gameObject.GetComponent<AnimatedAlpha>().alpha = 0f;
		combo.gameObject.GetComponent<AnimatedAlpha>().alpha = 0f;
		comment.gameObject.GetComponent<AnimatedColor>().color = new Color(1f,1f,1f,0f);
	//	comboLbStroke.gameObject.GetComponent<AnimatedAlpha>().alpha = 0f;
	//	comboStroke.gameObject.GetComponent<AnimatedAlpha>().alpha = 0f;

	}

	private void SetComboColor(bool isNormal){

		int comboNb = GameManager.Instance.combo;

		if(isNormal){
			colorNow = new Color(97f/255f,166f/255f, 15f/255f,1f);
		}else{
			colorNow = colorSet[Random.Range(0,colorSet.Length)];
		}

		comboLb.text = comboNb.ToString();
		comboLbStroke.text = comboNb.ToString();
		comboLb.color = new Color(1f,1f,1f,1f);
		combo.color = new Color(1f,1f,1f,1f);

		comboLbStroke.color = colorNow;
		comboStroke.color = colorNow;

	}


	public void ComboOff(){
		if(itsShowing){	
			GetComponent<Animation>().Play("comboOff");
			itsShowing=false;
		}
	}

	public void ShowCombo(){
		itsShowing = true;
		SetComboColor(true);
		GetComponent<Animation>().Play("comboShow");
	}

	public void ShowFeverCombo(){
		SetComboColor(false);
		GetComponent<Animation>().Play("comboFeverShow");
	}

	/*
	private IEnumerator UntilNextCombo(){

		float time = 0f;
		float alpha = 1f;

		while(DuringCombo){

			yield return null;

			alpha -= Time.deltaTime/comboInterval;
			if(alpha>0){

				comboLb.color = new Color(1f,1f,1f,alpha);
				comboLbStroke.color = new Color(colorNow.r,colorNow.g,colorNow.b,alpha);
				combo.color = new Color(1f,1f,1f,alpha);
				comboStroke.color = new Color(colorNow.r,colorNow.g,colorNow.b,alpha);

			}

			time += Time.deltaTime;
			if(time >= comboInterval){

				comboLb.color = new Color(1f,1f,1f,0f);
				comboLbStroke.color = new Color(colorNow.r,colorNow.g,colorNow.b,0f);
				combo.color = new Color(1f,1f,1f,0f);
				comboStroke.color = new Color(colorNow.r,colorNow.g,colorNow.b,0f);

				break;
			}
		}
	}*/

	public void ShowComment(bool isSuccess, bool isPerfect){

		StopCoroutine("WaitAndCloseComment");
		comment.gameObject.GetComponent<Animation>().Stop ();

		if(isSuccess){
			if(isPerfect){//perfect.
				comment.spriteName = "play_effect_result_perfect";
				comment.MakePixelPerfect();
			}else{//good.
				comment.spriteName = "play_effect_result_good";
				comment.MakePixelPerfect();
			}
		}else{ // fail - bad.
			comment.spriteName = "play_effect_result_bad";
			comment.MakePixelPerfect();
		}

		comment.gameObject.GetComponent<Animation>().Play ("commentShow");
		StartCoroutine("WaitAndCloseComment");
	}
	private IEnumerator WaitAndCloseComment(){
	//	yield return new WaitForSeconds(comment.gameObject.animation.GetClip ("commentShow").length);
		yield return new WaitForSeconds(0.8f);
		comment.gameObject.GetComponent<Animation>().Stop ();
		comment.gameObject.GetComponent<Animation>().Play ("commentOff");

	}

}
