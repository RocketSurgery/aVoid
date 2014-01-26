using UnityEngine;
using System.Collections;

public class RotatePlayer : MonoBehaviour {
	
	public float firstCheckPoint;
	public float finalCheckPoint;
	public float finalTorqueMagnitude;
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if (transform.position.x > firstCheckPoint) {
			
			if (transform.position.x < finalCheckPoint) {
				if (Vector3.Dot(transform.forward, new Vector3(-1,0,0)) < 0) {//facing away from the first beacon
					
					if (Vector3.Dot(transform.forward, new Vector3(0,0,1)) > 0) {//facing toward positive Z
						transform.Rotate(-Vector3.up * finalTorqueMagnitude * (transform.position.x - firstCheckPoint) / (finalCheckPoint - firstCheckPoint)* Mathf.Abs(Vector3.Dot(transform.forward, new Vector3(-1,0,0))));
					} else {
						transform.Rotate(Vector3.up * finalTorqueMagnitude  * (transform.position.x - firstCheckPoint) / (finalCheckPoint - firstCheckPoint)* Mathf.Abs(Vector3.Dot(transform.forward, new Vector3(-1,0,0))));
					}
				
				}
			} else {// if past final checkpoint, see double when looking back. shakey
				if (Vector3.Dot(transform.forward, new Vector3(0,0,1)) > 0) {//facing toward positive Z
					transform.Rotate(-Vector3.up * finalTorqueMagnitude * (transform.position.x - firstCheckPoint) / (finalCheckPoint - firstCheckPoint)* Mathf.Abs(Vector3.Dot(transform.forward, new Vector3(-1,0,0))));
				} else {
					transform.Rotate(Vector3.up * finalTorqueMagnitude  * (transform.position.x - firstCheckPoint) / (finalCheckPoint - firstCheckPoint)* Mathf.Abs(Vector3.Dot(transform.forward, new Vector3(-1,0,0))));
				}	
			}
		}
		
	}
}
