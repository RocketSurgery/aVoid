using UnityEngine;
using System.Collections;

public abstract class StaticLabyrinthGenerator : MonoBehaviour
{

	// compass for easy access to cardinal directions
	public enum COMPASS
	{
		NORTH,
		SOUTH,
		EAST,
		WEST
	}
	
	// super special Tile class to represent tiles and provide basic information about them
	private class Tile
	{

		public static float tileScale = 9f;

		public enum Type : int
		{
			STRAIGHT,
			CORNER,
			SPLIT,
			CROSS,
			DEAD,
			NUM_TYPES
		};

		public Transform location;

		public Tile (Transform loc)
		{
			// TODO write Tile constructor
		}

		// returns and array of booleans with 4 elements in it
		// each elements represents a cardinal direction,
		// true means that the tile opens up to that direction,
		// false means that the tile is closed to that direction
		public bool[] openings ()
		{
			// TODO write Tile.openeings()

			return null;
		}
	}

	// maximum distance from current origin to generate new platforms
	// distance is in manhattan distance
	[Range (1, 25)]
	public int
		maxPlatformDistance = 10;

	// references to tile prefabs
	// must be set by dragging over the prefabs in the editor
	public Transform straightTile, cornerTile, splitTile, crossTile, deadTile;
	private Transform[] prefabs = new Transform[(int)Tile.Type.NUM_TYPES];

	// public static variables to allow trigger scripts to communicate with world generation script
	// probably thread safe. Probably.
	public static bool triggerWorldUpdate = false;
	public static Transform currentTile = null;
	
	// Place first tile and then start initial level generation
	void Start ()
	{

		// place tile prefabs in an array for easy access
		prefabs [(int)Tile.Type.STRAIGHT] = straightTile;
		prefabs [(int)Tile.Type.CROSS] = crossTile;
		prefabs [(int)Tile.Type.CORNER] = cornerTile;
		prefabs [(int)Tile.Type.SPLIT] = splitTile;
		prefabs [(int)Tile.Type.DEAD] = deadTile;

		// begin world generation by create a tile at the origin
		Debug.Log ("intitial world generation started");
//		Transform first = Instantiate (platform, new Vector3 (0, 0, 0), Quaternion.identity) as Transform;
//		UpdateWorld (first);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (triggerWorldUpdate) {
			UpdateWorld (currentTile);
			triggerWorldUpdate = false;
			currentTile = null;
		}
	}
	
	public void UpdateWorld (Transform originTile)
	{
		
		// xOffset and zOffset is the offset in TILES from the origin tile
		// multiple by scale values to get world coordinates
		for (int xOffset = -1 * maxPlatformDistance; xOffset <= maxPlatformDistance; xOffset++) {
			for (int zOffset = -1 * maxPlatformDistance; zOffset <= maxPlatformDistance; zOffset++) {
				
				// valid coordinate ofset
				if (Mathf.Abs (xOffset) + Mathf.Abs (zOffset) <= maxPlatformDistance) {
					
					Vector3 newPosition = new Vector3 (xOffset, 0, zOffset);
					newPosition *= 5f;
					newPosition += originTile.position;
					Debug.Log (newPosition);
					
					bool alreadyExists = false;
//					foreach (Transform plat in tiles) {
//						if (plat.position == newPosition) {
//							Debug.Log ("Tile already exists at position: " + (newPosition));
//							alreadyExists = true;
//							break;
//						}
//					}
//					if (!alreadyExists) {
//						Instantiate (platform, newPosition, Quaternion.identity);
//					}
				}
			}
		}
	}
}
