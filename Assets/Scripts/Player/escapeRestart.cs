using UnityEngine;
using System.Collections;

public class escapeRestart : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {  
    		Application.LoadLevel (0);  
  		}  
		if (Input.GetKeyDown (KeyCode.Alpha1)) {  
    		Application.LoadLevel (2); 
  		}  
	}
}
