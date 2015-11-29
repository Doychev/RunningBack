using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	public float minSpeedX, maxSpeedX, minSpeedZ, maxSpeedZ;
	public MovementType movementType;

	private Vector3 velocity;
	private float y;

	private GameObject player;

	private bool pushed = false;

    private int lifes = 3;
	
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag (Constants.PLAYER);
		y = transform.position.y;
		setVelocity ();
	}

	void setVelocity() {
		switch (movementType) {
		case MovementType.RANDOM:
			setRandomVelocity();
			break;
		case MovementType.PREDICTED:
		case MovementType.FOLLOWING:
			setVelocityPredicted();
			break;
		default:
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (player.GetComponent<PlayerControllerScript> ().isPlaying()) {
            if (!pushed)
            {
                transform.position = new Vector3(transform.position.x + velocity.x / 60, y, transform.position.z + velocity.z / 60);
                var targetPosition = player.transform.position;
                targetPosition.y = transform.position.y;
                transform.LookAt(targetPosition);

                if (movementType.Equals(MovementType.FOLLOWING))
                {
                    setVelocityPredicted();
                }
            }
            else
            {
                pushed = false;
                velocity = new Vector3(-velocity.x, velocity.y, -velocity.z);
            }
		}
	}

	void setRandomVelocity() {
		float x = 0, z = 0;
		x = RandomSign() * Random.Range(minSpeedX, maxSpeedX);
		z = RandomSign() * Random.Range(minSpeedZ, maxSpeedZ);
		velocity = new Vector3 (x, 0, z);
//		GetComponent<Rigidbody>().velocity = velocity;
	}

	void setVelocityPredicted() {
		Vector3 playerPositionVector = player.transform.position;
		Vector3 movementVector = playerPositionVector - transform.position;
		movementVector.y = 0;
		if (movementVector.x > maxSpeedX) {
			movementVector.z = movementVector.z * maxSpeedX / movementVector.x;
			movementVector.x = maxSpeedX;
		} else if (movementVector.x < -maxSpeedX) {
			movementVector.z = -movementVector.z * maxSpeedX / movementVector.x;
			movementVector.x = -maxSpeedX;
		} else if (movementVector.x > 0 && movementVector.x < minSpeedX) {
			movementVector.x = minSpeedX;
		} else if (movementVector.x < 0 && movementVector.x > -minSpeedX) {
			movementVector.x = -minSpeedX;
		}

		if (movementVector.z > maxSpeedZ) {
			movementVector.z = maxSpeedZ;
		} else if (movementVector.z < -maxSpeedZ) {
			movementVector.z = -maxSpeedZ;
		} 

		velocity = movementVector;
//		GetComponent<Rigidbody> ().velocity = velocity;
	}

	void OnCollisionEnter (Collision col)
	{
        if (col.gameObject.tag.Equals(Constants.ENDZONE)) {
            switch (movementType)
            {
                case MovementType.RANDOM:
                    flipDirection(col);
                    break;
                case MovementType.PREDICTED:
                case MovementType.FOLLOWING:
                    setVelocityPredicted();
                    break;
                default:
                    break;
            }
        } else if (col.gameObject.tag.Equals(Constants.SIDELINES)) {
            lifes--;
            if (lifes == 0)
            {
                Destroy(gameObject, 0.5f);
            } else
            {
                switch (movementType)
                {
                    case MovementType.RANDOM:
                        flipDirection(col);
                        break;
                    case MovementType.PREDICTED:
                    case MovementType.FOLLOWING:
                        setVelocityPredicted();
                        break;
                    default:
                        break;
                }
            }
        }
        else if (col.gameObject.tag.Equals(Constants.PLAYER)) {
            pushed = true;
        }
	}

	void flipDirection(Collision col) {
		if (col.gameObject.tag.Equals (Constants.ENDZONE)) {
			velocity.x = -velocity.x;
		} else if (col.gameObject.tag.Equals (Constants.SIDELINES)) {
			velocity.z = -velocity.z;
		}
	}

	private int RandomSign() {
		if (Random.Range(0, 2) == 0) {
			return -1;
		}
		return 1;
	}
}
