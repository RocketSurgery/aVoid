using UnityEngine;
using System.Collections;

public class TriggerScript3D : MonoBehaviour
{

		// Use this for initialization
		void Start ()
		{
				Debug.Log ("Trigger: " + collider.isTrigger);
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		void OnTriggerEnter (Collider other)
		{
				Debug.Log ("entered");
		}

		void OnTriggerStay (Collider other)
		{
				
		}

		void OnTriggerExit (Collider other)
		{
				Debug.Log ("exited");
		}
}
