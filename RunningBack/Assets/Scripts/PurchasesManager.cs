using UnityEngine;
using UnityEngine.UI;
using Soomla.Store;
using System.Collections.Generic;

public class PurchasesManager : MonoBehaviour {

    public Text speedBoostsTxt, breakTacklesTxt;

	// Use this for initialization
	void Start () {
        StoreEvents.OnMarketPurchase += onMarketPurchase;
        updateTexts();
    }

    void updateTexts() {
        speedBoostsTxt.text = SecurePlayerPrefs.GetInt(Constants.AVAILABLE_BOOSTERS, Constants.SECURE_PASS).ToString();
        breakTacklesTxt.text = SecurePlayerPrefs.GetInt(Constants.AVAILABLE_BREAK_TACKLES, Constants.SECURE_PASS).ToString();
    }

    public void purchaseBreakTackles(int amount)
    {
        if (amount == 1)
        {
            StoreInventory.BuyItem(RunningBackAssets.BREAK_TACKLE_GOOD.ItemId);
        }
        else if (amount == 10)
        {
            StoreInventory.BuyItem(RunningBackAssets.BREAK_TACKLE_10_GOOD.ItemId);
        }
        else if (amount == 50)
        {
            StoreInventory.BuyItem(RunningBackAssets.BREAK_TACKLE_50_GOOD.ItemId);
        }
    }

    public void purchaseSpeedBoosts(int amount)
    {
        if (amount == 1)
        {
            StoreInventory.BuyItem(RunningBackAssets.SPEED_BOOST_GOOD.ItemId);
        }
        else if (amount == 10)
        {
            StoreInventory.BuyItem(RunningBackAssets.SPEED_BOOST_10_GOOD.ItemId);
        }
        else if (amount == 50)
        {
            StoreInventory.BuyItem(RunningBackAssets.SPEED_BOOST_50_GOOD.ItemId);
        }
    }

    public void StartGame()
    {
        Application.LoadLevel("levelScene");
    }

    public void goToMenu()
    {
        Application.LoadLevel("mainMenu");
    }

    public void onMarketPurchase(PurchasableVirtualItem pvi, string payload, Dictionary<string, string> extra)
    {
        if (pvi.ItemId.Equals(RunningBackAssets.SPEED_BOOST_GOOD.ItemId))
        {
            int availableBoosters = SecurePlayerPrefs.GetInt(Constants.AVAILABLE_BOOSTERS, Constants.SECURE_PASS);
            availableBoosters++;
            SecurePlayerPrefs.SetInt(Constants.AVAILABLE_BOOSTERS, availableBoosters, Constants.SECURE_PASS);
        }
        else if (pvi.ItemId.Equals(RunningBackAssets.SPEED_BOOST_10_GOOD.ItemId))
        {
            int availableBoosters = SecurePlayerPrefs.GetInt(Constants.AVAILABLE_BOOSTERS, Constants.SECURE_PASS);
            availableBoosters+= 10;
            SecurePlayerPrefs.SetInt(Constants.AVAILABLE_BOOSTERS, availableBoosters, Constants.SECURE_PASS);
        }
        else if (pvi.ItemId.Equals(RunningBackAssets.SPEED_BOOST_50_GOOD.ItemId))
        {
            int availableBoosters = SecurePlayerPrefs.GetInt(Constants.AVAILABLE_BOOSTERS, Constants.SECURE_PASS);
            availableBoosters+= 50;
            SecurePlayerPrefs.SetInt(Constants.AVAILABLE_BOOSTERS, availableBoosters, Constants.SECURE_PASS);
        }
        else if (pvi.ItemId.Equals(RunningBackAssets.BREAK_TACKLE_GOOD.ItemId))
        {
            int availableBreakTackles = SecurePlayerPrefs.GetInt(Constants.AVAILABLE_BREAK_TACKLES, Constants.SECURE_PASS);
            availableBreakTackles++;
            SecurePlayerPrefs.SetInt(Constants.AVAILABLE_BREAK_TACKLES, availableBreakTackles, Constants.SECURE_PASS);
        }
        else if (pvi.ItemId.Equals(RunningBackAssets.BREAK_TACKLE_10_GOOD.ItemId))
        {
            int availableBreakTackles = SecurePlayerPrefs.GetInt(Constants.AVAILABLE_BREAK_TACKLES, Constants.SECURE_PASS);
            availableBreakTackles+= 10;
            SecurePlayerPrefs.SetInt(Constants.AVAILABLE_BREAK_TACKLES, availableBreakTackles, Constants.SECURE_PASS);
        }
        else if (pvi.ItemId.Equals(RunningBackAssets.BREAK_TACKLE_50_GOOD.ItemId))
        {
            int availableBreakTackles = SecurePlayerPrefs.GetInt(Constants.AVAILABLE_BREAK_TACKLES, Constants.SECURE_PASS);
            availableBreakTackles+= 50;
            SecurePlayerPrefs.SetInt(Constants.AVAILABLE_BREAK_TACKLES, availableBreakTackles, Constants.SECURE_PASS);
        }
        updateTexts();
        // pvi is the PurchasableVirtualItem that was just purchased
        // payload is a text that you can give when you initiate the purchase operation and you want to receive back upon completion
        // extra will contain platform specific information about the market purchase.
        //      Android: The "extra" dictionary will contain "orderId" and "purchaseToken".
    }
}
