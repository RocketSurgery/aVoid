using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile
{

	// the with of a tile relative to Unity's native units
	// the models of the tiles are 9 times the native unit
	public const float SCALE = 9f;

	// public enum for representing the types of tile
	public enum Shape : int
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
	public Shape type;

	// a Transform object representing the position of the tile
	private Transform transform;
	private Compass orientation;

	public Tile (Shape t, Transform trans)
	{
		type = t;
		transform = trans;

		// reduce euler angle to integral value and convert to Compass type
		orientation = (Compass)(Mathf.Round (transform.eulerAngles.y / 90f) % 4);

		// Debug.Log ("Tile pos: " + tileX + ", " + tileZ);
	}

	// returns whether the direction of the tile is an opening or a wall
	public Path opening (Compass direction)
	{

		// CROSS TILE
		if (type == Shape.CROSS)
			return Path.OPEN;

		// DEAD END TILE
		// if orientations match, path is open
		// otherwise path is wall
		if (type == Shape.DEAD)
			return (orientation == direction) ? Path.OPEN : Path.WALL;

		// STRAIGHT TILE
		if (type == Shape.STRAIGHT) {

			// straight tiles can only have a dir of
			// SOUTH or WEST
			// reorient search direction to match orientation of 
			if (direction == Compass.EAST)
				direction = Compass.WEST;
			else if (direction == Compass.NORTH)
				direction = Compass.SOUTH;

			
			// if orientations match, path is open
			// otherwise path is wall
			return (direction == orientation) ? Path.OPEN : Path.WALL;
		}

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

	public static Shape RandomValidType (Path[] openings)
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
			return Shape.INVALID;

		// all openings
		if (numOpenings == 4)
			return Shape.CROSS;

		// no walls or openings
		if ((numOpenings == 0 || numOpenings == 1) && numWalls == 0)
			return (Shape)Random.Range (0, (int)Shape.NUM_TYPES);

		// 3 openings 1 wall
		if (numOpenings == 3 && numWalls == 1)
			return Shape.SPLIT;

		// 2 openings 2 walls
		if (numOpenings == 2 && numWalls == 2) {

			// if openings are opposite then 
			if ((openings [(int)Compass.NORTH] == Path.OPEN && openings [(int)Compass.SOUTH] == Path.OPEN) || (openings [(int)Compass.EAST] == Path.OPEN && openings [(int)Compass.WEST] == Path.OPEN))
				return Shape.STRAIGHT;
			else 
				return Shape.CORNER;
		}

		// 1 opening 3 walls
		if (numWalls == 3)
			return Shape.DEAD;

		// ALL THE REST
		// 0 WALLS, 2 OPENINGS
		List<Shape> potentialShapes = new List<Shape> ();
		if (numWalls == 0 && numOpenings == 2) {
			potentialShapes.Add(Shape.CROSS);
		}


		// if all else fails, return a cross
		return Shape.CROSS;
	}
	
	public static Vector3 CorrectOrientation (Shape type, Path[] openings)
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
		if (type == Shape.CROSS)
			return new Vector3 (0, 0, 0);

		// DEAD END TILES
		if (type == Shape.DEAD) {

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
		if (type == Shape.STRAIGHT) {

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
		if (type == Shape.SPLIT) {

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
		if (type == Shape.CORNER) {

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
