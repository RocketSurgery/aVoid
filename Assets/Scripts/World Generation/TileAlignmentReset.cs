using UnityEngine;
using System.Collections;

public class TileAlignmentReset : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		TileAlignmentAnchor[] anchors = FindObjectsOfType < TileAlignmentAnchor > ();
		Debug.Log ("number of tiles: " + anchors.Length);

		foreach(TileAlignmentAnchor anchor in anchors) {
			Debug.Log("old position: " + anchor.gameObject.transform.position);
			anchor.gameObject.transform.position.Set(Mathf.Round(anchor.gameObject.transform.position.x / 9.0f)*9,Mathf.Round(anchor.gameObject.transform.position.z / 9.0f)*9, 0);
			
			
			
			//if (anchor.gameObject.transform.position.y != 0f)
			//	Debug.Log("old position: " + anchor.gameObject.transform.position);
			//anchor.gameObject.transform.position.Scale(new Vector3(1f, 0f, 1f));
//			Debug.Log("new position: " + anchor.gameObject.transform.position);
		}
	}

}
