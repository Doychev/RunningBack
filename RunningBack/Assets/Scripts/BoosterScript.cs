using UnityEngine;
using System.Collections;

public class BoosterScript : MonoBehaviour {

	private float initialTime = 5;
	private float timeLeft;
	private Color color;

	// Use this for initialization
	void Start () {
		color = gameObject.GetComponent<Renderer>().material.color;
		timeLeft = initialTime;
	}
	
	// Update is called once per frame
	void Update () {
		timeLeft -= Constants.TIME_STEP;
		if (timeLeft <= 0) {
			Destroy (gameObject);
		} else {
			color.a -= 2 * 1f / (initialTime / Constants.TIME_STEP);
			gameObject.GetComponent<Renderer>().material.color = color;
		}
	}
}
