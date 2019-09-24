using UnityEngine;
using System.Collections;

public class Receipt : Singleton<Receipt> {

	public PlayMakerFSM playMaker;

	public int payment;
	public int price;
	public int change;

	private int prevChange; // 이전에 냈던 거스름돈.


	public UILabel paymentLb;
	public UILabel priceLb;
	public UILabel changeLb;

	private bool clickable;
	private string lang;
	private string receiptSpriteName;

	public void ReceiptLocalize(){
		lang = SaveManager.GetLanguage ();

		receiptSpriteName = "play_ui_receipt";

		if(lang=="English"){
			paymentLb.transform.localPosition = new Vector3(150f, paymentLb.transform.localPosition.y,0f);
			priceLb.transform.localPosition = new Vector3(150f,priceLb.transform.localPosition.y,0f);
			changeLb.transform.localPosition = new Vector3(150f, changeLb.transform.localPosition.y,0f);
			receiptSpriteName = "play_ui_receipt_en";
		}
	}


	public void ResetReceipt(){
		payment = 0;
		price = 0;
		change =0;
		prevChange=0;

		gameObject.GetComponent<UISprite>().spriteName="play_ui_receipt_ready";

		paymentLb.text = "";
		priceLb.text = "";
		changeLb.text = "";
		gameObject.transform.GetComponentInParent<BoxCollider2D>().enabled = true;
		clickable = false;
	}

	public void GoReady(){

		playMaker.SendEvent("GoReady");
	}

	public void ChangeSpriteAfterReady(){

		GetComponent<UISprite>().spriteName = receiptSpriteName;
	}

	public void AfterEndPayment(){
		//Start to animation "throw out the receipt paper" and "MakeNewReceipt".
		playMaker.SendEvent("EndPayment");

	}

	public void MakeNewReceipt(){
		int level = GameManager.Instance.level;

		MakeRandomPrice(level);

	}

	private void MakeRandomPrice(int level){
	
		//첫째항이 500, 그리고 계차가 300k.

		int minPrice = 500 + 175 * (level*level - level);
		int maxPrice = minPrice + level * 350;

		int tempPrice = Random.Range (minPrice,maxPrice);

		price = ReduceDigits (tempPrice);
		MakeRandomChange();
	}

	private void MakeRandomChange(){

		float minMultiplier = 0.20f;
		float maxMultiplier = 0.23f;

		int tempChange = (int) (( (float)price) * Random.Range (minMultiplier,maxMultiplier));

		change = ReduceDigits(tempChange);
		if(CompareToBefore()) MakeRandomChange ();
		MakePayment();

	}
	
	private void MakePayment(){

		payment = change + price;
		Display ();
		prevChange = change;
	}

	private int ReduceDigits(int number){

		if(number< 1200){ //1200원 이하일때는 10원 자리까지 나타냄.

			return (int)( (Mathf.Round(number/10f)) * 10 ); 

		}else if(number<12000){ // 1200~ 12000 사이에서는 100원자리 단위까지.

			return (int)( (Mathf.Round (number/100f))*100 );

		}else{//그 이상은 1000단위부터 나타냄.

			return (int) ((Mathf.Round (number/1000f))*1000 );

		}
	}

	private bool CompareToBefore(){

		if(change == prevChange) return true;

		return false;
	}
	//진상 커플을 위한 method.
	public void AfterDecisionOfBadSkeptic(){
		playMaker.SendEvent("BadSkeptic");
	}
	public void MakeNewReceiptForBadSkeptic(){
	//change를 좀더 증가시키는 방향으로.	
		if(price<1000){
			change += 10* Random.Range (1,10);
		}else if(price <5000){
			change += 50* Random.Range (1,10);
		}else if(price<10000){
			change += 100*Random.Range(1,10);
		}else{ // 10000원 이상일때.
			change += 500*Random.Range(1,10);
		}

		MakePayment();
	}

	public void ConfirmPayToGM(){
		if(clickable){
			if(!GameManager.Instance.onFeverMode){

				GameManager.Instance.ConfirmPay();

			}
		}
	}

	public void StopClick(bool stopC){
		clickable = !stopC;
	}
	
	private void Display(){
		if(lang != "English"){
			paymentLb.text = payment.ToString();
			priceLb.text = price.ToString ();
			changeLb.text = change.ToString();
		}else{ // english.
			float paymentUS = (float) payment/1000f;
			float priceUS = (float) price / 1000f;
			float changeUS = (float) change / 1000f;

			paymentLb.text = paymentUS.ToString("F2");
			priceLb.text = priceUS.ToString("F2");
			changeLb.text = changeUS.ToString("F2");
		}
	}

	public int GetChange(){
		return change;
	}
}
