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
	private ArrayList platforms = new ArrayList ();
	
	// Use this for initialization
	void Start ()
	{

		Debug.Log ("intitial world generation started");
		Transform first = Instantiate (platform, new Vector3 (0, 0, 0), Quaternion.identity) as Transform;
		platforms.Add (first);

		Debug.Log (first.position);

		currentTile = first;
		UpdateWorld();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (triggerWorldUpdate) {
			UpdateWorld();
			triggerWorldUpdate = false;
			currentTile = null;
		}
	}
	
	void UpdateWorld ()
	{

		Debug.Log ("UpdateWorld() called");

		Transform originTile = currentTile;

		// xOffset and zOffset is the offset in TILES from the origin tile
		// multiple by scale values to get world coordinates
		for (int xOffset = -1 * maxPlatformDistance; xOffset <= maxPlatformDistance; xOffset++) {
			for (int zOffset = -1 * maxPlatformDistance; zOffset <= maxPlatformDistance; zOffset++) {

//				Debug.Log(originTile.position);
//				Debug.Log("xOffset: " + xOffset);
//				Debug.Log("yOffset: " + zOffset);

				// valid coordinate ofset
				if (Mathf.Abs (xOffset) + Mathf.Abs (zOffset) <= maxPlatformDistance) {

					Vector3 newPosition = new Vector3(xOffset, 0, zOffset);
					newPosition *= 5f;
					newPosition += originTile.position;
					Debug.Log(newPosition);

					bool alreadyExists = false;
					foreach (Transform plat in platforms) {
						if (plat.position == newPosition) {
							Debug.Log ("Tile already exists at position: " + (newPosition));
							alreadyExists = true;
							break;
						}
					}
					if (!alreadyExists) {
//						Debug.Log ("Creating tile at position: " + (newPosition));
						Instantiate (platform, newPosition, Quaternion.identity);
					}

					Debug.Log ("UpdateWorld() yielding");
//					yield return null;
				}
			}
		}
	}
}
