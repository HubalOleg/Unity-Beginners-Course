// This script handles the player's ability to attack. The biggest responcibility of this script is to maintain the timing of attack cooldowns
// so that the player cannot attack too fast. Mostly, this is a "pass through" or "bridge" script, which means that it receives input from
// the PlayerInput scripts and then passes the input along to the appropriate attack. Very little attack logic (apart from timing) exists in this 
// script. 

using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
	[Header("Attacks")]
	[SerializeField]
	private LightningAttack _lightningAttack;	// Reference to a lightning attack script

	[Header("UI")]
	[SerializeField]
	private Countdown _countDown;				// A reference to the countdown slider

	private float _attackCooldown;				// How long the player must wait before attacking again
	private float _timeOfLastAttack;			// The time when the player last attacked
	private bool _canAttack = true;				// Whether or not the player can attack
	
	//---------------------------------------------------------------------
	// Public
	//---------------------------------------------------------------------

	// This method is called whenever the player presses the attack input
	public void Fire()
	{
		// If the attack isn't ready, or the player cannot attack, leave
		if (!ReadyToAttack() || !_canAttack) return;

		ShootLightning();
	}

	// Called from PlayerHealth script
	public void Defeated()
	{
		//Player cannot attack
		_canAttack = false;
		//Turn off all attacks
		DisableAttacks();
	}
	
	//---------------------------------------------------------------------
	// Helpers
	//---------------------------------------------------------------------
	
	// Handles telling the lightning attack to fire
	private void ShootLightning()
	{
		//If there is no lightning attack, leave
		if (_lightningAttack == null)
			return;

		//Fire lightning
		_lightningAttack.Fire();
		//Record the cooldown of the lightning attack
		_attackCooldown = _lightningAttack.Cooldown;
		//record how long to wait before we can attack again
		BeginCountdown();
	}
	
	private bool ReadyToAttack()
	{
		// If enough time has passed return true (the player can attack) otherwise return false
		return Time.time >= _timeOfLastAttack + _attackCooldown;
	}

	// This method sets the countdown until the player can attack again
	private void BeginCountdown()
	{
		//Record the current time
		_timeOfLastAttack = Time.time;
		//If there is a countdown slider, tell it to begin counting down
		if (_countDown != null)
			_countDown.BeginCountdown(_attackCooldown);
	}

	// This method turns off all attacks
	private void DisableAttacks()
	{
		//Go through each attack and if a game object for it exists, turn it off
		if(_lightningAttack != null) _lightningAttack.gameObject.SetActive(false);
	}
}
