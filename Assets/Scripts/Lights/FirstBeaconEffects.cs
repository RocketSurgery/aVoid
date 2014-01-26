using UnityEngine;
using System.Collections;

public class FirstBeaconEffects : MonoBehaviour {
	
	public GameObject mainCharacter;
	public Light pointLight;
	public float intensityStart;
	float scalar;
	public float radius;
	public float height;
	
	
	// Use this for initialization
	void Start () {
		renderer.material.color = new Color(1,1,1,.9f);
	}
	
	// Update is called once per frame
	void Update () {
		pointLight.range = 2*Vector3.Distance(transform.position, mainCharacter.transform.position);
		scalar = 1 / Vector3.Distance(transform.position, mainCharacter.transform.position);
		pointLight.intensity = intensityStart * scalar;
		transform.localScale = new Vector3(radius * scalar * .5f, height, radius * scalar * .5f);
		

	}
}
