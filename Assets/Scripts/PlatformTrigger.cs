using UnityEngine;
using System.Collections;

public class PlatformTrigger : MonoBehaviour
{

		void Start ()
		{
				Debug.Log ("Trigger: " + collider2D.isTrigger);
		}

		void OnTrigger2DEnter (Collider2D other)
		{
				Debug.Log ("entered");
		}

		void OnTrigger2DStay (Collider2D other)
		{
				Debug.Log ("staying");
		}

		void OnTrigger2DExit (Collider2D other)
		{
				Debug.Log ("exited");
		}
}
