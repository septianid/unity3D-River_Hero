using UnityEngine;
using System.Collections;

public class TargetSpawner : MonoBehaviour {

	public GameObject[] spawnObject;
	public Transform[] spawnObjectLocation;
	int locationIndex;
	int objectIndex;
	public float waitSpawn = 2f;
	public float countDown = 2f;

	// Use this for initialization
	void Start () {
	

	}
	
	// Update is called once per frame
	void Update () {

		locationIndex = Random.Range (0, spawnObjectLocation.Length);
		objectIndex = Random.Range (0, spawnObject.Length);
		SpawnObject ();
	}

	void SpawnObject(){
		
		countDown -= Time.deltaTime;

		if (countDown <= 0) {
			Instantiate (spawnObject[objectIndex], spawnObjectLocation[locationIndex].position, spawnObjectLocation[locationIndex].rotation);
			countDown = waitSpawn;
		}
	}
}
