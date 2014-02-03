using UnityEngine;
using System.Collections;

public class BeaconOffset : MonoBehaviour
{

	public GameObject mainCharacter;
	public float hoverHeight = 20;
	public float minHorizontalDistance = 100;

	private 

	// Use this for initialization
	void Start ()
	{
		transform.position = new Vector3 (mainCharacter.transform.position.x, hoverHeight, mainCharacter.transform.position.z + minHorizontalDistance);
	}
	
	// Update is called once per frame
	void Update ()
	{

		// as player 
		if (Mathf.Abs (mainCharacter.transform.position.z - transform.position.z) < Mathf.Abs (minHorizontalDistance)) {
			transform.position = new Vector3 (mainCharacter.transform.position.x, hoverHeight, mainCharacter.transform.position.z + minHorizontalDistance);
			//transform.position = mainCharacter.transform.position + new Vector3(beaconXOffset, beaconYOffset, 0);
		}
	}
}
