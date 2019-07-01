using UnityEngine;
using System.Collections;

public class BGScroller : MonoBehaviour {

	public BoxCollider2D groundCollider;       //This stores a reference to the collider attached to the Ground.
	public float groundHorizontalLength;       //A float to store the x-axis length of the collider2D attached to the Ground GameObject.
	public Transform cameraTransform;
	public float speed;

	// Use this for initialization
	void Start () 
	{
		//Get and store a reference to the collider2D attached to Ground.
		groundCollider = GetComponent<BoxCollider2D> ();
		//Store the size of the collider along the x axis (its length in units).
		groundHorizontalLength = groundCollider.size.x;

		cameraTransform = Camera.main.transform;
	}

	void Update()
	{
		transform.Translate (-GameController.scrollSpeed * Time.deltaTime, 0, 0);

		if ((transform.position.x + groundHorizontalLength) < cameraTransform.position.x)
		{
			//If true, this means this object is no longer visible and we can safely move it forward to be re-used.
			RepositionBackground ();
		}
			
	}

	private void RepositionBackground()
	{
		//This is how far to the right we will move our background object, in this case, twice its length. This will position it directly to the right of the currently visible background object.
		Vector2 groundOffSet = new Vector2(groundHorizontalLength * 2f, 0);

		//Move this object from it's position offscreen, behind the player, to the new position off-camera in front of the player.
		transform.position = (Vector2) transform.position + groundOffSet;
	}
}
