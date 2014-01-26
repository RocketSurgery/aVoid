using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DynamicFreeWorldGenerator : MonoBehaviour
{

	// a struct representing nodes in a graph of tiles around the player i guess
	public class Node : IEquatable<Node>
	{
		public Transform transform;
		public Node[] adjacent;

		public Node (Transform trans, Node[] adj)
		{
			transform = trans;
			adjacent = adj;
		}

		// two nodes are equal if their position vecors are the same
		public bool Equals (Node other)
		{
			return transform.position == other.transform.position;
		}
	}

	// compass for easy access to cardinal directions
	public enum Compass
	{
		NORTH,	// positive Z axis
		SOUTH,	// negative Z axis
		EAST,	// positive X axis
		WEST	// negative X axis
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
	public bool triggerWorldUpdate = false;
	public Node currentNode;
	
	// Place first tile and then start initial level generation
	void Start ()
	{
		
		// place tile prefabs in an array for easy access
		prefabs [(int)Tile.Type.STRAIGHT] = straightTile;
		prefabs [(int)Tile.Type.CROSS] = crossTile;
		prefabs [(int)Tile.Type.CORNER] = cornerTile;
		prefabs [(int)Tile.Type.SPLIT] = splitTile;
		prefabs [(int)Tile.Type.DEAD] = deadTile;
		
		// pick a random first tile
		DynamicFreeWorldGenerator_Trigger firstTrigger = Instantiate (prefabs [(int)Tile.Type.CROSS], new Vector3 (4.5f, 0, 4.5f), Quaternion.identity) as DynamicFreeWorldGenerator_Trigger;

		// assign generator reference
		firstTrigger.generator = this;

		// initialize array of adjacent nodes
		Node[] adj = new Node[4];
		for (int i = 0; i < 4; i++) {
			adj [i] = null;
		}
		Node firstNode = new Node (firstTrigger.transform, adj);

		firstTrigger.node = firstNode;
		
//		StartCoroutine("UpdateWorld", firstTransform);
	}

	IEnumerator UpdateWorld (Node origin)
	{

		// queue for breadth first search
		Queue<Node> queue = new Queue<Node> ();
		List<Node> visited = new List<Node> ();
		queue.Enqueue (origin);


		// iterate over each of the pairs of cooridantes
		while (queue.Count != 0) {

			// pop next node off queue
			Node current = queue.Dequeue ();

			// add current to visited
			visited.Add (current);

			// iterate over each of the adjacent nodes
			for (int i = 0; i < 4; i++) {
				Node next = current.adjacent [i];

				// if next is null, instantiate object and add node
				if (next == null) {

					// check if within bounds

					continue;
				}

				// node exists
				// if in visited, ignore
				if (visited.Contains (next))
					continue;

				// node not already visited



				// if too far, destroy and add to visited

			}


			yield return null;
		}
	}

//	IEnumerator UpdateWorld_old (Transform originTile)
//	{
//			
//		// xOffset and zOffset is the offset in TILES from the origin tile
//		// multiple by scaler values to get world coordinates
//		for (int xOffset = -1 * maxPlatformDistance; xOffset <= maxPlatformDistance; xOffset++) {
//			for (int zOffset = -1 * maxPlatformDistance; zOffset <= maxPlatformDistance; zOffset++) {
//					
//				// check if coordinate pair is not too far from originTile
//				// the sum of the distances is less or equal to the maximum distance
//				if (Mathf.Abs (xOffset) + Mathf.Abs (zOffset) <= maxPlatformDistance) {
//						
//					// create a vecor to represent the position of the new potential tile
//					// apply Tile.SCALE to it, and add it to originTile.position
//					Vector3 newPosition = new Vector3 (xOffset, 0, zOffset);
//					Debug.Log (newPosition);
//						
//					newPosition *= Tile.SCALE;
//					Debug.Log (newPosition);
//						
//					newPosition += originTile.position;
//					Debug.Log (newPosition);
//					Debug.Log (originTile.position);
//					newPosition.y = 0f;
//						
//					//					Debug.Log("new vector: " + newPosition);
//						
//					// create temporary Tile object to get tile indices
//					int tileX = (int)((newPosition.x > 0) ? (newPosition.x + 4.5) / 9 : (newPosition.x - 4.5) / 9);
//					int tileZ = (int)((newPosition.z > 0) ? (newPosition.z + 4.5) / 9 : (newPosition.z - 4.5) / 9);
//						
//					Debug.Log ("new tile pos: " + tileX + ", " + tileZ);
//						
//					// search appropriate quadrant to see if it already exists
//					// if not, pick a valid one tile option and instantiate it
//					switch (Tile.GetQuadrant (newPosition)) {
//					case Tile.Quadrant.FIRST:
//							
//						Debug.Log ("new tile in first quadrant");
//							
//						Debug.Log ("x pre-count: " + Q1.Count);
//							// ensure that Q1 is large enough along x to hold tile
//							// expand it if it's too small
//						if (Q1.Count < tileX) {
//								
//							// Q1 not wide enough to fit new tile
//							// expand Q1 along x to fit
//							for (int i = Q1.Count; i <= tileX; i++)
//								Q1.Insert (i, new List<Tile> ());
//						}
//							
//						Debug.Log ("x count: " + Q1.Count);
//							
//							// ensure that Q1 is large enough along x, z to hold tile
//							// expand it if it's too small
//						List<Tile> col = Q1 [tileX - 1];
//						if (col.Count < tileZ) {
//								
//							// Q1 not tall anough along x, z to fit new tile
//							// expand Q1 along x, z to fit
//							for (int i = col.Count; i < tileZ; i++)
//								col.Insert (i, null);
//						}
//							
//						Debug.Log ("z count: " + col.Count);
//							
//							// at this point, it's garunteed that there is a space for
//							// the new tile in the quadrant
//							// test for null, if null, add instantiate new item
//						if (Q1 [tileX - 1] [tileZ - 1] == null) {
//								
//							Debug.Log ("creating tile at: " + newPosition);
//								
//							Transform newTrans = Instantiate (prefabs [(int)Tile.Type.CROSS], newPosition, Quaternion.identity) as Transform;
//							Tile newTile = new Tile (Tile.Type.CROSS, newTrans);
//							Q1 [tileX - 1] [tileZ - 1] = newTile;
//						}
//							
//						break;
//					case Tile.Quadrant.INVALID:
//						throw new UnityException (); // this should never happen. like, ever.
//					}
//						
//					// check to see if tiles have been generated out that far
//						
//					// yield coroutine
//					yield return null;
//				}
//			}
//		}
//	}


































//
//
//
//
//	// Update is called once per frame
//	void Update ()
//	{
//		if (triggerWorldUpdate && currentTile != null) {
////			StopCoroutine("UpdateWorld");
//			
//			Debug.Log("current tile: " + currentTile);
////			StartCoroutine("UpdateWorld", currentTile);
//			triggerWorldUpdate = false;
//			currentTile = null;
//		}
//	}
//	

}
