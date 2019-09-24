using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;
using HutongGames.PlayMaker;


public enum CustomerType{

	Normal,
	Homeless,
	Ignorant,
	Thief,
	Skeptic

}

public class Customer: MonoBehaviour{

	public tk2dSprite customerHead;
	public tk2dSprite[] customerBody;
	public string prefixName;
	public CustomerType customerType;
	public GameObject tapEffect;


	protected float speed;
	protected float hp;
	protected bool onFever;
	public bool onBadEffect; // bad effect가 현재 켜져 있는지.

	public virtual void InitializeCustomer(){
		onFever = false;
		onBadEffect = false;
		transform.localScale = new Vector3(960f,960f,1f);
		transform.localPosition = new Vector3 (-500f, 0f, 0f);
		customerHead.SetSprite(prefixName +"_face");
		speed = 130f;
		hp = 100f;
		tapEffect.transform.localEulerAngles = new Vector3(0f,0f,0f);
		GetComponent<Animation>().Play (GetCustomerType().ToString()+"_Idle");
	}

	private void PlayIdle(){
		GetComponent<Animation>().Play (GetCustomerType().ToString()+"_Idle");
	}

	virtual public void ChangeFaceBeforeExit(bool isSuccess){

		if(isSuccess){	customerHead.SetSprite(prefixName+"_faceT");	}
		else { customerHead.SetSprite(prefixName+"_faceF");}

	}

	virtual public void SortImageInQ(int orderInQ){

		customerHead.SortingOrder = orderInQ;
		foreach(tk2dSprite imageOnCustom in customerBody){
			imageOnCustom.SortingOrder = orderInQ;
		}
	}

	public void MoveForward(int orderInQ){

		//orderInQ = 0 : moveOut/ 1 : 1st in Q/ 2 : 2nd in Q/ 3: 3rd in Q.
		if(orderInQ == 0){ 
			EndPayment();
			DespawnBadEffect();
			tapEffect.GetComponent<Animation>().Play();
			iTween.MoveTo(gameObject,iTween.Hash("x",1300f,"islocal",true, "time",0.8f,"space",Space.Self,"easetype",iTween.EaseType.linear,"oncomplete","DespawnChar"));

	//		playMaker.SendEvent("MoveOut");
		}else{
			if(orderInQ ==1) InFrontOfDesk();
			float destinationX = -90f * ((float)orderInQ -1f);
	
			iTween.MoveTo(gameObject,iTween.Hash("x",destinationX,"islocal",true, "time",0.6f,"space",Space.Self,"easetype",iTween.EaseType.linear));
//			playMaker.SendEvent("StartAt"+orderInQ.ToString());
		}

	}

	virtual public void FeverOnOff(bool isFever){

		onFever = isFever;
		if(isFever){
			customerHead.SetSprite(prefixName+"_faceT");
//			gameObject.GetComponent<PlayMakerFSM>().Fsm.Event ("FeverOn");
			GetComponent<Animation>().Stop();
			StartCoroutine("FeverRotateAni");

		}else{
			customerHead.SetSprite(prefixName+"_face");
//			gameObject.GetComponent<PlayMakerFSM>().Fsm.Event("FeverOff");
			tapEffect.transform.localEulerAngles = new Vector3(0f,0f,0f);
			StopCoroutine("FeverRotateAni");
			//Invoke ("PlayIdle",Random.Range (0.0f,0.5f));
			GetComponent<Animation>().Play (GetCustomerType().ToString()+"_Idle");
		}
	}

	virtual public IEnumerator FeverRotateAni(){
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

			if(tapEffect.transform.localEulerAngles.z >20f && tapEffect.transform.localEulerAngles.z<90f){
				direction = -1f;
				tapEffect.transform.localEulerAngles = new Vector3(0f,0f,20f);
			}
			if(tapEffect.transform.localEulerAngles.z<-20f+360f && tapEffect.transform.localEulerAngles.z>180f){
				direction = 1f;
				tapEffect.transform.localEulerAngles = new Vector3(0f,0f,-20f);
			}
			yield return null;
		}
	}

	virtual public void FeverAttack(){

		speed = 250f;
		hp -= 25f;

	}

	public void DespawnChar(){
		SpawnPool pools = PoolManager.Pools["Customer"];

		pools.Despawn (transform);

	}

	virtual public void InFrontOfDesk(){

		BadEffect ();
	}
	virtual public void BadEffect(){

	}
	virtual public void EndPayment(){
	
	}
	virtual public void DespawnBadEffect(){

	}

/*	virtual IEnumerator FeverRotAni(){



		yield return null;

	}
*/		
	public CustomerType GetCustomerType(){

		return customerType;
	}

	public float GetHP(){
		return hp;
	}

	//fever 및 move forward / move out 애니메이션은 Queue에서 조절(FSM).


	virtual public int GetNormalType(){
		return -1; //normal이 아닐때.
	}
	virtual public int GetBadType(){
		return -1; //bad가 아닐때. //ignorant 1 / homeless 2 / thief 3 / skeptic 4.
	}

}
