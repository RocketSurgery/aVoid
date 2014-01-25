using UnityEngine;
using System.Collections;

public class TriggerScript3D : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
//		Debug.Log ("Trigger: " + collider.isTrigger);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnTriggerEnter (Collider other)
	{
		SimpleWorldGen.triggerWorldUpdate = true;
		SimpleWorldGen.currentTile = gameObject.transform;
	}

	void OnTriggerStay (Collider other)
	{
				
	}

	void OnTriggerExit (Collider other)
	{

	}
}
