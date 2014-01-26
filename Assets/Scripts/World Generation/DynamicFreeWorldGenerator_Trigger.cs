using UnityEngine;
using System.Collections;

public class DynamicFreeWorldGenerator_Trigger : MonoBehaviour {

	public DynamicFreeWorldGenerator.Node node;
	public DynamicFreeWorldGenerator generator;

	void OnTriggerStay (Collider other)
	{
//		if (generator != null && generator.triggerWorldUpdate)
			// TODO call generation coroutine
//			generator.currentTile = gameObject.transform;
	}
	
	void OnTriggerExit (Collider other)
	{
//		generator.triggerWorldUpdate = true;
	}

}
