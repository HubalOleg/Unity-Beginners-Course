//This script handles the graphical "lightning bolt" that is a part of the lightning attack. Only graphical
//effects are handled in this script. All functional attack mechanics are handled in the LightningAttack script

using UnityEngine;

public class LightningBolt : MonoBehaviour
{
	[HideInInspector] public Vector3 EndPoint;		// The end point of the lightning bolt

	[Header("Bolt Properties")]
	[SerializeField]
	private float _rayHeight = 2.0f;		// How high the bolt is off the "ground" (if the floor is not at 0, then this needs to be changed)
	[SerializeField]
	private float _effectDuration = .75f;	// How long does the bolt last
	[SerializeField]
	private float _phaseDuration = .1f;		// How long does the bolt stay in each "phase" (the zig-zag of the bolt)

	[Header("Bolt rendering")]
	[SerializeField]
	private LineRenderer _rayRenderer;		// Reference to a line renderer component
	[SerializeField]
	private AnimationCurve[] _rayPhases;	// Array of animation curves used to draw the different bolt phases

	private int _phaseIndex;					// The phase our bolt currently is on
	private float _timeToChangePhase;			// When the bolt will change phase
	private float _timeSinceEffectStarted;		// How long the bolt has been alive
	private Vector3 _vectorOfBolt;				// The relative vector of the bolt
	
	//---------------------------------------------------------------------
	// Messages
	//---------------------------------------------------------------------

	//When the bolt is enabled, reset the time variables
	private void OnEnable()
	{
		_timeToChangePhase = 0f;
		_timeSinceEffectStarted = 0f;
	}

	private void Update()
	{
		//Accumulate how much time has passed since this bolt was activated
		_timeSinceEffectStarted += Time.deltaTime;
		//If the bolt has been alive long enough deactivate it
		if(_timeSinceEffectStarted >= _effectDuration)
			gameObject.SetActive(false);

		//Calculate the relative vector of the bolt by subtracting its end point from where the firing point of the player
		//currently is
		_vectorOfBolt = EndPoint - transform.position;

		//If it's time to change phases...
		if (_timeSinceEffectStarted >= _timeToChangePhase)
		{
			//...determine the new time to change phases and...
			_timeToChangePhase = _timeSinceEffectStarted + _phaseDuration;
            //...change the phase
			ChangePhase();
		}
	}
	
	//---------------------------------------------------------------------
	// Helpers
	//---------------------------------------------------------------------

	//This method calculates the seemingly random appearance of a bolt of lightning
	private void ChangePhase()
	{
		//Increase our phase index
		_phaseIndex++;
		//If the phase index is larger than our array reset it back to 0
		if (_phaseIndex >= _rayPhases.Length)
			_phaseIndex = 0;

		//Grab the current animation curve
		var curve = _rayPhases[_phaseIndex];
		//Tell the line renderer how many corners or vertices it will have
		_rayRenderer.SetVertexCount(curve.keys.Length);
		//Loop through the vertices in our current animation curve and...
		for (var index = 0; index < curve.keys.Length; ++index)
		{
			//...grab the current keyframe, then...
			var key = curve.keys[index];

			//...calculate a new point based on our starting position, the vector of the bolt, and the position of the key (time
			//is the position of the keyframe along the animation curve timeline and it will have a value between 0 and 1)...
			var point = transform.position + _vectorOfBolt * key.time;
			//...move our new point along the Y axis so that it is raised off of the ground...
			point += Vector3.up  *key.value * _rayHeight;
			//...finally, set a vertices of the line renderer to our new point.
            _rayRenderer.SetPosition(index, point);
		}
	}
}
