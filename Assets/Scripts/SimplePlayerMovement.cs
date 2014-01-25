using UnityEngine;
using System.Collections;

public class SimplePlayerMovement : MonoBehaviour {

	public float movementSpeed = 20f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKey (KeyCode.RightArrow)) {
			rigidbody.AddForce (Vector3.right * movementSpeed);
		} else if (Input.GetKey (KeyCode.LeftArrow)) {
			rigidbody.AddForce(Vector3.right * -1 * movementSpeed);
		}
	}
}
