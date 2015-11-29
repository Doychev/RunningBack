using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradeManager : MonoBehaviour
{

    public Text speedTxt, sidespeedTxt, agilityTxt, luckTxt, upgradePointsTxt;
    public Button speedBtn, sidespeedBtn, agilityBtn, luckBtn;
    public GameObject popupPanel;
    private int speed, sidespeed;
    private float agility, luck;
    private int upgradePoints;
    private int totalUpgrades;
    private bool hasRated;

    void Start()
    {

        speed = SecurePlayerPrefs.GetInt(Constants.PLAYER_SPEED, Constants.PLAYER_INITIAL_SPEED, Constants.SECURE_PASS);
        sidespeed = SecurePlayerPrefs.GetInt(Constants.PLAYER_SIDESPEED, Constants.PLAYER_INITIAL_SIDESPEED, Constants.SECURE_PASS);
        agility = SecurePlayerPrefs.GetFloat(Constants.PLAYER_AGILITY, Constants.PLAYER_INITIAL_AGILITY, Constants.SECURE_PASS);
        luck = SecurePlayerPrefs.GetFloat(Constants.PLAYER_LUCK, Constants.PLAYER_INITIAL_LUCK, Constants.SECURE_PASS);
        upgradePoints = SecurePlayerPrefs.GetInt(Constants.PLAYER_UPGRADE_POINTS, 0, Constants.SECURE_PASS);
        totalUpgrades = SecurePlayerPrefs.GetInt(Constants.TOTAL_UPGRADES, 0, Constants.SECURE_PASS);
        hasRated = SecurePlayerPrefs.GetInt(Constants.HAS_RATED, 0, Constants.SECURE_PASS) == 1;

         /*
		 * Uncomment this to reset values.
		 * 
		speed = Constants.PLAYER_INITIAL_SPEED;
		sidespeed = Constants.PLAYER_INITIAL_SIDESPEED;
		agility = Constants.PLAYER_INITIAL_AGILITY;
		luck = Constants.PLAYER_INITIAL_LUCK;
		upgradePoints = 0;
		SecurePlayerPrefs.SetInt (Constants.PLAYER_SPEED, speed, Constants.SECURE_PASS);
		SecurePlayerPrefs.SetInt (Constants.PLAYER_SIDESPEED, sidespeed, Constants.SECURE_PASS);
		SecurePlayerPrefs.SetFloat (Constants.PLAYER_AGILITY, agility, Constants.SECURE_PASS);
		SecurePlayerPrefs.SetFloat (Constants.PLAYER_LUCK, luck, Constants.SECURE_PASS);
		SecurePlayerPrefs.SetInt (Constants.PLAYER_UPGRADE_POINTS, upgradePoints, Constants.SECURE_PASS);
		 */

        speedTxt.text = speed + "";
        sidespeedTxt.text = sidespeed + "";
        agilityTxt.text = agility + "";
        luckTxt.text = luck + "";
        upgradePointsTxt.text = upgradePoints + "";

        //enable/disable buttons according to points
        if (upgradePoints < 10)
        {
            speedBtn.interactable = false;
            sidespeedBtn.interactable = false;
            agilityBtn.interactable = false;
            luckBtn.interactable = false;
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

    private void updateUpgradePoints()
    {
        totalUpgrades++;
        SecurePlayerPrefs.SetInt(Constants.TOTAL_UPGRADES, totalUpgrades, Constants.SECURE_PASS);
        upgradePoints -= 10; //TODO: scale it according to past upgrades
        SecurePlayerPrefs.SetInt(Constants.PLAYER_UPGRADE_POINTS, upgradePoints, Constants.SECURE_PASS);
        upgradePointsTxt.text = upgradePoints + "";

        //enable/disable buttons according to points
        if (upgradePoints < 10)
        {
            speedBtn.interactable = false;
            sidespeedBtn.interactable = false;
            agilityBtn.interactable = false;
            luckBtn.interactable = false;
        }
        if (totalUpgrades % 5 == 0 && !hasRated)
        {
            StartCoroutine(showRatePopup());
        }
    }

    public IEnumerator showRatePopup()
    {
        yield return new WaitForSeconds(0.2f);
        popupPanel.SetActive(true);
    }

    public void rateNow()
    {
        hasRated = true;
        SecurePlayerPrefs.SetInt(Constants.HAS_RATED, 1, Constants.SECURE_PASS);
        popupPanel.SetActive(false);
        Application.OpenURL("market://details?id=com.smd.runningback");
    }

    public void rateLater()
    {
        popupPanel.SetActive(false);
    }

    public void alreadyRated()
    {
        hasRated = true;
        SecurePlayerPrefs.SetInt(Constants.HAS_RATED, 1, Constants.SECURE_PASS);
        popupPanel.SetActive(false);
    }

    public void upgradePlayerSpeed()
    {
        updateUpgradePoints();
        speed++;
        SecurePlayerPrefs.SetInt(Constants.PLAYER_SPEED, speed, Constants.SECURE_PASS);
        speedTxt.text = speed + "";
    }

    public void upgradePlayerSidespeed()
    {
        updateUpgradePoints();
        sidespeed++;
        SecurePlayerPrefs.SetInt(Constants.PLAYER_SIDESPEED, sidespeed, Constants.SECURE_PASS);
        sidespeedTxt.text = sidespeed + "";
    }

    public void upgradePlayerAgility()
    {
        updateUpgradePoints();
        agility += 0.1f;
        SecurePlayerPrefs.SetFloat(Constants.PLAYER_AGILITY, agility, Constants.SECURE_PASS);
        agilityTxt.text = agility + "";
    }

    public void upgradePlayerLuck()
    {
        updateUpgradePoints();
        luck += 0.1f;
        SecurePlayerPrefs.SetFloat(Constants.PLAYER_LUCK, luck, Constants.SECURE_PASS);
        luckTxt.text = luck + "";
    }
}
