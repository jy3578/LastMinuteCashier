using UnityEngine;
using System.Collections;

public class RepositFeverBallLasers : MonoBehaviour {

	public GameObject[] Lasers;


	public void OnEnable(){

		StartCoroutine ("Reposit");

	}

	IEnumerator Reposit(){

		while(true){

			for(int i=0;i<Lasers.Length;i++){
				Lasers[i].transform.localRotation = Quaternion.Euler (new Vector3(0f,0f, (float) Random.Range (-70,70)));
				Lasers[i].transform.localScale = new Vector3((float) Random.Range (0.8f,1.3f),1f,1f);

				switch(Random.Range (1,7)){
				case 1: 
					Lasers[i].GetComponent<tk2dSprite>().color = new Color(Random.Range (0f,1f),1f,1f,1f);
					break;
				case 2:
					Lasers[i].GetComponent<tk2dSprite>().color = new Color(1f,Random.Range (0f,1f),1f,1f);
					break;
				case 3:
					Lasers[i].GetComponent<tk2dSprite>().color = new Color(1f,1f,Random.Range (0f,1f),1f);
					break;
				case 4:
					Lasers[i].GetComponent<tk2dSprite>().color = new Color(Random.Range (0.5f,1f),Random.Range (0.5f,1f),1f,1f);
					break;
				case 5:
					Lasers[i].GetComponent<tk2dSprite>().color = new Color(1f, Random.Range (0.5f,1f),Random.Range (0.5f,1f),1f);
					break;
				case 6:
					Lasers[i].GetComponent<tk2dSprite>().color = new Color(Random.Range (0.5f,1f),1f,Random.Range (0.5f,1f),1f);
					break;
				}
			}
			yield return new WaitForSeconds(0.2f);
		}
	
	}
	

}
