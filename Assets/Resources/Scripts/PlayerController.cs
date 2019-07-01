using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private GameObject player;
	private float playerSpeed = 3.0f;
	public Sprite[] playerSprites;

	void Awake () {
	
		playerSprites = Resources.LoadAll<Sprite> ("Sprites/Boat");
	}

	// Use this for initialization
	void Start () {
		
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey(KeyCode.LeftArrow)){
			transform.position += Vector3.left * playerSpeed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.RightArrow)){
			transform.position += Vector3.right * playerSpeed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.UpArrow)){
			transform.position += Vector3.up * playerSpeed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.DownArrow)){
			transform.position += Vector3.down * playerSpeed * Time.deltaTime;
		}

		if (Input.GetKey(KeyCode.Q)) {
			player.GetComponent<SpriteRenderer>().sprite = playerSprites [0];
		}
		if (Input.GetKey(KeyCode.W)) {
			player.GetComponent<SpriteRenderer>().sprite = playerSprites [2];
		}
		if (Input.GetKey(KeyCode.E)) {
			player.GetComponent<SpriteRenderer>().sprite = playerSprites [1];
		}
	}
}
