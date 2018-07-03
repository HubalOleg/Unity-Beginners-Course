//This script controls the spawning of enemies. The enemy spawner is a "pooled" spawner. A "pool" is effectively a collection of objects
//that are generally disabled. Then when they are needed, an object is enabled, used, and then disabled again when it is done. This is in
//contrast to a system where we intantiate and destroy objects when we need them and need to get rid of them. Instantiating and destroying 
//can cause lag as well as memory spikes which are both bad things. This script also has a maximum number of enemies it can spawn to prevent
//the scene from being flooded with enemies which makes the game very difficult and can cause lag / crashes.

using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	
	[SerializeField] 
	private GameObject _enemyPrefab;		//The enemy prefab to spawn
	[SerializeField]
	private float _spawnRate = 5f;			//Rate, in seconds, to spawn enemies
	[SerializeField]
	private int _maxEnemies = 10;			//Maximum number of enemies that this spawner can have at a time
	
	private GameObject[] _enemies;			//An array of the pooled enemies
	private WaitForSeconds _spawnDelay;		//The delay between attempts to spawn an enemy

	//---------------------------------------------------------------------
	// Messages
	//---------------------------------------------------------------------
	
	private void Awake()
	{
		//Create an array to store the pool of enemies
		_enemies = new GameObject[_maxEnemies];
		//Loop through the array and...
		for (var i = 0; i < _maxEnemies; i++)
		{
			//...instantiate an enemy game object from a prefab...
			var enemy = Instantiate(_enemyPrefab);
			//...parent it to this gamn object...
			enemy.transform.parent = transform;
			//...disable it...
			enemy.SetActive(false);
			//...finally, store the enemy's health script in the pool
			_enemies[i] = enemy;
		}
		//Create the WaitForSeconds variable that will be used to delay spawning
		_spawnDelay = new WaitForSeconds(_spawnRate);
	}

	//This is an interesting use of Start() where the Start() method itself is
	//used as a coroutine. You could have the Start() method run a different coroutine to
	//achieve the same effect, but it's useful to know that using the Start() method like
	//this is possible
	private IEnumerator Start()
	{
		while (true)
		{
			//...wait the specified delay...
			yield return _spawnDelay;
			//...and then spawn an enemy before looping again
			SpawnEnemy();

			//In case you are wondering, you could swap lines 55 and 57. Doing
			//so would cause an enemy to be immediately spawned when the game starts.
			//The way it is now, however, the spawner waits first which gives the 
			//player a chance to orient themselves}
		}
	}
	
	//---------------------------------------------------------------------
	// Helpers
	//---------------------------------------------------------------------
	
	//This method spawns an enemy into the scene by searching the pool for an available enemy
	//and enabling it. It's worth nothing that it would be more efficient to create a system
	//where we didn't have to search the pool for an available enemy and instead pulled any
	//enemies that weren't available out of the pool. It is constructed this way, however, in 
	//an attempt to keep the code as simple and clean as possible. Also, the size of the pools are
	//very small, so the difference in efficiency between the two ways of doing this is negligable
	private void SpawnEnemy()
	{
		//Loop through the pool of enemies
		foreach (var enemy in _enemies)
		{
			//If the current enemy is available (not active)...
			if (enemy.gameObject.activeSelf) continue;
			
			//...orient it with the spawner...
			enemy.transform.position = transform.position;
			enemy.transform.rotation = transform.rotation;
			//...enable it...
			enemy.SetActive(true);
			//...and leave this method so it doesn't accidently spawn more enemies
			return;
		}
	}
}

