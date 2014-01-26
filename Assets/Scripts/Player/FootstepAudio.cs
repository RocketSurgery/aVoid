using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FootstepAudio : MonoBehaviour
{

	public List<AudioClip> footStepSounds = new List<AudioClip> ();
	public float soundDelayScalar = 1f;

	void Awake ()
	{
		StartCoroutine (playFootstepSounds ());
	}

	public IEnumerator playFootstepSounds ()
	{
		CharacterController controller = GetComponent<CharacterController> ();

		while (true) {
			if (controller.isGrounded && controller.velocity.magnitude > 0) {
				audio.clip = footStepSounds [Random.Range (0, footStepSounds.Count)];
				audio.Play ();
				yield return new WaitForSeconds (controller.velocity.magnitude * soundDelayScalar);
			} else
				yield return null;
		}
	}
}
