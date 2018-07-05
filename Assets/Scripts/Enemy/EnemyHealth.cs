// This script controls the health functions of the enemies. This script also is responsible for turning
// the enemy movement and attack off in the event of the enemy being defeated. Since the enemies aren't destroyed after
// being defeated (they are just disabled since the game maintains 'pools' or collections of enemies) there is
// code in place to reset the values of the enemies when they respawn

using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
	[HideInInspector] public EnemySpawner Spawner;		//A Reference to the spawner that created this enemy

	[Header("Health Properties")]
	[SerializeField] private int _maxHealth = 100;				//How much health this enemy has
	[SerializeField] private int _scoreValue = 10; 				//How many points this enemy is worth

	[Header("Defeated Effects")]
	[SerializeField] private float _sinkSpeed = 2.5f;			//How fast the enemy sinks into the ground		
	[SerializeField] private float _deathEffectTime = 2f;		//How long it takes the enemy to play its full death sequence before being deactivated
	[SerializeField] private AudioClip _deathClip;				//Audio clip of the death sound of the enemy
	[SerializeField] private AudioClip _hurtClip;				//Audio clip of the hurt sound of the enemy
	
	private Animator _animator;									//Reference to the animator component
	private CapsuleCollider _capsuleCollider;					//Reference to the capsule collider component
	private AudioSource _audioSource;							//Reference to the audio source component
	private EnemyAttack _enemyAttack;							//Reference to the enemy's attack script
	private EnemyMovement _enemyMovement;						//Reference to the enemy's movement script

	private int _currentHealth;									//Current health amount of enemy
	private bool _isSinking;									//Is the enemy currently sinking?
	
	//---------------------------------------------------------------------
	// Messages
	//---------------------------------------------------------------------

	private void Awake()
	{
		// Grab references to all of the needed enemy components
		_enemyAttack = GetComponent<EnemyAttack>();
		_enemyMovement = GetComponent<EnemyMovement>();

		_animator = GetComponent<Animator>();
		_capsuleCollider = GetComponent<CapsuleCollider>();
		_audioSource = GetComponent<AudioSource>();
	}

	// When this game object is enabled...
	private void OnEnable ()
	{
		// ...reset the health, isSinking
		_currentHealth = _maxHealth;
		_isSinking = false;
		
		//If there is an audio source, set the clip to the hurt sound
		if(_audioSource != null) _audioSource.clip = _hurtClip;
	}

	private void Update()
	{
		// If the enemy isn't currently sinking, return
		if(!_isSinking) return;
		// If the enemy is sinking, move downward along the -Y axis
		transform.Translate(-Vector3.up * _sinkSpeed * Time.deltaTime);
	}
	
	//---------------------------------------------------------------------
	// Public
	//---------------------------------------------------------------------

	// This method is called whenever the enemy is damaged by an attack
	public void TakeDamage(int amount)
	{
		// If the enemy is already defeated or is invulnerable, leave
		if (_currentHealth <= 0) return;

		// Reduce the current health by the amount of damage
		_currentHealth -= amount;

		// If the current health is now at or below 0, the enemy is defeated
		if (_currentHealth <= 0) Defeated();
		
		//If there is an audio source, play it
		if(_audioSource != null) _audioSource.Play();
	}
	
	// Accessed from an event in the enemy's Death animation
	public void StartSinking()
	{
		// The enemy is now sinking
		_isSinking = true;
	}
	
	//---------------------------------------------------------------------
	// Helpers
	//---------------------------------------------------------------------

	// Called when the enemy health is reduce to 0 or lower
	private void Defeated()
	{
		// Enabled the animator (in case it was disabled by a frost debuff)
		_animator.enabled = true;
		// Trigger the "Dead" parameter of the animator
		_animator.SetTrigger("Dead");
		
		//If there is an audio source, set its clip to the death sound
		if(_audioSource != null) _audioSource.clip = _deathClip;

		// Tell the attack and movement script that the enemy has been defeated
		_enemyAttack.Defeated();
		_enemyMovement.Defeated();

		// Tell the game manager to give the player some points
		GameManager.Instance.AddScore(_scoreValue);
		// Call the TurnOff() method after a period of time
		Invoke("TurnOff", _deathEffectTime);
	}

	// Called once the enemy's "defeated" effects have finished playing
	private void TurnOff()
	{
		// Disable the game object
		gameObject.SetActive(false);
	}
}