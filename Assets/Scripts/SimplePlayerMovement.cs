using UnityEngine;
using System.Collections;

public class SimplePlayerMovement : MonoBehaviour {

	public float movementSpeed = 1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKey (KeyCode.RightArrow)) {
			rigidbody2D.AddForce (Vector2.right * movementSpeed);
		} else if (Input.GetKey (KeyCode.LeftArrow)) {
			rigidbody2D.AddForce(Vector2.right * -1 * movementSpeed);
		}
	}
}
