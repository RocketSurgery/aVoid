using UnityEngine;
using System.Collections;

public class Tile
{

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
		INVALID
	}
	;

	// public enum for identifying quadrants
	public enum Quadrant
	{
		FIRST,
		SECOND,
		THIRD,
		FOURTH,
		INVALID
	}

	public enum Path
	{
		OPEN,
		WALL,
		EMPTY
	}

	public enum Compass
	{
		SOUTH = 0,	// negative Z axis
		WEST = 1,	// negative X axis
		NORTH = 2,	// positive Z axis
		EAST = 3	// positive X axis
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
	public Path opening (Compass direction)
	{
		// TODO write Tile.openeings()
		
		return Path.OPEN;
	}

	public static Quadrant GetQuadrant (Vector3 pos)
	{

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

	public static Type RandomValidType (Path[] openings)
	{
		int numWalls = 0;
		foreach (Path o in openings)
			if (o == Path.WALL)
				numWalls++;

		int numOpenings = 0;
		foreach (Path o in openings)
			if (o == Path.OPEN)
				numOpenings++;

		// assert(numOpenings + numWalls <= 4)
		if (numOpenings + numWalls > 4)
			throw new UnityException ();

		// all walls
		if (numWalls == 4)
			return Type.INVALID;

		// all openings
		if (numOpenings == 4)
			return Type.CROSS;

		// no walls or openings
		if ((numOpenings == 0 || numOpenings == 1) && numWalls == 0)
			return (Type)Random.Range (0, (int)Type.NUM_TYPES);

		// 3 openings 1 wall
		if (numOpenings == 3 && numWalls == 1)
			return Type.SPLIT;

		// 2 openings 2 walls
		if (numOpenings == 2 && numWalls == 2) {

			// if openings are opposite
			if ((openings [(int)Compass.NORTH] == Path.OPEN && openings [(int)Compass.SOUTH] == Path.OPEN) || (openings [(int)Compass.EAST] == Path.OPEN && openings [(int)Compass.WEST] == Path.OPEN))
				return Type.STRAIGHT;
			else 
				return Type.CORNER;
		}

		// 1 opening 3 walls
		if (numWalls == 3)
			return Type.DEAD;

		// ALL THE REST

		// if all else fails, return a cross
		return Type.CROSS;
	}

	public static Vector3 CorrectOrientation (Type type, Path[] openings)
	{

		int numWalls = 0;
		foreach (Path o in openings)
			if (o == Path.WALL)
				numWalls++;
		
		int numOpenings = 0;
		foreach (Path o in openings)
			if (o == Path.OPEN)
				numOpenings++;

		// CROSS TILES
		if (type == Type.CROSS)
			return new Vector3 (0, 0, 0);

		// DEAD TILES
		if (type == Type.DEAD) {

			// exactly 1 opening
			// must connect with opening
			if (numOpenings == 1)
				for (int i = 0; i < 4; i++)
					if (openings [i] == Path.OPEN)
						return new Vector3 (0, i * 90f, 0);

			// no openings, just be sure not to face towards a wall
			// keep trying until a non-wall orientation is found
			while (true) {
				int rand = Random.Range (0, 4);
				if (openings [rand] != Path.WALL)
					return new Vector3 (0, rand * 90f, 0);
			}

		}

		// STRAIGHT TILES
		if (type == Type.STRAIGHT) {

			// if 2 openings, determine which orientation
			if (numOpenings == 2)
				return (openings [(int)Compass.SOUTH] == Path.OPEN) ? new Vector3 (0, 0, 0) : new Vector3 (0, 90f, 0);

			// 1 opening, must face opening
			if (numOpenings == 1) {
				if (openings [(int)Compass.NORTH] == Path.OPEN || openings [(int)Compass.SOUTH] == Path.OPEN)
					return new Vector3 (0, 0, 0);
				else
					return new Vector3 (0, 90f, 0);
			}

			// no openings, just dont face a wall
			if (openings [(int)Compass.EAST] == Path.WALL || openings [(int)Compass.WEST] == Path.WALL) // walls on sides, orient vertically
				return new Vector3 (0, 0, 0);
			else // walls on top or bottom, orient horizontally
				new Vector3 (0, 90f, 0);
		}

		// SPLIT TILES
		if (type == Type.SPLIT) {

			// if 1 wall, face away from the wall
			// splits orient towards their middle opening
			// so to orient the back towards a wall
			// find the direction to the wall, then add 180
			if (numWalls == 1) {
				for (int i = 0; i < 4; i++)
					if (openings [i] == Path.WALL)
						return new Vector3 (0, 90f * i + 180f, 0);
			}

			// TODO not simple solution
			// find first empty neighbor and orient back towards it
			for (int i = 0; i < 4; i++)
				if (openings [i] == Path.EMPTY)
					return new Vector3 (0, 90f * i + 180f, 0);
		}

		// CORNER TILES
		if (type == Type.CORNER) {

			// 2 openings, orient so that the corner is between them
			// corner defaults to being between SOUTH and WEST
			for (int i = 0; i < 4; i++)
				if (openings [i] == Path.OPEN && openings [(i + 1) % 4] == Path.OPEN)
					return new Vector3 (0, i * 90f, 0);

			// 1 opening, find opening and then pick one of the two paths at random
			// TODO implement more robust solution
			if (numOpenings == 1) {
				for (int i = 0; i < 4; i++)
					if (openings [i] == Path.EMPTY) {
						return (Random.Range (0, 1) == 0) ? new Vector3 (0, i * 90f, 0) : new Vector3 (0, (i - 1) * 90f, 0);
					}
			}

			// no openings, orient however, just don't face a wall
			for (int i = 0; i < 4; i++)
				if (openings [i] != Path.WALL && openings [(i + 1) % 4] != Path.WALL)
					return new Vector3 (0, i * 90f, 0);
		}

		// catchall return
		return new Vector3 (0, 0, 0);
	}
}
