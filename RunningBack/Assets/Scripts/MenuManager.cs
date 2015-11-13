using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;

public class MenuManager : MonoBehaviour {

    private int audioSetting;

    public Text audioTxt;

	void Start() {

        Application.targetFrameRate = 60;
        audioSetting = PlayerPrefs.GetInt(Constants.AUDIO_SETTING, 1);
        audioTxt.text = audioSetting == 0 ? ("SOUND OFF") : ("SOUND ON");
        PlayGamesPlatform.Activate();
        //speedTxt.text = Social.localUser.userName.ToString();
        Social.localUser.Authenticate((bool success) => {
            //speedTxt.text = success.ToString();
            //Debug.Log(Social.localUser.userName.ToString());
        });
    }

    public void StartGame() {
		Application.LoadLevel ("levelScene");
	}

    public void showLeaderboards() {
        Social.ShowLeaderboardUI();
    }

    public void goToMenu()
    {
        Application.LoadLevel("mainMenu");
    }

    public void goToUpgrades()
    {
        Application.LoadLevel("upgradeMenu");
    }

    public void goToPurchases()
    {
        Application.LoadLevel("purchasesMenu");
    }

    public void goToAbout() {
        Application.LoadLevel("aboutMenu");
    }

    public void switchAudio()
    {
        audioSetting++;
        audioSetting %= 2;
        PlayerPrefs.SetInt(Constants.AUDIO_SETTING, audioSetting);
        audioTxt.text = audioSetting == 0 ? ("SOUND OFF") : ("SOUND ON");
    }

    public void shareApp()
    {
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
        //shareIntent.setType("image/jpeg");
        intentObject.Call<AndroidJavaObject>("setType", "text/plain");
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "SUBJECT");
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "This is my text to send.");
        //shareIntent.putExtra(Intent.EXTRA_STREAM, uriToImage);
        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
        currentActivity.Call("startActivity", intentObject);
    }

    public void rateApp()
    {
        Application.OpenURL("market://details?id=com.smd.runningback");
    }
}
