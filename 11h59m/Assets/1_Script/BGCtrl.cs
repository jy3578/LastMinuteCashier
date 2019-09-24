using UnityEngine;
using System.Collections;

public class BGCtrl : MonoBehaviour {


	public Transform[] clouds1; //speed1로 진행할 cloud 그룹.
	public Transform[] clouds2; // speed2로 진행할 cloud 그룹.
	public Transform moon;
	public Transform shutter;
	public UISprite sky;


	public float speed1;
	public float speed2;


	void Start(){

		StartCoroutine ("MoveCloud");
	}


	private IEnumerator MoveCloud(){


		while(true){
			foreach(Transform cloud in clouds1){

				cloud.Translate(new Vector3(-0.005f,0f,0f)*speed1);
				if(cloud.localPosition.x <-100) cloud.localPosition = new Vector3(1200f,cloud.localPosition.y,cloud.localPosition.z);

			}

			foreach(Transform cloud in clouds2){

				cloud.Translate(new Vector3(-0.005f,0f,0f)*speed2);
				if(cloud.localPosition.x <-100) cloud.localPosition = new Vector3(1200f,cloud.localPosition.y,cloud.localPosition.z);


			}
			yield return new WaitForSeconds(0.05f);
		}
	}



	public IEnumerator MoveBG(){


		float distanceXM = (1200f-(-330f))/60f; // moon.
		float distanceYM = -300f /60f;
		float distanceYS = -(sky.localSize.y-10f)/60f; //shutter.
	
		GameManager GM = GameManager.Instance;

		while(true){
			if(GM.GS != GameState.Play) break; //

			moon.localPosition = new Vector3(moon.localPosition.x+distanceXM,moon.localPosition.y+distanceYM,0f);
			shutter.localPosition = new Vector3(800f,shutter.localPosition.y+distanceYS,0f);

			if(shutter.localPosition.y < (distanceYS * 60f)){
				shutter.localPosition = new Vector3(800f,distanceYS*60f,0f);
				break;
			}

			yield return new WaitForSeconds(1f);
		}
	}
	
	public void ResetBGPosit(){

		moon.localPosition = new Vector3(0f,0f,0f);
		shutter.localPosition=new Vector3(800f,0f,0f);

	}
}
