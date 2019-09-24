using UnityEngine;
using System.Collections;
using PathologicalGames;

public class MissionPaperPopUp : MonoBehaviour {

	public UISprite icon;
	public Animation exitBtnAnim;
	public UILabel target;
	public UILabel my;
	public UILabel title;
	public UILabel unit;

	public CollectionStuffLabel stuffLabel;

	protected bool OnClose;
	protected int category;
	protected MissionState MS;

	void OnEnable(){
		OnClose = false;
	}

	//mission paper가 닫히는데에는 2가지 route가 존재.
	//1) 열린 상태에서, 다른 label을 눌러서 닫히는 경우 flow : CollectionCtrl 콜 -> CollectionStuffLabel -> MissionPaperPopup .
	//2) 해당 paper를 클릭하여 닫는 경우 flow : MissionPaperPopUp -> CollectionCtrl -> CollectionStuffLabel.

	//닫힘과 관련된 메소드.
	public void CloseMissionPaper(){ // 1).
		if(!OnClose){
			OnClose = true;
			StartCoroutine("WaitForDespawnPopup");
		}
	}
	public void CloseMissionPaperbyClick(){  //2).
		CollectionCtrl.Instance.CloseMissionPaperbyClick();
		exitBtnAnim.Play ("BtnClick");
		CloseMissionPaper();
	}

	protected IEnumerator WaitForDespawnPopup(){
		GetComponent<Animation>().Play("mission_Close");
		yield return new WaitForSeconds(GetComponent<Animation>().GetClip ("mission_Close").length);
		PoolManager.Pools["Mission"].Despawn(transform,CollectionCtrl.Instance.gameObject.transform);
	}

	virtual public void SetDataInPaper(MissionState newMS, int category, int unlocked){

	}

	virtual public void ClickTryBtn(){

	}
}
