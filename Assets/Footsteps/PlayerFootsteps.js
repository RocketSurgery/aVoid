var footStepSounds : AudioClip[];
var stepLengthwalk = 0.3;
var stepLengthrun = 0.3;
var stepLengthcrouch = 0.3;



function Awake()
{
	PlayFootStepSounds();
}

function PlayFootStepSounds()
{
	var controller : CharacterController = GetComponent(CharacterController);
	
	while (true)
		if (controller.isGrounded && controller.velocity.magnitude > 5 )
		{
			audio.clip = footStepSounds[Random.Range(0, footStepSounds.length)];
			audio.Play();
			yield WaitForSeconds(stepLengthrun);
		}
		else if (controller.isGrounded && controller.velocity.magnitude > 1)
		{
			audio.clip = footStepSounds[Random.Range(0, footStepSounds.length)];
			audio.PCharacterControllerlay();
			yield WaitForSeconds(stepLengthwalk);
		}
		else if (controller.isGrounded && controller.velocity.magnitude > 1)
		{
			audio.clip = footStepSounds[Random.Range(0, footStepSounds.length)];
			audio.Play();
			yield WaitForSeconds(stepLengthcrouch);
		}
		else
		{
			yield;
		}
}