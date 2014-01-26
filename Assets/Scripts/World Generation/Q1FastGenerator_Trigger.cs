using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Q1FastGenerator_Trigger : MonoBehaviour
{
	
	void OnTriggerEnter (Collider other)
	{
		//		Q1FastGenerator.triggerWorldUpdate = true;
		//		Q1FastGenerator.currentTile = gameObject.transform;
	}
	
	void OnTriggerStay (Collider other)
	{
		if (Q1FastGenerator.triggerWorldUpdate)
			Q1FastGenerator.currentTile = gameObject.transform;
	}
	
	void OnTriggerExit (Collider other)
	{
		Q1FastGenerator.triggerWorldUpdate = true;
	}
}
