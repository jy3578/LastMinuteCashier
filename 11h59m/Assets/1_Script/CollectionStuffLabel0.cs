using UnityEngine;
using System.Collections;

public class CollectionStuffLabel0 : CollectionStuffLabel {
	
	public override void ShowStuffOnDisplay () // 처음 호출시.
	{
		MS = (MissionState) ((int)SaveManager.GetCollectionStatesInCategory(category));
		int unlock = SaveManager.GetCollectionUnlockedInCategory(category);
		if(MS == MissionState.Complete){
			for (int i=0;i<stuffOnDisplay.Length;i++){
				stuffOnDisplay[i].GetComponent<Renderer>().enabled = true;
				stuffOnDisplay[i].SetSprite("collection_icon01"+"_"+(i+1).ToString());
			}
			complete.SetActive (true);
			missionBG.SetActive (false);

		}else{
			stuffOnDisplay[unlock].SetSprite("collection_boxL");
			stuffOnShelf = stuffOnDisplay[unlock];

			for(int i=0;i<stuffOnDisplay.Length;i++){
				if(i<unlock){
					stuffOnDisplay[i].GetComponent<Renderer>().enabled = true;
					stuffOnDisplay[i].SetSprite("collection_icon"+(category+1).ToString("D2")+"_"+(i+1).ToString());
				}else{
					stuffOnDisplay[i].GetComponent<Renderer>().enabled = false;
				}
			}
		}
	}


	public override void ShowStuffOnShelf ()
	{	
		int state = SaveManager.GetCollectionStatesInCategory(category);
		MS = (MissionState) state;

		int unlocked = SaveManager.GetCollectionUnlockedInCategory(category);


		if(MS != MissionState.Complete){
			targetNumber.text = MissionManager.Instance.GetMissionObjective(category).ToString();
			myNumber.text = ((int)SaveManager.GetMyNumbers(category)).ToString();
			stuffOnShelf = stuffOnDisplay[unlocked];
			stuffOnShelf.GetComponent<Animation>().Stop();
		}

		ShowStampOff();


		switch(state){
		case 3: // success.
			ShowSuccessStamp();
			stuffOnShelf.GetComponent<Renderer>().enabled = true;
			stuffOnShelf.SetSprite("collection_boxL");
			stuffOnShelf.GetComponent<Animation>().Play ("newBoxIdle");
			break;
		case 4: //complete.
			missionBG.SetActive (false);
			complete.SetActive (true);
			break;
		}

	}
	
	public override void UnlockAnimation ()
	{

		int unlocked = SaveManager.GetCollectionUnlockedInCategory(category);

		stuffOnShelf = stuffOnDisplay[unlocked]; //원래물건.

		stuffOnShelf.GetComponent<Animation>().Stop ();
		stuffOnShelf.transform.localScale = new Vector3(1f,1f,1f);
		
		newEffect = paperPool.Spawn ("OpenBox"+boxSize.ToString(),stuffOnShelf.transform);
		newEffect.localPosition = new Vector3(0f,0f,0f);
		newEffect.localScale = new Vector3(1f,1f,1f);
		stuffOnShelf.GetComponent<Renderer>().enabled = false;
		
		newEffect.gameObject.GetComponent<tk2dSpriteAnimator>().Play ();
		StartCoroutine("WaitAndDespawnEffect",unlocked);
		
		MissionManager.Instance.UnlockAndChangeState(category,MT);
	}

	protected override IEnumerator WaitAndDespawnEffect(int unlock){
		yield return new WaitForSeconds(0.1f);
		stuffOnShelf = stuffOnDisplay[unlock];
		stuffOnShelf.GetComponent<Renderer>().enabled = true;
		stuffOnShelf.SetSprite("collection_icon"+(category+1).ToString("D2")+"_"+(unlock+1).ToString()); //물건 unlock되고.
		yield return new WaitForSeconds(0.9f);
		paperPool.Despawn(newEffect,CollectionCtrl.Instance.gameObject.transform);

	}

	public override void ShowNumbersOnly ()
	{
		MS = (MissionState) ((int)SaveManager.GetCollectionStatesInCategory(category));
		int unlocked = SaveManager.GetCollectionUnlockedInCategory(category);
		if(MS == MissionState.Complete){
			missionBG.SetActive(false);
			complete.SetActive(true);
			missionLv.text ="MAX";
		}else{
			targetNumber.text = MissionManager.Instance.GetMissionObjective(category).ToString();
			myNumber.text = ((int)SaveManager.GetMyNumbers(category)).ToString();
			missionLv.text = "Lv"+(SaveManager.GetCollectionUnlockedInCategory(category)+1).ToString();
			stuffOnShelf = stuffOnDisplay[unlocked];
		}
	}

}
