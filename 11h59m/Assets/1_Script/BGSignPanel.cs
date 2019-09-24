using UnityEngine;
using System.Collections;

public class BGSignPanel : Singleton<BGSignPanel> {

	public Animation anim;

	void Start(){
		SignStartAni();
	}

	public void SignStartAni(){
		anim.Play("Sign_Start");
	}

	public void MoveToCollection(){
		anim.Play ("Sign_MoveOut");
	}

	public void MoveOutCollection(){
		anim.Play ("Sign_MoveIn");
	}

	public void SignClicked(){
		anim.Stop ();
		anim.Play ("Sign_Clicked");
	}
}
