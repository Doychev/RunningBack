using UnityEngine;
using System.Collections;

public class EnemySpawningScript : MonoBehaviour {

	public GameObject enemy0Prefab;
	public GameObject enemy1Prefab;
	public GameObject enemy2Prefab;
	public GameObject enemy3Prefab;
	public GameObject enemy4Prefab;
	public GameObject enemy5Prefab;

	public int minCoordinateX, maxCoordinateX, minCoordinateZ, maxCoordinateZ;

	private int enemyCount = 0;
	private Vector3 position;

	// Use this for initialization
	void Start () {
		position = new Vector3 ();
		position.y = 0.5f;
		SpawnEnemies (0);
	}

	public void SpawnEnemies (int waveNumber) {
		if (waveNumber == 0) {
			for (int i = 0; i < 3; i++) {
				spawnLevel0Enemy ();
			}
		} else if (waveNumber == 1) {
			for (int i = 0; i < 3; i++) {
				spawnLevel1Enemy ();
			}
		} else if (waveNumber == 2) {
			for (int i = 0; i < 3; i++) {
				spawnLevel2Enemy ();
			}
		} else if (waveNumber == 3) {
			for (int i = 0; i < 3; i++) {
				spawnLevel3Enemy ();
			}
		} else if (waveNumber == 4) {
			for (int i = 0; i < 3; i++) {
				spawnLevel4Enemy ();
			}
		} else if (waveNumber > 4) {
			spawnLevel5Enemy ();
		} else {
			//do nothing
		}
	}

	private void spawnLevel0Enemy() {
		position.x = Random.Range(minCoordinateX, maxCoordinateX);
		position.z = getZCoordinate();
		Instantiate(enemy0Prefab, position, Quaternion.identity);
		enemyCount++;
	}

	private void spawnLevel1Enemy() {
		position.x = Random.Range(minCoordinateX, maxCoordinateX);
		position.z = getZCoordinate();
		Instantiate(enemy1Prefab, position, Quaternion.identity);
		enemyCount++;
	}

	private void spawnLevel2Enemy() {
		position.x = Random.Range(minCoordinateX, maxCoordinateX);
		position.z = getZCoordinate();
		Instantiate(enemy2Prefab, position, Quaternion.identity);
		enemyCount++;
	}

	private void spawnLevel3Enemy() {
		position.x = Random.Range(minCoordinateX, maxCoordinateX);
		position.z = getZCoordinate();
		Instantiate(enemy3Prefab, position, Quaternion.identity);
		enemyCount++;
	}

	private void spawnLevel4Enemy() {
		position.x = Random.Range(minCoordinateX, maxCoordinateX);
		position.z = getZCoordinate();
		Instantiate(enemy4Prefab, position, Quaternion.identity);
		enemyCount++;
	}

	private void spawnLevel5Enemy() {
		position.x = Random.Range(minCoordinateX, maxCoordinateX);
		position.z = getZCoordinate();
		Instantiate(enemy5Prefab, position, Quaternion.identity);
		enemyCount++;
	}

	private int getZCoordinate() {
		if (Random.Range(0, 2) == 0) {
			return minCoordinateZ;
		}
		return maxCoordinateZ;
	}

}
