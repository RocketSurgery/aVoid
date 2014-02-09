using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile {

    // the with of a tile relative to Unity's native units
    // the models of the tiles are 9 times the native unit
    public const float SCALE = 9f;

    // public enum for representing the types of tile
    public enum Shape : int {
        STRAIGHT,
        CORNER,
        SPLIT,
        CROSS,
        DEAD,
        NUM_TYPES,
        INVALID
    };

    // public enum for identifying quadrants
    public enum Quadrant {
        FIRST,
        SECOND,
        THIRD,
        FOURTH,
        INVALID
    };

    public enum Path {
        OPEN,
        WALL,
        EMPTY
    };

    public enum Compass {
        SOUTH = 0,  // negative Z axis
        WEST = 1,   // negative X axis
        NORTH = 2,  // positive Z axis
        EAST = 3    // positive X axis
    };

    // the coordinates in TILES, rather than relative to world coordinates
    // values can never be 0, must be >= 1 or <= -1
    //  public int tileX, tileZ;
    public Shape type;

    // a Transform object representing the position of the tile
    private Transform transform;
    private Compass orientation;

    public Tile(Shape t, Transform trans) {
        type = t;
        transform = trans;

        // reduce euler angle to integral value and convert to Compass type
        if (trans != null)
            orientation = (Compass)(Mathf.Round (transform.eulerAngles.y / 90f) % 4);
    }

    // returns whether the direction of the tile is an opening or a wall
    public Path opening(Compass direction) {

        int dir = (int) direction;
        int orient = (int) orientation;

        if (type == Shape.INVALID)
            return Path.WALL;
        if (type == Shape.CROSS)                // CROSS TILE
            return Path.OPEN;
        else if (type == Shape.DEAD) {          // DEAD END TILE

            // if orientations match, path is open
            // otherwise path is wall
            return (orientation == direction) ? Path.OPEN : Path.WALL;
        } else if (type == Shape.STRAIGHT) {    // STRAIGHT TILE

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
        } else if (type == Shape.CORNER) {      // CORNER TILE
            if (dir == orient || dir == (orient + 1) % 4)
                return Path.OPEN;
            else
                return Path.WALL;
        } else if (type == Shape.SPLIT) {       // SPLIT TILE
            if (dir == orient || dir == (orient + 1) % 4 || dir == (orient + 3) % 4)
                return Path.OPEN;
            else
                return Path.WALL;
        }

        return Path.EMPTY;
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

    public static Shape RandomValidShape(Path[] openings) {
        int numWalls = 0, numOpenings = 0;
        foreach (Path o in openings) {
            if (o == Path.WALL)
                numWalls++;
            else if (o == Path.OPEN)
                numOpenings++;
        }

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
        if (numWalls == 0) {
            potentialShapes.Add(Shape.CROSS);
            potentialShapes.Add(Shape.SPLIT);

            if (numOpenings == 2) {
                // if openings are opposite, add straight, otherwise add corner
                if ((openings[(int)Compass.NORTH] == Path.OPEN && openings[(int)Compass.SOUTH] == Path.OPEN) || (openings[(int)Compass.EAST] == Path.OPEN && openings[(int)Compass.WEST] == Path.OPEN))
                    potentialShapes.Add(Shape.STRAIGHT);
                else
                    potentialShapes.Add(Shape.CORNER);
            }
        } else if (numWalls == 1) {
            potentialShapes.Add(Shape.SPLIT);

            if (numOpenings <= 1)
                // potentialShapes.Add(Shape.DEAD);

                if (numOpenings == 0) {
                    potentialShapes.Add(Shape.STRAIGHT);
                    potentialShapes.Add(Shape.CORNER);
                } else if (numOpenings == 3) {
                    // do nothing, SPLIT already added
                } else if (((openings[(int)Compass.NORTH] == Path.OPEN || openings[(int)Compass.NORTH] == Path.WALL) && (openings[(int)Compass.SOUTH] == Path.OPEN || openings[(int)Compass.SOUTH] == Path.WALL) && openings[(int)Compass.NORTH] != openings[(int)Compass.SOUTH])
                           || ((openings[(int)Compass.EAST] == Path.OPEN || openings[(int)Compass.EAST] == Path.WALL) && (openings[(int)Compass.WEST] == Path.OPEN || openings[(int)Compass.WEST] == Path.WALL) && openings[(int)Compass.EAST] != openings[(int)Compass.WEST]))
                    // a wall is not opposite to an opening
                    potentialShapes.Add(Shape.STRAIGHT);
                else
                    potentialShapes.Add(Shape.CORNER);
        } else if (numWalls == 2) {
            if (numOpenings <= 1)
                // potentialShapes.Add(Shape.DEAD);

                // if opposites are walls, add straight, otherwise add corner
                if ((openings[(int)Compass.NORTH] == Path.WALL && openings[(int)Compass.SOUTH] == Path.WALL) || (openings[(int)Compass.EAST] == Path.WALL && openings[(int)Compass.WEST] == Path.WALL))
                    potentialShapes.Add(Shape.STRAIGHT);
                else
                    potentialShapes.Add(Shape.CORNER);
        }

        // pick a random type from list of available options
        return potentialShapes[Random.Range(0, potentialShapes.Count)];
    }

    public static Vector3 RandomValidOrientation(Shape type, Path[] openings) {

        // CROSS TILE
        // rotationally symmetric, so no need to orient
        if (type == Shape.CROSS || type == Shape.INVALID)
            return new Vector3 (0, 0, 0);

        // Iterate over the 4 orientations and add all the valid ones to a list.
        // An orientation is valid as long as none of the openings face a wall,
        // and none of the walls face an opening.
        List<Vector3> validOrientations = new List<Vector3>();
        for (int i = 0; i < 4; i++) {

            // boolean check to avoid repeated code
            bool isValid = false;

            // DEAD TILE
            if (type == Shape.DEAD) {
                if (    openings[i] != Path.WALL &&             // opening not facing a wall
                        openings[(i + 1) % 4] != Path.OPEN &&   // wall not facing an opening
                        openings[(i + 2) % 4] != Path.OPEN &&   // wall not facing an opening
                        openings[(i + 3) % 4] != Path.OPEN      // wall not facing an opening
                   )
                    isValid = true;
            }
            // STRAIGHT TILE
            // Straight tiles may only have an orientation of 0 or 1,
            // as 2 and 3 are redundant
            else if (type == Shape.STRAIGHT && (i == 0 || i == 1)) {
                if (    openings[i] != Path.WALL &&         // opening not facing a wall
                        openings[i + 2] != Path.WALL &&     // opening not facing a wall
                        openings[i + 1] != Path.OPEN &&     // wall not facing an opening
                        openings[(i + 3) % 4] != Path.OPEN  // wall not facing an opening
                   )
                    isValid = true;
            }
            // CORNER TILE
            else if (type == Shape.CORNER) {
                if (    openings[i] != Path.WALL &&             // opening not facing a wall
                        openings[(i + 1) % 4] != Path.WALL &&   // opening not facing a wall
                        openings[(i + 2) % 4] != Path.OPEN &&   // wall not facing an opening
                        openings[(i + 3) % 4] != Path.OPEN      // wall not facing an opening
                   )
                    isValid = true;
            }
            // SPLIT TILE
            else if (type == Shape.SPLIT) {
                if (    openings[i] != Path.WALL &&             // opening not facing a wall
                        openings[(i + 3) % 4] != Path.WALL &&   // opening not facing a wall
                        openings[(i + 1) % 4] != Path.WALL &&   // opening not facing a wall
                        openings[(i + 2) % 4] != Path.OPEN      // wall not facing an opening
                   )
                    isValid = true;
            }


            // if orientation is valid, add corresponding euler angles to list
            if (isValid)
                validOrientations.Add(new Vector3(0, 90 * i, 0));
        }

        if (validOrientations.Count == 0)
            throw new UnityException();

        return validOrientations[Random.Range(0, validOrientations.Count)];
    }

}
