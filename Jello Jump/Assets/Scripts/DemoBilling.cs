using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using OnePF;


public class DemoBilling : MonoBehaviour {



	public const string SKU = "no_ads";
	public const string googleKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAq10GM/8BMThZDCG7pkUHkx98KaHzWpyQYgwm/RXK5jle/gEjNOWch2qF+/IU3kCbdxrHGzCqD3dwppvEj4uSmdCr4h1Di6itIjkazA4KeHTa2pL830W+kR2+wCKlDfNctrKkOe1JNbEHE6Q0JH1oZKgcOzk7TqVzCAJSeKIubB/c3I71YdaSrD3gDsQQ+6Npuz8qTcaq9b6dCcdPYyxlZOm57nyHPePc406S33K5wANbH7XAOpUv6dWQP+7SmgK6rF1w1lJUv7zqtxKthDeuOeuA4DQp7MY7VYdXSNthmMkdPfgKkbh8Scv5qBMUajSM5FsF3nFJi6uht3Qx5g9w9QIDAQAB";
	//public const string iosKey = "";
	private void Awake()
	{
		// Subscribe to all billing events
		OpenIABEventManager.billingSupportedEvent += OnBillingSupported;
		OpenIABEventManager.billingNotSupportedEvent += OnBillingNotSupported;
		OpenIABEventManager.purchaseSucceededEvent += OnPurchaseSucceded;
		OpenIABEventManager.purchaseFailedEvent += OnPurchaseFailed;
		OpenIABEventManager.consumePurchaseSucceededEvent += OnConsumePurchaseSucceeded;
		OpenIABEventManager.consumePurchaseFailedEvent += OnConsumePurchaseFailed;
		OpenIABEventManager.transactionRestoredEvent += OnTransactionRestored;
		OpenIABEventManager.restoreSucceededEvent += OnRestoreSucceeded;
		OpenIABEventManager.restoreFailedEvent += OnRestoreFailed;
	}

	private void OnDestroy()
	{
		// Unsubscribe to avoid nasty leaks
		OpenIABEventManager.billingSupportedEvent -= OnBillingSupported;
		OpenIABEventManager.billingNotSupportedEvent -= OnBillingNotSupported;
		OpenIABEventManager.purchaseSucceededEvent -= OnPurchaseSucceded;
		OpenIABEventManager.purchaseFailedEvent -= OnPurchaseFailed;
		OpenIABEventManager.consumePurchaseSucceededEvent -= OnConsumePurchaseSucceeded;
		OpenIABEventManager.consumePurchaseFailedEvent -= OnConsumePurchaseFailed;
		OpenIABEventManager.transactionRestoredEvent -= OnTransactionRestored;
		OpenIABEventManager.restoreSucceededEvent -= OnRestoreSucceeded;
		OpenIABEventManager.restoreFailedEvent -= OnRestoreFailed;
	}

	void Start() 
	{

		OpenIAB.mapSku(SKU, OpenIAB_Android.STORE_GOOGLE, "no_ads");
		OpenIAB.mapSku(SKU, OpenIAB_iOS.STORE, "no_ads");
		var options = new OnePF.Options ();
		//options.storeKeys.Add(OpenIAB_iOS.STORE,);
		options.storeKeys.Add (OpenIAB_Android.STORE_GOOGLE, googleKey);
		OpenIAB.init (options);
		NoAds();
	}

	public void NoAds()
	{
		OpenIAB.purchaseProduct (SKU);
	
	}


	private void OnBillingSupported()
	{
		Debug.Log("Billing is supported");
		OpenIAB.queryInventory(new string[] { SKU });
	}
	
	private void OnBillingNotSupported(string error)
	{
		Debug.Log("Billing not supported: " + error);
	}
	
	
	private void OnQueryInventoryFailed(string error)
	{
		Debug.Log("Query inventory failed: " + error);
	}
	
	private void OnPurchaseSucceded(Purchase purchase)
	{
		SPlayerPrefs.SetString("jgdfkjgbbsdfusdhufhfksfggrwebnasksdfsd","SUCCED");
		this.SendMessage("CheckSKU");
	}
	
	private void OnPurchaseFailed(int errorCode, string error)
	{
		Debug.Log("Purchase failed: " + error);

	}
	
	private void OnConsumePurchaseSucceeded(Purchase purchase)
	{
		Debug.Log("Consume purchase succeded: " + purchase.ToString());
	}
	
	private void OnConsumePurchaseFailed(string error)
	{
		Debug.Log("Consume purchase failed: " + error);
	}
	
	private void OnTransactionRestored(string sku)
	{
		Debug.Log("Transaction restored: " + sku);
	}
	
	private void OnRestoreSucceeded()
	{
		Debug.Log("Transactions restored successfully");
	}
	
	private void OnRestoreFailed(string error)
	{
		Debug.Log("Transaction restore failed: " + error);
	}








}