using UnityEngine;
using System.Collections;

public class TutorialManager : MonoBehaviour {

    public GameObject player;
    public GameObject tutorial0, tutorial1, tutorial2, tutorial3, tutorial4, tutorial5;
    private GameObject[] tutorialArray;

    public void initializeTutorial(bool showTutorial)
    {
        gameObject.SetActive(showTutorial);
        tutorial0.SetActive(showTutorial);
        tutorialArray = new GameObject[] { tutorial0, tutorial1, tutorial2, tutorial3, tutorial4, tutorial5 };
    }

    public void hideTutorial()
    {
        gameObject.SetActive(false);
        player.GetComponent<PlayerControllerScript>().unpauseGame();
    }

    public void hideAndDontShow()
    {
        hideTutorial();
        SecurePlayerPrefs.SetInt(Constants.SHOW_TUTORIAL, 0, Constants.SECURE_PASS);
    }

    public void showNextStep(int step)
    {
        tutorialArray[step - 1].SetActive(false);
        tutorialArray[step].SetActive(true);
    }
}
