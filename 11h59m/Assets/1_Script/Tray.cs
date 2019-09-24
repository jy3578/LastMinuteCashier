using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;


public class Tray : Singleton<Tray> {


	public UILabel valueLb;
	
	private int kindsOfBill;
	private int kindsOfCoin;
	
	public float gapBtwBill;  //지폐 1장 1장 사이의 간격.
	public float gapBtwCoin;  //coin deck과 coin deck 사이의 간격.
	public float gapBtwDeck;  //지폐 deck과 지폐 deck 사이의 간격.

	private int totalValue;
	private string lang;

	List<Transform> decksOfCoinOnTray;
	List<Transform> decksOfBillOnTray;

	void Awake(){

		kindsOfBill = 0;
		kindsOfCoin = 0;

		gapBtwBill = 0f;
		gapBtwCoin = 0f;
		gapBtwDeck = 0f;

		totalValue = 0;
		valueLb.text = "0";

		decksOfBillOnTray = new List<Transform>();
		decksOfCoinOnTray = new List<Transform>();

	}

	public void SetLocalMoney(string language){
		lang = language; //deck의 value 표시되는 부분때문.
	}

	public void InitializeTray(){
	
		kindsOfBill = 0;
		kindsOfCoin = 0;

		gapBtwBill = 0f; 
		gapBtwCoin = 0f;
		gapBtwDeck = 0f;

		totalValue = 0;
		valueLb.text = "0";

		RemoveAll ();

		if(decksOfBillOnTray.Count>0)	decksOfBillOnTray.Clear ();

		if(decksOfCoinOnTray.Count >0)	decksOfCoinOnTray.Clear ();

	}


	public void PlaceDeck(Transform slotPosition, MoneyType moneyType){


		if(moneyType == MoneyType.Bill){

			kindsOfBill++;
			decksOfBillOnTray.Add (slotPosition);

		}else{
			kindsOfCoin++;
			decksOfCoinOnTray.Add(slotPosition);
		}
		CalculateGapsAndPlaceDecks();

	}

	public void CalculateGapsAndPlaceDecks(){

	
		gapBtwDeck = (90f - 10f * (float)kindsOfBill) * 1.6875f; //사이즈 문제 때문에 1.6875로 조정.
		gapBtwCoin = (48f - 7.5f* (float)kindsOfCoin) * 1.6875f; 
		gapBtwBill = (12f - 1.5f * (float)kindsOfBill) * 1.6875f;

		if(kindsOfBill>0){
			switch(kindsOfBill)
			{
			case 1:
				decksOfBillOnTray[0].localPosition = new Vector3(0f,0f,0f);
				break;

			case 2:
				decksOfBillOnTray[0].localPosition = new Vector3(-gapBtwDeck * 0.5f, 0f, 0f);
				break;
			case 3:
				decksOfBillOnTray[0].localPosition = new Vector3(-gapBtwDeck * 1f,0f,0f);
				break;
			case 4:
				decksOfBillOnTray[0].localPosition = new Vector3(-gapBtwDeck * 1.5f,0f,0f);
				break;
			}

	
			float pivotX = decksOfBillOnTray[0].localPosition.x;
		

			for(int i = 1; i< decksOfBillOnTray.Count;i++){

				decksOfBillOnTray[i].localPosition = new Vector3( pivotX + gapBtwDeck *  (float)i, 0f, 0f);
			
			}

		}

		if(kindsOfCoin>0){

			switch(kindsOfCoin)
			{
			case 1:
				decksOfCoinOnTray[0].localPosition = new Vector3 (230f,0f,0f);
				break;
			case 2:
				decksOfCoinOnTray[0].localPosition = new Vector3 (230f,gapBtwCoin*0.5f,0f);
				break;
			case 3:
				decksOfCoinOnTray[0].localPosition = new Vector3 (230f,gapBtwCoin*1f,0f);
				break;
			case 4:
				decksOfCoinOnTray[0].localPosition = new Vector3 (230f,gapBtwCoin*1.5f,0f);
				break;
			}

			float pivotY = decksOfCoinOnTray[0].localPosition.y;

			for(int i=1;i<decksOfCoinOnTray.Count;i++){
				decksOfCoinOnTray[i].localPosition = new Vector3(230f, pivotY - gapBtwCoin* (float)i,0f);
		
			}
		}
	}
	


	public void RemoveAll(){


		foreach(Transform decks in decksOfBillOnTray){
			decks.gameObject.GetComponent<TraySlot>().InitializeMoney();
		}
		foreach(Transform decks in decksOfCoinOnTray){
			decks.gameObject.GetComponent<TraySlot>().InitializeMoney();
		}

	}

	public void AddToValueOnTray(int addedAmount){
		totalValue += addedAmount;
		if(lang != "English"){
			valueLb.text = totalValue.ToString ();
		}else{
			float totalValueUS = (float) totalValue / 1000f;
			valueLb.text = totalValueUS.ToString("F2");
		}

	}

	public int GetValueOnTray(){

		return totalValue;
	}


}
