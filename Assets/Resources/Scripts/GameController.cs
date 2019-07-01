using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public static GameController instance;
	public static int scoreValue;
	public static float seaHealthValue;
	public static float boatHealthValue;
	public static float distanceValue;
	public static int failValue;
	public Text score;
	public Text seaHealth;
	public Text boatHealth;
	public Text distance;
	public Text fail;
	public static float scrollSpeed;
	public static float startCountingPoint;
	public static int increaseSpeedPoint;


	/*void Awake(){
		//If we don't currently have a game control...
		if (instance == null)
			//...set this one to be it...
			instance = this;
		//...otherwise...
		else if(instance != this)
			//...destroy this one because it is a duplicate.
			Destroy (gameObject);
	}*/

	void Start(){

		scrollSpeed = 3.0f;
		startCountingPoint = 0f;
		increaseSpeedPoint = 0;

		score = GameObject.Find ("Canvas/ScoreText").GetComponent<Text> ();
		seaHealth = GameObject.Find ("Canvas/SeaHealthText").GetComponent<Text> ();
		boatHealth = GameObject.Find ("Canvas/PlayerHealthText").GetComponent<Text> ();
		distance = GameObject.Find ("Canvas/DistanceText").GetComponent<Text> ();
		fail = GameObject.Find ("Canvas/HitCountText").GetComponent<Text> ();
		scoreValue = 0;
		failValue = 10;
		seaHealthValue = 80f;
		boatHealthValue = 80f;
		distanceValue = 0;
	}

	void Update(){

		distanceValue = distanceValue + Time.deltaTime;
		startCountingPoint += Time.deltaTime;

		score.text = "" + scoreValue;
		seaHealth.text = "" + (int)seaHealthValue;
		boatHealth.text = "" + (int)boatHealthValue;
		distance.text = "" + (int)distanceValue;
		fail.text = "Hit : " + failValue;

		if (seaHealthValue <= 0) {
			seaHealthValue = 0;
			Time.timeScale = 0;
		} 
		else if (boatHealthValue <= 0) {
			boatHealthValue = 0;
			Time.timeScale = 0;
		} 
		else if (seaHealthValue >= 100) {
			seaHealthValue = 100;
		} 
		else if (boatHealthValue >= 100) {
			boatHealthValue = 100;
		}

		if (increaseSpeedPoint >= 500) {
			increaseSpeedPoint = 0;
			scrollSpeed += 1.0f;
		}
	}
}
