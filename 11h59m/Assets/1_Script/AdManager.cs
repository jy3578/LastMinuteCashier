using UnityEngine;
using System.Collections;
using Prime31;
using ChartboostSDK;
using UnityEngine.Advertisements;

public class AdManager : Singleton<AdManager>{

   	public UISprite iAPBtn;
	//	public GameObject iAPBtn;
	public GameObject restoreBtn;

	public GameObject inPlayAd; // prefab
	public Transform inPlayAdSpawnPoint;

	private GameObject mInplayAd = null;

	private GameObject mContinue;

	void Awake(){
//		Chartboost.didFailToLoadInterstitial += didFailToLoadInterstitial;
		Chartboost.didCloseInterstitial += didCloseInterstitial;
		Chartboost.didClickInterstitial += didClickInterstitial;
		Chartboost.didDisplayInterstitial += didDisplayInterstitial;
//		Chartboost.didCacheInterstitial -= didCacheInterstitial;
	}
	
	void Start(){
		ShowBanner ();
		LoadInterstitials ();
	}

	public void CheckPurchaseAtStart(){   //called from the onBillerReady.
		bool purchaseItem = BillingManager.instance.HasPurchasedNonconsumable("remove_ads");
		if (purchaseItem) {
			SaveManager.SetRemoveAdsPurchased (true);
			DestroyAds();
			HidePurchaseBtn ();
		}
		else
		{
			SaveManager.SetRemoveAdsPurchased (false);
			ShowBanner ();
			LoadInterstitials ();
		}
		ShowBanner ();
		LoadInterstitials ();
	}

	private void ShowBanner(){
	
		if(!SaveManager.GetRemoveAdsPurchased()){
			AdMob.createBanner( "ca-app-pub-6553814907597998/7730326661", "ca-app-pub-6553814907597998/3300127061", AdMobBanner.SmartBanner, AdMobLocation.BottomCenter );
		}
	}

	public void DestroyAds(){

		if(SaveManager.GetRemoveAdsPurchased()){
			AdMob.destroyBanner();
			HidePurchaseBtn();
			if(mInplayAd !=null){
				Destroy((Object)mInplayAd);
			}
		}

	}

	public void HidePurchaseBtn(){
		 iAPBtn.spriteName = "main_ui_restore";
		 iAPBtn.MakePixelPerfect ();
	//	iAPBtn.SetActive (false);
		restoreBtn.SetActive (false);

	}

	public void LoadInterstitials(){
		if(!SaveManager.GetRemoveAdsPurchased()){
			if(!AdMob.isInterstitalReady()){
				AdMob.requestInterstital("ca-app-pub-6553814907597998/1683793064","ca-app-pub-6553814907597998/9207059865");
			}
			if(!Chartboost.hasInterstitial(CBLocation.GameOver)){
				Chartboost.cacheInterstitial(CBLocation.GameOver);
			}
	//		Inplay.Cache();
		}
	}


	public bool ShowInterstitials(){
		//게임끝난 후 & exit from the collection scene 호출.

				float adRatio = 0.3f;
				float typeRatio1 = 0.5f; // type1 : admob과 chartboost 광고 나올 비율.

				if (Random.Range (0.00f, 1.00f) <= adRatio) {
						//광고 show!
						if (Random.Range (0.00f, 1.00f) <= typeRatio1) {
								StartCoroutine ("WaitAndShowAdMob");
						} else {
								StartCoroutine ("WaitAndShowCB");
						}
						return true;
				} else {
						return false;
				}


	/*
		if(!SaveManager.GetRemoveAdsPurchased()){
			ShowInPlayAd();

			int stopAd = Config.GetInt("ad_stop",-1);    // -1 : 광고 게재 x , 0 : 모든 type 광고 게재, 1: admob 광고 제외. 2: chartboost 광고 제외.

			if(stopAd != -1){
				float adRatio = Config.GetFloat("ad_ratio",0.20f);
				float typeRatio1 = Config.GetFloat("ad_ratio_type1",0f); // type1 : admob과 chartboost 광고 나올 비율.

				if(Random.Range (0.00f,1.00f)<= adRatio){
					//광고 show!
					if(Random.Range (0.00f,1.00f) <= typeRatio1){
						StartCoroutine("WaitAndShowAdMob");
					}else{
						StartCoroutine("WaitAndShowCB");
					}
					return true;
				}
			}

		}
		return false;
		*/
	}


	private void ShowInPlayAd(){
		if(!SaveManager.GetRemoveAdsPurchased()){
			if(Inplay.IsCached()){
				CachedInplay inplay = Inplay.Get ();
				SetInPlayAd(inplay);
			}
		}
	}


	public void SetInPlayAd(CachedInplay inplay){  //call from ShowInPlayAd & after click the inplayad.
		if(inplay != null){
			AdMob.destroyBanner (); //inplay광고로 대체됨.
			if(mInplayAd != null){
				GameObject.Destroy(mInplayAd);
				mInplayAd = null;
			}

			mInplayAd = (GameObject) Instantiate(inPlayAd,Vector3.zero,Quaternion.identity);
			mInplayAd.transform.parent = inPlayAdSpawnPoint;
			mInplayAd.GetComponent<InPlayAd>().SetInplayData(inplay);
			inplay.Show();
		}else{
			if(mInplayAd != null){

				GameObject.Destroy(mInplayAd);
				mInplayAd = null;
				ShowBanner();
			}
		}
	}


	private IEnumerator WaitAndShowAdMob(){
		float delay = Config.GetFloat("ad_delay",0.2f);
		yield return new WaitForSeconds(delay);
		if(AdMob.isInterstitalReady())
		{
			int playCount = PlayerPrefs.GetInt("Play Admob Ads", 0);
			playCount++;
			PlayerPrefs.SetInt("Play Admob Ads", playCount);
			PlayerPrefs.Save();

			AdMob.displayInterstital();
			GAManager.Instance.GAAdMobEvent("display");
		}
		else if(Chartboost.hasInterstitial(CBLocation.GameOver))
		{ //load가 안되었을 경우 대비.
			int playCount = PlayerPrefs.GetInt("Play Chartboost Ads", 0);
			playCount++;
			PlayerPrefs.SetInt("Play Chartboost Ads", playCount);
			PlayerPrefs.Save();

			Chartboost.showInterstitial(CBLocation.GameOver);
		}
	}


	private IEnumerator WaitAndShowCB(){
		float delay = Config.GetFloat("ad_delay",0.2f);
		yield return new WaitForSeconds(delay);
		if(Chartboost.hasInterstitial(CBLocation.GameOver))
		{
			int playCount = PlayerPrefs.GetInt("Play Chartboost Ads", 0);
			playCount++;
			PlayerPrefs.SetInt("Play Chartboost Ads", playCount);
			PlayerPrefs.Save();

			Chartboost.showInterstitial(CBLocation.GameOver);
			
		}
		else if(AdMob.isInterstitalReady())
		{
			int playCount = PlayerPrefs.GetInt("Play Admob Ads", 0);
			playCount++;
			PlayerPrefs.SetInt("Play Admob Ads", playCount);
			PlayerPrefs.Save();
						
			AdMob.displayInterstital();
			GAManager.Instance.GAAdMobEvent("display");
		}
	}
	

	private void interstitialFailedToReceiveAdEvent( string error )
	{
		AdMob.requestInterstital("ca-app-pub-6553814907597998/1683793064","ca-app-pub-6553814907597998/9207059865");
	}
	
	public void didCloseInterstitial(CBLocation location){
		GAManager.Instance.GAChartBoostEvent("close");
	}

	public void didClickInterstitial(CBLocation location){
		GAManager.Instance.GAChartBoostEvent("click");
	}

	public void didDisplayInterstitial(CBLocation location){
		GAManager.Instance.GAChartBoostEvent("display");
	}

}
