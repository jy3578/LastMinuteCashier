using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Advertisements;

public class ContinuePopup : MonoBehaviour {
		public UILabel continueMsg;
		public PlayMakerFSM closing;

		private bool   first = true;
		ShowOptions AdsOpt = new ShowOptions();

		void OnEnable()
		{
				first = true;
				AdsOpt.resultCallback = UnityAdsCallback;
				GetComponent<Animation>().Play ("tutorial_show");
				GAManager.Instance.GAUnityadsEvent("popup");
				continueMsg.text = "15 more seconds, after watching video?";
		}

		public void ShowVideo()
		{
				SoundManager.PlaySFX ("button_click");
				if(first)
				{
						first = false;

						GAManager.Instance.GAUnityadsEvent("show");
						int playCount = PlayerPrefs.GetInt("Play Unity Ads", 0);
						playCount++;
						PlayerPrefs.SetInt("Play Unity Ads", playCount);
						PlayerPrefs.Save();

						if(Advertisement.IsReady()) 
						{								
								Advertisement.Show(null, AdsOpt);
						}
						else NoContinue();
				}
		}

		public void CloseVideo()
		{
				SoundManager.PlaySFX ("button_click");
				if(first)
				{
						first = false;
						NoContinue();
				}
		}

		void UnityAdsCallback(UnityEngine.Advertisements.ShowResult result)
		{
				if( result != UnityEngine.Advertisements.ShowResult.Failed )
				{
						closing.SendEvent ("YesContinue");
						GAManager.Instance.GAUnityadsEvent("complete");
						AfterWatchingAds ();
				}
				else
				{
						NoContinue();
				}
		}

		private void NoContinue()
		{
				closing.SendEvent ("NoContinue");
		}

		public void AfterWatchingAds()
		{
				closing.SendEvent ("EndWatchingAds");
		}

		private IEnumerator CloseAni()
		{
				GetComponent<Animation>().Play ("tutorial_close");
				yield return new WaitForSeconds(GetComponent<Animation>().GetClip("tutorial_close").length);
				this.gameObject.SetActive(false);
		}
}