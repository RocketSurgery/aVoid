using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour {
	
	public GameObject startMenu;
	public GameObject startPlane;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > 1 && Input.anyKey) {
			Destroy(startMenu);
			Destroy(startPlane);
		}
	}
}
