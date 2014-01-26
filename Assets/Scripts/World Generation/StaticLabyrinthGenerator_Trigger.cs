using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StaticLabyrinthGenerator_Trigger : MonoBehaviour
{

	void OnTriggerEnter (Collider other)
	{
//		StaticLabyrinthGenerator.triggerWorldUpdate = true;
//		StaticLabyrinthGenerator.currentTile = gameObject.transform;
	}
	
	void OnTriggerStay (Collider other)
	{
		if (StaticLabyrinthGenerator.triggerWorldUpdate)
			StaticLabyrinthGenerator.currentTile = gameObject.transform;
	}
	
	void OnTriggerExit (Collider other)
	{
		StaticLabyrinthGenerator.triggerWorldUpdate = true;
	}
}
