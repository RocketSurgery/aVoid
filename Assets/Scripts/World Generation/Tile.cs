using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
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
		location = loc;
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
