using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;


public class IAP_Store : MonoBehaviour
{


    [Header("Consumable")]
    public TextMeshProUGUI coinText;

    [Header("Non Consumable")]
    public GameObject adsPurchasedWindow;
    public GameObject adsBanner;

    [Header("Subscription")]
    public GameObject subActivatedWindow;
    public GameObject premiumLogo;

    [Serializable]
    public class SkuDetails
    {
        public string productId;
        public string type;
        public string title;
        public string name;
        public string iconUrl;
        public string description;
        public string price;
        public long price_amount_micros;
        public string price_currency_code;
        public string skuDetailsToken;
    }

    [Serializable]
    public class PayloadData
    {
        public string orderId;
        public string packageName;
        public string productId;
        public long purchaseTime;
        public int purchaseState;
        public string purchaseToken;
        public int quantity;
        public bool acknowledged;
    }

    [Serializable]
    public class Payload
    {
        public string json;
        public string signature;
        public List<SkuDetails> skuDetails;
        public PayloadData payloadData;
    }

    [Serializable]
    public class Data
    {
        public string Payload;
        public string Store;
        public string TransactionID;
    }

    public Data data;
    public Payload payload;
    public PayloadData payloadData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.Log(product.definition.id);
        Debug.Log(failureDescription.reason);
    }

    public void OnPurchaseCoins100Complete(Product product)
    {
        Debug.Log(product.definition.id);
        try
        {
            if (product.hasReceipt)
            {
                string receipt = product.receipt;
                Data data = JsonUtility.FromJson<Data>(receipt);
                Payload payload = JsonUtility.FromJson<Payload>(data.Payload);
                PayloadData payloadData = JsonUtility.FromJson<PayloadData>(payload.json);

                int quantity = payloadData.quantity;

                for (int i = 0; i < quantity; i++)
                {
                    AddCoin(100);
                }
            }
        }
        catch (Exception)
        {
            Debug.Log("It only work for Google store, app store, amazon store, you are using fake store!");
            //For in Fake Store Only
            AddCoin(100);
        }
        
    }

    void AddCoin(int num)
    {
        int coins = PlayerPrefs.GetInt("totalCoins");
        coins += num;
        PlayerPrefs.SetInt("totalCoins", coins);
        coinText.text = coins.ToString();
    }

    void DisplayAds(bool active)
    {
        if (!active)
        {
            adsPurchasedWindow.SetActive(true);
            adsBanner.SetActive(false);
        }
        else
        {
            adsPurchasedWindow.SetActive(false);
            adsBanner.SetActive(true);
        }
    }

   
    void RemoveAds()
    {
        DisplayAds(false);
    }

    
    void ShowAds()
    {
        DisplayAds(true);
    }

    
    public void OnPurchaseRemoveAdsComplete(Product product)
    {
        Debug.Log(product.definition.id);
        RemoveAds();
    }

    public void CheckNonConsumable(Product product)
    {
        if (product != null)
        {
            if (product.hasReceipt) //purchased
            {
                RemoveAds();
            }
            else
            {
                ShowAds();
            }
        }
    }

    void SetupBattlePass(bool active)
    {
        if (active)
        {
            subActivatedWindow.SetActive(true);
            premiumLogo.SetActive(true);
        }
        else
        {
            subActivatedWindow.SetActive(false);
            premiumLogo.SetActive(false);
        }
    }

    // 0 references
    void ActivateBattlePass()
    {
        SetupBattlePass(true);
    }

    // 0 references
    void DeactivateBattlePass()
    {
        SetupBattlePass(false);
    }

    // 0 references
    public void OnPurchaseActivateBattlePassComplete(Product product)
    {
        Debug.Log(product.definition.id);
        ActivateBattlePass();
    }

    public void CheckSubscription(Product subProduct)
    {
        try
        {
            if (subProduct.hasReceipt)
            {
                var subManager = new SubscriptionManager(subProduct, null);
                var info = subManager.getSubscriptionInfo();

                if (info.isSubscribed() == Result.True)
                {
                    Debug.Log("we are subscribed");
                    ActivateBattlePass();
                }
                else
                {
                    Debug.Log("Unsubscribed");
                    DeactivateBattlePass();
                }
            }
            else
            {
                Debug.Log("receipt not found !");
            }
        }
        catch (Exception)
        {
            Debug.Log("It only work for Google store, app store, amazon store, you are using fake store!");
        }
    }
}
