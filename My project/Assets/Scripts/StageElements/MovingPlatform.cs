using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    //Variables
    float _delay = 1f;
    float _delayCd;
    float _elapsedTime;
    float _timeToWaypoint;
    int _index;
    CharacterController _player;
    Transform _previousWaypoint;
    Transform _targetWaypoint;
    Vector3 _platformMovement;

    //Editable Variables
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform[] _waypoints;
    [SerializeField] float _speed;

    private void Start()
    {
        _index = 0;
        GetWaypoint();
    }

    private void OnDisable()
    {
        _index = 0;
        GetWaypoint();
    }

    private void FixedUpdate()
    {
        if (_delayCd < 0)
        {
            _elapsedTime += Time.deltaTime;
            float elapsedPercentage = _elapsedTime / _timeToWaypoint;
            _platformMovement = Vector3.Lerp(_previousWaypoint.position, _targetWaypoint.position, elapsedPercentage);

            rb.MovePosition(_platformMovement);


            if (elapsedPercentage >= 1)
            {
                GetWaypoint();
                _delayCd = _delay;
            }
        }
    }

    private void Update()
    {
        if (_delayCd >= 0)
        {
            _delayCd -= Time.deltaTime;
        }
    }

    private void GetWaypoint()
    {
        _previousWaypoint = _waypoints[_index];
        _index++;
        if (_waypoints.Length <= _index )
        {
            _index = 0;
        }
        _targetWaypoint = _waypoints[_index];

        _elapsedTime = 0;

        float distanceToWaypoint = Vector3.Distance(_previousWaypoint.position, _targetWaypoint.position);
        _timeToWaypoint = distanceToWaypoint / _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _player = other.gameObject.GetComponent<CharacterController>();
            Debug.Log("Platform Entered");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _player.Move((rb.linearVelocity * .5f) * Time.deltaTime);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _player = null;
        }
    }
}
