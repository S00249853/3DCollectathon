using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GroundEnemyStateMachine : MonoBehaviour, IStompable
{
    PlayerStateMachine _playerMachine;

    //State Variables
    EnemyBaseState _currentState;
    GroundEnemyStateFactory _states;

    //Navigation Variables
    NavMeshAgent _agent;
    Transform _playerLocation;
    [SerializeField] Transform[] _wayPoints;
    Vector3 _home;
    [SerializeField] float _homeRadius;


    //Getters and Setters
    public PlayerStateMachine PlayerMachine {  get { return _playerMachine; } }
    public EnemyBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public NavMeshAgent Agent {  get { return _agent; } set { _agent = value; } }
    public Transform PlayerLocation { get { return _playerLocation; } set { _playerLocation = value; } }
    public Transform[] WayPoints { get { return _wayPoints; } }
    public Vector3 Home { get { return _home; } }
    public float HomeRadius { get { return _homeRadius; } }

    public void Stomped()
    {
        Destroy(this.gameObject);
    }

    private void Awake()
    {
        _states = new GroundEnemyStateFactory(this);
        _currentState = _states.Patrol();
        _agent = GetComponent<NavMeshAgent>();
        _playerLocation = GameObject.Find("Player").transform;
        _playerMachine = GameObject.Find("Player").GetComponent<PlayerStateMachine>();
        _home = transform.position;
    }

    private void Update()
    {
        _currentState.UpdateStates();
    }
}
