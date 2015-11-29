using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using Soomla.Store;

public class MenuManager : MonoBehaviour {

    private int audioSetting;

    public Text audioTxt;

	void Start() {

        Application.targetFrameRate = 60;
        audioSetting = SecurePlayerPrefs.GetInt(Constants.AUDIO_SETTING, 1, Constants.SECURE_PASS);
        AudioListener.volume = audioSetting;
        audioTxt.text = audioSetting == 0 ? ("SOUND OFF") : ("SOUND ON");
        PlayGamesPlatform.Activate();
        //speedTxt.text = Social.localUser.userName.ToString();
        Social.localUser.Authenticate((bool success) => {
            //speedTxt.text = success.ToString();
            //Debug.Log(Social.localUser.userName.ToString());
        });
        if (!SoomlaStore.Initialized)
        {
            SoomlaStore.Initialize(new RunningBackAssets());
            SoomlaStore.StartIabServiceInBg();
        }
    }

    public void StartGame() {
		Application.LoadLevel ("levelScene");
	}

    public void showLeaderboards()
    {
        Social.ShowLeaderboardUI();
    }

    public void showAchievements()
    {
        Social.ShowAchievementsUI();
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
        AudioListener.volume = audioSetting;
        SecurePlayerPrefs.SetInt(Constants.AUDIO_SETTING, audioSetting, Constants.SECURE_PASS);
        audioTxt.text = audioSetting == 0 ? ("SOUND OFF") : ("SOUND ON");
    }

    public void shareApp()
    {

        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
        intentObject.Call<AndroidJavaObject>("setType", "text/plain");

        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "Try RunningBack!");
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "I am playing the RunningBack game - it's really fun! Try it out! Join me! https://play.google.com/store/apps/details?id=com.smd.runningback");

        //AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
        //AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file:///sdcard/expl.jpg");
        //intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);

        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
        currentActivity.Call("startActivity", intentObject);

        AchievementsManager.submitAcheivement(GPGConstants.achievement_team_player);
    }

    public void rateApp()
    {
        Application.OpenURL("market://details?id=com.smd.runningback");
    }
}
