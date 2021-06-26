using UnityEngine;
using System.Collections;

public class Invoker : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void invoke_normal(float timer){
		Invoke ("change_normal", timer);
	}

	public void change_normal (){
		ObjectScroller.scorePoint = 50;

	}
}
