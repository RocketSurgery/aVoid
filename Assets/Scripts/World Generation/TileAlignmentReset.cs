using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TileAlignmentReset : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		TileAlignmentAnchor[] anchors = FindObjectsOfType < TileAlignmentAnchor > ();
		Debug.Log ("number of tiles: " + anchors.Length);

		foreach (TileAlignmentAnchor anchor in anchors) {
			Vector3 oldPos = anchor.gameObject.transform.position;

			Debug.Log ("old position:\t" + oldPos);

			Vector3 newPos = new Vector3(Mathf.Round(oldPos.x / 9f) * 9, 0, Mathf.Round(oldPos.z / 9f) * 9);

			anchor.gameObject.transform.Translate (newPos - oldPos, Space.World);
			anchor.gameObject.transform.localScale = new Vector3(1, 1, 1);

			Debug.Log("position actual:\t" + anchor.gameObject.transform.position);
			Debug.Log("position ideal:\t\t" + newPos);
			Debug.Log("scale:\t\t" + anchor.gameObject.transform.localScale);
		}
	}

}
