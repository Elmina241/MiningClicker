using UnityEngine;
using UnityEngine.Purchasing;

public class GameListener : MonoBehaviour {
    
    public Achievment ach;

    private void Awake()
    {
        PurchaseManager.OnPurchaseNonConsumable += PurchaseManager_OnPurchaseNonConsumable;
        PurchaseManager.OnPurchaseConsumable += PurchaseManager_OnPurchaseConsumable;
        PurchaseManager.PurchaseFailed += PurchaseManager_PurchaseFailed;
    }
    //в случае фейла 
    private void PurchaseManager_PurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        
    }

    // Use this for initialization
    void Start () {
       
        if (PurchaseManager.CheckBuyState("auto_exchange"))
        {
            ach.buyAutoExchanger();
        }
	}

    // Метод, который будет запускаться при успешной покупке многоразовых предметов
    private void PurchaseManager_OnPurchaseConsumable(PurchaseEventArgs args)
    {
        print("You bought: " + args.purchasedProduct.definition.id + "- многоразовый");
        //Через if, потому что предметов у нас может быть много
        if(args.purchasedProduct.definition.id == "money_add")
        {
            ach.buy12000();
        }
    }
    // Метод, который будет запускаться при успешной покупке одноразовых предметов
    private void PurchaseManager_OnPurchaseNonConsumable(PurchaseEventArgs args)
    {
        print("You bought: " + args.purchasedProduct.definition.id + "- одноразовый");
        if (args.purchasedProduct.definition.id == "auto_exchange")
        {
            ach.buyAutoExchanger();
        }
    }
}
