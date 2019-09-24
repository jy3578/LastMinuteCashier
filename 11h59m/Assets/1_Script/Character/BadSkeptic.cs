using UnityEngine;
using System.Collections;

public class BadSkeptic : CustomerBad{

	public tk2dSprite secondHead;
	public tk2dSprite secondBody;
	public string secondPrefixName;

	public GameObject tapEffect2;

	private bool decided;

	public override void InitializeCustomer ()
	{
		msgBox.transform.GetChild(0).gameObject.GetComponent<tk2dSprite>().SetSprite(prefixName+"_icon1");
		base.InitializeCustomer ();
		decided = true;
		tapEffect2.transform.localEulerAngles = new Vector3(0f,0f,0f);
	}
	public override void FeverOnOff (bool onFever)
	{
		base.FeverOnOff (onFever);
		if(!onFever) tapEffect2.transform.localEulerAngles = new Vector3(0f,0f,0f);
	}

	public override  void ChangeFaceBeforeExit (bool isSuccess)
	{
		base.ChangeFaceBeforeExit (isSuccess);
		if(isSuccess){ secondHead.SetSprite(secondPrefixName+"_faceT");}
		else{ secondHead.SetSprite(secondPrefixName+"_faceF");}

	}

	public override void SortImageInQ(int orderInQ){
		base.SortImageInQ(orderInQ);
		secondHead.SortingOrder = orderInQ;
		secondBody.SortingOrder = orderInQ;

	}


	public override IEnumerator FeverRotateAni ()
	{
		speed = 130f;
		
		float direction;
		if(Random.Range(0,2)==0){ direction = 1f;}
		else{ direction = -1f;}
		
		while(true){
			if(speed > 130f){
				speed -= 2f;
				if(speed<=130f) speed = 130f;
			}
			
			//tapEffect.transform.Rotate(new Vector3(0f,0f,speed*Time.deltaTime*direction),Space.Self);
			
			tapEffect.transform.localEulerAngles = new Vector3(0f,0f,tapEffect.transform.localEulerAngles.z + speed*Time.deltaTime*direction);
			tapEffect2.transform.localEulerAngles = new Vector3(0f,0f,tapEffect.transform.localEulerAngles.z + speed*Time.deltaTime*direction);

			if(tapEffect.transform.localEulerAngles.z >20f && tapEffect.transform.localEulerAngles.z<90f){
				direction = -1f;
				tapEffect.transform.localEulerAngles = new Vector3(0f,0f,20f);
				tapEffect2.transform.localEulerAngles = new Vector3(0f,0f,20f);
			}
			if(tapEffect.transform.localEulerAngles.z<-20f+360f && tapEffect.transform.localEulerAngles.z>180f){
				direction = 1f;
				tapEffect.transform.localEulerAngles = new Vector3(0f,0f,-20f);
				tapEffect2.transform.localEulerAngles = new Vector3(0f,0f,20f);
			}

			yield return null;
		}
	}
	
	public override void BadEffect(){

		StartCoroutine("BadLateDecision");
	}

	private IEnumerator BadLateDecision(){
	
		decided = false;
		yield return new WaitForSeconds(2.5f);

		msgBox.transform.GetChild (0).gameObject.GetComponent<tk2dSprite>().SetSprite(prefixName+"_icon2");

		Receipt.Instance.AfterDecisionOfBadSkeptic();
		decided = true;

	}

	override public void EndPayment(){
		if(onBadEffect){ // bad effect가 실행되었다면.
			if(!decided){
				StopCoroutine("BadLateDecision");

			}
			onBadEffect = false;
		}
	}

	override public void DespawnBadEffect(){

	}

	override public int GetBadType(){
		return 4;
	}
}
