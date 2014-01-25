using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleWorldGen : MonoBehaviour
{

		public Transform platform;
		public Transform player;
		private List<Object> platforms = new List<Object> ();

		// Use this for initialization
		void Start ()
		{

		Debug.Log ("world generation started");
				for (int i = -4; i <= 4; i++) {
						platforms.Add (Instantiate (platform, new Vector3 (i, 0, 0), Quaternion.identity));
				}
		}
	
		// Update is called once per frame
		void Update ()
		{
				
		}

		void UpdateWorld ()
		{
				
		}
}
