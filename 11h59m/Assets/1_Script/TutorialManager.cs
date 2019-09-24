using UnityEngine;
using System.Collections;

public class TutorialManager : Singleton<TutorialManager> {

	public GameObject tutorialGO;
	public Transform mainSpawnPoint;

	private GameObject mTutorial = null;

	public void ShowStartTutorial(){

		if(!SaveManager.GetStartTutorial()){
			mTutorial = (GameObject) Instantiate(tutorialGO,Vector3.zero,Quaternion.identity);
			mTutorial.transform.parent = mainSpawnPoint;
			mTutorial.transform.localPosition = new Vector3(0f,40f,0f);
			mTutorial.transform.localScale = new Vector3(1.6875f,1.6875f,1f);
			mTutorial.GetComponent<Tutorial>().ShowTutorial("start",1,6);
		}
	}

	public void ShowGameTutorial(){

		if(!SaveManager.GetGameTutorial()){
			//game 화면에 처음으로 들어갈때.
			StartCoroutine(WaitAndShowGameTutorial());
		}
	}

	private IEnumerator WaitAndShowGameTutorial(){
		yield return new WaitForSeconds(3f);
		mTutorial = (GameObject) Instantiate(tutorialGO,Vector3.zero,Quaternion.identity);
		mTutorial.transform.parent = mainSpawnPoint;
		mTutorial.transform.localPosition = new Vector3(1046f,40f,0f);
		mTutorial.transform.localScale = new Vector3(1.6875f,1.6875f,1f);
		mTutorial.GetComponent<Tutorial> ().ShowTutorial ("game", 2, 3);

	}
	public void ShowFeverTutorial(){

		if (!SaveManager.GetFeverTutorial ()) {
			mTutorial = (GameObject) Instantiate(tutorialGO,Vector3.zero,Quaternion.identity);
			mTutorial.transform.parent = mainSpawnPoint;
			mTutorial.transform.localPosition = new Vector3(1046f,40f,0f);
			mTutorial.transform.localScale = new Vector3(1.6875f,1.6875f,1f);
			mTutorial.GetComponent<Tutorial>().ShowTutorial("fever",3,5);

		}


	}

	public void ShowMissionTutorial(){
		if(!SaveManager.GetMissionTutorial()){

			mTutorial = (GameObject) Instantiate(tutorialGO,Vector3.zero,Quaternion.identity);
			mTutorial.transform.parent = CollectionCtrl.Instance.gameObject.transform;
			mTutorial.transform.localPosition = new Vector3(0f,-535f,0f);
			mTutorial.transform.localScale = new Vector3(1.6875f,1.6875f,1f);
			mTutorial.GetComponent<Tutorial>().ShowTutorial("col",4,2);
		}


	}




	public void DestroyMissionTutorial(){
		if (!SaveManager.GetMissionTutorial ()) {

			if (mTutorial != null) {
				Destroy((Object) mTutorial);
				
			}

		}

	}


}
