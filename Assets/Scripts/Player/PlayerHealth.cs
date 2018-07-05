//This script keeps track of the player's health

using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
	[Header("Health Properties")]
	[SerializeField]
	private int _maxHealth = 100;				//Player's maximum health	
	[SerializeField]
	private AudioClip _deathClip;				//Sound clip for the player's death
	[SerializeField]
	private AudioClip _hurtClip;				//Audio clip of the hurt sound of the player

	[Header("UI")]
	[SerializeField]
	private FlashFade _damageImage;
	[SerializeField]
	private Slider _healthSlider;
	
	private PlayerMovement _playerMovement;		//Reference to the player's movement script
	private Animator _animator;					//Reference to the animator component
	private AudioSource _audioSource;			//Reference to the audio source component
	
	private int _currentHealth;					//The current health of the player
	
	//---------------------------------------------------------------------
	// Messages
	//---------------------------------------------------------------------

	private void Awake()
	{
		//Grab the needed component references
		_playerMovement = GetComponent<PlayerMovement>();
		_animator = GetComponent<Animator>();
		_audioSource = GetComponent<AudioSource>();
	}

	private void Start()
	{
		_currentHealth = _maxHealth;
		_audioSource.clip = _hurtClip;
	}
	
	//---------------------------------------------------------------------
	// Public
	//---------------------------------------------------------------------

	public void TakeDamage(int damageAmount)
	{
		if (!IsAlive()) return;
		
		_currentHealth -= damageAmount;
		
		//If there is a damage image, tell it to flash
		if(_damageImage != null) _damageImage.Flash();
		
		//If there is a health slider, update its value
		if(_healthSlider != null) _healthSlider.value = _currentHealth / (float) _maxHealth;
		
		//If there is an audio source, tell it to play
		if(_audioSource != null) _audioSource.Play();

		if (IsAlive()) return;
		
		//If there is a player movement script, tell it to be defeated
		if (_playerMovement != null) _playerMovement.Defeated();

		//Set the Die parameter in the animator
		_animator.SetTrigger("Die");
		
		//...if there is an audio source, assign the deathclip to it
		if (_audioSource != null)
		{
			_audioSource.clip = _deathClip;
			_audioSource.Play();
		}
		
		//...finally, tell the GameManager that the player has been defeated
		GameManager.Instance.PlayerDied();
	}
	
	public bool IsAlive()
	{
		//If the currentHealth is above 0 return true (the player is alive), otherwise return false
		return _currentHealth > 0;
	}
}

