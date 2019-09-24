using UnityEngine;
using System.Collections;

public class tempBtn : MonoBehaviour {


	public Receipt RC;
	public int level;

	void Start(){

		level =1;
	}

	public void Reset(){

		level++;
//		RC.MakeNewReceipt(false);

	}
}
