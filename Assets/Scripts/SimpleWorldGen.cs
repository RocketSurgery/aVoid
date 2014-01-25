using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleWorldGen : MonoBehaviour
{

	// Public Settings
	public int maxPlatformDistance = 10;
	public static bool triggerWorldUpdate = false;
	public static Transform currentTile = null;
	public Transform platform;
	public Transform player;
	private ArrayList platforms = new ArrayList ();
	
	// Use this for initialization
	void Start ()
	{

		Debug.Log ("intitial world generation started");
		Transform first = Instantiate (platform, new Vector3 (0, 0, 0), Quaternion.identity) as Transform;
		platforms.Add (first);

		UpdateWorld (first);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (triggerWorldUpdate) {
			UpdateWorld(currentTile);
			triggerWorldUpdate = false;
			currentTile = null;
		}
	}

	delegate bool matches(Transform plat);
	public void UpdateWorld (Transform originTile)
	{

		for (int x = -1 * maxPlatformDistance; x <= maxPlatformDistance; x++) {
			for (int z = -1 * maxPlatformDistance; z <= maxPlatformDistance; z++) {

				Vector3 differenceVector = new Vector3 (x, 0, z);

				// valid coordinate ofset
				if (Mathf.Abs (x) + Mathf.Abs (z) <= maxPlatformDistance) {

					bool alreadyExists = false;
					foreach (Transform plat in platforms) {
						if (plat.position == originTile.position + differenceVector) {
							alreadyExists = true;
							break;
						}
					}
					if (!alreadyExists)
							Instantiate (platform, originTile.position + differenceVector, Quaternion.identity);
				}
			}
		}
	}
}
