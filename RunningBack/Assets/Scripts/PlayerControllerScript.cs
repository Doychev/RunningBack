using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
	private int boostRemaining = 0, boostStart = 180;
	private int breakTackleRemaining = 0, breakTackleStart = 120;
	private float boostMultiplier = 1.75f, boostMultiplierStart = 1.75f;

	public int boosters, breakTackles;

	public Button boostersBtn, breakTacklesBtn;
	public GameObject panel;

    public GameObject tutorialPanel;

    public GameObject touchdownFlashingTxt;

    private bool playing = true;

	private float y;

	private CrowdSoundsScript reactionsAudio;

    private int boostsUsed = 0;

	// Use this for initialization
	void Start () {
        Time.timeScale = 1;

        bool showTutorial = SecurePlayerPrefs.GetInt(Constants.SHOW_TUTORIAL, 1, Constants.SECURE_PASS) == 1;
        if (showTutorial)
        {
            pauseGame();
            tutorialPanel.GetComponent<TutorialManager>().initializeTutorial(true);
        }

        movementSpeed = SecurePlayerPrefs.GetInt (Constants.PLAYER_SPEED, Constants.PLAYER_INITIAL_SPEED, Constants.SECURE_PASS);
		horizontalSpeedMax = SecurePlayerPrefs.GetInt (Constants.PLAYER_SIDESPEED, Constants.PLAYER_INITIAL_SIDESPEED, Constants.SECURE_PASS);
		horizontalSpeedIncrement = SecurePlayerPrefs.GetFloat (Constants.PLAYER_AGILITY, Constants.PLAYER_INITIAL_AGILITY, Constants.SECURE_PASS);
		luck = SecurePlayerPrefs.GetFloat (Constants.PLAYER_LUCK, Constants.PLAYER_INITIAL_LUCK, Constants.SECURE_PASS);

		boosters = SecurePlayerPrefs.GetInt (Constants.AVAILABLE_BOOSTERS, 10, Constants.SECURE_PASS);
		updateBoostersUi ();
		breakTackles = SecurePlayerPrefs.GetInt (Constants.AVAILABLE_BREAK_TACKLES, 10, Constants.SECURE_PASS);
		updateBreakTacklesUi ();

		lastPosition = transform.position;
		y = lastPosition.y;

        boostsUsed = SecurePlayerPrefs.GetInt(Constants.BOOSTS_USED, 0, Constants.SECURE_PASS);
        
		enemySpawner = FindObjectOfType<EnemySpawningScript> ();
		boostersSpawner = FindObjectOfType<BoostersSpawningScript> ();

        reactionsAudio = FindObjectOfType<CrowdSoundsScript> ();
	}

	// Update is called once per frame
	void Update () {
        if (playing)
        {
            if (!scoring)
            {
                yardsGained += Mathf.Abs(lastPosition.x - transform.position.x);
                yardsGainedTxt.text = yardsGained + "";
            }
            totalYards += Vector3.Distance(lastPosition, transform.position);
            totalYardsTxt.text = totalYards + "";
        }
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
        if (playing && !scoring) {
			transform.position = new Vector3 (lastPosition.x + verticalSpeed / 60, y, lastPosition.z + (horizontalDirection * currentHorizontalSpeed / 60));
		}
	}

    public void pauseGame()
    {
        playing = false;
        Time.timeScale = 0;
    }

    public void unpauseGame()
    {
        playing = true;
        Time.timeScale = 1;
    }

    public void gameOver() {
		reactionsAudio.playCheering ();
        //        audioManager.GetComponent<AudioSource>().Stop();
        playing = false;
        int playerUpgradePoints = SecurePlayerPrefs.GetInt (Constants.PLAYER_UPGRADE_POINTS, 0, Constants.SECURE_PASS);
		playerUpgradePoints += touchdowns;
		SecurePlayerPrefs.SetInt(Constants.PLAYER_UPGRADE_POINTS, playerUpgradePoints, Constants.SECURE_PASS);
		boostersBtn.interactable = false;
		breakTacklesBtn.interactable = false;
        submitLeaderboardScores();
        SecurePlayerPrefs.SetInt(Constants.BOOSTS_USED, boostsUsed, Constants.SECURE_PASS);
        AchievementsManager.updateAcheivements(touchdowns, SecurePlayerPrefs.GetInt(Constants.TOTAL_TOUCHDOWNS, 0, Constants.SECURE_PASS), boostsUsed);
		panel.SetActive(true);
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

            int savedTouchdowns = SecurePlayerPrefs.GetInt(Constants.TOTAL_TOUCHDOWNS, 0, Constants.SECURE_PASS) + touchdowns;
            SecurePlayerPrefs.SetInt(Constants.TOTAL_TOUCHDOWNS, savedTouchdowns, Constants.SECURE_PASS);

            Social.ReportScore(savedTouchdowns, GPGConstants.leaderboard_total_touchdowns, (bool success) => {
                // handle success or failure
            });

            int savedYards = SecurePlayerPrefs.GetInt(Constants.TOTAL_YARDS_GAINED, 0, Constants.SECURE_PASS) + System.Convert.ToInt32(yardsGained);
            SecurePlayerPrefs.SetInt(Constants.TOTAL_YARDS_GAINED, savedYards, Constants.SECURE_PASS);

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
            flashOutOfBoundsTxt();
            gameOver();
		} else if (col.gameObject.tag.Equals (Constants.ENEMY) && breakTackleRemaining == 0) {
            flashTackledTxt();
            gameOver();
        } else if (col.gameObject.tag.Equals (Constants.BOOSTER)) {
			increaseBoosters (col.gameObject);
		} else if (col.gameObject.tag.Equals (Constants.FIELD)) {
//			scoring = false;
		}
	}

	public void scoreTouchdown() {
        flashTouchdownTxt();
        reactionsAudio.playBooing ();
		scoring = true;
		yardsGained += 3.5f; //fix for the less yards counted on each touchdown
		touchdowns++;
		touchdownsTxt.text = touchdowns + "";
		changeDirection ();
		enemySpawner.SpawnEnemies (touchdowns);
		if (Random.value < luck) 
			boostersSpawner.SpawnBooster ();
	}

    void flashTouchdownTxt()
    {
        touchdownFlashingTxt.GetComponent<UIAnimationScript>().playAnimation("TOUCHDOWN");
    }

    void flashOutOfBoundsTxt()
    {
        touchdownFlashingTxt.GetComponent<UIAnimationScript>().playAnimation("OUT OF BOUNDS");
    }

    void flashTackledTxt()
    {
        touchdownFlashingTxt.GetComponent<UIAnimationScript>().playAnimation("TACKLED");
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

	public void changeDirection() {
		transform.Rotate (new Vector3 (0, 1, 0), 180);
		movingLeft = false;
		movingRight = false;
		movingForward = !movingForward;
        Time.timeScale = 0;
		Camera.main.GetComponent<CameraController> ().setIsRotating (movingForward);
	}

	public void enableBreakTackle() {
        boostsUsed++;
        if (breakTackleRemaining == 0) {
			if (breakTackles > 0) {
				breakTackles--;
				updateBreakTacklesUi ();
				SecurePlayerPrefs.SetInt(Constants.AVAILABLE_BREAK_TACKLES, breakTackles, Constants.SECURE_PASS);
				breakTackleRemaining = breakTackleStart;
			} else {
				//show no break tackles message
			}
		}
	}

	public void enableBoost() {
        boostsUsed++;
        if (boostRemaining == 0) {
			if (boosters > 0) {
				boosters--;
				updateBoostersUi ();
				SecurePlayerPrefs.SetInt(Constants.AVAILABLE_BOOSTERS, boosters, Constants.SECURE_PASS);
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
			SecurePlayerPrefs.SetInt (Constants.AVAILABLE_BOOSTERS, boosters, Constants.SECURE_PASS);
		} else {
			breakTackles++;
			updateBreakTacklesUi ();
			SecurePlayerPrefs.SetInt (Constants.AVAILABLE_BREAK_TACKLES, breakTackles, Constants.SECURE_PASS);
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

    public void goToUpgrades()
    {
        Application.LoadLevel("upgradeMenu");
    }

    public void goToPurchases()
    {
        Application.LoadLevel("purchasesMenu");
    }

    public void setScoring(bool scoring) {
		this.scoring = scoring;
	}

	public bool isPlaying() {
		return playing && !scoring;
	}
}
