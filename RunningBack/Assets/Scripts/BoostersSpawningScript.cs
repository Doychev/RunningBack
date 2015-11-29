using UnityEngine;
using System.Collections;

public class BoostersSpawningScript : MonoBehaviour {

	public GameObject boosterPrefab;
	public int minCoordinateX, maxCoordinateX, minCoordinateZ, maxCoordinateZ;

	// Use this for initialization
	void Start () {
	}

	private int getZCoordinate() {
		if (Random.Range(0, 2) == 0) {
			return minCoordinateZ;
		}
		return maxCoordinateZ;
	}

	public void SpawnBooster() {
		Vector3 position = new Vector3 ();
		position.y = 2.5f;
		position.x = Random.Range(minCoordinateX, maxCoordinateX);
		position.z = Random.Range(minCoordinateZ, maxCoordinateZ);
		Instantiate(boosterPrefab, position, Quaternion.identity);
	}
}
