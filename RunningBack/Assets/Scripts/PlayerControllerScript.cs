using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class PlayerControllerScript : MonoBehaviour {

	public float movementSpeed;
	public float horizontalSpeedMin, horizontalSpeedMax, horizontalSpeedIncrement;
	public float luck;

	public float cameraRotationSpeed;

	public int touchdowns = 0;
	public float totalYards = 0;
	public float yardsGained = 0;

	public Text touchdownsTxt, yardsGainedTxt, totalYardsTxt, boostersTxt, breakTacklesTxt;

	private EnemySpawningScript enemySpawner;
	private BoostersSpawningScript boostersSpawner;

	private float currentHorizontalSpeed; 
	
	private Vector3 lastPosition;

	private bool movingLeft = false, movingRight = false, movingForward = true, scoring = false;
	private int boostRemaining = 0, boostStart = 60;
	private int breakTackleRemaining = 0, breakTackleStart = 120;
	private float boostMultiplier = 1.5f, boostMultiplierStart = 1.5f;

	public int boosters, breakTackles;

	public Button boostersBtn, breakTacklesBtn;
	public GameObject panel;

	private bool playing = true;

	private float y;

	private CrowdSoundsScript reactionsAudio;

    public GameObject audioManager;

	// Use this for initialization
	void Start () {

		movementSpeed = PlayerPrefs.GetInt (Constants.PLAYER_SPEED, Constants.PLAYER_INITIAL_SPEED);
		horizontalSpeedMax = PlayerPrefs.GetInt (Constants.PLAYER_SIDESPEED, Constants.PLAYER_INITIAL_SIDESPEED);
		horizontalSpeedIncrement = PlayerPrefs.GetFloat (Constants.PLAYER_AGILITY, Constants.PLAYER_INITIAL_AGILITY);
		luck = PlayerPrefs.GetFloat (Constants.PLAYER_LUCK, Constants.PLAYER_INITIAL_LUCK);

		boosters = PlayerPrefs.GetInt (Constants.AVAILABLE_BOOSTERS, 10);
		updateBoostersUi ();
		breakTackles = PlayerPrefs.GetInt (Constants.AVAILABLE_BREAK_TACKLES, 10);
		updateBreakTacklesUi ();

		lastPosition = transform.position;
		y = lastPosition.y;

		enemySpawner = FindObjectOfType<EnemySpawningScript> ();
		boostersSpawner = FindObjectOfType<BoostersSpawningScript> ();

		reactionsAudio = FindObjectOfType<CrowdSoundsScript> ();
	}

	// Update is called once per frame
	void Update () {
		if (!scoring) {
			yardsGained += Mathf.Abs (lastPosition.x - transform.position.x);
			yardsGainedTxt.text = yardsGained + "";
		}
		totalYards += Vector3.Distance (lastPosition, transform.position);
		totalYardsTxt.text = totalYards + "";
		lastPosition = transform.position;

		int horizontalDirection = 0, verticalDirection = 1;
		if (movingLeft) {
			horizontalDirection = 1;
			currentHorizontalSpeed += horizontalSpeedIncrement;
		} else if (movingRight) {
			horizontalDirection = -1;
			currentHorizontalSpeed += horizontalSpeedIncrement;
		} else {
			currentHorizontalSpeed = horizontalSpeedMin;
		}
		if (currentHorizontalSpeed > horizontalSpeedMax) {
			currentHorizontalSpeed = horizontalSpeedMax;
		}
		if (!movingForward) {
			verticalDirection = -1;
		}
		float verticalSpeed = verticalDirection * movementSpeed;
		if (boostRemaining > 0) {
			verticalSpeed *= boostMultiplier;
			boostMultiplier -= (boostMultiplierStart - 1) / boostStart;
			boostRemaining--;
		}
		if (breakTackleRemaining > 0) {
			breakTackleRemaining--;
		}
        if (playing) {
			transform.position = new Vector3 (lastPosition.x + verticalSpeed / 60, y, lastPosition.z + (horizontalDirection * currentHorizontalSpeed / 60));
		}
	}

	public void gameOver() {
		reactionsAudio.playCheering ();
        audioManager.GetComponent<AudioSource>().Stop();
		playing = false;
		int playerUpgradePoints = PlayerPrefs.GetInt (Constants.PLAYER_UPGRADE_POINTS, 0);
		playerUpgradePoints += touchdowns;
		PlayerPrefs.SetInt(Constants.PLAYER_UPGRADE_POINTS, playerUpgradePoints);
		boostersBtn.interactable = false;
		breakTacklesBtn.interactable = false;
        submitLeaderboardScores();
		panel.SetActive(true);
		Time.timeScale = 0;

		//		restartGame ();
	}

    void submitLeaderboardScores() {
        if (Social.localUser.authenticated)
        {
            Social.ReportScore(touchdowns, GPGConstants.leaderboard_most_touchdowns, (bool success) => {
                // handle success or failure
            });

            Social.ReportScore(System.Convert.ToInt32(yardsGained), GPGConstants.leaderboard_most_yards_gained, (bool success) => {
                // handle success or failure
            });

            int savedTouchdowns = PlayerPrefs.GetInt(Constants.TOTAL_TOUCHDOWNS, 0) + touchdowns;
            PlayerPrefs.SetInt(Constants.TOTAL_TOUCHDOWNS, savedTouchdowns);

            Social.ReportScore(savedTouchdowns, GPGConstants.leaderboard_total_touchdowns, (bool success) => {
                // handle success or failure
            });

            int savedYards = PlayerPrefs.GetInt(Constants.TOTAL_YARDS_GAINED, 0) + System.Convert.ToInt32(yardsGained);
            PlayerPrefs.SetInt(Constants.TOTAL_YARDS_GAINED, savedYards);

            Social.ReportScore(savedYards, GPGConstants.leaderboard_total_yards_gained, (bool success) => {
                // handle success or failure
            });
        }
    }

    void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.tag.Equals (Constants.ENDZONE) && !scoring) {
			scoreTouchdown();
		} else if (col.gameObject.tag.Equals (Constants.SIDELINES)) {
			gameOver ();
		} else if (col.gameObject.tag.Equals (Constants.ENEMY) && breakTackleRemaining == 0) {
            gameOver();
        } else if (col.gameObject.tag.Equals (Constants.BOOSTER)) {
			increaseBoosters (col.gameObject);
		} else if (col.gameObject.tag.Equals (Constants.FIELD)) {
			scoring = false;
		}
	}

	public void scoreTouchdown() {
		reactionsAudio.playBooing ();
		scoring = true;
		yardsGained += 3.5f; //fix for the less yards counted on each touchdown
		touchdowns++;
		touchdownsTxt.text = touchdowns + "";
		StartCoroutine (changeDirection ());
		enemySpawner.SpawnEnemies (touchdowns);
		if (Random.value < luck) 
			boostersSpawner.SpawnBooster ();
	}
	
	public void moveLeft(bool mouseDown) {
		if (movingForward) {
			movingLeft = mouseDown;
			movingRight = false;
		} else {
			movingLeft = false;
			movingRight = mouseDown;
		}
	}

	public void moveRight(bool mouseDown) {
		if (movingForward) {
			movingLeft = false;
			movingRight = mouseDown;
		} else {
			movingLeft = mouseDown;
			movingRight = false;
		}
	}

	public IEnumerator changeDirection() {
		yield return new WaitForSeconds (0.2f);
		transform.Rotate (new Vector3 (0, 1, 0), 180);
		movingLeft = false;
		movingRight = false;
		movingForward = !movingForward;
		Camera.main.GetComponent<CameraController> ().setIsRotating (movingForward);
	}

	public void enableBreakTackle() {
		if (breakTackleRemaining == 0) {
			if (breakTackles > 0) {
				breakTackles--;
				updateBreakTacklesUi ();
				PlayerPrefs.SetInt(Constants.AVAILABLE_BREAK_TACKLES, breakTackles);
				breakTackleRemaining = breakTackleStart;
			} else {
				//show no break tackles message
			}
		}
	}

	public void enableBoost() {
		if (boostRemaining == 0) {
			if (boosters > 0) {
				boosters--;
				updateBoostersUi ();
				PlayerPrefs.SetInt(Constants.AVAILABLE_BOOSTERS, boosters);
				boostMultiplier = boostMultiplierStart;
				boostRemaining = boostStart;
			} else {
				//show no boosters message
			}
		}
	}

	public void increaseBoosters(GameObject gameObject) {
		if (Random.value > 0.5f) {
			boosters++;
			updateBoostersUi ();
			PlayerPrefs.SetInt (Constants.AVAILABLE_BOOSTERS, boosters);
		} else {
			breakTackles++;
			updateBreakTacklesUi ();
			PlayerPrefs.SetInt (Constants.AVAILABLE_BREAK_TACKLES, breakTackles);
		}
		Destroy (gameObject);
	}

	void updateBoostersUi() {
		boostersTxt.text = boosters + "";
		boostersBtn.interactable = (boosters != 0);
	}
	
	void updateBreakTacklesUi() {
		breakTacklesTxt.text = breakTackles + "";
		breakTacklesBtn.interactable = (breakTackles != 0);
	}
	
	public void restartGame() {
		Time.timeScale = 1;
		Application.LoadLevel ("levelScene");
	}

	public void goToMenu() {
		Time.timeScale = 1;
		Application.LoadLevel ("mainMenu");
	}

public void setScoring(bool scoring) {
		this.scoring = scoring;
	}

	public bool isPlaying() {
		return playing;
	}
}
