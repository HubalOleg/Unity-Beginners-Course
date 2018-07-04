// This script is used to player a particle effect and audio effect (AV being short for Audio Visual). This is useful because it allows us to 
// play effects one time without needing to worry about turning game objects off at the right time. It is
// basically "fire and forget" without needing extra component references in other scripts.

using UnityEngine;

public class AVPlayer : MonoBehaviour
{
	[SerializeField]
	private ParticleSystem _particleEffect;	// Reference to a particle system component
	[SerializeField]
	private AudioSource _audioEffect;		// Reference to an audio source component
	
	//---------------------------------------------------------------------
	// Messages
	//---------------------------------------------------------------------

	private void Awake()
	{
		//Grab references to the needed components
		_particleEffect = GetComponent<ParticleSystem>();
		_audioEffect = GetComponent<AudioSource>();
	}
	
	//---------------------------------------------------------------------
	// Public
	//---------------------------------------------------------------------

	// This method plays the particle effect and audio source if they exist
	public void Play()
	{
		if (_particleEffect != null)
			_particleEffect.Play(true);	// Play() is called with 'true' which means that any particle systems nested under this one will also play

		if (_audioEffect != null)
			_audioEffect.Play();
	}
}
