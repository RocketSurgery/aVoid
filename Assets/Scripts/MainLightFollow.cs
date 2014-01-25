using UnityEngine;
using System.Collections;

public class MainLightFollow : MonoBehaviour {
	
	public GameObject mainCharacter;
	public float beaconYOffset;
	public float beaconXOffset;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = mainCharacter.transform.position + new Vector3(beaconXOffset, beaconYOffset, 0);
	}
}
