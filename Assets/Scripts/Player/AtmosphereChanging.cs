using UnityEngine;
using System.Collections;

public class AtmosphereChanging : MonoBehaviour {
	
	public GameObject mainCharacter;
	
	public float firstCheckPoint;
	public float finalCheckPoint;
	
	public AudioSource[] musicLayers;
	bool[] isPlaying;
	
	public GameObject[] anxietyOverlays;
	
	public AudioSource heavyBreathing;
	
	// Use this for initialization
	void Start () {
		isPlaying = new bool[musicLayers.Length];
		musicLayers[0].Play();
		isPlaying[0] = true;
	}
	
	// Update is called once per frame
	void Update () {
		
		for (int i = 1; i < musicLayers.Length; i++) {
			if (!isPlaying[i] && transform.position.x > firstCheckPoint + i / (float)musicLayers.Length * (finalCheckPoint - firstCheckPoint))
			{
				musicLayers[i].PlayDelayed(15 - (Time.time % 15));
				//musicLayers[i].Play();
				isPlaying[i] = true;
			}
		}
		
		for (int i = 0; i < anxietyOverlays.Length; i++) {
			if (mainCharacter.transform.position.x > firstCheckPoint + i / (float)anxietyOverlays.Length * (finalCheckPoint - firstCheckPoint)) {
				anxietyOverlays[i].guiTexture.enabled = true;
			} else {
				anxietyOverlays[i].guiTexture.enabled = false;
			}
		}
		
	}
}
