//This script controls the movement of the enemies. The enemy uses a navmesh agent to navigate the scene. 

using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
	[SerializeField]
	private float _updateDelay = 0.3f; //The delay between updating the navmesh agent (for efficiency)

	private Transform _target;
	private UnityEngine.AI.NavMeshAgent _navMeshAgent;
	private float _interval;

	//---------------------------------------------------------------------
	// Messages
	//---------------------------------------------------------------------
	
	private void Awake()
	{
		_navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
	}

	private void Start()
	{
		_target = GameManager.Instance.Player.transform;
	}

	private void Update()
	{
		if (_target == null) return;
		
		_interval += Time.deltaTime;
		if (_interval < _updateDelay) return;
		_interval = 0;
		
		_navMeshAgent.SetDestination(_target.position);
	}
}

