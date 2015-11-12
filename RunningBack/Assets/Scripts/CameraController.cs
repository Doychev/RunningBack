using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;

	private Vector3 lastPlayerPosition;
	private float distanceToMove;

	private bool isRotating = false;
	private int rotation = 0;
	private bool movingForward;

	// Use this for initialization
	void Start () {
        AudioListener.volume = PlayerPrefs.GetInt(Constants.AUDIO_SETTING, 1);
		lastPlayerPosition = player.transform.position;
		transform.position = new Vector3 (lastPlayerPosition.x - Constants.DISTANCE_TO_PLAYER, transform.position.y, transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		distanceToMove = player.transform.position.x - lastPlayerPosition.x;
		transform.position = new Vector3 (transform.position.x + distanceToMove, transform.position.y, transform.position.z);
		lastPlayerPosition = player.transform.position;
		if (isRotating) {
			Time.timeScale = 0;
			rotation += 9;
			transform.eulerAngles  = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + (movingForward ? -1 : 1) * 9, transform.eulerAngles.z);
			if (rotation >= 180) {
				Time.timeScale = 1;
				player.GetComponent<PlayerControllerScript>().setScoring(false);
				isRotating = false;
				rotation = 0;
			}
		}
	}

	public void setIsRotating(bool movingForward) {
		isRotating = true;
		this.movingForward = movingForward;
		transform.position = new Vector3 (lastPlayerPosition.x + (movingForward ? -1 : 1) * Constants.DISTANCE_TO_PLAYER, transform.position.y, transform.position.z);
	}
}
