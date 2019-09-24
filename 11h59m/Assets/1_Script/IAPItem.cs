using UnityEngine;
using System.Collections;

public class IAPItem : MonoBehaviour {

	public string itemID = "";
	
	public void PurchaseItem(){
		//		isPurchased = SaveManage
		SoundManager.PlaySFX("button_click");
		GAManager.Instance.GAMainBtnEvent("RemoveAdsClick");
		if (!SaveManager.GetRemoveAdsPurchased ()) {
			BillingManager.instance.PurchaseItem (itemID);
		}// else {
		//	BillingManager.Instance.RestorePurchaseItem();
		//}
		
	}

	public void RestoreAll(){

		SoundManager.PlaySFX("button_click");
		GAManager.Instance.GAMainBtnEvent("RestoreClick");
		if(!SaveManager.GetRemoveAdsPurchased()){
			BillingManager.instance.Restore();
		}
	}
}
