using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIAnimationScript : MonoBehaviour {

    public void playAnimation(string message) {
        GetComponent<Text>().text = message;
        GetComponent<Animator>().SetBool("showUiAnimation", false);
        GetComponent<Animator>().SetBool("showUiAnimation", true);
    }

}
