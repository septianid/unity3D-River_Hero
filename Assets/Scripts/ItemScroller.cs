using UnityEngine;
using System.Collections;

public class ItemScroller : MonoBehaviour {

	public float itemTimer;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		transform.Translate(-GameController.scrollSpeed * Time.deltaTime, 0, 0);
	}

	void OnTriggerEnter2D (Collider2D others){

		string itemType = this.GetComponent<SpriteRenderer> ().sprite.texture.name;

		if (others.gameObject.tag == "Destroyer") {

			Destroy (this.gameObject);
		} 
		else if (others.gameObject.tag == "Player") {

			if (itemType == "pt1") {

				GameController.boatHealthValue += (5 * ItemSpawner.Z_addHPPlayer);
				Destroy (this.gameObject);
			} 
			else if (itemType == "pt2") {

				GameController.seaHealthValue += (5 * ItemSpawner.Z_addHPSea);
				Destroy (this.gameObject);
			} 
			else if (itemType == "pt3") {

				GameController.scrollSpeed -= (0.05f * ItemSpawner.Z_slowerSpeed);
				Destroy (this.gameObject);
			} 
			else if (itemType == "pt4") {

				itemTimer = 1.0f * ItemSpawner.Z_doubleScore;
				Debug.Log(itemTimer);
				ObjectScroller.scorePoint = 150;
				Destroy (this.gameObject);
				GameObject.Find ("Invoker").GetComponent<Invoker> ().invoke_normal (itemTimer);
			}
		}

	}


}
