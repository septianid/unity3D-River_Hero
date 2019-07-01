using UnityEngine;
using System.Collections;

public class ObjectScroller : MonoBehaviour {

	public static int scorePoint = 50;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log(scorePoint);
		transform.Translate(-GameController.scrollSpeed * Time.deltaTime, 0, 0);
	}

	void OnTriggerEnter2D (Collider2D other){

		string objectType = this.GetComponent<SpriteRenderer> ().sprite.texture.name;

		if (other.gameObject.tag == "Destroyer") {

			if (objectType == "32x32_map_tile v1.2") {

				Destroy (this.gameObject);
			} 
			else {

				GameController.seaHealthValue -= 15;
				GameController.failValue += 1;
				Destroy (this.gameObject);
			}


		} 

		else if (other.gameObject.tag == "Player") {

			string playerStatus = other.GetComponent<SpriteRenderer> ().sprite.texture.name;

			if (playerStatus == "red" && (objectType == "oli" || objectType == "kaca")) {

				GameController.scoreValue += scorePoint;
				GameController.increaseSpeedPoint += scorePoint;
				Destroy (this.gameObject);
			} 
			else if (playerStatus == "yellow" && (objectType == "kaleng" || objectType == "botol")) {

				GameController.scoreValue += scorePoint;
				GameController.increaseSpeedPoint += scorePoint;
				Destroy (this.gameObject);
			} 
			else if (playerStatus == "green-orange" && (objectType == "pisang" || objectType == "apple")) {

				GameController.scoreValue += scorePoint;
				GameController.increaseSpeedPoint += scorePoint;
				Destroy (this.gameObject);
			} 
			else if ((playerStatus == "red" || playerStatus == "yellow" || playerStatus == "green-orange") && objectType == "32x32_map_tile v1.2") {

				GameController.boatHealthValue -= 12;
				GameController.failValue += 1;
			} 
			else {
				Destroy (this.gameObject);
				GameController.boatHealthValue -= 24;
			}
		}
	}

}
