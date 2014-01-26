using UnityEngine;
using System.Collections;

public class MainLightFollow : MonoBehaviour
{
	
	public GameObject mainCharacter;
	public float verticalOffset = 20;
	public float horizontalOffset = 100;
	
	// Use this for initialization
	void Start ()
	{
		transform.position = new Vector3 (mainCharacter.transform.position.x, mainCharacter.transform.position.y + verticalOffset, mainCharacter.transform.position.z + horizontalOffset);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		if (Mathf.Abs (mainCharacter.transform.position.x - transform.position.x) < Mathf.Abs (horizontalOffset)) {
			transform.position = new Vector3 (mainCharacter.transform.position.x, mainCharacter.transform.position.y + verticalOffset, mainCharacter.transform.position.z + horizontalOffset);
			//transform.position = mainCharacter.transform.position + new Vector3(beaconXOffset, beaconYOffset, 0);
		}
	}
}
