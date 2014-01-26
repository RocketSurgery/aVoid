using UnityEngine;
using System.Collections;

public class Tile {

	// the with of a tile relative to Unity's native units
	// the models of the tiles are 9 times the native unit
	public const float SCALE = 9f;

	// public enum for representing the types of tile
	public enum Type : int
	{
		STRAIGHT,
		CORNER,
		SPLIT,
		CROSS,
		DEAD,
		NUM_TYPES,
		UNKNOWN
	};

	// public enum for identifying quadrants
	public enum Quadrant {
		FIRST,
		SECOND,
		THIRD,
		FOURTH,
		INVALID
	}

	// the coordinates in TILES, rather than relative to world coordinates
	// values can never be 0, must be >= 1 or <= -1
//	public int tileX, tileZ;
	public Type tileType;

	// a Transform object representing the position of the tile
	private Transform transform;

	public Tile (Type t, Transform trans)
	{
//		tileX = (int) ((loc.position.x > 0) ? (loc.position.x + 4.5) / 9 : (loc.position.x - 4.5) / 9);
//		tileZ = (int) ((loc.position.z > 0) ? (loc.position.z + 4.5) / 9 : (loc.position.z - 4.5) / 9);
		tileType = t;
		transform = trans;

//		Debug.Log ("Tile pos: " + tileX + ", " + tileZ);
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

	public static Quadrant GetQuadrant(Vector3 pos) {

		// first quadrant
		if (pos.x > 0 && pos.z > 0)
			return Quadrant.FIRST;

		// second quadrant
		if (pos.x < 0 && pos.z > 0)
			return Quadrant.SECOND;

		// third quadrant
		if (pos.x < 0 && pos.z < 0)
			return Quadrant.THIRD;

		if (pos.x > 0 && pos.z < 0)
			return Quadrant.FOURTH;

		return Quadrant.INVALID;
	}
}
