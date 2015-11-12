using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{

    public Text speedTxt, sidespeedTxt, agilityTxt, luckTxt, upgradePointsTxt;
    public Button speedBtn, sidespeedBtn, agilityBtn, luckBtn;
    private int speed, sidespeed;
    private float agility, luck;
    private int upgradePoints;

    void Start()
    {

        speed = PlayerPrefs.GetInt(Constants.PLAYER_SPEED, Constants.PLAYER_INITIAL_SPEED);
        sidespeed = PlayerPrefs.GetInt(Constants.PLAYER_SIDESPEED, Constants.PLAYER_INITIAL_SIDESPEED);
        agility = PlayerPrefs.GetFloat(Constants.PLAYER_AGILITY, Constants.PLAYER_INITIAL_AGILITY);
        luck = PlayerPrefs.GetFloat(Constants.PLAYER_LUCK, Constants.PLAYER_INITIAL_LUCK);
        upgradePoints = PlayerPrefs.GetInt(Constants.PLAYER_UPGRADE_POINTS, 0);

        /*
		 * Uncomment this to reset values.
		 * 
		speed = Constants.PLAYER_INITIAL_SPEED;
		sidespeed = Constants.PLAYER_INITIAL_SIDESPEED;
		agility = Constants.PLAYER_INITIAL_AGILITY;
		luck = Constants.PLAYER_INITIAL_LUCK;
		upgradePoints = 0;
		PlayerPrefs.SetInt (Constants.PLAYER_SPEED, speed);
		PlayerPrefs.SetInt (Constants.PLAYER_SIDESPEED, sidespeed);
		PlayerPrefs.SetFloat (Constants.PLAYER_AGILITY, agility);
		PlayerPrefs.SetFloat (Constants.PLAYER_LUCK, luck);
		PlayerPrefs.SetInt (Constants.PLAYER_UPGRADE_POINTS, upgradePoints);
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
        upgradePoints -= 10; //TODO: scale it according to past upgrades
        PlayerPrefs.SetInt(Constants.PLAYER_UPGRADE_POINTS, upgradePoints);
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

    public void upgradePlayerSpeed()
    {
        updateUpgradePoints();
        speed++;
        PlayerPrefs.SetInt(Constants.PLAYER_SPEED, speed);
        speedTxt.text = speed + "";
    }

    public void upgradePlayerSidespeed()
    {
        updateUpgradePoints();
        sidespeed++;
        PlayerPrefs.SetInt(Constants.PLAYER_SIDESPEED, sidespeed);
        sidespeedTxt.text = sidespeed + "";
    }

    public void upgradePlayerAgility()
    {
        updateUpgradePoints();
        agility += 0.1f;
        PlayerPrefs.SetFloat(Constants.PLAYER_AGILITY, agility);
        agilityTxt.text = agility + "";
    }

    public void upgradePlayerLuck()
    {
        updateUpgradePoints();
        luck += 0.1f;
        PlayerPrefs.SetFloat(Constants.PLAYER_LUCK, luck);
        luckTxt.text = luck + "";
    }
}
