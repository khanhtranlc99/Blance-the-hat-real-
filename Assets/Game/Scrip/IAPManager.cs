using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour 

{
    private string noAds = "com.Tester";
    public void OnPurChaseComplete(Product product)
    {

        if (product.definition.id == noAds)
        {
            DataManager.UserData.isRemovedAds = true;
            Debug.Log("noAds");
        }
    }
    public void OnPurChaseFalse(Product product, PurchaseFailureReason reason)
    {
        Debug.Log("Purchase of " + product.definition.id + " false " + reason);
    }
}
