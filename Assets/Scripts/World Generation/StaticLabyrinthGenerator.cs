using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StaticLabyrinthGenerator : MonoBehaviour {

    // variables for determining average run time
    int num_iterations = 0;
    double total_time = 0.0;
    double average_time = 0.0;

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
    // one 2D list for each coordinate to keep coordinates consistent   public Transform location;
    // convention for access is Q#[xCoord][zCoord]
    List<List<Tile>> Q1 = new List<List<Tile>> ();

    // Place first tile and then start initial level generation
    void Start () {

        // place tile prefabs in an array for easy access
        prefabs [(int)Tile.Shape.STRAIGHT] = straightTile;
        prefabs [(int)Tile.Shape.CROSS] = crossTile;
        prefabs [(int)Tile.Shape.CORNER] = cornerTile;
        prefabs [(int)Tile.Shape.SPLIT] = splitTile;
        prefabs [(int)Tile.Shape.DEAD] = deadTile;

        // set up Q1
        Q1.Insert (0, new List<Tile> ());

        Transform firstTransform = Instantiate (prefabs [(int)Tile.Shape.CORNER], new Vector3 (4.5f, 0f, 4.5f), Quaternion.Euler (new Vector3 (0f, 180f, 0f))) as Transform;
        Tile first = new Tile (Tile.Shape.CORNER, firstTransform);
        Q1 [0].Add(first);
        StartCoroutine ("UpdateWorld", firstTransform);
    }

    // Update is called once per frame
    void Update () {
        if (triggerWorldUpdate && currentTile != null) {
            StopCoroutine ("UpdateWorld");
            StartCoroutine ("UpdateWorld", currentTile);
            triggerWorldUpdate = false;
            currentTile = null;
        }
    }

    /**
     * @brief   Coroutine to update the tiles in the world.
     * @details Iterates over all tiles with a manhattan distance <= maxPlatformDistance
     *          creating any platforms that don't yet exist. UpdateWorld is a coroutine,
     *          and only generates at most 1 tile each frame to preserve runtime performance.
     *
     * @param   originTile The tile the player is currently standing on. Tiles are
     *          generated within manhattan distance of this tile.
     * @return  Return value is used only for coroutine functionality.
     */
    IEnumerator UpdateWorld (Transform originTile) {

        // debuggy stuff
        double startTime = Time.time;

        // xOffset and zOffset is the offset in TILES from the origin tile
        // multiply them by the size of a tile to get world coordinates.
        for (int xOffset = -1 * maxPlatformDistance; xOffset <= maxPlatformDistance; xOffset++) {
            for (int zOffset = -1 * maxPlatformDistance; zOffset <= maxPlatformDistance; zOffset++) {

                // check if coordinate pair is not too far from originTile
                // the sum of the distances is less or equal to the maximum distance
                if (Mathf.Abs (xOffset) + Mathf.Abs (zOffset) > maxPlatformDistance)
                    continue;

                // create a vecor to represent the position of the new potential tile
                // apply Tile.SCALE to it, and add it to originTile.position
                Vector3 newPosition = new Vector3 (xOffset, 0, zOffset);
                newPosition *= Tile.SCALE;
                newPosition += originTile.position;
                newPosition.y = 0f; // without this line, the tiles rise up by 1 each iteration

                // calculate tile indices
                // tile indices start at 1 and go up, or at -1 and go down
                // this is to keep strict adherance to the quadrant format
                // as a value of 0 would be between quadrants.
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

        // update average time
        total_time += Time.time - startTime;
        average_time = total_time / ++num_iterations;
        Debug.Log("average time so far:\t" + average_time);
    }

    /**
     * @brief A method to spawn the next appropriate tile.
     * @details [long description]
     *
     * @param tileX [description]
     * @param tileZ [description]
     * @param newPosition [description]
     */
    void createTile (int tileX, int tileZ, Vector3 newPosition) {

        // Debug.Log ("Creating tile at: " + tileX + ", " + tileZ);

        // get number of tiles adjacent to new tile that open onto new tile
        // get each of the adjacent tiles
        Tile.Path[] openings = new Tile.Path[4];

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

        // print out tile surroundings
        Debug.Log("Tile (" + tileX + ",\t" + tileZ + "\t)");
        for (int i = 0; i < 4; i++) {

            // determine direction
            Tile t = null;
            switch ((Tile.Compass) i) {
            case Tile.Compass.SOUTH:
                t = south;
                break;
            case Tile.Compass.WEST:
                t = west;
                break;
            case Tile.Compass.NORTH:
                t = north;
                break;
            case Tile.Compass.EAST:
                t = east;
                break;
            }

            // print shit out
            if (t != null)
                Debug.Log((Tile.Compass) i + ":\t" + openings[i] + ",\t" + t.type);
            else
                Debug.Log((Tile.Compass) i + ":\t" + openings[i] + ",\tNULL");
        }

        // use Tile.RandomValidType() to get a random type for the new tile
        Tile.Shape newShape = Tile.RandomValidShape(openings);

        // check against invalid tile position (surrounded by walls)
        if (newShape == Tile.Shape.INVALID)
            return;

        // call helper method to get a random, valid orientation
        Vector3 orientation = Tile.RandomValidOrientation(newShape, openings);

        // instantiate the new tile
        Transform newTrans = Instantiate (prefabs [(int)newShape], newPosition, Quaternion.Euler(orientation)) as Transform;
        Tile newTile = new Tile (newShape, newTrans);
        Q1 [tileX - 1] [tileZ - 1] = newTile;
    }

    /**
     * @brief   Saftey method for accessing tiles in a quadrant.
     * @details Retrieves the Tile at (x, z), returning null if the tile does not exist.
     *          The Quadrant will be expanded if it is not large enough to hold (x, z).
     *          After calling getFromQuadrant(), calling Q[x][z] is guaranteed not to throw an exception.
     *
     * @param Q The quadrant to be accessed. To ensure no ambiguity as to which quadrant a coordinate pair
     *          is in, coordinates are indexed at 1, so neither x nor z may be 0.
     *
     * @param x The x coordinate within the quadrant. The coordinate must be given relative to the quarant,
     *          meaning that any negative values must be multiplied by -1 before passing in.
     *
     * @param z The z coordinate within the quadrant. The coordinate must be given relative to the quarant,
     *          meaning that any negative values must be multiplied by -1 before passing in.
     *
     * @return  The Tile object at coordinates (x, z) if Tile exists,
     *          null otherwise.
     */
    private Tile getFromQuadrant (List<List<Tile>> Q, int x, int z) {

        // Debug.Log("Retrieving Q[" + (x - 1) + "][" + (z  - 1) + "]");

        // VALIDITY CHECKING
        // ensure that x and z are positive
        if (x <= 0 || z <= 0)
            // return null;
            // return invalid till to wall off out of bounds areas
            return new Tile(Tile.Shape.INVALID, null);

        // ensure that Q1 is large enough along x to hold tile and one further
        // expand it if it's too small
        while (Q.Count < x)
            Q.Add(new List<Tile> ());

        // ensure that Q1 is large enough along z to hold tile and one further
        // expand it if it's too small
        List<Tile> col = Q [x - 1];
        while (col.Count < z)
            col.Add(null);

        return Q [x - 1] [z - 1];
    }
}
