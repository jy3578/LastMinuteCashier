using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.Purchasing;
using System;
using UnityEngine.Purchasing.Security;

public class BillingManager : MonoBehaviour, IStoreListener
{
	public const string kItemRemoveAds = "remove_ads";

	private static BillingManager _instance = null;
	public static BillingManager instance { 
		get {
			if(_instance == null)
				_instance = GameObject.FindObjectOfType<BillingManager>();
			return _instance;
		}
	}

	public static UnityAction<string> OnPurchasedItem { get; set; }

	private IStoreController m_StoreController = null;
	private IExtensionProvider m_StoreExtensionProvider = null;

	public bool isInitialized {
		get { return m_StoreController != null && m_StoreExtensionProvider != null; }
	}

	public void Start()
	{
		Initialize();
	}

	void Initialize()
	{
		var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

		builder.AddProducts(new ProductDefinition[] {
			new ProductDefinition(kItemRemoveAds, "com.papayacompany.lastminutecashier.removeads", ProductType.NonConsumable)
		});

		UnityPurchasing.Initialize(this, builder);
	}

	public void PurchaseItem(string itemId)
	{
		// If the stores throw an unexpected exception, use try..catch to protect my logic here.
		try
		{
			// If Purchasing has been initialized ...
			if(isInitialized)
			{
				// ... look up the Product reference with the general product identifier and the Purchasing system's products collection.
				Product product = m_StoreController.products.WithID(itemId);

				// If the look up found a product for this device's store and that product is ready to be sold ... 
				if(product != null && product.availableToPurchase)
				{
					Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
					m_StoreController.InitiatePurchase(product);
				}
				else // Otherwise ...
				{
					// ... report the product look-up failure situation  
					Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
				}
			}
			else
			{
				// ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or retrying initiailization.
				Debug.Log("BuyProductID FAIL. Not initialized.");
			}
		}
		catch(Exception e) // Complete the unexpected exception handling ...
		{
			// ... by reporting any unexpected exception for later diagnosis.
			Debug.Log("BuyProductID: FAIL. Exception during purchase. " + e);
		}
	}

	public void Restore()
	{
		// If Purchasing has not yet been set up ...
		if(!isInitialized)
		{
			// ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
			Debug.Log("RestorePurchases FAIL. Not initialized.");
			return;
		}

		// If we are running on an Apple device ... 
		if(Application.platform == RuntimePlatform.IPhonePlayer ||
			Application.platform == RuntimePlatform.OSXPlayer)
		{
			// ... begin restoring purchases
			Debug.Log("RestorePurchases started ...");

			// Fetch the Apple store-specific subsystem.
			var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
			// Begin the asynchronous process of restoring purchases. Expect a confirmation response in the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
			apple.RestoreTransactions(success => {
				OnCompleteRestore(success);

				// The first phase of restoration. If no more responses are received on ProcessPurchase then no purchases are available to be restored.
				Debug.Log("RestorePurchases continuing: " + success + ". If no further messages, no purchases available to restore.");
			});
		}
		// Otherwise ...
		else
		{
			// We are not running on an Apple device. No work is necessary to restore purchases.
			Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
		}
	}

	void OnCompleteRestore(bool success)
	{

	}

	bool ValidateReceipt(PurchaseEventArgs e)
	{
#if UNITY_EDITOR
		return true;

		// Unity IAP's validation logic is only included on these platforms.
#elif UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX
		// Prepare the validator with the secrets we prepared in the Editor
		// obfuscation window.
		var validator = new CrossPlatformValidator(GooglePlayTangle.Data(),
			AppleTangle.Data(), Application.bundleIdentifier);

		try
		{
			// On Google Play, result will have a single product Id.
			// On Apple stores receipts contain multiple products.
			var result = validator.Validate(e.purchasedProduct.receipt);
			Debug.Log("Receipt is valid. Contents:");
			foreach(IPurchaseReceipt productReceipt in result)
			{
				Debug.Log(productReceipt.productID);
				Debug.Log(productReceipt.purchaseDate);
				Debug.Log(productReceipt.transactionID);
			}
			// Unlock the appropriate content here.
			return true;
		}
		catch(IAPSecurityException)
		{
			Debug.Log("Invalid receipt, not unlocking content");
			return false;
		}
#endif
	}

	void OnPurchaseConsumable(string id, PurchaseEventArgs e)
	{
		
	}

	void OnPurchaseNoncomsumable(string id, PurchaseEventArgs e)
	{
		if(!HasPurchasedNonconsumable(id))
		{
			GAManager.Instance.GAPurchaseRemoveAds(true,true);
			SaveManager.SetRemoveAdsPurchased(true);
			AdManager.Instance.DestroyAds();
		}

		PlayerPrefs.SetInt(id, 1);
		PlayerPrefs.Save();
	}

	void OnPurchaseSubscription(string id, PurchaseEventArgs e)
	{
		
	}

	public bool HasPurchasedNonconsumable(string id)
	{
		return PlayerPrefs.GetInt(id, 0) > 0;
	}

	#region implements IStoreListener
	void IStoreListener.OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		m_StoreController = controller;
		m_StoreExtensionProvider = extensions;

		AdManager.Instance.CheckPurchaseAtStart ();
	}

	void IStoreListener.OnInitializeFailed(InitializationFailureReason error)
	{
		Debug.Log("IAP: Initialize Failed");
	}

	PurchaseProcessingResult IStoreListener.ProcessPurchase(PurchaseEventArgs e)
	{
		string itemId = e.purchasedProduct.definition.id;
		var type = e.purchasedProduct.definition.type;

		if(ValidateReceipt(e))
		{
			// Validation Success
			switch(type)
			{
				case ProductType.Consumable:
					OnPurchaseConsumable(itemId, e);
					break;
				case ProductType.NonConsumable:
					OnPurchaseNoncomsumable(itemId, e);
					break;
				case ProductType.Subscription:
					OnPurchaseSubscription(itemId, e);
					break;
			}

			if(OnPurchasedItem != null)
				OnPurchasedItem(itemId);
		}
		else
		{
			// Validation Failed
		}

		return PurchaseProcessingResult.Complete;
	}

	void IStoreListener.OnPurchaseFailed(Product i, PurchaseFailureReason p)
	{
		if(i.definition.storeSpecificId == "com.papayacompany.lastminutecashier.removeads")
		{
			GAManager.Instance.GAPurchaseRemoveAds(false,false);
		}
		Debug.Log("IAP: Purchase Failed");
	}
	#endregion
}
