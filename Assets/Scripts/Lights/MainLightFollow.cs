using UnityEngine;
using System.Collections;

public class MainLightFollow : MonoBehaviour {
	
	public GameObject mainCharacter;
	public float beaconYOffset;
	public float beaconXOffset;
	
	// Use this for initialization
	void Start () {
		transform.position = new Vector3(mainCharacter.transform.position.x + beaconXOffset, mainCharacter.transform.position.y + beaconYOffset, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Mathf.Abs (mainCharacter.transform.position.x - transform.position.x) < Mathf.Abs(beaconXOffset)) {
			transform.position = new Vector3(mainCharacter.transform.position.x + beaconXOffset, mainCharacter.transform.position.y + beaconYOffset, 0);
			//transform.position = mainCharacter.transform.position + new Vector3(beaconXOffset, beaconYOffset, 0);
		}
	}
}
