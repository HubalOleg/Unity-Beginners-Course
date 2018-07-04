//This script handles the lightning attack. The lightning attack is an instant line attack that does a moderate
//amount of damage with a small cooldown.

using UnityEngine;

public class LightningAttack : MonoBehaviour
{
	[Header("Weapon Specs")]
	[SerializeField]
	private float _cooldown = 1f;				//The cooldown of the attach

	[SerializeField]
	private int _damage = 20; 				//How much damage the attack does
	[SerializeField]
	private float _range = 20.0f;			//How far the attack can shoot
	[SerializeField]
	private LayerMask _strikeableMask; 		//Layermask that determines what the attack can hit

	[Header("Weapon References")]
	[SerializeField]
	private LightningBolt _lightningBolt;	//Reference to the lightningBolt script on the lightning bolt game object
	[SerializeField]
	private AVPlayer _lightningHit;			//Reference to the AVPlayer (Audio Visual Player) on the lightning hit game object
	
	//---------------------------------------------------------------------
	// Properties
	//---------------------------------------------------------------------

	public float Cooldown
	{
		get { return _cooldown; }
	}

	//---------------------------------------------------------------------
	// Public
	//---------------------------------------------------------------------

	//Called from PlayerAttack script
	public void Fire()
	{
		//Create a ray from the current position and extending straight forward
		Ray ray = new Ray(transform.position, transform.forward);
		//Create a RaycastHit variable which will store information about the raycast
		RaycastHit hit;

		//If our raycast hits something...
		if (Physics.Raycast(ray, out hit, _range, _strikeableMask))
		{
			//...move the lightning hit game object to the point of the hit...
			_lightningHit.transform.position = hit.point;
			//...and play the effect...
			_lightningHit.Play();
			//...then set the end point of the lightning bolt..
			_lightningBolt.EndPoint = hit.point;
			//...then try to get a reference to an EnemyHealth script...
			EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
			//...if the script exists...
			if (enemyHealth != null)
			{	
				//...tell the enemy to take damage
				enemyHealth.TakeDamage(_damage);
			}
		}
		//Otherwise, if our raycast doesn't hit anything...
		else
		{
			//...place the end of the bolt at maximum range
			_lightningBolt.EndPoint = ray.GetPoint(_range);
		}
		//Turn the lightning bolt game object on
		_lightningBolt.gameObject.SetActive(true);
	}
}
	