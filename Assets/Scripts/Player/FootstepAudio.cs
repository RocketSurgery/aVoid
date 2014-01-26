using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FootstepAudio : MonoBehaviour
{

	public List<AudioClip> footStepSounds = new List<AudioClip> ();

	[Range (0, 10)]
	public float soundDelayScalar = 1f;

	private CharacterMotor motor;
	private CharacterController controller;

	void Start() {

	}

	void Awake ()
	{
		controller = GetComponent<CharacterController> ();
		motor = GetComponent<CharacterMotor>();
		StartCoroutine (playFootstepSounds ());
	}

	public IEnumerator playFootstepSounds ()
	{

		while (true) {
			if (controller.isGrounded && controller.velocity.magnitude > 0) {
				audio.clip = footStepSounds [Random.Range (0, footStepSounds.Count)];
				audio.Play ();
				yield return new WaitForSeconds (motor.movement.maxForwardSpeed / controller.velocity.magnitude * soundDelayScalar);
			} else
				yield return null;
		}
	}
}
