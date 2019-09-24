using UnityEngine;
using System.Collections;
using PathologicalGames;


public class TraySlot : MonoBehaviour {

	public int slotPrice;
	public MoneyType moneyType;	

	public SlotState slotstt;
	public int numberOfMoney;
	

	public WalletSlot pairOnWallet;
	private Tray tray;
	private SpawnPool pool;

	private int localizedSlotPrice;

	public Transform[] moneys;
	
	void Start(){


		moneyType = pairOnWallet.moneyType;

		tray = Tray.Instance;
			
		slotstt = SlotState.HaveNoMoney;
		numberOfMoney = 0;

		pool = PoolManager.Pools["TrayMoney"];

		moneys = new Transform[5];
		//InitializeMoney();

	}

	public void LocalizeMoney(string lang){
		localizedSlotPrice = slotPrice;
		if(lang == "English" && slotPrice == 500){
			localizedSlotPrice = 250;
		}
	}


	public void InitializeMoney(){

		CancelAll ();

	}
	
		
	public int GetValue(){
		
		return numberOfMoney * slotPrice;
	}


	public void MakeMoney(){


		if(slotstt == SlotState.HaveNoMoney){

			slotstt = SlotState.HaveMoney;
			tray.PlaceDeck(gameObject.transform, moneyType);

		}
		numberOfMoney += 1;


		if(numberOfMoney <= 5){
		
			moneys[numberOfMoney-1] = pool.Spawn("MoneyT"+slotPrice.ToString(), transform);
			moneys[numberOfMoney-1].localScale = new Vector3(960f,960f,1f);
			moneys[numberOfMoney-1].gameObject.GetComponent<TrayMoney>().SortingImgInDeck(numberOfMoney); //sorting image.

			PlaceMoney();
		}
		tray.AddToValueOnTray(localizedSlotPrice);

	}
	
	public void RemoveMoney(){
	
		if(slotstt == SlotState.HaveMoney){
			pool.Despawn(moneys[numberOfMoney-1],tray.transform);
			numberOfMoney -= 1;
			if(numberOfMoney == 0) slotstt = SlotState.HaveNoMoney;
		}		
	}

	public void CancelAll(){

		if(numberOfMoney>5) numberOfMoney = 5; //despawn해야 할 money prefab은 5개 이므로.

		while(slotstt==SlotState.HaveMoney){

			RemoveMoney();
			
		}
		
	}

	public void PlaceMoney(){
		//money가 추가될때마다, 재배치.
	
		float gap = 13f;

		if(moneyType == MoneyType.Bill){
			gap = tray.gapBtwBill;

		}


		switch(numberOfMoney)
		{
		case 1:
			moneys[0].localPosition = Vector3.zero;
			break;
		case 2:
			moneys[0].localPosition = new Vector3(-gap*0.5f,0f,0f);
			break;
		case 3:
			moneys[0].localPosition = new Vector3(-gap,0f,0f);
			break;
		case 4:
			moneys[0].localPosition = new Vector3(-gap*1.5f,0f,0f);
			break;
		case 5:
			moneys[0].localPosition = new Vector3(-2f*gap,0f,0f);
			break;
		}


		float pivotX = moneys[0].transform.localPosition.x;


		for(int i=1 ; i<numberOfMoney ; i++){

			moneys[i].localPosition = new Vector3(pivotX + ((float)i) * gap, 0f, 0f);
		
		}

	}


}
