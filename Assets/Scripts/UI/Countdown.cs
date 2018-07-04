//This script controls the slider that acts as a visible countdown timer for the player's attacks

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
	private static readonly WaitForSeconds UPDATE_DELAY = new WaitForSeconds(.25f);	// The delay between updating the slider (for efficiency).
	
	private Slider _slider;							// Reference to the slider component
	private float _timeOfCountdownFinish;			// The time when the countdown will be complete

	//---------------------------------------------------------------------
	// Messages
	//---------------------------------------------------------------------

	private void Awake()
	{
		// Grab a reference to the slider component
		_slider = GetComponent<Slider>();
		
		// Start off by reducing the slider values
		_slider.minValue = 0f;
		_slider.maxValue = 0f;
	}
	
	//---------------------------------------------------------------------
	// Public
	//---------------------------------------------------------------------

	// Called by PlayerAttack script when an attack with a cooldown has been made
	public void BeginCountdown(float cooldown)
	{
		// Record the time the cooldown will be complete
		_timeOfCountdownFinish = Time.time + cooldown;

		// Set the max value and current value of the slider to the total cooldown amount
		_slider.maxValue = cooldown;
		_slider.value = cooldown;
		
		// Start the UpdateCountdownBar() coroutine
		StartCoroutine("UpdateCountdownBar");
	}
	
	//---------------------------------------------------------------------
	// Helpers
	//---------------------------------------------------------------------

	// This coroutine reduces the value of the slider so the player can see when their attack will be ready
	private IEnumerator UpdateCountdownBar()
	{
		// While the value of the slider is greater than 0...
		while (_slider.value > 0f)
		{
			// ...reduce the value of the slider...
			_slider.value = _timeOfCountdownFinish - Time.time;
			// ...wait a period of time before looping
			yield return UPDATE_DELAY;
		}
		// Once the countdown is finished, set the max value to 0 so the slider dissapears
		_slider.maxValue = 0f;
	}
}

