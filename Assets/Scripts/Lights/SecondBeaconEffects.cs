using UnityEngine;
using System.Collections;

public class SecondBeaconEffects : MonoBehaviour {
	
	public GameObject mainCharacter;
	public Light pointLight;
	public float intensityStart;
	float scalar;
	public float radius;
	public float height;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		pointLight.range = 2*Vector3.Distance(transform.position, mainCharacter.transform.position);
		scalar = Vector3.Distance(transform.position, mainCharacter.transform.position) / 100;
		pointLight.intensity = intensityStart * scalar;
		transform.localScale = new Vector3(radius * scalar * .5f, height, radius * scalar * .5f);
	}
}
