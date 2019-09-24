using UnityEngine;
using System.Collections;

public class ToastMessageToExit : MonoBehaviour {


	public UILabel toastMsg;

#if UNITY_ANDROID
	void Start(){
		toastMsg.text = Localization.Get ("toastMsg");
		StartCoroutine (WaitAndDestroyMsg ());
	}

	void Update () {
	
		if(Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit();
		}
	}

	private IEnumerator WaitAndDestroyMsg(){
		yield return new WaitForSeconds(2f);
		SceneManager.Instance.OffToastMsg ();
		Destroy ((Object) this.gameObject);

	}
#endif
}
