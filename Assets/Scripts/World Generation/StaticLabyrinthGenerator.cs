using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StaticLabyrinthGenerator : MonoBehaviour
{

	// compass for easy access to cardinal directions

	// maximum distance from current origin to generate new platforms
	// distance is in manhattan distance
	[Range (1, 10)]
	public int
		maxPlatformDistance = 3;

	// references to tile prefabs
	// must be set by dragging over the prefabs in the editor
	public Transform straightTile, cornerTile, splitTile, crossTile, deadTile;
	private Transform[] prefabs = new Transform[(int)Tile.Shape.NUM_TYPES];

	// public static variables to allow trigger scripts to communicate with world generation script
	// probably thread safe. Probably.
	public static bool triggerWorldUpdate = false;
	public static Transform currentTile = null;

	// 2D List to hold the generated tiles
	// one 2D list for each coordinate to keep coordinates consistent	public Transform location;
	// convention for access is Q#[xCoord][zCoord]
	List<List<Tile>> Q1 = new List<List<Tile>> ();
	
	// Place first tile and then start initial level generation
	void Start ()
	{

		// place tile prefabs in an array for easy access
		prefabs [(int)Tile.Shape.STRAIGHT] = straightTile;
		prefabs [(int)Tile.Shape.CROSS] = crossTile;
		prefabs [(int)Tile.Shape.CORNER] = cornerTile;
		prefabs [(int)Tile.Shape.SPLIT] = splitTile;
		prefabs [(int)Tile.Shape.DEAD] = deadTile;

		// set up Q1
		Q1.Insert (0, new List<Tile> ());

		// pick a random first tile
		int firstType = Random.Range (0, (int)Tile.Shape.NUM_TYPES);

		Transform firstTransform = Instantiate (prefabs [(int)Tile.Shape.CORNER], new Vector3 (4.5f, 0f, 4.5f), Quaternion.Euler (new Vector3 (0f, 180f, 0f))) as Transform;
		Tile first = new Tile ((Tile.Shape)firstType, firstTransform);
		Q1 [0].Insert (0, first);
		StartCoroutine ("UpdateWorld", firstTransform);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (triggerWorldUpdate && currentTile != null) {
			StopCoroutine ("UpdateWorld");
			StartCoroutine ("UpdateWorld", currentTile);
			triggerWorldUpdate = false;
			currentTile = null;
		}
	}
	
	IEnumerator UpdateWorld (Transform originTile)
	{

		// xOffset and zOffset is the offset in TILES from the origin tile
		// multiple by scaler values to get world coordinates
		for (int xOffset = -1 * maxPlatformDistance; xOffset <= maxPlatformDistance; xOffset++) {
			for (int zOffset = -1 * maxPlatformDistance; zOffset <= maxPlatformDistance; zOffset++) {
				
				// check if coordinate pair is not too far from originTile
				// the sum of the distances is less or equal to the maximum distance
				if (Mathf.Abs (xOffset) + Mathf.Abs (zOffset) <= maxPlatformDistance) {

					// create a vecor to represent the position of the new potential tile
					// apply Tile.SCALE to it, and add it to originTile.position
					Vector3 newPosition = new Vector3 (xOffset, 0, zOffset);

					newPosition *= Tile.SCALE;

					newPosition += originTile.position;
					newPosition.y = 0f;

					// calculate tile indices
					// tile indices start at 1 and go up, or at -1 and go down
					// this is to keep strict adherance to the quadrant format
					int tileX = (int)((newPosition.x > 0) ? (newPosition.x + 4.5) / 9 : (newPosition.x - 4.5) / 9);
					int tileZ = (int)((newPosition.z > 0) ? (newPosition.z + 4.5) / 9 : (newPosition.z - 4.5) / 9);

					// search appropriate quadrant to see if it already exists
					// if not, pick a valid one tile option and instantiate it
					switch (Tile.GetQuadrant (newPosition)) {
					case Tile.Quadrant.FIRST:

						// test for null, if null, add instantiate new item
						if (getFromQuadrant (Q1, tileX, tileZ) == null) {
							createTile (tileX, tileZ, newPosition);
						}

						break;

					case Tile.Quadrant.INVALID:
						throw new UnityException (); // this should never happen. like, ever.
					}

					// yield coroutine
					// magic efficiency code
					yield return null;
				}
			}
		}
	}

	void createTile (int tileX, int tileZ, Vector3 newPosition)
	{

		// Debug.Log ("Creating tile at: " + tileX + ", " + tileZ);

		// get number of tiles adjacent to new tile that open onto new tile
		// get each of the adjacent tiles
		Tile.Path[] openings = new Tile.Path[4];

		// NORTH OPENING
		Tile north = getFromQuadrant (Q1, tileX, tileZ + 1);
		if (north != null)
			openings [(int)Tile.Compass.NORTH] = north.opening (Tile.Compass.SOUTH);
		else
			openings [(int)Tile.Compass.NORTH] = Tile.Path.EMPTY;

		// EAST OPENING
		Tile east = getFromQuadrant (Q1, tileX + 1, tileZ);
		if (east != null)
			openings [(int)Tile.Compass.EAST] = east.opening (Tile.Compass.WEST);
		else
			openings [(int)Tile.Compass.EAST] = Tile.Path.EMPTY;

		// SOUTH OPENING
		Tile south = getFromQuadrant (Q1, tileX, tileZ - 1);
		if (south != null)
			openings [(int)Tile.Compass.SOUTH] = south.opening (Tile.Compass.NORTH);
		else
			openings [(int)Tile.Compass.SOUTH] = Tile.Path.EMPTY;

		// WEST OPENING
		Tile west = getFromQuadrant (Q1, tileX - 1, tileZ);
		if (west != null)
			openings [(int)Tile.Compass.WEST] = west.opening (Tile.Compass.EAST);
		else
			openings [(int)Tile.Compass.WEST] = Tile.Path.EMPTY;

		// use Tile.RandomValidType() to get a random type
		// for the new tile
		int newType = (int)Tile.RandomValidType (openings);

		// check against invalid tile position (surrounded by walls)
		if ((Tile.Shape)newType == Tile.Shape.INVALID)
			return;

		Vector3 orientation = Tile.CorrectOrientation ((Tile.Shape)newType, openings);

		Transform newTrans = Instantiate (prefabs [newType], newPosition, Quaternion.Euler(orientation)) as Transform;
		Tile newTile = new Tile (Tile.Shape.CROSS, newTrans);
		Q1 [tileX - 1] [tileZ - 1] = newTile;
	}

	// a safety method for getting tiles from a quadrant
	// returns null if the list isn't large enough
	// in either direction to have the requested tile
	// paremeters are indices relative to quarant, so they have an index of 1
	// method will expand the lists if the lists aren't big enough
	private Tile getFromQuadrant (List<List<Tile>> Q, int x, int z)
	{

		// Debug.Log ("GFQ: " + x + ", " + z);

		if (x == 0 || z == 0)
			return null;

		// ensure that Q1 is large enough along x to hold tile and one further
		// expand it if it's too small
		if (Q1.Count < x) {
			
			// Q1 not wide enough to fit new tile
			// expand Q1 along x to fit
			for (int i = Q1.Count; Q1.Count < x; i++)
				Q1.Insert (i, new List<Tile> ());
		}
		
		// ensure that Q1 is large enough along z to hold tile and one further
		// expand it if it's too small
		List<Tile> col = Q1 [x - 1];
		if (col.Count < z) {
			
			// Q1 not tall anough along z to fit new tile
			// expand Q1 along z to fit
			for (int i = col.Count; col.Count < z; i++)
				col.Insert (i, null);
		}

		return Q1 [x - 1] [z - 1];
	}
}
