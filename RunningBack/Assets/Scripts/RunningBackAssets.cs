using System.Collections.Generic;
using Soomla.Store;

public class RunningBackAssets : IStoreAssets
{

    public const string SPEED_BOOST_1_PRODUCT_ID = "speed_boost_1";
    public const string BREAK_TACKLE_1_PRODUCT_ID = "break_tackle_1";
    public const string SPEED_BOOST_10_PRODUCT_ID = "speed_boost_10";
    public const string BREAK_TACKLE_10_PRODUCT_ID = "break_tackle_10";
    public const string SPEED_BOOST_50_PRODUCT_ID = "speed_boost_50";
    public const string BREAK_TACKLE_50_PRODUCT_ID = "break_tackle_50";

    public int GetVersion()
    {
        return 0;
    }

    public VirtualCurrency[] GetCurrencies()
    {
        return new VirtualCurrency[] { };
    }

    public VirtualGood[] GetGoods()
    {
        return new VirtualGood[] { SPEED_BOOST_GOOD, BREAK_TACKLE_GOOD, SPEED_BOOST_10_GOOD, BREAK_TACKLE_10_GOOD, SPEED_BOOST_50_GOOD, BREAK_TACKLE_50_GOOD };
    }

    public VirtualCurrencyPack[] GetCurrencyPacks()
    {
        return new VirtualCurrencyPack[] { };
    }

    public VirtualCategory[] GetCategories()
    {
        return new VirtualCategory[] { GENERAL_CATEGORY };
    }


    public static VirtualGood SPEED_BOOST_GOOD = new SingleUseVG(
            "Speed Boost",
            "Temporarily increase your speed",
            SPEED_BOOST_1_PRODUCT_ID,
            new PurchaseWithMarket(SPEED_BOOST_1_PRODUCT_ID, 0.99));

    public static VirtualGood BREAK_TACKLE_GOOD = new SingleUseVG(
            "Break Tackle",
            "Temporary break all tackles from opponents",
            BREAK_TACKLE_1_PRODUCT_ID,
            new PurchaseWithMarket(BREAK_TACKLE_1_PRODUCT_ID, 0.99));

    public static VirtualGood SPEED_BOOST_10_GOOD = new SingleUseVG(
            "10 Speed Boosts",
            "Temporarily increase your speed",
            SPEED_BOOST_10_PRODUCT_ID,
            new PurchaseWithMarket(SPEED_BOOST_10_PRODUCT_ID, 0.99));

    public static VirtualGood BREAK_TACKLE_10_GOOD = new SingleUseVG(
            "10 Break Tackles",
            "Temporary break all tackles from opponents",
            BREAK_TACKLE_10_PRODUCT_ID,
            new PurchaseWithMarket(BREAK_TACKLE_10_PRODUCT_ID, 0.99));

    public static VirtualGood SPEED_BOOST_50_GOOD = new SingleUseVG(
            "50 Speed Boosts",
            "Temporarily increase your speed",
            SPEED_BOOST_50_PRODUCT_ID,
            new PurchaseWithMarket(SPEED_BOOST_50_PRODUCT_ID, 0.99));

    public static VirtualGood BREAK_TACKLE_50_GOOD = new SingleUseVG(
            "50 Break Tackles",
            "Temporary break all tackles from opponents",
            BREAK_TACKLE_50_PRODUCT_ID,
            new PurchaseWithMarket(BREAK_TACKLE_50_PRODUCT_ID, 0.99));


    public static VirtualCategory GENERAL_CATEGORY = new VirtualCategory(
            "General", new List<string>(new string[] { SPEED_BOOST_1_PRODUCT_ID, BREAK_TACKLE_1_PRODUCT_ID, SPEED_BOOST_10_PRODUCT_ID, BREAK_TACKLE_10_PRODUCT_ID, SPEED_BOOST_50_PRODUCT_ID, BREAK_TACKLE_50_PRODUCT_ID})
    );
}