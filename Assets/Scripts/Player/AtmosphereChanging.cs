using UnityEngine;
using System.Collections;

public class AtmosphereChanging : MonoBehaviour {
	
	public GameObject parent;
	
	public float firstCheckPoint;
	public float finalCheckPoint;
	
	public AudioSource[] musicLayers;
	bool[] isPlaying;
	
	// Use this for initialization
	void Start () {
		isPlaying = new bool[musicLayers.Length];
		musicLayers[0].Play();
		isPlaying[0] = true;
	}
	
	// Update is called once per frame
	void Update () {
		
		for (int i = 1; i < 5; i++) {
			if (!isPlaying[i] && transform.position.x > firstCheckPoint + i / 5.0 * (finalCheckPoint - firstCheckPoint))
			{
				musicLayers[i].PlayDelayed(15 - (Time.time % 15));
				//musicLayers[i].Play();
				isPlaying[i] = true;
			}
		}
		

	}
}
